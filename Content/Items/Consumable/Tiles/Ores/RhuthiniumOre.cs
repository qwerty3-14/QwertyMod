using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace QwertyMod.Content.Items.Consumable.Tiles.Ores
{
    public class RhuthiniumOre : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Rhuthinium Ore");
            //Tooltip.SetDefault("");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 9999;
            Item.value = 100;
            Item.rare = ItemRarityID.Orange;
            Item.createTile = TileType<RhuthiniumOreT>();
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
        }
    }
}