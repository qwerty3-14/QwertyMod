using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Weapon.Melee.Top
{
    public abstract class Top : ModProjectile
    {
        private bool runOnce = true;
        private float initVel;
        protected bool hitGround;
        private int timeOutTimer;
        protected float friction = .002666f;
        protected float enemyFriction = .1f;
        protected int frameDelay = 1;
        public override void AI()
        {
            if (runOnce)
            {
                initVel = (float)Math.Abs(Projectile.velocity.Length());
                friction = friction * (initVel - 2);
                runOnce = false;
            }
            Projectile.frameCounter++;
            if(Projectile.frameCounter % (frameDelay *(initVel < 2 ? 2 : 1)) == 0)
            {
                Projectile.frame++;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }
            if (hitGround)
            {
                if (Projectile.velocity.X < 0)
                {
                    Projectile.velocity.X = -initVel;
                }
                else
                {
                    Projectile.velocity.X = initVel;
                }

                if (initVel < 2)
                {
                    Projectile.friendly = false;
                    initVel = .5f;
                    timeOutTimer++;
                    if (timeOutTimer > 325)
                    {
                        Projectile.Kill();
                    }
                    else if (timeOutTimer > 255)
                    {
                        initVel = 0f;
                        Projectile.rotation = (float)MathHelper.ToRadians(-45);
                        Projectile.frame = 0;
                    }
                    else if (timeOutTimer > 180)
                    {
                        Projectile.rotation = (float)MathHelper.ToRadians(210 - timeOutTimer);
                    }
                    else if (timeOutTimer > 120)
                    {
                        Projectile.rotation = (float)MathHelper.ToRadians(timeOutTimer - 150);
                    }
                    else if (timeOutTimer > 60)
                    {
                        Projectile.rotation = (float)MathHelper.ToRadians(90 - timeOutTimer);
                    }
                    else if (timeOutTimer > 30)
                    {
                        Projectile.rotation = (float)MathHelper.ToRadians(timeOutTimer - 30);
                    }
                    else
                    {
                        Projectile.rotation = 0;
                        Projectile.rotation += (float)MathHelper.ToRadians(1);
                    }
                }
                else
                {
                    Projectile.rotation = 0;

                    initVel -= friction;
                }
            }
            else
            {
                Projectile.rotation = 0;
            }
            ExtraTopNonesense();
        }

        public override bool OnTileCollide(Vector2 velocityChange)
        {
            hitGround = true;
            if (Projectile.velocity.X != velocityChange.X)
            {
                Projectile.velocity.X = -velocityChange.X;
                initVel -= friction;
            }

            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.usesIDStaticNPCImmunity = true;
            int immutime = 20;
            Projectile.perIDStaticNPCImmunity[Projectile.type][target.whoAmI] = (uint)(Main.GameUpdateCount + immutime);

            initVel -= enemyFriction;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            knockback = ((float)Math.Abs(Projectile.velocity.X) / initVel) * Projectile.knockBack;
            hitDirection = Projectile.velocity.X > 0 ? -1 : 1;
            TopHit(target);
        }

        public virtual void ExtraTopNonesense()
        {
        }
        public virtual void TopHit(NPC target)
        {

        }
    }
}