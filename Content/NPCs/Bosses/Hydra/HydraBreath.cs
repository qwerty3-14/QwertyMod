using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Content.Dusts;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertyMod.Content.NPCs.Bosses.Hydra
{
    public class HydraBreath : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 240;
            Projectile.tileCollide = false;
            Projectile.light = 1f;
        }
        private float trigCounter;
        private float amplitude = 10;
        private Vector2[] pseudoProjectileVelocities = new Vector2[2];

        public override void AI()
        {
            trigCounter += MathF.PI / 30;

            pseudoProjectileVelocities[0] = Projectile.velocity + QwertyMethods.PolarVector(MathF.Cos(trigCounter) * amplitude, Projectile.rotation);
            pseudoProjectileVelocities[1] = Projectile.velocity + QwertyMethods.PolarVector(MathF.Cos(trigCounter + MathF.PI) * amplitude, Projectile.rotation);
            Dust.NewDustPerfect(Projectile.Center + QwertyMethods.PolarVector(MathF.Sin(trigCounter) * amplitude, Projectile.rotation), ModContent.DustType<HydraBreathGlow>(), Vector2.Zero);
            Dust.NewDustPerfect(Projectile.Center + QwertyMethods.PolarVector(MathF.Sin(trigCounter) * amplitude, Projectile.rotation - MathF.PI), ModContent.DustType<HydraBreathGlow>(), Vector2.Zero);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            if (Math.Cos(trigCounter) > 0)
            {
                Main.EntitySpriteDraw(texture, (Projectile.Center + QwertyMethods.PolarVector(MathF.Sin(trigCounter) * amplitude, Projectile.rotation)) - Main.screenPosition,
                            texture.Frame(), Color.White, pseudoProjectileVelocities[0].ToRotation() + MathF.PI / 2,
                            texture.Size() / 2f, 1f, SpriteEffects.None, 0);
                Main.EntitySpriteDraw(texture, (Projectile.Center + QwertyMethods.PolarVector(MathF.Sin(trigCounter + MathF.PI) * amplitude, Projectile.rotation)) - Main.screenPosition,
                                texture.Frame(), Color.White, pseudoProjectileVelocities[1].ToRotation() + MathF.PI / 2,
                                texture.Size() / 2f, 1f, SpriteEffects.None, 0);
            }
            else
            {
                Main.EntitySpriteDraw(texture, (Projectile.Center + QwertyMethods.PolarVector(MathF.Sin(trigCounter + MathF.PI) * amplitude, Projectile.rotation)) - Main.screenPosition,
                                texture.Frame(), Color.White, pseudoProjectileVelocities[1].ToRotation() + MathF.PI / 2,
                                texture.Size() / 2f, 1f, SpriteEffects.None, 0);
                Main.EntitySpriteDraw(texture, (Projectile.Center + QwertyMethods.PolarVector(MathF.Sin(trigCounter) * amplitude, Projectile.rotation)) - Main.screenPosition,
                            texture.Frame(), Color.White, pseudoProjectileVelocities[0].ToRotation() + MathF.PI / 2,
                            texture.Size() / 2f, 1f, SpriteEffects.None, 0);
            }

            return false;
        }
    }
}