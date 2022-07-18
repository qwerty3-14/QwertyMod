using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common;
using QwertyMod.Content.Dusts;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Minion.AncientMinion
{
    public class AncientMinionFreindly : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Minion");
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projFrames[Projectile.type] = 1;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
        }


        public override void SetDefaults()
        {
            Projectile.width = 42;
            Projectile.height = 56;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.minion = true;
            Projectile.minionSlots = 1;
            Projectile.timeLeft = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;

        }
        public override bool MinionContactDamage()
        {
            return true;
        }
        public const int minionRingRadius = 50;
        public const int minionRingDustQty = 50;
        public int timer;
        public bool charging;
        public NPC target;

        private int waitTime = 10;
        private int chargeTime = 20;
        private Vector2 moveTo;
        private bool justTeleported;
        private float chargeSpeed = 12;
        private bool runOnce = true;

        private float maxDistance = 1000f;
        private int noTargetTimer = 0;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (player.GetModPlayer<MinionManager>().AncientMinion)
            {
                Projectile.timeLeft = 2;
            }

            if (runOnce)
            {

                if (Main.netMode != NetmodeID.Server)
                {
                    moveTo = Projectile.Center;
                    Projectile.netUpdate = true;
                }
                runOnce = false;
            }

            if (QwertyMethods.ClosestNPC(ref target, maxDistance, player.Center, player.MinionAttackTargetNPC != -1, player.MinionAttackTargetNPC))
            {
                timer++;
                if (timer > waitTime + chargeTime)
                {
                    for (int k = 0; k < 200; k++)
                    {
                        Projectile.localNPCImmunity[k] = 0;
                    }
                    for (int i = 0; i < minionRingDustQty; i++)
                    {
                        float theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);

                        Dust dust = Dust.NewDustPerfect(Projectile.Center + QwertyMethods.PolarVector(minionRingRadius, theta), DustType<AncientGlow>(), QwertyMethods.PolarVector(-minionRingRadius / 10, theta));
                        dust.noGravity = true;
                    }
                    if (Main.netMode != 2)
                    {
                        Projectile.ai[1] = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                        Projectile.netUpdate = true;
                    }
                    moveTo = new Vector2(target.Center.X + (float)Math.Cos(Projectile.ai[1]) * 120, target.Center.Y + (float)Math.Sin(Projectile.ai[1]) * 180);
                    if (Main.netMode != NetmodeID.Server)
                    {
                        Projectile.netUpdate = true;
                    }
                    justTeleported = true;
                    timer = 0;
                    noTargetTimer = 0;
                }
                else if (timer > waitTime)
                {
                    charging = true;
                }
                else
                {
                    if (timer == 2)
                    {

                        SoundEngine.PlaySound(SoundID.Item8, Projectile.Center);
                        for (int i = 0; i < minionRingDustQty; i++)
                        {
                            float theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                            Dust dust = Dust.NewDustPerfect(Projectile.Center, DustType<AncientGlow>(), QwertyMethods.PolarVector(minionRingRadius / 10, theta));
                            dust.noGravity = true;
                        }
                    }
                    charging = false;
                }
                if (charging)
                {
                    Projectile.velocity = new Vector2((float)Math.Cos(Projectile.rotation), (float)Math.Sin(Projectile.rotation)) * chargeSpeed;
                }
                else
                {
                    Projectile.Center = new Vector2(moveTo.X, moveTo.Y);
                    Projectile.velocity = new Vector2(0, 0);
                    float targetAngle = new Vector2(target.Center.X - Projectile.Center.X, target.Center.Y - Projectile.Center.Y).ToRotation();
                    Projectile.rotation = targetAngle;
                }
            }
            else
            {
                noTargetTimer++;
                if (noTargetTimer == 2)
                {
                    SoundEngine.PlaySound(SoundID.Item8, Projectile.Center);
                    for (int i = 0; i < minionRingDustQty; i++)
                    {
                        float theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                        Dust dust = Dust.NewDustPerfect(Projectile.Center, DustType<AncientGlow>(), QwertyMethods.PolarVector(minionRingRadius / 10, theta));
                        dust.noGravity = true;
                    }
                }
                if ((Projectile.Center - player.Center).Length() > 300)
                {
                    if (Main.netMode != 2)
                    {
                        Projectile.ai[1] = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                        Projectile.netUpdate = true;
                    }
                    for (int i = 0; i < minionRingDustQty; i++)
                    {
                        float theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);

                        Dust dust = Dust.NewDustPerfect(Projectile.Center + QwertyMethods.PolarVector(minionRingRadius, theta), DustType<AncientGlow>(), QwertyMethods.PolarVector(-minionRingRadius / 10, theta));
                        dust.noGravity = true;
                    }
                    noTargetTimer = 0;
                    moveTo = new Vector2(player.Center.X + (float)Math.Cos(Projectile.ai[1]) * 100, player.Center.Y + (float)Math.Sin(Projectile.ai[1]) * 100);
                    justTeleported = true;
                }

                Projectile.Center = moveTo;

                float targetAngle = new Vector2(player.Center.X - Projectile.Center.X, player.Center.Y - Projectile.Center.Y).ToRotation();
                Projectile.rotation = targetAngle;
            }
            if (justTeleported)
            {
                justTeleported = false;
            }

            Projectile.frameCounter++;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(texture, new Vector2(Projectile.Center.X - Main.screenPosition.X, Projectile.Center.Y - Main.screenPosition.Y),
                        new Rectangle(0, Projectile.frame * texture.Height, texture.Width, texture.Height), lightColor, Projectile.rotation,
                        new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), 1f, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(Request<Texture2D>("QwertyMod/Content/Items/Weapon/Minion/AncientMinion/AncientMinionFreindly_Glow").Value, new Vector2(Projectile.Center.X - Main.screenPosition.X, Projectile.Center.Y - Main.screenPosition.Y),
                        new Rectangle(0, Projectile.frame * texture.Height, texture.Width, texture.Height), Color.White, Projectile.rotation,
                        new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), 1f, SpriteEffects.None, 0);
            return false;
        }
    }
}
