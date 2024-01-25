using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;


namespace QwertyMod.Content.Items.Equipment.Accessories
{
    public class BloodyMedalion : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Blue;

            Item.value = 1000;
            Item.width = 32;
            Item.height = 34;

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
                /*
                if (Player.HeldItem != null)
                {
                    if (Player.HeldItem.type == ItemID.CrimsonRod || Player.HeldItem.type == ItemID.NimbusRod || Player.HeldItem.type == ItemID.MagnetSphere)
                    {
                        Player.GetDamage(DamageClass.Magic) *= 1.4f;
                    }
                    else
                    {
                        Player.GetDamage(DamageClass.Magic) *= 2f;
                    }
                }
                */

            }
        }
		public override void PostItemCheck()
        {
            if(Player.statMana < Player.statManaMax2 && effect && Player.itemAnimation > 0)
            {
                int amt = Player.statManaMax2 - Player.statMana;
                int lifeDrain = BloodMedialionItemEffect.GetLifeCost(amt);
                Player.statMana += amt;
                Player.statLife -= lifeDrain;
                if (Player.statLife <= 0)
                {
                    Player.KillMe(PlayerDeathReason.ByCustomReason(Player.name + " madly drained " + (Player.Male ? "his" : "her") + " lifeforce!"), lifeDrain, 0);
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
        public static int GetLifeCost(int manaCost)
        {
            return (int)MathHelper.Max(manaCost / (ModLoader.HasMod("TRAEProject") ? 2 : 1), 1);
        }
		/*
        public override void UseAnimation(Item item, Player player)
        {
            if (player.GetModPlayer<BloodMedalionEffect>().effect && item.mana > 0)
            {
                int lifeCost = GetLifeCost((int)(item.mana * player.manaCost));
                if (lifeCost < 0)
                {
                    lifeCost = 0;
                }
                player.statLife -= lifeCost;
                if (player.statLife <= 0)
                {
                    player.KillMe(PlayerDeathReason.ByCustomReason(player.name + " madly drained " + (player.Male ? "his" : "her") + " lifeforce!"), (int)(item.mana * player.manaCost), 0);
                }
                player.manaCost = 0f;
            }
        }
        */
		public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
		{
			if(player.GetModPlayer<BloodMedalionEffect>().effect)
            {
                damage *= 2;
            }
		}
        
        
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (!item.IsAir && Main.LocalPlayer.GetModPlayer<BloodMedalionEffect>().effect)
            {
                foreach (TooltipLine line in tooltips) //runs through all tooltip lines
                {
                    if (line.Mod == "Terraria" && line.Name == "UseMana") //this checks if it's the line we're interested in
                    {
                        int lifeCost = GetLifeCost((int)(item.mana * Main.LocalPlayer.manaCost));
                        if (lifeCost < 0)
                        {
                            lifeCost = 0;
                        }
                        line.Text = Language.GetTextValue(Mod.GetLocalizationKey("CustomTooltipBloodCostStart")) + lifeCost + Language.GetTextValue(Mod.GetLocalizationKey("CustomTooltipBloodCostEnd"));//change tooltip
                        line.OverrideColor = Color.Crimson;
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
            bloodMoon.OnSuccess(ItemDropRule.Common(ModContent.ItemType<BloodyMedalion>(), 200));
            globalLoot.Add(bloodMoon);
        }
    }
}
