using Microsoft.Xna.Framework;
using QwertyMod.Content.Buffs;
using QwertyMod.Content.Dusts;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Consumable.Ammo.Bullet.Caelite
{
    public class CaeliteBulletP : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.light = 0.5f;
            Projectile.scale = 1.2f;
            Projectile.timeLeft = 600;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.extraUpdates = 2;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }

        public override void AI()
        {
            for (int d = 0; d < 1; d++)
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<CaeliteDust>(), Vector2.Zero);
                dust.frame.Y = 0;
            }
        }

        public override void OnKill(int timeLeft)
        {
            for(int i =0; i < 30; i++)
            {
                int d = i % 6;
                if(d == 4)
                {
                    d = 2;
                }
                if(d == 5)
                {
                    d = 1;
                }
                Dust dust = Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<CaeliteDust>(), QwertyMethods.PolarVector(d * 1, Projectile.velocity.ToRotation() + 2 * MathF.PI * ((float)i / 30f)));
                dust.frame.Y = 0;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<PowerDown>(), 300);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<PowerDown>(), 300);
        }
    }
}
