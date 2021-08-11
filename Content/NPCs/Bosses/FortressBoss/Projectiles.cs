using Microsoft.Xna.Framework;
using QwertyMod.Content.Buffs;
using QwertyMod.Content.Dusts;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.NPCs.Bosses.FortressBoss
{
    public class BarrierSpread : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Barrier Spread");
            Main.projFrames[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            //aiType = ProjectileID.Bullet;
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 1200;
            Projectile.tileCollide = false;
        }

        public int dustTimer;
        private Projectile clearCheck;

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Dust dust2 = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<CaeliteDust>())];
            dust2.scale = .5f;
            if (Projectile.frameCounter % 10 == 0)
            {
                Projectile.frame++;
                if (Projectile.frame > 1)
                {
                    Projectile.frame = 0;
                }
            }
            for (int p = 0; p < 1000; p++)
            {
                clearCheck = Main.projectile[p];
                if (clearCheck.friendly && !clearCheck.sentry && clearCheck.minionSlots <= 0 && Collision.CheckAABBvAABBCollision(Projectile.position, Projectile.Size, clearCheck.position, clearCheck.Size))
                {
                    clearCheck.Kill();
                }
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (target.HasBuff(BuffID.PotionSickness))
            {
                target.buffTime[target.FindBuffIndex(BuffID.PotionSickness)] += 600;
            }
            else
            {
                target.AddBuff(BuffID.PotionSickness, 600);
            }
        }
    }

    public class Deflect : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Static Barrier");

            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.light = .6f;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 1200;
        }

        public int dustTimer;
        private Projectile clearCheck;
        private int counter = 60;

        public override void AI()
        {
            Projectile.rotation += (float)Math.PI / 30;
            if (Projectile.timeLeft < (60 * 30))
            {
                Vector2 flyTo = new Vector2(Projectile.ai[0], Projectile.ai[1]);
                Projectile.velocity = (flyTo - Projectile.Center) * .08f;
            }
            for (int p = 0; p < 1000; p++)
            {
                clearCheck = Main.projectile[p];
                if (clearCheck.friendly && !clearCheck.sentry && clearCheck.velocity != Vector2.Zero && clearCheck.damage > 0 && clearCheck.minionSlots <= 0 && Collision.CheckAABBvAABBCollision(Projectile.position, Projectile.Size, clearCheck.position, clearCheck.Size))
                {
                    clearCheck.Kill();
                }
            }
            
        }
    }

    public class CaeliteSaw : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Caelite Saw");
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            //aiType = ProjectileID.Bullet;
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = 6;
            Projectile.light = .6f;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 600;
        }


        public override void AI()
        {
            Projectile.rotation += (float)Math.PI / 7.5f;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, NPCType<FortressBoss>());
            }
        }

        public override bool OnTileCollide(Vector2 velocityChange)
        {
            Projectile.penetrate--;
            if (Projectile.velocity.X != velocityChange.X)
            {
                Projectile.velocity.X = -velocityChange.X;
            }
            if (Projectile.velocity.Y != velocityChange.Y)
            {
                Projectile.velocity.Y = -velocityChange.Y;
            }
            return false;
        }


        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffType<PowerDown>(), 300);
        }
    }
    public class DivineBolt : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Divine Bolt");
            Main.projFrames[Type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = 3;
            Projectile.light = .6f;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            Projectile.frameCounter++;
            if(Projectile.frameCounter % 6 ==0)
            {
                Projectile.frame++;
                if(Projectile.frame >= 4)
                {
                    Projectile.frame = 0;
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.White;
            return base.PreDraw(ref lightColor);
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffType<PowerDown>(), 300);
        }
    }
}
