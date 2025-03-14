﻿using Microsoft.Xna.Framework;
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
            player.GetModPlayer<BloodMedalionEffect>().effect++;
        }
    }

    public class BloodMedalionEffect : ModPlayer
    {
        public int effect = 0;

        public override void ResetEffects()
        {
            effect = 0;
        }

        public override void PostUpdateEquips()
        {
            if (effect > 0)
            {
                Player.spaceGun = false;
            }
        }
		public override void PostItemCheck()
        {
            if(Player.statMana < Player.statManaMax2 && effect > 0 && Player.itemAnimation > 0)
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
		public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
		{
			if(player.GetModPlayer<BloodMedalionEffect>().effect > 0 && item.DamageType == DamageClass.Magic)
            {
                damage *= (1 + player.GetModPlayer<BloodMedalionEffect>().effect);
            }
		}
        
        
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (!item.IsAir && Main.LocalPlayer.GetModPlayer<BloodMedalionEffect>().effect > 0)
            {
                foreach (TooltipLine line in tooltips) //runs through all tooltip lines
                {
                    if (line.Mod == "Terraria" && line.Name == "UseMana") //this checks if it's the line we're interested in
                    {
                        int lifeCost = GetLifeCost((int)(item.mana * Main.LocalPlayer.manaCost)) * Main.LocalPlayer.GetModPlayer<BloodMedalionEffect>().effect;
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
