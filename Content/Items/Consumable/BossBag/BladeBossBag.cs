using QwertyMod.Content.Items.Equipment.Accessories.Expert.Sheath;
using QwertyMod.Content.Items.Equipment.Accessories.Sword;
using QwertyMod.Content.NPCs.Bosses.BladeBoss;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Consumable.BossBag
{
    public class BladeBossBag : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Treasure Bag");
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.consumable = true;
            Item.width = 36;
            Item.height = 34;
            Item.rare = 9;
            Item.expert = true;
            //bossBagNPC = mod.NPCType("BladeBoss");
        }

        public override int BossBagNPC => NPCType<Imperious>();

        public override bool CanRightClick()
        {
            return true;
        }

        public override void OpenBossBag(Player player)
        {
            int[] spawnThese = QwertyMod.ImperiousLoot.Draw(3);
            player.QuickSpawnItem((spawnThese[0]));
            player.QuickSpawnItem((spawnThese[1]));
            player.QuickSpawnItem((spawnThese[2]));

            player.QuickSpawnItem(73, 15);
            if(Main.rand.Next(4) == 0)
            {
                player.QuickSpawnItem(ItemType<SwordsmanBadge>());
            }
            player.QuickSpawnItem(ItemType<ImperiousSheath>());
        }
    }
}