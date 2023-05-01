using Microsoft.Xna.Framework;
using QwertyMod.Common.RuneBuilder;
using QwertyMod.Content.Dusts;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.NPCs.Bosses.RuneGhost
{
    public class IceRune : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 36;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 720;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            for (int i = 0; i < 4; i++)
            {
                if (Collision.CheckAABBvAABBCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center + QwertyMethods.PolarVector(dist, Projectile.rotation + i * MathF.PI / 2f) + new Vector2(-18, -18), new Vector2(36, 36)))
                {
                    return true;
                }
            }
            return false;
        }
        float dist = 200;
        int timer;
        public override void AI()
        {
            timer++;
            if (Projectile.timeLeft > 150)
            {
                Projectile.rotation += MathF.PI / 60f;
            }
            if (Projectile.timeLeft > 120)
            {
                Projectile.Center = Main.LocalPlayer.Center;
            }
            else if (Projectile.timeLeft < 60)
            {
                dist -= 8;
            }

        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 4; i++)
            {
                Vector2 pos = Projectile.Center + QwertyMethods.PolarVector(dist, Projectile.rotation + i * MathF.PI / 2f) + new Vector2(-18, -18);
                for (int d = 0; d <= 40; d++)
                {
                    Dust.NewDust(pos, 36, 36, DustType<IceRuneDeath>());
                }
            }

        }
        public override bool PreDraw(ref Color lightColor)
        {
            for (int i = 0; i < 4; i++)
            {
                float c = (timer / 60f);
                if (c > 1f)
                {
                    c = 1f;
                }
                int frame = timer / 3;
                if (frame > 19)
                {
                    frame = 19;
                }
                Main.EntitySpriteDraw(RuneSprites.runeTransition[(int)Runes.IceRune][frame], Projectile.Center + QwertyMethods.PolarVector(dist, Projectile.rotation + i * MathF.PI / 2f) - Main.screenPosition, null, new Color(c, c, c, c), Projectile.rotation, new Vector2(9, 9), Vector2.One * 2, 0, 0);
            }

            return false;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.Frozen, 60);
        }
    }
}
