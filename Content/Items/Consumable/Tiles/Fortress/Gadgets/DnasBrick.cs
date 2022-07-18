using QwertyMod.Content.Items.Consumable.Tiles.Fortress.BuildingBlocks;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Consumable.Tiles.Fortress.Gadgets
{
    public class DnasBrick : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dnas Painted Fortress Brick");
            Tooltip.SetDefault("The underside of the brick is painted with Dnas you know what will happen if you bonk your head on it...");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
        }


        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 999;
            Item.value = 0;
            Item.rare = 3;
            Item.createTile = TileType<DnasBrickT>();
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = 1;
            Item.consumable = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemType<FortressBrick>(), 1)
                .AddIngredient(ItemType<ReverseSand>(), 1)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}