using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Consumable.Tiles.Ores
{
    public class LuneOre : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lune Ore");
            Tooltip.SetDefault("Found high in the sky");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 999;
            Item.value = 100;
            Item.rare = 1;
            Item.createTile = TileType<LuneOreT>();
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = 1;
            Item.consumable = true;
        }
    }
}