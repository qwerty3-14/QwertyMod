using QwertyMod.Content.Items.Equipment.Accessories;
using QwertyMod.Content.Items.Equipment.Accessories.Sword;
using QwertyMod.Content.Items.Weapon.Minion.MiniTank;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using QwertyMod.Content.Items.Weapon.Ranged.Gun.Charging;
using QwertyMod.Content.Items.Equipment.Armor.Hero;
using QwertyMod.Content.Items.Equipment.Vanity.CocktailDress;
using QwertyMod.Content.Items.Equipment.Vanity.ScarletBallGown;
using QwertyMod.Content.Items.Equipment.VanityAccessories;
using QwertyMod.Content.Items.Equipment.Vanity.Casual;

namespace QwertyMod.Common
{
    public class NPCItemSales : GlobalNPC
    {
        public override void ModifyShop(NPCShop shop)
        {
            
            if (shop.NpcType == NPCID.SkeletonMerchant && Main.moonPhase < 4)
            {
                shop.Add<ArcaneArmorBreaker>();
            }
            if (shop.NpcType == NPCID.ArmsDealer)
            {
                if (DownedBossSystem.downedAncient)
                {
                    shop.Add<MiniTankStaff>();
                }
                if (DownedBossSystem.downedRuneGhost)
                {
                    shop.Add<ChargingShotgun>();
                }
                if(shop.NpcType == NPCID.Clothier && !Main.dayTime)
                {
                    shop.Add<CocktailDressTop>();
                    shop.Add<CocktailDressSkirt>();
                }
                if(shop.NpcType == NPCID.Clothier && Main.dayTime)
                {
                    shop.Add<CasualSkirt>();
                }
            }
        }
        public override void SetupTravelShop(int[] shop, ref int nextSlot)
        {
            if (Main.rand.NextBool(3))
            {
                shop[nextSlot] = ItemType<SwordEnlarger>();
                nextSlot++;
            }
            switch(Main.rand.Next(8))
            {
                case 0:
                    shop[nextSlot] = ItemType<HeroPlate>();
                    nextSlot++;
                    shop[nextSlot] = ItemType<HeroPants>();
                    nextSlot++;
                    shop[nextSlot] = ItemType<HeroShield>();
                    nextSlot++;
                break;
                case 1:
                    shop[nextSlot] = ItemType<ScarletBallGown>();
                    nextSlot++;
                    shop[nextSlot] = ItemType<ScarletFan>();
                    nextSlot++;
                break;
                case 2:
                    shop[nextSlot] = ItemType<Shrug>();
                    nextSlot++;
                break;
                case 3:
                    shop[nextSlot] = ItemType<Jacket>();
                    nextSlot++;
                break;
                case 4:
                    shop[nextSlot] = ItemType<Purse>();
                    nextSlot++;
                break;
            }
            if(Main.rand.NextBool(8) && NPC.downedBoss2)
            {
                shop[nextSlot] = ItemType<HeroPlate>();
                nextSlot++;
                shop[nextSlot] = ItemType<HeroPants>();
                nextSlot++;
                shop[nextSlot] = ItemType<HeroShield>();
                nextSlot++;

            }
        }
    }
}
