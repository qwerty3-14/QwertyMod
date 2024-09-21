using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Consumable.Ammo.Arrow.Gun
{
    public class GunArrowP : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.width = 10;
            Projectile.height = 10;
            
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.arrow = true;
            Projectile.timeLeft = 99;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
        }

        public int timer = 0;
        public int bullet = 14;
        public float speed = 8f;

        //public Item item = new item();
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            timer++;
            int weaponDamage = Projectile.damage;
            float weaponKnockback = Projectile.knockBack;
            
            Projectile.velocity *= 0.94f;
            if (Projectile.timeLeft % 20 == 0 && Projectile.timeLeft < 90 && Main.myPlayer == Projectile.owner)
            {
                if (Projectile.UseAmmo(AmmoID.Bullet, ref bullet, ref speed, ref weaponDamage, ref weaponKnockback, !Main.rand.NextBool(4)))
                {
                    Projectile b = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Projectile.velocity + QwertyMethods.PolarVector(speed, Projectile.velocity.ToRotation()), bullet, weaponDamage, weaponKnockback, Projectile.owner)];
                    b.damage = (int)(b.damage * 0.4f);
                    SoundEngine.PlaySound(SoundID.Item11, Projectile.Center);
                }

            }

        }

        public override void OnKill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
        }
        public override bool OnTileCollide(Vector2 velocityChange)
        {
            if (Projectile.velocity.X != velocityChange.X)
            {
                Projectile.velocity.X = -velocityChange.X;
            }
            if (Projectile.velocity.Y != velocityChange.Y)
            {
                Projectile.velocity.Y = -velocityChange.Y;
            }
            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return false;
        }
    }
}
