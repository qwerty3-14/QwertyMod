using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Content.Items.Weapon.Ranged.Gun.SoEF;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.NPCs.Bosses.CloakedDarkBoss
{
    public class EtimsicCannon : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Heaven Raider Cannon");
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 34;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
        }

        private int shootTimer = 0;
        private int laserLength = 2000;

        public override void AI()
        {
            Projectile.rotation = Projectile.ai[0];
            shootTimer++;
            if (shootTimer == 180)
            {
                if (Main.netMode != 2)
                {
                    SoundEngine.PlaySound(SoundID.Item12, Projectile.Center);
                    //SoundEngine.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/QuickBeam").WithVolume(.8f).WithPitchVariance(.5f), Projectile.Center);
                }

                Projectile.ai[1] = 1;
            }

            if (shootTimer > 200)
            {
                Projectile.Kill();
            }
        }

        private void DrawLaser(Texture2D texture, Color color)
        {
            for (int i = 0; i < laserLength; i += 4)
            {
                Main.EntitySpriteDraw(texture, Projectile.Center + QwertyMethods.PolarVector(17 + i, Projectile.rotation) - Main.screenPosition, null, color, Projectile.rotation, Vector2.UnitY * texture.Height * .5f, 1f, SpriteEffects.None, 0);
            }
        }

        public override void PostDraw(Color lightColor)
        {
            if (shootTimer > 180)
            {
                DrawLaser(Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/CloakedDarkBoss/CannonBeam").Value, Color.White);
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0f;
            return shootTimer > 180 && Collision.CheckAABBvLineCollision(targetHitbox.Location.ToVector2(), targetHitbox.Size(), Projectile.Center, Projectile.Center + QwertyMethods.PolarVector(laserLength, Projectile.rotation), 10, ref point);
        }
    }

    public class EtimsicWall : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Etimsic Barrier");
            Main.projFrames[Projectile.type] = 2;
        }


        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 38;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
        }

        private int shootTimer = 0;
        private int laserLength = 2000;

        public override void AI()
        {
            Projectile.rotation = Projectile.ai[0];
            shootTimer++;
            if (shootTimer == 30)
            {
                if (Main.netMode != 2)
                {
                    SoundEngine.PlaySound(SoundID.Item12, Projectile.Center);
                    // SoundEngine.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/QuickBeam").WithVolume(.8f).WithPitchVariance(.5f), Projectile.Center);
                }

                Projectile.ai[1] = 1;
            }
            if (shootTimer > 30 && shootTimer % 10 == 0)
            {
                Projectile.frame = Projectile.frame == 0 ? 1 : 0;
            }
        }

        private void DrawLaser(Texture2D texture, Color color)
        {
            for (int i = 0; i < laserLength; i += 4)
            {
                Main.EntitySpriteDraw(texture, Projectile.Center + QwertyMethods.PolarVector(22 + i, Projectile.rotation) - Main.screenPosition, null, color, Projectile.rotation, Vector2.UnitY * texture.Height * .5f, 1f, SpriteEffects.None, 0);
                Main.EntitySpriteDraw(texture, Projectile.Center + QwertyMethods.PolarVector(-22 - i, Projectile.rotation) - Main.screenPosition, null, color, Projectile.rotation + (float)Math.PI, Vector2.UnitY * texture.Height * .5f, 1f, SpriteEffects.None, 0);
            }
        }

        public override void PostDraw(Color lightColor)
        {
            if (shootTimer > 30)
            {
                DrawLaser(Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/CloakedDarkBoss/WallBeam").Value, Color.White);
            }
            /*
            else if (shootTimer > 20)
            {
                DrawLaser(spriteBatch, mod.GetTexture("NPCs/CloakedDarkBoss/WarningLaser"), (shootTimer % 10 > 5 ? Color.White : Color.Red));
            }
            else if (shootTimer > 0)
            {
                DrawLaser(spriteBatch, mod.GetTexture("NPCs/CloakedDarkBoss/WarningLaser"), (shootTimer % 20 > 10 ? Color.White : Color.Red));
            }
            */
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0f;
            return shootTimer > 30 && Collision.CheckAABBvLineCollision(targetHitbox.Location.ToVector2(), targetHitbox.Size(), Projectile.Center + QwertyMethods.PolarVector(-laserLength, Projectile.rotation), Projectile.Center + QwertyMethods.PolarVector(laserLength * 2, Projectile.rotation), 10, ref point);
        }
    }

    public class EtimsicRay : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Etimsic Ray");
        }


        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.extraUpdates = 1;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.light = 1f;
            Projectile.GetGlobalProjectile<EtimsProjectile>().effect = true;
        }
    }
}