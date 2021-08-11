using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
namespace QwertyMod.Content.Items.Weapon.Minion.MiniTank
{
    public class MiniTankCannonBallFreindly : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tank!!");
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;

            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 600;
            Projectile.usesLocalNPCImmunity = true;
        }

        public bool runOnce = true;

        public override void AI()
        {
            Projectile.ai[0] -= 0.8f;
            //Main.NewText(Projectile.damage);
            if (runOnce)
            {
                SoundEngine.PlaySound(SoundID.Item62, Projectile.position);
                for (int i = 0; i < 4; i++)
                {
                    int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 31, 0f, 0f, 100, default(Color), 1f);
                    Main.dust[dustIndex].velocity *= .6f;
                    Main.dust[dustIndex].noGravity = true;
                }
                // Fire Dust spawn
                for (int i = 0; i < 8; i++)
                {
                    int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6, 0f, 0f, 100, default(Color), 1.5f);
                    Main.dust[dustIndex].noGravity = true;
                    Main.dust[dustIndex].velocity *= 2f;
                    dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6, 0f, 0f, 100, default(Color), 1f);
                    Main.dust[dustIndex].velocity *= 1f;
                    Main.dust[dustIndex].noGravity = true;
                }
                runOnce = false;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[Projectile.owner] = 0;
            Projectile e = Main.projectile[Projectile.NewProjectile(new ProjectileSource_ProjectileParent(Projectile), Projectile.Center,Vector2.Zero, ProjectileType<MiniTankBlast>(), Projectile.damage, Projectile.knockBack, Projectile.owner)];
            e.localNPCImmunity[target.whoAmI] = -1;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile e = Main.projectile[Projectile.NewProjectile(new ProjectileSource_ProjectileParent(Projectile), Projectile.Center, Vector2.Zero, ProjectileType<MiniTankBlast>(), Projectile.damage, Projectile.knockBack, Projectile.owner)];
            return true;
        }

        public override void Kill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];

            SoundEngine.PlaySound(SoundID.Item62, Projectile.position);
            for (int i = 0; i < 4; i++)
            {
                float theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                Dust dustIndex = Dust.NewDustPerfect(Projectile.Center, 31, QwertyMethods.PolarVector(Main.rand.NextFloat() * 2f, theta), Scale: .5f);
                dustIndex.noGravity = true;
            }
            // Fire Dust spawn
            for (int i = 0; i < 8; i++)
            {
                float theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                Dust dustIndex = Dust.NewDustPerfect(Projectile.Center, 6, QwertyMethods.PolarVector(Main.rand.NextFloat() * 2f, theta), Scale: .5f);
                dustIndex.noGravity = true;
                theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                dustIndex = Dust.NewDustPerfect(Projectile.Center, 6, QwertyMethods.PolarVector(Main.rand.NextFloat() * 2f, theta), Scale: 1f);
                dustIndex.noGravity = true;
            }
        }
    }

    public class MiniTankBlast : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tank!!");
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.DamageType = DamageClass.Summon;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[Projectile.owner] = 0;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
}
