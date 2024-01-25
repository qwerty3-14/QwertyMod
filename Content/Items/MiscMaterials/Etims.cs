
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.MiscMaterials
{
    public class Etims : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }


        public override void SetDefaults()
        {
            Item.value = 10000;
            Item.width = Item.height = 32;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.Orange;
        }
    }
}