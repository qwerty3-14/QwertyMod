using QwertyMod.Content.Items.Consumable.Tiles.Ores;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Consumable.Tiles.Bars
{
    public class LuneBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lune Bar");
            Tooltip.SetDefault("");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 24;
            Item.maxStack = 999;
            Item.value = 20000;
            Item.rare = 1;
            Item.createTile = TileType<LuneBarT>();
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = 1;
            Item.consumable = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemType<LuneOre>(), 4)
                .AddTile(TileID.Furnaces)
                .Register();
        }
    }
}