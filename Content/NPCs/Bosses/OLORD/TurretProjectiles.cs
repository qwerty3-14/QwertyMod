using Microsoft.Xna.Framework;
using QwertyMod.Content.Dusts;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.NPCs.Bosses.OLORD
{
    public class TurretShot : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pew Pew");
        }

        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 36;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 720;
            Projectile.tileCollide = false;
            //Projectile.hide = true; // Prevents projectile from being drawn normally. Use in conjunction with DrawBehind.
        }

        /*
        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
            drawCacheProjsOverWiresUI.Add(index);
        }*/

        public override bool PreAI()
        {
            return base.PreAI();
        }

        public override void AI()
        {
            Projectile.rotation += (float)Math.PI / 30;
            for (int i = 0; i < 1; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<B4PDust>());
            }
        }
    }

    public class BurstShot : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pew Pew");
        }


        public override void SetDefaults()
        {
            Projectile.width = 102;
            Projectile.height = 104;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 720;
            Projectile.tileCollide = false;
        }

        public float distance;
        private float closest = 250;
        private Player player;

        public override void AI()
        {
            if (Main.netMode != 1)
            {
                for (int i = 0; i < 255; i++)
                {
                    if (Main.player[i].active && (Projectile.Center - Main.player[i].Center).Length() < closest)
                    {
                        Projectile.Kill();
                    }
                }
            }
            Projectile.rotation += (float)Math.PI / 30;
            for (int i = 0; i < 1; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<B4PDust>());
            }
        }

        public float shotSpeed = 3;

        public override void Kill(int timeLeft)
        {
            if (Main.netMode != 1 && timeLeft > 1)
            {
                QwertyMethods.ProjectileSpread(new EntitySource_Misc(""), Projectile.Center, 4, shotSpeed, ProjectileType<TurretShot>(), Projectile.damage, Projectile.knockBack, Main.myPlayer);
                QwertyMethods.ProjectileSpread(new EntitySource_Misc(""), Projectile.Center, 4, shotSpeed * 1.5f, ProjectileType<TurretShot>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, rotation: (float)Math.PI / 4);
            }
        }
    }

    public class TurretGrav : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pew Pew");
        }


        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 36;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 360;
            Projectile.tileCollide = false;
        }

        public float horiSpeed;
        public float horiAccCon = .075f;
        public float vertSpeed;
        public float vertAccCon = .075f;
        public float direction;
        public float maxSpeed = 12f;
        private float closest = 10000;

        public override void AI()
        {
            if (Main.netMode != 1)
            {
                for (int i = 0; i < 255; i++)
                {
                    if (Main.player[i].active && (Projectile.Center - Main.player[i].Center).Length() < closest)
                    {
                        closest = (Projectile.Center - Main.player[i].Center).Length();
                        Projectile.ai[0] = (Main.player[i].Center - Projectile.Center).ToRotation();
                        Projectile.netUpdate = true;
                    }
                }
            }

            horiSpeed += (float)Math.Cos(Projectile.ai[0]) * horiAccCon;
            vertSpeed += (float)Math.Sin(Projectile.ai[0]) * vertAccCon;
            Projectile.velocity = new Vector2(horiSpeed, vertSpeed);

            if (Projectile.velocity.Length() > maxSpeed)
            {
                Projectile.velocity = Projectile.velocity.SafeNormalize(-Vector2.UnitY) * maxSpeed;
            }
            Projectile.rotation += (float)Math.PI / 30;
            for (int i = 0; i < 1; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<B4PDust>());
            }
            closest = 10000;
        }
    }

    public class MagicMineLayer : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magic Mine");
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = false;
        }

        public float horiSpeed;
        public float vertSpeed;
        public float direction;
        public float pullSpeed = .5f;
        public float dustSpeed = 20f;
        public NPC origin;

        public int frameTimer;
        public float distance;

        public override void AI()
        {
            origin = Main.npc[(int)Projectile.ai[0]];

            Player player = Main.player[Projectile.owner];

            if ((origin.Center - Projectile.Center).Length() > Projectile.ai[1])
            {
                Projectile.Kill();
            }
        }

        public override void Kill(int timeLeft)
        {
            if (Main.netMode != 1)
            {
                Projectile.NewProjectile(new EntitySource_Misc(""), Projectile.Center.X, Projectile.Center.Y, 0, 0, ProjectileType<MagicMine>(), Projectile.damage, 0, Main.myPlayer);
            }
        }
    }

    public class MagicMine : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magic Mine");
            Main.projFrames[Projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
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
        public float distance;

        public override void AI()
        {
            Projectile.velocity = new Vector2(0, 0);

            Player player = Main.player[Projectile.owner];

            frameTimer++;
            if (frameTimer % 5 == 0)
            {
                Projectile.frame++;
                if (Projectile.frame > 4)
                {
                    Projectile.frame = 0;
                }
            }
        }
    }
}