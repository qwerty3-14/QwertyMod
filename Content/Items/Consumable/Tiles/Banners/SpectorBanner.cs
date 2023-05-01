using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
namespace QwertyMod.Content.Items.Consumable.Tiles.Banners
{
    public class SpectorBanner : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Sneaking Ghost Banner");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
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
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(0, 0, 10, 0);
            Item.createTile = TileType<BannersT>();
            Item.placeStyle = 5;
        }
    }
}