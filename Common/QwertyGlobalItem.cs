using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using QwertyMod.Content.Items.Equipment.Accessories;
using Terraria.GameContent.ItemDropRules;

namespace QwertyMod.Common
{
    public class QwertyGlobalItem : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
        }
        public override bool CanConsumeAmmo(Item weapon, Item ammo, Player player)
        {
            return Main.rand.NextFloat() <= player.GetModPlayer<CommonStats>().ammoReduction;
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type > ItemID.Count && (item.CountsAsClass(DamageClass.Summon)) && item.ModItem.Mod.Name == "QwertyMod")
            {
                foreach (TooltipLine line in tooltips) //runs through all tooltip lines
                {
                    if (line.Mod == "Terraria" && line.Name == "BuffTime") //this checks if it's the line we're interested in
                    {
                        //tooltips.Remove(line);
                        line.Text = "";
                    }
                }
            }
        }
        public override void ModifyItemScale(Item item, Player player, ref float scale)
        {
            if(item.useStyle == ItemUseStyleID.Swing && item.DamageType == DamageClass.Melee && item.damage > 0)
            {
                scale *= player.GetModPlayer<CommonStats>().weaponSize;
            }
        }
        
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            if(item.type == ItemID.DungeonFishingCrate || item.type == ItemID.DungeonFishingCrateHard)
            {
				itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<VulgarDictionary>(), 10));
            }
        }
    }
}
