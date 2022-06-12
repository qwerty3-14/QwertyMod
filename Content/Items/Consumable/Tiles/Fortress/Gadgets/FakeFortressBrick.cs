using QwertyMod.Content.Items.Consumable.Tiles.Fortress.BuildingBlocks;
using QwertyMod.Content.Items.MiscMaterials;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
namespace QwertyMod.Content.Items.Consumable.Tiles.Fortress.Gadgets
{
    public class FakeFortressBrick : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Enchanted Fortress Brick");
            Tooltip.SetDefault("Comes alive when broken or powered!");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
        }


        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 999;
            Item.value = 0;
            Item.rare = 3;
            Item.createTile = TileType<FakeFortressBrickT>();
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = 1;
            Item.consumable = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(16).AddIngredient(ItemType<FortressBrick>(), 16)
                .AddIngredient(ItemType<CaeliteCore>())
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}