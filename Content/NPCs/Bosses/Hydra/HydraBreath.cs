using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Content.Dusts;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.NPCs.Bosses.Hydra
{
    public class HydraBreath : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydra Breath");
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

        private int frameCounter;
        private float trigCounter;
        private float amplitude = 10;
        private Vector2[] pseudoProjectileVelocities = new Vector2[2];

        public override void AI()
        {
            trigCounter += (float)Math.PI / 30;

            pseudoProjectileVelocities[0] = Projectile.velocity + QwertyMethods.PolarVector((float)Math.Cos(trigCounter) * amplitude, Projectile.rotation);
            pseudoProjectileVelocities[1] = Projectile.velocity + QwertyMethods.PolarVector((float)Math.Cos(trigCounter + (float)Math.PI) * amplitude, Projectile.rotation);
            Dust.NewDustPerfect(Projectile.Center + QwertyMethods.PolarVector((float)Math.Sin(trigCounter) * amplitude, Projectile.rotation), DustType<HydraBreathGlow>(), Vector2.Zero);
            Dust.NewDustPerfect(Projectile.Center + QwertyMethods.PolarVector((float)Math.Sin(trigCounter) * amplitude, Projectile.rotation - (float)Math.PI), DustType<HydraBreathGlow>(), Vector2.Zero);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            if (Math.Cos(trigCounter) > 0)
            {
                Main.EntitySpriteDraw(texture, (Projectile.Center + QwertyMethods.PolarVector((float)Math.Sin(trigCounter) * amplitude, Projectile.rotation)) - Main.screenPosition,
                            texture.Frame(), Color.White, pseudoProjectileVelocities[0].ToRotation() + (float)Math.PI / 2,
                            texture.Size() / 2f, 1f, SpriteEffects.None, 0);
                Main.EntitySpriteDraw(texture, (Projectile.Center + QwertyMethods.PolarVector((float)Math.Sin(trigCounter + (float)Math.PI) * amplitude, Projectile.rotation)) - Main.screenPosition,
                                texture.Frame(), Color.White, pseudoProjectileVelocities[1].ToRotation() + (float)Math.PI / 2,
                                texture.Size() / 2f, 1f, SpriteEffects.None, 0);
            }
            else
            {
                Main.EntitySpriteDraw(texture, (Projectile.Center + QwertyMethods.PolarVector((float)Math.Sin(trigCounter + (float)Math.PI) * amplitude, Projectile.rotation)) - Main.screenPosition,
                                texture.Frame(), Color.White, pseudoProjectileVelocities[1].ToRotation() + (float)Math.PI / 2,
                                texture.Size() / 2f, 1f, SpriteEffects.None, 0);
                Main.EntitySpriteDraw(texture, (Projectile.Center + QwertyMethods.PolarVector((float)Math.Sin(trigCounter) * amplitude, Projectile.rotation)) - Main.screenPosition,
                            texture.Frame(), Color.White, pseudoProjectileVelocities[0].ToRotation() + (float)Math.PI / 2,
                            texture.Size() / 2f, 1f, SpriteEffects.None, 0);
            }

            return false;
        }
    }
}