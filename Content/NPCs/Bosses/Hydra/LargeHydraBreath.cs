using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Content.Dusts;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.NPCs.Bosses.Hydra
{
    public class LargeHydraBreath : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Doom Breath");
            Main.projFrames[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.width = 96;
            Projectile.height = 54;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = false;
            Projectile.light = 1f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        private int frameCounter;
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
            float point = 0;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center + QwertyMethods.PolarVector(26, Projectile.rotation + MathF.PI / 2f), Projectile.Center + QwertyMethods.PolarVector(26, Projectile.rotation + MathF.PI / -2f), 54, ref point);
		}

        public override void AI()
        {
            frameCounter++;
            if (frameCounter > 20)
            {
                frameCounter = 0;
            }
            else if (frameCounter > 10)
            {
                Projectile.frame = 1;
            }
            else
            {
                Projectile.frame = 0;
            }
            CreateDust();
        }

        public virtual void CreateDust()
        {
            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<HydraBreathGlow>());
        }
    }
}
