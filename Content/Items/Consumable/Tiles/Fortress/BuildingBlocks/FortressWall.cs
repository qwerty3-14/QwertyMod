using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Consumable.Tiles.Fortress.BuildingBlocks
{
    public class FortressWall : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {
            Item.rare = 3;
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 7;
            Item.useStyle = 1;
            Item.consumable = true;
            Item.createWall = WallType<FortressWallT>();
        }

        public override void AddRecipes()
        {
            CreateRecipe(4).AddIngredient(ItemType<FortressBrick>(), 1)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}