using QwertyMod.Content.Items.Equipment.Accessories;
using QwertyMod.Content.Items.Equipment.Accessories.Sword;
using QwertyMod.Content.Items.Weapon.Minion.MiniTank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

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
            }
        }
        public override void SetupTravelShop(int[] shop, ref int nextSlot)
        {
            if(Main.rand.Next(3)==0)
            {
                shop[nextSlot] = ItemType<SwordEnlarger>();
                nextSlot++;
            }
        }
    }
}
