using QwertyMod.Content.Items.Equipment.Accessories.Expert.Doppleganger;
using QwertyMod.Content.Items.MiscMaterials;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using Terraria.ID;

namespace QwertyMod.Content.Items.Consumable.BossBag
{
    public class NoehtnapBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
            ItemID.Sets.BossBag[Type] = true; // This set is one that every boss bag should have, it, for example, lets our boss bag drop dev armor..
			ItemID.Sets.PreHardmodeLikeBossBag[Type] = true; // ..But this set ensures that dev armor will only be dropped on special world seeds, since that's the behavior of pre-hardmode boss bags.
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
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Etims>(), 1, 20, 36));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Doppleganger>(), 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<NoehtnapBag>(), 7, 1, 1));
            itemLoot.Add(ItemDropRule.Coins(80000, true));
            
        }
    }
}