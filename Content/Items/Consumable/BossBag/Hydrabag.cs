using QwertyMod.Content.Items.MiscMaterials;
using QwertyMod.Content.Items.Tool.FishingRod;
using QwertyMod.Content.NPCs.Bosses.Hydra;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Consumable.BossBag
{
    public class HydraBag : ModItem
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
            Item.width = 76;
            Item.height = 35;
            Item.rare = 9;
            Item.expert = true;
            //bossBagNPC = mod.NPCType("Hydra");
        }

        public override int BossBagNPC => NPCType<Hydra>();

        public override bool CanRightClick()
        {
            return true;
        }

        public override void OpenBossBag(Player player)
        {
            int getHook = Main.rand.Next(0, 100);

            int[] spawnThese = QwertyMod.HydraLoot.Draw(3);
            player.QuickSpawnItem(new EntitySource_Misc(""), spawnThese[0]);
            player.QuickSpawnItem(new EntitySource_Misc(""), spawnThese[1]);
            player.QuickSpawnItem(new EntitySource_Misc(""), spawnThese[2]);

            if (Main.rand.Next(5) == 0)
            {
                player.QuickSpawnItem(new EntitySource_Misc(""), ItemType<Hydrator>());
            }

            player.QuickSpawnItem(new EntitySource_Misc(""), 73, 12);
            //player.QuickSpawnItem(new EntitySource_Misc(""), mod.ItemType("HydraWings"));
            player.QuickSpawnItem(new EntitySource_Misc(""), ItemType<HydraScale>(), Main.rand.Next(30, 41));
        }
    }
}