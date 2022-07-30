using QwertyMod.Content.Items.Equipment.Accessories;
using QwertyMod.Content.Items.Equipment.Accessories.Sword;
using QwertyMod.Content.Items.Weapon.Minion.MiniTank;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using QwertyMod.Content.Items.Weapon.Ranged.Gun.Charging;

namespace QwertyMod.Common
{
    public class NPCItemSales : GlobalNPC
    {
        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            if (type == NPCID.SkeletonMerchant && Main.moonPhase < 4)
            {
                shop.item[nextSlot].SetDefaults(ItemType<ArcaneArmorBreaker>());
                nextSlot++;
            }
            if (type == NPCID.ArmsDealer)
            {
                if (DownedBossSystem.downedAncient)
                {
                    shop.item[nextSlot].SetDefaults(ItemType<MiniTankStaff>());
                    nextSlot++;
                }
                if (DownedBossSystem.downedRuneGhost)
                {
                    shop.item[nextSlot].SetDefaults(ItemType<ChargingShotgun>());
                    nextSlot++;
                }
            }
        }
        public override void SetupTravelShop(int[] shop, ref int nextSlot)
        {
            if (Main.rand.Next(3) == 0)
            {
                shop[nextSlot] = ItemType<SwordEnlarger>();
                nextSlot++;
            }
        }
    }
}
