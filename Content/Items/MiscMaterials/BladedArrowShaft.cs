using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.MiscMaterials
{
    public class BladedArrowShaft : ModItem
    {

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Lime;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.width = 26;
            Item.height = 14;
        }
    }
}