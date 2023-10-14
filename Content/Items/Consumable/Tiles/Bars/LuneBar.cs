using QwertyMod.Content.Items.Consumable.Tiles.Ores;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertyMod.Content.Items.Consumable.Tiles.Bars
{
    public class LuneBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.value = 20000;
            Item.rare = ItemRarityID.Blue;
            Item.createTile = ModContent.TileType<LuneBarT>();
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<LuneOre>(), 3)
                .AddTile(TileID.Furnaces)
                .Register();
        }
    }
}