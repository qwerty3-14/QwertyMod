
using QwertyMod.Content.Items.Equipment.Accessories.Expert.Doppleganger;
using QwertyMod.Content.Items.MiscMaterials;
using QwertyMod.Content.NPCs.Bosses.CloakedDarkBoss;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using Terraria.GameContent.ItemDropRules;

namespace QwertyMod.Content.Items.Consumable.BossBag
{
    public class NoehtnapBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }


        public override void SetDefaults()
        {
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.width = 48;
            Item.height = 32;
            Item.rare = ItemRarityID.Cyan;
            Item.expert = true;
            //bossBagNPC = mod.NPCType("CloakedDarkBoss");
        }


        public override bool CanRightClick()
        {
            return true;
        }
        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            itemLoot.Add(ItemDropRule.Common(ItemType<Etims>(), 1, 20, 36));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Doppleganger>(), 1));
            itemLoot.Add(ItemDropRule.Coins(80000, true));
            
        }
    }
}