using QwertyMod.Content.Items.Consumable.Tiles.Bars;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Consumable.Potion.CaeliteFlask
{
    public class CaeliteFlask : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flask of Caelite Wrath");
            Tooltip.SetDefault("Melee attacks inflict caelite wrath");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 20;
        }

        public override void SetDefaults()
        {
            Item.UseSound = SoundID.Item3;
            Item.useStyle = 2;
            Item.useTurn = true;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.maxStack = 30;
            Item.consumable = true;
            Item.width = 14;
            Item.height = 24;
            Item.buffType = BuffType<CaeliteImbune>();
            Item.buffTime = 72000;
            Item.value = Item.sellPrice(0, 0, 5, 0);
            Item.rare = 4;
        }

        public override bool CanUseItem(Player player)
        {
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemType<CaeliteBar>(), 1)
                .AddTile(TileID.ImbuingStation)
                .Register();
        }
    }
}