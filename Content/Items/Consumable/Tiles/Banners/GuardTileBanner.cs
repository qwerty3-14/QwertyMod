using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
namespace QwertyMod.Content.Items.Consumable.Tiles.Banners
{
    public class GuardTileBanner : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Guard Tile Banner");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 28;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(0, 0, 10, 0);
            Item.createTile = TileType<BannersT>();
            Item.placeStyle = 2;
        }
    }
}