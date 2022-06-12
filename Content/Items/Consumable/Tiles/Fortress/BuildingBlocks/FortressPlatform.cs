
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Consumable.Tiles.Fortress.BuildingBlocks
{
    public class FortressPlatform : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fortress Platform");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 200;
        }


        public override void SetDefaults()
        {
            Item.rare = 3;
            Item.width = 8;
            Item.height = 10;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = 1;
            Item.consumable = true;
            Item.createTile = TileType<FortressPlatformT>();
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemType<FortressBrick>(), 2)
                .Register();
        }
    }
}