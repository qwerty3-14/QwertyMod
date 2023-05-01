using QwertyMod.Content.Items.Consumable.Tiles.Bars;
using QwertyMod.Content.Items.Consumable.Tiles.Fortress.BuildingBlocks;
using QwertyMod.Content.Items.MiscMaterials;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Consumable.Tiles.Fortress.Gadgets
{
    public class FortressTrap : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Caelite Pulse Trap");
            //Tooltip.SetDefault("Shoots 2 pulses");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 10;
        }


        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 9999;
            Item.value = 0;
            Item.rare = ItemRarityID.Orange;
            Item.createTile = TileType<FortressTrapT>();
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(2).AddIngredient(ItemType<FortressBrick>(), 2)
                .AddIngredient(ItemType<CaeliteBar>(), 1)
                .AddIngredient(ItemType<CaeliteCore>(), 1)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}