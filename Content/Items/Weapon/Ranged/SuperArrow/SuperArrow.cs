using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Ranged.SuperArrow
{
    class SuperArrow : ModPlayer
    {
        int[] shootTime = new int[58];
        public override void PostItemCheck()
        {
            if (Player.HeldItem.useAmmo == 40)
            {
                for (int i = 0; i < 58; i++)
                {
                    if (shootTime[i] > 0)
                    {
                        shootTime[i]--;
                    }
                    if ((Player.inventory[i].type == ItemType<Aqueous.Aqueous>() || Player.inventory[i].type == ItemType<BladedArrow.BladedArrow>()) && Player.inventory[i].stack > 0 && Main.LocalPlayer == Player)
                    {
                        if (shootTime[i] == 0 && Player.itemTimeMax != 0 && Player.itemTime == Player.itemTimeMax)
                        {
                            Projectile p = Main.projectile[Projectile.NewProjectile(new EntitySource_Misc(""),
                                Player.Center,
                                Player.inventory[i].shootSpeed * (QwertyMod.GetLocalCursor(Player.whoAmI) - Player.Center).RotatedByRandom(Math.PI / 32),
                                Player.inventory[i].shoot,
                                (int)(Player.inventory[i].damage * Player.GetDamage(DamageClass.Ranged).Multiplicative),
                                Player.inventory[i].knockBack,
                                Player.whoAmI)];
                            p.localAI[0] = Player.inventory[i].crit;
                            shootTime[i] = Player.inventory[i].useTime;
                        }
                    }
                }
            }
        }
    }
}
