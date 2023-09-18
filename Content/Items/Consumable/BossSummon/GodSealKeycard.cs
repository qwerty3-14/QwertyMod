using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Consumable.BossSummon
{
    public class GodSealKeycard : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.SortingPriorityBossSpawns[Item.type] = 13; // This helps sort inventory know this is a boss summoning Item.
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }

        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 28;
            Item.maxStack = 9999;
            Item.value = 10000;
            Item.rare = ItemRarityID.Orange;

            Item.useTurn = true;

            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
        }
    }
}