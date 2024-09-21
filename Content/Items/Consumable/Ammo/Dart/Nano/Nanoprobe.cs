using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Consumable.Ammo.Dart.Nano
{
    public class Nanoprobe : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.width = Projectile.height = 10;
            Projectile.extraUpdates = 5;
            Projectile.friendly = true;
            Projectile.timeLeft = 1200;
        }
        float dir;
        bool runOnce = true;
        int counter = 0;
        NPC target;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (counter < 40)
            {
                return false;
            }
            return base.Colliding(projHitbox, targetHitbox);
        }
        public override void AI()
        {
            if (runOnce)
            {
                dir = Projectile.velocity.ToRotation();
                Projectile.velocity = QwertyMethods.PolarVector(4.5f, dir);
                runOnce = false;
            }
            counter++;
            if (counter > 40)
            {
                if (QwertyMethods.ClosestNPC(ref target, 1000, Projectile.Center))
                {
                    dir = Projectile.velocity.ToRotation();
                    dir.SlowRotation((target.Center - Projectile.Center).ToRotation(), MathF.PI / 60f);
                    Projectile.velocity = QwertyMethods.PolarVector(4.5f, dir);
                }
            }
            Dust d = Dust.NewDustPerfect(Projectile.Center, 135, Vector2.Zero, 100);
            d.noGravity = true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Confused, 60 * 10);
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
    }
}
