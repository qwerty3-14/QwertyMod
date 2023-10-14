using QwertyMod.Content.Items.Consumable.Tiles.Bars;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertyMod.Content.Items.Consumable.Potion.CaeliteFlask
{
    public class CaeliteFlask : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 20;
        }

        public override void SetDefaults()
        {
            Item.UseSound = SoundID.Item3;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useTurn = true;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.width = 22;
            Item.height = 34;
            Item.buffType = ModContent.BuffType<CaeliteImbune>();
            Item.buffTime = 72000;
            Item.value = Item.sellPrice(0, 0, 5, 0);
            Item.rare = ItemRarityID.LightRed;
        }

        public override bool CanUseItem(Player player)
        {
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ModContent.ItemType<CaeliteBar>(), 1)
                .AddTile(TileID.ImbuingStation)
                .Register();
        }
    }
}