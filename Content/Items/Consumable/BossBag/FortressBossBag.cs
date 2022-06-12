using QwertyMod.Content.Items.Consumable.Tiles.Bars;
using QwertyMod.Content.Items.Equipment.Accessories.Sword;
using QwertyMod.Content.Items.Equipment.Vanity.BossMasks;
using QwertyMod.Content.Items.MiscMaterials;
using QwertyMod.Content.Items.Pet;
using QwertyMod.Content.Items.Weapon.Magic.RestlessSun;
using QwertyMod.Content.Items.Weapon.Melee.Misc;
using QwertyMod.Content.Items.Weapon.Minion.Priest;
using QwertyMod.Content.Items.Weapon.Ranged.Bow.HolyExiler;
using QwertyMod.Content.NPCs.Bosses.FortressBoss;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Consumable.BossBag
{
    public class FortressBossBag : ModItem
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
            //bossBagNPC = mod.NPCType("FortressBoss");
        }

        public override int BossBagNPC => NPCType<FortressBoss>();

        public override bool CanRightClick()
        {
            return true;
        }

        public override void OpenBossBag(Player player)
        {
            player.QuickSpawnItem(new EntitySource_Misc(""), ItemType<CaeliteBar>(), Main.rand.Next(18, 31));
            player.QuickSpawnItem(new EntitySource_Misc(""), ItemType<CaeliteCore>(), Main.rand.Next(9, 16));
            player.QuickSpawnItem(new EntitySource_Misc(""), ItemType<ExpertChalice>());
            if (Main.rand.Next(7) == 0)
                player.QuickSpawnItem(new EntitySource_Misc(""), ItemType<DivineLightMask>());
            player.QuickSpawnItem(new EntitySource_Misc(""), 73, 10);
            if (Main.rand.Next(5) == 0)
            {
                player.QuickSpawnItem(new EntitySource_Misc(""), ItemType<Lightling>());
            }
            if (Main.rand.Next(5) == 0)
            {
                player.QuickSpawnItem(new EntitySource_Misc(""), ItemType<SkywardHilt>());
            }
            switch (Main.rand.Next(4))
            {
                case 0:
                    player.QuickSpawnItem(new EntitySource_Misc(""), ItemType<CaeliteMagicWeapon>());
                    break;
                case 1:
                    player.QuickSpawnItem(new EntitySource_Misc(""), ItemType<HolyExiler>());
                    break;
                case 2:
                    player.QuickSpawnItem(new EntitySource_Misc(""), ItemType<CaeliteRainKnife>());
                    break;
                case 3:
                    player.QuickSpawnItem(new EntitySource_Misc(""), ItemType<PriestStaff>());
                    break;
            }
        }
    }
}