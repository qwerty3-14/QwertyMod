using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Consumable.Tiles.Fortress.BuildingBlocks
{
    public class FortressPillar : ModItem
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

            Item.rare = ItemRarityID.Orange;
            Item.createTile = ModContent.TileType<FortressPillarT>();
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(2).AddIngredient(ModContent.ItemType<FortressBrick>(), 1)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}