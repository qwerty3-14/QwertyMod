using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Consumable.Ammo.Arrow.Gun
{
    public class GunArrowP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gun Arrow");
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.arrow = true;
            Projectile.timeLeft = 60 * 5;
            Projectile.tileCollide = true;
        }

        public int timer = 0;
        public int bullet = 14;
        public float speed = 14f;

        //public Item item = new item();
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            timer++;
            int weaponDamage = Projectile.damage;
            float weaponKnockback = Projectile.knockBack;

            if (Projectile.timeLeft == (60 * 5 - 10) || Projectile.timeLeft == (60 * 5 - 60))
            {
                if (Projectile.UseAmmo(AmmoID.Bullet, ref bullet, ref speed, ref weaponDamage, ref weaponKnockback, false))
                {
                    Projectile b = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Projectile.velocity + QwertyMethods.PolarVector(speed, Projectile.velocity.ToRotation()), bullet, weaponDamage, weaponKnockback, Main.myPlayer)];
                    SoundEngine.PlaySound(SoundID.Item11, Projectile.Center);
                }

            }

        }

        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
        }
    }
}
