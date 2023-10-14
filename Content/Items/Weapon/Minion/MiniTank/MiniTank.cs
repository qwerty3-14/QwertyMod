using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;


namespace QwertyMod.Content.Items.Weapon.Minion.MiniTank
{
    public class MiniTank : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            Main.projPet[Projectile.type] = true;
        }
        public override bool MinionContactDamage()
        {
            return false;
        }

        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.minion = true;
            Projectile.minionSlots = 1;
            Projectile.timeLeft = 2;
            Projectile.aiStyle = -1;
        }

        private int tankCount = 0;
        private const float terminalVelocity = 10;
        private const float garvityAcceleration = .2f;
        private float gotoX;
        private const float spacing = 70;
        private const float maxSpeedX = 8;
        private bool returnToPlayer;
        private float gunRotation = 0;
        private NPC target;
        private float aim;
        private int shootCounter = 0;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (player.GetModPlayer<MinionManager>().miniTank)
            {
                Projectile.timeLeft = 2;
            }
            tankCount = player.ownedProjectileCounts[ ModContent.ProjectileType<MiniTank>()];
            if (returnToPlayer)
            {
                Projectile.velocity = (player.Top - Projectile.Center) * .1f;
                if ((player.Top - Projectile.Center).Length() < 200)
                {
                    returnToPlayer = false;
                }
                Projectile.tileCollide = false;
            }
            else
            {
                if ((player.Center - Projectile.Center).Length() > 1500)
                {
                    returnToPlayer = true;
                }
                Projectile.tileCollide = true;

                if (Projectile.velocity.Y < terminalVelocity)
                {
                    Projectile.velocity.Y += garvityAcceleration;
                }
                gotoX = player.Center.X + -player.direction * (player.width / 2 + spacing + (MinionManager.GetIdentity(Projectile) * spacing));
                Projectile.velocity.X = (gotoX - Projectile.Center.X) * .1f;
                if (Math.Abs(Projectile.velocity.X) > maxSpeedX)
                {
                    Projectile.velocity.X = Projectile.velocity.X > 0 ? maxSpeedX : -maxSpeedX;
                }

                Projectile.spriteDirection = Projectile.velocity.X > 0 ? 1 : -1;
                if (QwertyMethods.ClosestNPC(ref target, 1000, Projectile.Top, false, player.MinionAttackTargetNPC, delegate (NPC possibleTarget) { return possibleTarget.Top.Y < Projectile.Bottom.Y; }))
                {
                    if (target.Center.Y > Projectile.Top.Y)
                    {
                        if (target.Center.X > Projectile.Top.X)
                        {
                            aim = 0;
                        }
                        else
                        {
                            aim = MathF.PI;
                        }
                    }
                    else
                    {
                        aim = (target.Center - Projectile.Top).ToRotation();
                    }
                    if (shootCounter >= 60)
                    {
                        shootCounter = 0;
                        Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Top + QwertyMethods.PolarVector(30, gunRotation), QwertyMethods.PolarVector(14, gunRotation), ModContent.ProjectileType<MiniTankCannonBallFreindly>(), Projectile.damage, Projectile.knockBack, player.whoAmI);
                    }
                }
                else
                {
                    aim = Projectile.spriteDirection == 1 ? 0 : MathF.PI;
                }
                shootCounter++;
                gunRotation = QwertyMethods.SlowRotation(gunRotation, aim, 3);
                if (gunRotation > 0)
                {
                    if (gunRotation > MathF.PI / 2)
                    {
                        gunRotation = MathF.PI;
                    }
                    else
                    {
                        gunRotation = 0;
                    }
                }
                //Projectile.velocity = Collision.TileCollision(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height, player.Center.Y > Projectile.Bottom.Y, player.Center.Y > Projectile.Bottom.Y);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Point origin = new Vector2(Projectile.Center.X + (Projectile.width / 2 - 6) * Projectile.spriteDirection, Projectile.Bottom.Y).ToTileCoordinates();
            Point point;
            if ((oldVelocity.X != Projectile.velocity.X) && WorldUtils.Find(origin, Searches.Chain(new Searches.Down(1), new GenCondition[]
                                            {
                                            new Conditions.IsSolid()
                                            }), out point))
            {
                Projectile.velocity.Y = -6;
            }
            return false;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = Main.player[Projectile.owner].Center.Y - Projectile.Center.Y > 64;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("QwertyMod/Content/Items/Weapon/Minion/MiniTank/MiniTankGun").Value;
            Main.EntitySpriteDraw(texture, Projectile.Top - Main.screenPosition,
                        new Rectangle(0, 0, 40, 20), lightColor, gunRotation,
                        new Vector2(10, 10), 1f, SpriteEffects.None, 0);
            return true;
        }
    }
}
