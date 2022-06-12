using QwertyMod.Content.Items.Consumable.Tiles.Fortress.BuildingBlocks;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

using static Terraria.ModLoader.ModContent;
namespace QwertyMod.Content.Items.Consumable.Tiles.Fortress.Furniture
{
    public class FortressPiano : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 24;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = 1;
            Item.consumable = true;
            Item.value = 250;
            Item.createTile = TileType<FortressPianoT>();
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemType<FortressBrick>(), 15)
                .AddIngredient(ItemID.Bone, 4)
                .AddIngredient(ItemID.Book, 1)
                .AddTile(TileID.Sawmill)
                .Register();
        }
    }
}