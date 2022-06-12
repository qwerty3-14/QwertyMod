using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace QwertyMod.Content.NPCs.Bosses.BladeBoss
{
    public class PhantomBlade : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Phantom Blade");
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 10;
            Projectile.tileCollide = false;
            Projectile.hostile = true;
        }

        private int totalLength = 398;
        private int bladeLength = 308;
        private int bladeWidth = 82;
        private int a = 80;

        public override bool PreDraw(ref Color drawColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(a, a, a, a), Projectile.rotation, new Vector2(18, texture.Height / 2f), new Vector2(1f, 1f), SpriteEffects.None, 0);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition + new Vector2(-10 + Main.rand.Next(21), -10 + Main.rand.Next(21)), null, new Color(a, a, a, a), Projectile.rotation, new Vector2(18, texture.Height / 2f), new Vector2(1f, 1f), SpriteEffects.None, 0);
            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float CP = 0;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center + QwertyMethods.PolarVector(-18 + totalLength - bladeLength, Projectile.rotation), Projectile.Center + QwertyMethods.PolarVector(-18 + totalLength, Projectile.rotation), bladeWidth, ref CP);
        }

        public override void Kill(int timeLeft)
        {
            for (int d = 0; d < 100; d++)
            {
                int lengthOffset = -18 + Main.rand.Next(bladeLength);
                int widthOffset = +Main.rand.Next(bladeWidth) - bladeWidth / 2;
                Dust dust = Dust.NewDustPerfect(Projectile.Center + QwertyMethods.PolarVector(lengthOffset, Projectile.rotation) + QwertyMethods.PolarVector(widthOffset, Projectile.width + (float)Math.PI / 2), 15);
                dust.noGravity = true;
            }
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.ai[0];
        }
    }
}