using System;
using Terraria;
using Terraria.ModLoader;


namespace QwertyMod.Content.Items.Equipment.Accessories.SuperArrow
{
    public class SuperArrow : ModPlayer
    {
        public int arrowType = -1;
        public override void ResetEffects()
        {
            arrowType = -1;
        }
        int[] shootTime = new int[10];
        public override void PostItemCheck()
        {
            if (Player.HeldItem.useAmmo == 40)
            {
                
                for (int i = 0; i < 10; i++)
                {
                    if (shootTime[i] > 0)
                    {
                        shootTime[i]--;
                    }
                    if ((Player.armor[i].type == ModContent.ItemType<Aqueous.Aqueous>() || Player.armor[i].type == ModContent.ItemType<BladedArrow.BladedArrow>()) && Player.armor[i].stack > 0 && Main.LocalPlayer == Player)
                    {
                        if (shootTime[i] == 0 && Player.itemTimeMax != 0 && Player.itemTime == Player.itemTimeMax)
                        {
                            Projectile p = Main.projectile[Projectile.NewProjectile(Player.GetSource_Accessory(Player.armor[i]),
                                Player.Center,
                                Player.armor[i].shootSpeed * (QwertyMod.GetLocalCursor(Player.whoAmI) - Player.Center).RotatedByRandom(Math.PI / 32),
                                Player.armor[i].shoot,
                                (int)(Player.armor[i].damage * Player.GetDamage(DamageClass.Ranged).Multiplicative),
                                Player.armor[i].knockBack,
                                Player.whoAmI)];
                            p.localAI[0] = Player.armor[i].crit;
                            shootTime[i] = Player.armor[i].useTime;
                        }
                    }
                }
            }
        }
    }
}
