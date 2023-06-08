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
using QwertyMod.Content.Items.Equipment.Vanity.PurpleDress;
using QwertyMod.Content.Items.Equipment.VanityAccessories;
using QwertyMod.Content.Items.Equipment.Vanity.Casual;

namespace QwertyMod.Common
{
    public class NPCItemSales : GlobalNPC
    {
        public override void ModifyShop(NPCShop shop)
        {
            if (shop.NpcType == NPCID.SkeletonMerchant)
            {
                shop.Add<ArcaneArmorBreaker>(Condition.MoonPhasesEven);
            }
            if (shop.NpcType == NPCID.ArmsDealer)
            {
                shop.Add<MiniTankStaff>(new Condition("downedAM", () => DownedBossSystem.downedAncient));
                shop.Add<ChargingShotgun>(new Condition("downedRunGhost", () => DownedBossSystem.downedRuneGhost));
            }
            if(shop.NpcType == NPCID.Clothier)
            {
                shop.Add<CocktailDressTop>(Condition.TimeNight);
                shop.Add<CocktailDressSkirt>(Condition.TimeNight);
                shop.Add<CasualSkirt>(Condition.TimeDay);
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
                    if(WorldGen.crimson)
                    {
                        shop[nextSlot] = ItemType<PurpleBonnet>();
                        nextSlot++;
                        shop[nextSlot] = ItemType<PurpleDress>();
                        nextSlot++;
                        shop[nextSlot] = ItemType<PurpleUmbrella>();
                        nextSlot++;
                    }
                    else
                    {
                        shop[nextSlot] = ItemType<ScarletHat>();
                        nextSlot++;
                        shop[nextSlot] = ItemType<ScarletBallGown>();
                        nextSlot++;
                        shop[nextSlot] = ItemType<ScarletFan>();
                        nextSlot++;
                    }
                    
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
        }
    }
}
