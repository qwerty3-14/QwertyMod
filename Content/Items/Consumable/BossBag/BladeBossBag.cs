using QwertyMod.Content.Items.Equipment.Accessories.Expert.Sheath;
using QwertyMod.Content.Items.Equipment.Accessories.Sword;
using QwertyMod.Content.NPCs.Bosses.BladeBoss;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;

namespace QwertyMod.Content.Items.Consumable.BossBag
{
    public class BladeBossBag : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Treasure Bag");
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
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


        public override bool CanRightClick()
        {
            return true;
        }

        public override void RightClick(Player player)
        {
            int[] spawnThese = QwertyMod.ImperiousLoot.Draw(3);
            player.QuickSpawnItem(new EntitySource_Misc(""), (spawnThese[0]));
            player.QuickSpawnItem(new EntitySource_Misc(""), (spawnThese[1]));
            player.QuickSpawnItem(new EntitySource_Misc(""), (spawnThese[2]));

            player.QuickSpawnItem(new EntitySource_Misc(""), 73, 15);
            if (Main.rand.Next(4) == 0)
            {
                player.QuickSpawnItem(new EntitySource_Misc(""), ItemType<SwordsmanBadge>());
            }
            player.QuickSpawnItem(new EntitySource_Misc(""), ItemType<ImperiousSheath>());
        }
    }
}