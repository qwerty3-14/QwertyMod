
using QwertyMod.Content.Items.Equipment.Accessories.Expert.Doppleganger;
using QwertyMod.Content.Items.MiscMaterials;
using QwertyMod.Content.NPCs.Bosses.CloakedDarkBoss;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Consumable.BossBag
{
    public class NoehtnapBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Treasure Bag");
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }


        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.consumable = true;
            Item.width = 48;
            Item.height = 32;
            Item.rare = 9;
            Item.expert = true;
            //bossBagNPC = mod.NPCType("CloakedDarkBoss");
        }

        public override int BossBagNPC => NPCType<CloakedDarkBoss>();

        public override bool CanRightClick()
        {
            return true;
        }

        public override void OpenBossBag(Player player)
        {
            player.QuickSpawnItem(73, 8);
            //player.QuickSpawnItem(mod.ItemType("Doppleganger"));

            int number = Item.NewItem((int)player.position.X, (int)player.position.Y, player.width, player.height, ItemType<Doppleganger>(), 1, false, 0, false, false);
            if (Main.netMode == 1)
            {
                NetMessage.SendData(21, -1, -1, null, number, 1f, 0f, 0f, 0, 0, 0);
            }

            player.QuickSpawnItem(ItemType<Etims>(), 20 + Main.rand.Next(17));
        }
    }
}