using QwertyMod.Content.Items.Consumable.Tiles.Fortress.BuildingBlocks;
using QwertyMod.Content.Items.MiscMaterials;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Consumable.Tiles.Fortress.Gadgets
{
    public class Launchpad : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 10;
        }


        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 16;
            Item.maxStack = 9999;
            Item.value = 0;
            Item.rare = ItemRarityID.Orange;
            Item.createTile = ModContent.TileType<LaunchPadT>();
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ModContent.ItemType<FortressBrick>(), 4)
                .AddIngredient(ModContent.ItemType<CaeliteCore>(), 1)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}