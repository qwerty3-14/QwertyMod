using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.ID;

namespace QwertyMod.Content.Items.Consumable.Tiles.Trophy.FortressBoss
{
    public class FortressBossTrophy : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = 50000;
            Item.createTile = ModContent.TileType<FortressBossTrophyT>();
            Item.placeStyle = 0;
        }

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
    }
}