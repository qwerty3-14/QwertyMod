using QwertyMod.Content.Items.Consumable.Tiles.Fortress.BuildingBlocks;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Consumable.Tiles.Fortress.Furniture
{
    public class FortressSink : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = 250;
            Item.createTile = ModContent.TileType<FortressSinkT>();
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ModContent.ItemType<FortressBrick>(), 6)
                .AddIngredient(ItemID.WaterBucket)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}