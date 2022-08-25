using QwertyMod.Content.Items.Tool.Mining.TheDevourer;
using QwertyMod.Content.Items.Weapon.Magic.BlackHole;
using QwertyMod.Content.Items.Weapon.Magic.Plasma;
using QwertyMod.Content.Items.Weapon.Minion.UrQuan;
using QwertyMod.Content.Items.Weapon.Ranged.Bow.B4Bow;
using QwertyMod.Content.NPCs.Bosses.OLORD;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;

namespace QwertyMod.Content.Items.Consumable.BossBag
{
    public class B4Bag : ModItem
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
            Item.width = 60;
            Item.height = 34;
            Item.rare = 9;
            Item.expert = true;
            //bossBagNPC = mod.NPCType("WeakPoint");
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void RightClick(Player player)
        {
            int mainLoot = 0;
            switch (Main.rand.Next(5))
            {
                case 0:
                    mainLoot = ItemType<BlackHoleStaff>();
                    break;
                case 1:
                    mainLoot = ItemType<ExplosivePierce>();
                    break;
                case 2:
                    mainLoot = ItemType<DreadnoughtStaff>();
                    break;
                case 3:
                    mainLoot = ItemType<B4Bow>();
                    break;
            }
            player.QuickSpawnItem(new EntitySource_Misc(""), mainLoot);

            if (Main.rand.Next(5) == 0)
            {
                player.QuickSpawnItem(new EntitySource_Misc(""), ItemType<TheDevourer>());
            }
            player.QuickSpawnItem(new EntitySource_Misc(""), ItemType<B4ExpertItem>());
        }
    }
}