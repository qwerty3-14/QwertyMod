using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common;
using QwertyMod.Content.Dusts;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Minion.HigherPriest
{
    class HigherPriestMinion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Priest Minion");
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projFrames[Projectile.type] = 6;
        }
        public override void SetDefaults()
        {
            Projectile.minion = true;
            Projectile.minionSlots = 1;
            Projectile.friendly = true;
            Projectile.width = 36;
            Projectile.height = 36;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return false;
        }

        NPC target;
        NPC savedTarget;
        bool attacking = false;
        bool justAttacked = false;
        int attackCycleTime = 60;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (player.GetModPlayer<MinionManager>().HighPriestMinion)
            {
                Projectile.timeLeft = 2;
            }
            int identity = MinionManager.GetIdentity(Projectile);
            int priestCount = player.ownedProjectileCounts[Projectile.type];
            if (priestCount != 0)
            {
                int timer = player.GetModPlayer<MinionManager>().PriestSynchroniser;
                if (timer % attackCycleTime == 0)
                {
                    justAttacked = false;
                    if (QwertyMethods.ClosestNPC(ref target, 2000, player.Center, false, player.MinionAttackTargetNPC))
                    {
                        SoundEngine.PlaySound(SoundID.Item8, Projectile.position);
                        for (int num67 = 0; num67 < 15; num67++)
                        {
                            int num75 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<CaeliteDust>(), 0f, 0f, 100, default(Color), 2.5f);
                            Main.dust[num75].velocity *= 3f;
                            Main.dust[num75].noGravity = true;
                        }

                        Projectile.Center = target.Center + QwertyMethods.PolarVector(80f + 10f * priestCount, player.GetModPlayer<MinionManager>().PriestAngle) + QwertyMethods.PolarVector((40f * priestCount) * ((float)(identity + 1) / (priestCount + 1)) - (20f * priestCount), player.GetModPlayer<MinionManager>().PriestAngle + MathF.PI / 2f);
                        Projectile.velocity = Vector2.Zero;
                        attacking = true;
                        savedTarget = target;

                        SoundEngine.PlaySound(SoundID.Item8, Projectile.position);
                        for (int num76 = 0; num76 < 15; num76++)
                        {
                            int num84 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<CaeliteDust>(), 0f, 0f, 100, default(Color), 2.5f);
                            Main.dust[num84].velocity *= 3f;
                            Main.dust[num84].noGravity = true;
                        }
                    }
                    else
                    {
                        attacking = false;
                    }
                }

                if (attacking)
                {
                    if (!justAttacked)
                    {
                        if (savedTarget != null && !savedTarget.active)
                        {
                            savedTarget = null;
                        }
                        Vector2 shotPos = Projectile.Center + new Vector2(22 * Projectile.spriteDirection, 1);
                        float shootSpeed = 10;
                        float? aimAt = null;
                        Vector2? flyTorawd = null;
                        if (savedTarget != null)
                        {
                            aimAt = QwertyMethods.PredictiveAim(shotPos, shootSpeed * 2, savedTarget.Center, savedTarget.velocity);
                            flyTorawd = savedTarget.Center;
                        }
                        else if (QwertyMethods.ClosestNPC(ref target, 2000, Projectile.Center, false, player.MinionAttackTargetNPC))
                        {
                            aimAt = QwertyMethods.PredictiveAim(shotPos, shootSpeed * 2, target.Center, target.velocity);
                            flyTorawd = target.Center;
                        }
                        if(flyTorawd != null)
                        {
                            Projectile.velocity = ((Vector2)flyTorawd - Projectile.Center) * 0.08f;
                        }
                        else
                        {
                            Projectile.velocity = Vector2.Zero;
                        }
                        if (aimAt != null && !float.IsNaN(((float)aimAt)))
                        {
                            Projectile.spriteDirection = Math.Sign(Math.Cos((float)aimAt));
                        }
                        if ((timer % attackCycleTime) == (int)((float)(identity + 1) / (priestCount + 1) * attackCycleTime))
                        {
                            justAttacked = true;
                            if (aimAt != null && !float.IsNaN(((float)aimAt)))
                            {
                                Projectile.NewProjectile(Projectile.InheritSource(Projectile), shotPos, QwertyMethods.PolarVector(shootSpeed, (float)aimAt), ProjectileType<HigherPriestPulse>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                            }
                        }
                    }
                    else
                    {
                        Projectile.velocity = Vector2.Zero;
                    }
                }
                else
                {
                    Vector2 goHere = player.Center + QwertyMethods.PolarVector(25f + 5f * priestCount, -MathF.PI / 2 + ((float)(identity + 1) / (priestCount + 1)) * MathF.PI / 2f * player.direction * -1);
                    Vector2 dif = goHere - Projectile.Center;
                    Projectile.spriteDirection = Math.Sign(player.Center.X - Projectile.Center.X);
                    if (dif.Length() > 300f)
                    {
                        SoundEngine.PlaySound(SoundID.Item8, Projectile.position);
                        for (int num67 = 0; num67 < 15; num67++)
                        {
                            int num75 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<CaeliteDust>(), 0f, 0f, 100, default(Color), 2.5f);
                            Main.dust[num75].velocity *= 3f;
                            Main.dust[num75].noGravity = true;
                        }

                        Projectile.velocity = Vector2.Zero;
                        Projectile.Center = goHere;

                        SoundEngine.PlaySound(SoundID.Item8, Projectile.position);
                        for (int num67 = 0; num67 < 15; num67++)
                        {
                            int num75 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<CaeliteDust>(), 0f, 0f, 100, default(Color), 2.5f);
                            Main.dust[num75].velocity *= 3f;
                            Main.dust[num75].noGravity = true;
                        }
                    }
                    else if (dif.Length() > 18f)
                    {
                        Projectile.velocity = dif.SafeNormalize(Vector2.UnitY) * 18f;
                    }
                    else
                    {
                        Projectile.velocity = Vector2.Zero;
                        Projectile.Center = goHere;
                    }
                }
            }

            //animation
            Projectile.frameCounter++;
            Projectile.frame = Projectile.frameCounter % 20 > 10 ? 1 : 0;
            if (attacking)
            {
                Projectile.frame += 2;
            }
            if (justAttacked)
            {
                Projectile.frame += 2;
            }

        }
        public override void PostDraw(Color lightColor)
        {
            if (Projectile.frame == 2 || Projectile.frame == 3)
            {
                Texture2D texture = Request<Texture2D>("QwertyMod/Content/Items/Weapon/Minion/HigherPriest/HigherPriestPulse").Value;
                Main.EntitySpriteDraw(texture, Projectile.Center + new Vector2(Projectile.spriteDirection * 6, -30) - Main.screenPosition, new Rectangle(0, (Projectile.frame - 2) * 30, 30, 30), Color.White, 0f, Vector2.One * 15, Vector2.One, 0, 0);
            }

        }
    }
}
