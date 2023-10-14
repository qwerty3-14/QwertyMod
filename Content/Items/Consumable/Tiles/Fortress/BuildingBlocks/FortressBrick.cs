using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Consumable.Tiles.Fortress.BuildingBlocks
{
    public class FortressBrick : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
        }


        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 9999;
            Item.value = 0;
            Item.rare = ItemRarityID.Orange;
            Item.createTile = ModContent.TileType<FortressBrickT>();
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ModContent.ItemType<FortressWall>(), 4)
                .AddTile(TileID.WorkBenches)
                .Register();
            CreateRecipe().AddIngredient(ModContent.ItemType<FortressPillar>(), 2)
                .AddTile(TileID.WorkBenches)
                .Register();
            CreateRecipe().AddIngredient(ModContent.ItemType<FortressPlatform>(), 2)
                .Register();
        }
    }
}