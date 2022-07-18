using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common.RuneBuilder;
using System;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.NPCs.Bosses.RuneGhost
{
    public class AggroRune : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 62;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 720;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return false;
        }
        int timer;
        bool runOnce = true;
        Vector2 middle;
        public override void AI()
        {
            timer++;
            if (runOnce)
            {
                middle = Projectile.Center;
                runOnce = false;
                Projectile.position += QwertyMethods.PolarVector(200, Projectile.rotation);
            }
            if (timer % 120 == 29)
            {
                Projectile.velocity = Vector2.Zero;
                if (Main.netMode != 1)
                {
                    Projectile.netUpdate = true;
                }
            }
            if (timer % 120 == 90 && Main.netMode != 1)
            {
                Projectile.NewProjectile(new EntitySource_Misc(""), middle, QwertyMethods.PolarVector(1, Projectile.rotation), ProjectileType<AggroStrike>(), Projectile.damage, 0);
            }
            if (timer % 120 == 119)
            {
                if (Main.netMode != 1)
                {
                    Vector2 goTo = middle + QwertyMethods.PolarVector(200, Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI));
                    Projectile.velocity = (goTo - Projectile.Center) / 30f;
                    Projectile.netUpdate = true;
                }
            }
            Projectile.rotation = (Projectile.Center - middle).ToRotation();
        }
        public override bool PreDraw(ref Color lightColor)
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
            Main.EntitySpriteDraw(RuneSprites.runeTransition[(int)Runes.Aggro][frame], Projectile.Center - Main.screenPosition, null, new Color(c, c, c, c), Projectile.rotation, new Vector2(15.5f, 15.5f), Vector2.One * 2, 0, 0);
            return false;
        }
        public override void PostDraw(Color lightColor)
        {
            if (timer % 120 > 30 && timer % 120 < 90 && middle != null)
            {
                Texture2D texture = Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/RuneGhost/AggroLaser").Value;
                Main.EntitySpriteDraw(texture, middle - Main.screenPosition, null, Color.White, Projectile.rotation, Vector2.UnitY, new Vector2(1500, 1), 0, 0);
            }
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(Projectile.velocity);
            writer.WriteVector2(Projectile.position);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {

            Projectile.velocity = reader.ReadVector2();
            Projectile.position = reader.ReadVector2();
        }
    }
    public class AggroStrike : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 2;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 30;
        }
        bool runOnce = true;
        int timer;
        public override void AI()
        {
            timer++;
            if (runOnce)
            {
                runOnce = false;
                Projectile.rotation = Projectile.velocity.ToRotation();
                Projectile.velocity = Vector2.Zero;
            }

        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (timer < 5)
            {
                return false;
            }
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + QwertyMethods.PolarVector(1000, Projectile.rotation));
        }
        public override bool PreDraw(ref Color lightColor)
        {
            int frame = timer / 2;
            if (timer > 22)
            {
                frame = (30 - timer) / 2;
            }
            if (frame > 3)
            {
                frame = 3;
            }
            float c = (float)frame / 3f;
            for (int i = 0; i < 3000; i += 8)
            {
                Main.EntitySpriteDraw(RuneSprites.aggroStrike[frame], Projectile.Center + QwertyMethods.PolarVector(i, Projectile.rotation) - Main.screenPosition, null, new Color(c, c, c, c), Projectile.rotation, new Vector2(0, 3), Vector2.One * 2, 0, 0);
            }

            return false;
        }
    }
}
