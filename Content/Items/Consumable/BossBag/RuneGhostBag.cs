using QwertyMod.Content.Items.Equipment.Accessories.Expert.HyperRunestone;
using QwertyMod.Content.Items.Equipment.Accessories.RuneScrolls;
using QwertyMod.Content.Items.Equipment.Vanity.BossMasks;
using QwertyMod.Content.Items.Equipment.Vanity.RunicRobe;
using QwertyMod.Content.Items.MiscMaterials;
using QwertyMod.Content.NPCs.Bosses.RuneGhost;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Consumable.BossBag
{
    public class RuneGhostBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Treasure Bag");
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(10, 36));
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.consumable = true;
            Item.width = 32;
            Item.height = 32;
            Item.rare = 9;
            Item.expert = true;
            //bossBagNPC = mod.NPCType("RuneSpector");
        }


        public override bool CanRightClick()
        {
            return true;
        }

        public override void RightClick(Player player)
        {
            int runeCount = Main.rand.Next(30, 41);
            int selectScroll = Main.rand.Next(1, 5);
            if (Main.rand.Next(7) == 0)
            {
                player.QuickSpawnItem(new EntitySource_Misc(""), ItemType<RuneGhostMask>());
            }
            
            if (Main.rand.Next(7) == 0)
            {
                player.QuickSpawnItem(new EntitySource_Misc(""), ItemType<RunicRobe>());
            }


            if (selectScroll == 1)
            {
                player.QuickSpawnItem(new EntitySource_Misc(""), ItemType<IceScroll>());
            }
            if (selectScroll == 2)
            {
                player.QuickSpawnItem(new EntitySource_Misc(""), ItemType<PursuitScroll>());
            }
            if (selectScroll == 3)
            {
                player.QuickSpawnItem(new EntitySource_Misc(""), ItemType<LeechScroll>());
            }
            if (selectScroll == 4)
            {
                player.QuickSpawnItem(new EntitySource_Misc(""), ItemType<AggroScroll>());
            }

            player.QuickSpawnItem(new EntitySource_Misc(""), ItemType<HyperRunestone>());
            player.QuickSpawnItem(new EntitySource_Misc(""), 73, 35);

            player.QuickSpawnItem(new EntitySource_Misc(""), ItemType<CraftingRune>(), runeCount);
        }
    }
}