using QwertyMod.Content.Items.Equipment.Accessories;
using QwertyMod.Content.Items.Equipment.Accessories.Sword;
using QwertyMod.Content.Items.Equipment.Armor.Hero;
using QwertyMod.Content.Items.Equipment.Vanity.Casual;
using QwertyMod.Content.Items.Equipment.Vanity.CocktailDress;
using QwertyMod.Content.Items.Equipment.Vanity.PurpleDress;
using QwertyMod.Content.Items.Equipment.Vanity.ScarletBallGown;
using QwertyMod.Content.Items.Equipment.VanityAccessories;
using QwertyMod.Content.Items.Weapon.Minion.MiniTank;
using QwertyMod.Content.Items.Weapon.Ranged.Gun.Charging;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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
                shop[nextSlot] = ModContent.ItemType<SwordEnlarger>();
                nextSlot++;
            }
            switch(Main.rand.Next(8))
            {
                case 0:
                    shop[nextSlot] = ModContent.ItemType<HeroPlate>();
                    nextSlot++;
                    shop[nextSlot] = ModContent.ItemType<HeroPants>();
                    nextSlot++;
                    shop[nextSlot] = ModContent.ItemType<HeroShield>();
                    nextSlot++;
                break;
                case 1:
                    if(WorldGen.crimson)
                    {
                        shop[nextSlot] = ModContent.ItemType<PurpleBonnet>();
                        nextSlot++;
                        shop[nextSlot] = ModContent.ItemType<PurpleDress>();
                        nextSlot++;
                        shop[nextSlot] = ModContent.ItemType<PurpleUmbrella>();
                        nextSlot++;
                    }
                    else
                    {
                        shop[nextSlot] = ModContent.ItemType<ScarletHat>();
                        nextSlot++;
                        shop[nextSlot] = ModContent.ItemType<ScarletBallGown>();
                        nextSlot++;
                        shop[nextSlot] = ModContent.ItemType<ScarletFan>();
                        nextSlot++;
                    }
                    
                break;
                case 2:
                    shop[nextSlot] = ModContent.ItemType<Shrug>();
                    nextSlot++;
                break;
                case 3:
                    shop[nextSlot] = ModContent.ItemType<Jacket>();
                    nextSlot++;
                break;
                case 4:
                    shop[nextSlot] = ModContent.ItemType<Purse>();
                    nextSlot++;
                break;
            }
        }
    }
}
