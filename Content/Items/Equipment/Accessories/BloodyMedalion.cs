using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Equipment.Accessories
{
    public class BloodyMedalion : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bloody Medallion");
            Tooltip.SetDefault("Doubles magic damage" + "\nWhat normaly drains mana drains you instead!" + "\nLess effective with certain weapons");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Blue;

            Item.value = 1000;
            Item.width = 14;
            Item.height = 14;

            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<BloodMedalionEffect>().effect = true;
        }
    }

    public class BloodMedalionEffect : ModPlayer
    {
        public bool effect;

        public override void ResetEffects()
        {
            effect = false;
        }

        public override void PostUpdateEquips()
        {
            if (effect)
            {
                Player.spaceGun = false;
                if (Player.HeldItem.type == ItemID.CrimsonRod || Player.HeldItem.type == ItemID.NimbusRod || Player.HeldItem.type == ItemID.MagnetSphere)
                {
                    Player.GetDamage(DamageClass.Magic) *= 1.4f;
                }
                else
                {
                    Player.GetDamage(DamageClass.Magic) *= 2f;
                }

            }
        }
    }

    public class BloodMedialionItemEffect : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override GlobalItem Clone(Item item, Item itemClone)
        {
            return base.Clone(item, itemClone);
        }
        private int k;

        public override bool CanUseItem(Item item, Player player)
        {
            if (player.GetModPlayer<BloodMedalionEffect>().effect && item.mana > 0)
            {
                int lifeCost = (int)(item.mana * player.manaCost);
                if (lifeCost < 0)
                {
                    lifeCost = 0;
                }
                player.statLife -= lifeCost;
                if (player.statLife <= 0)
                {
                    player.KillMe(PlayerDeathReason.ByCustomReason(player.name +" madly drained "+ (player.Male ? "his" : "her") + " lifeforce!"), (int)(item.mana * player.manaCost), 0);
                }
                player.manaCost = 0f;
                return true;
            }

            return base.CanUseItem(item, player);
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (Main.LocalPlayer.GetModPlayer<BloodMedalionEffect>().effect)
            {
                foreach (TooltipLine line in tooltips) //runs through all tooltip lines
                {
                    if (line.mod == "Terraria" && line.Name == "UseMana") //this checks if it's the line we're interested in
                    {
                        int lifeCost = (int)(item.mana * Main.LocalPlayer.manaCost);
                        if (lifeCost < 0)
                        {
                            lifeCost = 0;
                        }
                        line.text = "Uses " + lifeCost + " life!";//change tooltip
                        line.overrideColor = Color.Crimson;
                    }
                }
            }
        }
    }
    public class BloodyMedalionDrop : GlobalNPC
    {
        public override void ModifyGlobalLoot(GlobalLoot globalLoot)
        {
            LeadingConditionRule bloodMoon = new LeadingConditionRule(new Conditions.IsBloodMoonAndNotFromStatue());
            bloodMoon.OnSuccess(ItemDropRule.Common(ItemType<BloodyMedalion>(), 50));
            globalLoot.Add(bloodMoon);
        }
    }
}
