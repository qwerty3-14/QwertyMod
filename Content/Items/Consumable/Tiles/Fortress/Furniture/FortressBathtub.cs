using QwertyMod.Content.Items.Consumable.Tiles.Fortress.BuildingBlocks;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertyMod.Content.Items.Consumable.Tiles.Fortress.Furniture
{
    public class FortressBathtub : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 26;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = 250;
            Item.createTile = ModContent.TileType<FortressBathtubT>();
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ModContent.ItemType<FortressBrick>(), 14)
                .AddTile(TileID.Sawmill)
                .Register();
        }
    }
}