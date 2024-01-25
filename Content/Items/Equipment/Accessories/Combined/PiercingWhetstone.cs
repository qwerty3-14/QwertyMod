using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertyMod.Content.Items.Equipment.Accessories.Combined
{
    class PiercingWhetstone : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.value = 10000;
            Item.rare = ItemRarityID.Blue;
            Item.width = 24;
            Item.height = 24;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<WhetStoneEffect>().effect += .2f;
            player.GetModPlayer<WhetStoneEffect>().AP += 12;
            player.GetModPlayer<FieryWhetStoneEffect>().AP += 12;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ModContent.ItemType<EnchantedWhetstone>(), 1)
                .AddIngredient(ModContent.ItemType<ArcaneArmorBreaker>(), 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}