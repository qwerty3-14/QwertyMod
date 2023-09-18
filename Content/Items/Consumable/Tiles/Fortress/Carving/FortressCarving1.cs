using QwertyMod.Content.Items.Consumable.Tiles.Fortress.BuildingBlocks;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Consumable.Tiles.Fortress.Carving
{
    public class FortressCarving1 : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }


        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 32;
            Item.maxStack = 9999;
            Item.value = 0;
            Item.rare = ItemRarityID.Orange;
            Item.createTile = TileType<FortressCarving1T>();
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemType<FortressBrick>(), 10)
                .AddTile(TileID.HeavyWorkBench)
                .Register();
        }
    }
}