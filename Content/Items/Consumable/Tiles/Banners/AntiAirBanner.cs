using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
namespace QwertyMod.Content.Items.Consumable.Tiles.Banners
{
    public class AntiAirBanner : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Anti Air Banner");
        }

        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 24;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = 1;
            Item.consumable = true;
            Item.rare = 1;
            Item.value = Item.buyPrice(0, 0, 10, 0);
            Item.createTile = TileType<BannersT>();
            Item.placeStyle = 9;
        }
    }
}