
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Consumable.Tiles.Fortress.BuildingBlocks
{
    public class FortressPillar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fortress Pillar");
            Tooltip.SetDefault("");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
        }


        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 999;

            Item.rare = 3;
            Item.createTile = TileType<FortressPillarT>();
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = 1;
            Item.consumable = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(2).AddIngredient(ItemType<FortressBrick>(), 1)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}