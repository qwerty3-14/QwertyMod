using Microsoft.Xna.Framework;
using QwertyMod.Content.Dusts;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;


namespace QwertyMod.Content.NPCs.Bosses.OLORD
{
    public class BlackHoleSeed : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Pew Pew");
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 30;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
        }

        public override void Kill(int timeLeft)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, ProjectileType<BlackHole>(), Projectile.damage, 3f, Main.myPlayer, Projectile.ai[0]);
            }
        }
    }

    public class BlackHole : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("BlackHole");
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 480;
            Projectile.tileCollide = false;
        }

        public float horiSpeed;
        public float vertSpeed;
        public float direction;
        public float pullSpeed = .5f;
        public float dustSpeed = 20f;
        public NPC mass;
        public Projectile proj;
        public int frameTimer;
        public Dust dust;
        public Item item;

        public override void AI()
        {
            Projectile.velocity = new Vector2(0, 0);
            Projectile.timeLeft -= (int)Projectile.ai[0] - 1;
            //Player player = Main.player[Projectile.owner];

            for (int p = 0; p < Main.maxPlayers; p++)
            {
                Player player = Main.player[p];
                if (player.active && !player.dead)
                {
                    direction = (Projectile.Center - player.Center).ToRotation();
                    horiSpeed = MathF.Cos(direction) * pullSpeed / 2;
                    vertSpeed = MathF.Sin(direction) * pullSpeed / 2;
                    player.velocity += new Vector2(horiSpeed, vertSpeed);

                    for (int i = 0; i < 1; i++)
                    {
                        int dust = Dust.NewDust(player.position, player.width, player.height, DustType<B4PDust>(), 0, 0);
                    }
                }
            }
            /*
            for (int g = 0; g < 3; g++)
            {
                Dust blackEs = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, mod.DustType("B4PDust"), 0, 0)];
                direction = (Projectile.Center - blackEs.position).ToRotation();
                horiSpeed = MathF.Cos(direction) * pullSpeed * 50;
                vertSpeed = MathF.Sin(direction) * pullSpeed * 50;
                blackEs.velocity += new Vector2(horiSpeed, vertSpeed);
            }
            */

            for (int d = 0; d < 80; d++)
            {
                float theta = Main.rand.NextFloat(-MathF.PI, MathF.PI);
                Dust dust = Dust.NewDustPerfect(Projectile.Center + QwertyMethods.PolarVector(Main.rand.NextFloat(10, 200), theta), DustType<BlackHoleMatter>(), QwertyMethods.PolarVector(6, theta + MathF.PI / 2));
                dust.scale = 1f;
            }

            for (int i = 0; i < Main.dust.Length; i++)
            {
                dust = Main.dust[i];
                if (!dust.noGravity)
                {
                    direction = (Projectile.Center - dust.position).ToRotation();
                    horiSpeed = MathF.Cos(direction) * pullSpeed * 5;
                    vertSpeed = MathF.Sin(direction) * pullSpeed * 5;
                    dust.velocity += new Vector2(horiSpeed, vertSpeed);
                }
                if (dust.type == DustType<BlackHoleMatter>())
                {
                    direction = (Projectile.Center - dust.position).ToRotation();
                    dust.velocity += QwertyMethods.PolarVector(.8f, direction);
                    if ((dust.position - Projectile.Center).Length() < 10)
                    {
                        dust.scale = 0f;
                    }
                    else
                    {
                        dust.scale = .35f;
                    }
                }
            }
            for (int i = 0; i < 200; i++)
            {
                mass = Main.npc[i];
                if (!mass.boss && mass.active && mass.knockBackResist != 0f)
                {
                    direction = (Projectile.Center - mass.Center).ToRotation();
                    horiSpeed = MathF.Cos(direction) * pullSpeed;
                    vertSpeed = MathF.Sin(direction) * pullSpeed;
                    mass.velocity += new Vector2(horiSpeed, vertSpeed);
                    for (int g = 0; g < 1; g++)
                    {
                        int dust = Dust.NewDust(mass.position, mass.width, mass.height, DustType<B4PDust>(), horiSpeed * dustSpeed, vertSpeed * dustSpeed);
                    }
                }
            }
            for (int i = 0; i < Main.item.Length; i++)
            {
                item = Main.item[i];
                if (item.position != new Vector2(0, 0))
                {
                    direction = (Projectile.Center - item.Center).ToRotation();
                    horiSpeed = MathF.Cos(direction) * pullSpeed;
                    vertSpeed = MathF.Sin(direction) * pullSpeed;
                    item.velocity += new Vector2(horiSpeed, vertSpeed);
                    for (int g = 0; g < 1; g++)
                    {
                        int dust = Dust.NewDust(item.position, item.width, item.height, DustType<B4PDust>(), horiSpeed * dustSpeed, vertSpeed * dustSpeed);
                    }
                }
            }
            for (int i = 0; i < Main.projectile.Length; i++)
            {
                proj = Main.projectile[i];
                if (proj.active && proj.type != ProjectileType<BlackHole>() && proj.type != ProjectileType<SideLaser>())
                {
                    direction = (Projectile.Center - proj.Center).ToRotation();
                    horiSpeed = MathF.Cos(direction) * pullSpeed;
                    vertSpeed = MathF.Sin(direction) * pullSpeed;
                    proj.velocity += new Vector2(horiSpeed, vertSpeed);
                    for (int g = 0; g < 1; g++)
                    {
                        int dust = Dust.NewDust(proj.position, proj.width, proj.height, DustType<B4PDust>(), horiSpeed * dustSpeed, vertSpeed * dustSpeed);
                    }
                }
            }
        }
    }

    public class BurstShot2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Pew Pew");
        }


        public override void SetDefaults()
        {
            Projectile.width = 102;
            Projectile.height = 104;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 120;
            Projectile.tileCollide = false;
        }

        public float distance;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.rotation += MathF.PI / 30;
            for (int i = 0; i < 1; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<B4PDust>());
            }
        }

        public float shotSpeed = 3;

        public override void Kill(int timeLeft)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                QwertyMethods.ProjectileSpread(Projectile.GetSource_FromThis(), Projectile.Center, 4, shotSpeed, ProjectileType<TurretShot>(), Projectile.damage, Projectile.knockBack, Main.myPlayer);
                QwertyMethods.ProjectileSpread(Projectile.GetSource_FromThis(), Projectile.Center, 4, shotSpeed * 1.5f, ProjectileType<TurretShot>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, rotation: MathF.PI / 4);
            }
        }
    }

    public class MegaBurst : ModProjectile
    {


        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Pew Pew");
        }

        public override void SetDefaults()
        {
            Projectile.width = 300;
            Projectile.height = 300;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 60;
            Projectile.tileCollide = false;
        }

        public float distance;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.rotation += MathF.PI / 30;
            for (int i = 0; i < 3; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<B4PDust>());
            }
            Projectile.timeLeft -= (int)Projectile.ai[0] - 1;
        }

        public float shotSpeed = 3;

        public override void Kill(int timeLeft)
        {
            for (int r = 0; r < 6; r++)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, MathF.Cos(r * (2 * MathF.PI / 6)) * shotSpeed * 1.5f, MathF.Sin(r * (2 * MathF.PI / 6)) * shotSpeed * 1.5f, ProjectileType<BurstShot2>(), Projectile.damage, 0, Main.myPlayer);
                }
            }
        }
    }
}