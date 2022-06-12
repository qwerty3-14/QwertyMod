using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common;
using QwertyMod.Common.RuneBuilder;
using QwertyMod.Content.Dusts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Minion.LeechRune
{
    public class RunicMinionFreindly : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Leech Rune");
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            Main.projPet[Projectile.type] = true;
        }
        public override bool MinionContactDamage()
        {
            return true;
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Summon;
            Projectile.width = 40; 
            Projectile.height = 40;  
            Projectile.friendly = true;  
            Projectile.ignoreWater = true;    
            Projectile.penetrate = -1; 
            Projectile.tileCollide = false; 
            Projectile.minion = true;
            Projectile.minionSlots = 1;
            Projectile.timeLeft = 2;
            Projectile.usesLocalNPCImmunity = true;
        }
        public const int minionRingRadius = 50;
        public const int minionRingDustQty = 3;
        public int timer;
        public bool charging;
        public NPC target;

        private int waitTime = 20;
        private int chargeTime = 40;
        private Vector2 moveTo;
        private bool justTeleported;
        private float chargeSpeed = 12;
        private bool runOnce = true;

        private float maxDistance = 1000f;
        private int noTargetTimer = 0;
        float rot = 0;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (player.GetModPlayer<MinionManager>().RuneMinion)
            {
                Projectile.timeLeft = 2;
            }

            if (runOnce)
            {
                if (Main.netMode != 2)
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

                        Dust dust = Dust.NewDustPerfect(Projectile.Center + QwertyMethods.PolarVector(minionRingRadius, theta), DustType<LeechRuneDeath>(), QwertyMethods.PolarVector(-minionRingRadius / 10, theta));
                        dust.noGravity = true;
                    }
                    if (Main.netMode != 2)
                    {
                        Projectile.ai[1] = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                        Projectile.netUpdate = true;
                    }
                    moveTo = new Vector2(target.Center.X + (float)Math.Cos(Projectile.ai[1]) * 120, target.Center.Y + (float)Math.Sin(Projectile.ai[1]) * 180);
                    if (Main.netMode != 2)
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
                            Dust dust = Dust.NewDustPerfect(Projectile.Center, DustType<LeechRuneDeath>(), QwertyMethods.PolarVector(minionRingRadius / 10, theta));
                            dust.noGravity = true;
                        }
                    }
                    charging = false;
                }
                if (charging)
                {
                    Projectile.velocity = new Vector2((float)Math.Cos(rot), (float)Math.Sin(rot)) * chargeSpeed;
                }
                else
                {
                    Projectile.Center = new Vector2(moveTo.X, moveTo.Y);
                    Projectile.velocity = new Vector2(0, 0);
                    float targetAngle = new Vector2(target.Center.X - Projectile.Center.X, target.Center.Y - Projectile.Center.Y).ToRotation();
                    rot = targetAngle;
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
                        Dust dust = Dust.NewDustPerfect(Projectile.Center, DustType<LeechRuneDeath>(), QwertyMethods.PolarVector(minionRingRadius / 10, theta));
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

                        Dust dust = Dust.NewDustPerfect(Projectile.Center + QwertyMethods.PolarVector(minionRingRadius, theta), DustType<LeechRuneDeath>(), QwertyMethods.PolarVector(-minionRingRadius / 10, theta));
                        dust.noGravity = true;
                    }
                    noTargetTimer = 0;
                    moveTo = new Vector2(player.Center.X + (float)Math.Cos(Projectile.ai[1]) * 100, player.Center.Y + (float)Math.Sin(Projectile.ai[1]) * 100);
                    justTeleported = true;
                }

                Projectile.Center = moveTo;

                float targetAngle = new Vector2(player.Center.X - Projectile.Center.X, player.Center.Y - Projectile.Center.Y).ToRotation();
                rot = targetAngle;
            }
            if (justTeleported)
            {
                Projectile.frameCounter = 0;
                justTeleported = false;
            }

            Projectile.frameCounter++;
            Projectile.rotation += Math.Sign(Projectile.velocity.X) * (float)Math.PI / 60f;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!target.immortal && !target.SpawnedFromStatue && Main.rand.Next(5) == 0)
            {
                Player player = Main.player[Projectile.owner];
                player.statLife++;
                player.HealEffect(1, true);
            }
            Projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[Projectile.owner] = 0;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            float c = (Projectile.frameCounter / 20f);
            if (c > 1f)
            {
                c = 1f;
            }
            int frame = Projectile.frameCounter;
            if (frame > 19)
            {
                frame = 19;
            }
            Main.EntitySpriteDraw(RuneSprites.runeTransition[(int)Runes.Leech][frame], Projectile.Center - Main.screenPosition, null, new Color(c, c, c, c), Projectile.rotation, new Vector2(10, 10), Vector2.One * 2, 0, 0);
            return false;
        }
    }
}
