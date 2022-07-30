using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common.PlayerLayers;
using QwertyMod.Content.Items.Tool.Mining.Ancient;
using QwertyMod.Content.NPCs.Bosses.AncientMachine;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using QwertyMod.Content.Items.Equipment.Accessories.Expert.AncientGemstone;

namespace QwertyMod.Content.Items.Consumable.BossBag
{
    public class AncientMachineBag : ModItem
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
            Item.width = 48;
            Item.height = 32;
            Item.rare = 9;
            Item.expert = true;

            if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("QwertyMod/Content/Items/Consumable/BossBag/AncientMachineBag_Glow").Value;
            }
        }

        public override int BossBagNPC => NPCType<AncientMachine>();

        public override bool CanRightClick()
        {
            return true;
        }

        public override void OpenBossBag(Player player)
        {
            int[] loot = QwertyMod.AMLoot.Draw(3);

            foreach (int item in loot)
            {
                player.QuickSpawnItem(new EntitySource_Misc(""), item);
            }

            if (Main.rand.Next(100) < 20)
            {
                player.QuickSpawnItem(new EntitySource_Misc(""), ItemType<AncientMiner>());
            }
            player.QuickSpawnItem(new EntitySource_Misc(""), 73, 8);
            player.QuickSpawnItem(new EntitySource_Misc(""), ModContent.ItemType<AncientGemstone>());
        }
    }
}