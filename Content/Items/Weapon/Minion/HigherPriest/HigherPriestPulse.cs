using QwertyMod.Content.Buffs;
using QwertyMod.Content.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using Microsoft.Xna.Framework;

namespace QwertyMod.Content.Items.Weapon.Minion.HigherPriest
{
    public class HigherPriestPulse : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Preist Pulse");
            Main.projFrames[Projectile.type] = 2;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Summon;
            Projectile.friendly = true;
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 180;
            Projectile.extraUpdates = 1;
            Projectile.light = 1f;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }
        bool exploded = false;
        void explode()
        {
            if (!exploded)
            {
                exploded = true;
                Projectile.timeLeft = 5;
                Projectile.position -= Vector2.One * ((120 - 30) / 2);
                Projectile.width = 120;
                Projectile.height = 120;
                Projectile.tileCollide = false;
                Projectile.velocity = Vector2.Zero;
            }
        }
        public override void AI()
        {
            Projectile.frameCounter++;
            Projectile.frame = Projectile.frameCounter % 40 > 20 ? 1 : 0;
            if (!exploded)
            {
                if (Projectile.timeLeft < 5)
                {
                    explode();
                }
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            explode();
            if (Main.rand.NextBool(10))
            {
                target.AddBuff(BuffType<PowerDown>(), 120);
            }
        }
        public override void OnKill(int timeLeft)
        {
            int spokeLengths = 18;
            for(int i =0; i < spokeLengths * 5; i++)
            {
                int d = i % spokeLengths;
                if(d > spokeLengths / 2)
                {
                    d = spokeLengths - d;
                }
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustType<CaeliteDust>(), QwertyMethods.PolarVector(d * 1, Projectile.velocity.ToRotation() + 2 * MathF.PI * ((float)i / (float)(spokeLengths * 5))));
                dust.frame.Y = 0;
                //dust.velocity *= 2;
            }
        }
    }
}
