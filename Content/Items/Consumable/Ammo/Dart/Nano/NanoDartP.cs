using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Consumable.Ammo.Dart.Nano
{
    public class NanoDartP : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.width = Projectile.height = 14;
            Projectile.extraUpdates = 1;
            Projectile.friendly = true;
        }
        public override void AI()
        {
            Dust.NewDustPerfect(Projectile.Center + QwertyMethods.PolarVector(-6, Projectile.rotation + MathF.PI / 2), 135, Vector2.Zero, 100);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[Projectile.owner] = 0;
            target.AddBuff(BuffID.Confused, 60 * 10);
        }
        public override void Kill(int timeLeft)
        {
            for (int r = 0; r < 5; r++)
            {
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, QwertyMethods.PolarVector(1f, (r / 5f) * MathF.PI * 2 + Projectile.velocity.ToRotation()), ProjectileType<Nanoprobe>(), (int)(.4f * Projectile.damage), 0, Projectile.owner);
            }
        }
    }
}
