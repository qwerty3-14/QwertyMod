
using QwertyMod.Content.Items.Consumable.Tiles.Bars;
using QwertyMod.Content.NPCs.Bosses.BladeBoss;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Consumable.BossSummon
{
    public class BladeBossSummon : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Icon of the Conqueror");
            Tooltip.SetDefault("Summons Imperious");
            ItemID.Sets.SortingPriorityBossSpawns[Item.type] = 13; // This helps sort inventory know this is a boss summoning Item.
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 22;
            Item.maxStack = 20;
            Item.rare = 3;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = 4;
            Item.UseSound = SoundID.Item44;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            if (!NPC.AnyNPCs(NPCType<Imperious>()))
            {
                NPC.SpawnOnPlayer(player.whoAmI, NPCType<Imperious>());
                SoundEngine.PlaySound(SoundID.Roar, player.position, 0);
                Item.stack--;
                return true;
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemType<RhuthiniumBar>(), 8)
                .AddIngredient(ItemID.HallowedBar, 6)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}