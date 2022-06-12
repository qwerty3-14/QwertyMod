using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.MiscMaterials
{
    public class BladedArrowShaft : ModItem
    {

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Used to craft a powerful arrow!");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.rare = 7;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.width = 26;
            Item.height = 14;
        }
    }
}