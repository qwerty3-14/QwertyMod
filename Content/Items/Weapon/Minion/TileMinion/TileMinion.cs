using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Weapon.Minion.TileMinion
{
    public class TileMinion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true; //This is necessary for right-click targeting
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 2;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 18;
            Projectile.minionSlots = 1;
            Projectile.minion = true;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.localNPCImmunity[target.whoAmI] = Projectile.localNPCHitCooldown;
            target.immune[Projectile.owner] = 0;
        }

        private Vector2 flyTo;
        private NPC target;
        private int velocityTime = 0;
        private Vector2 direction;
        private const float maxSpeed = 20f;
        private float acceleration = maxSpeed / 30;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (player.GetModPlayer<MinionManager>().TileMinion)
            {
                Projectile.timeLeft = 2;
            }

            if (QwertyMethods.ClosestNPC(ref target, 2000, Projectile.Center, true, player.MinionAttackTargetNPC, delegate (NPC possibleTarget) { return Collision.CanHit(player.Center, 1, 1, possibleTarget.position, possibleTarget.width, possibleTarget.height); }))
            {
                flyTo = target.Center + target.velocity * 10 + new Vector2(Main.rand.Next(-60, 61), Main.rand.Next(-60, 61));
            }
            else
            {
                flyTo = player.Center + player.velocity * 10 + new Vector2(Main.rand.Next(-60, 61), Main.rand.Next(-60, 61));
            }
            if (velocityTime == 0)
            {
                Vector2 oldDirection = direction;

                direction = (flyTo - Projectile.Center);
                if ((direction.Y > 0 && oldDirection.Y < 0) || (direction.X > 0 && oldDirection.X < 0) || (direction.Y < 0 && oldDirection.Y > 0) || (direction.X < 0 && oldDirection.X > 0) || oldDirection == Vector2.Zero)
                {
                    if (Math.Abs(direction.Y) > Math.Abs(direction.X))
                    {
                        direction = direction.Y > 0 ? Vector2.UnitY : -Vector2.UnitY;
                        Projectile.frame = 0;
                    }
                    else
                    {
                        direction = direction.X > 0 ? Vector2.UnitX : -Vector2.UnitX;
                        if (direction.X > 0)
                        {
                            Projectile.frame = 1;
                        }
                        else
                        {
                            Projectile.frame = 2;
                        }
                    }
                }
                else
                {
                    direction = oldDirection;
                }

                if (direction != oldDirection)
                {
                    velocityTime = 10;
                    Projectile.velocity = Vector2.Zero;
                }
            }
            else
            {
                velocityTime--;
            }
            Projectile.velocity += direction * acceleration;
            if (Projectile.velocity.Length() > maxSpeed)
            {
                Projectile.velocity = direction * maxSpeed;
            }
            if ((player.Center - Projectile.Center).Length() > 2000)
            {
                Projectile.Center = player.Center;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.velocity.Length() > 16f)
            {
                Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
                for (int k = 0; k < Projectile.oldPos.Length; k++)
                {
                    Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                    Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                    Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, drawPos, new Rectangle(0, Projectile.frame * Projectile.height, Projectile.width, Projectile.height), color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
                }
            }
            return base.PreDraw(ref lightColor);
        }
    }
}
