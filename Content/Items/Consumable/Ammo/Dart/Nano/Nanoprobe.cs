using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Consumable.Ammo.Dart.Nano
{
    public class Nanoprobe : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.width = Projectile.height = 6;
            Projectile.extraUpdates = 5;
            Projectile.friendly = true;
            Projectile.timeLeft = 1200;
        }
        float dir;
        bool runOnce = true;
        int counter = 0;
        NPC target;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (counter < 40)
            {
                return false;
            }
            return base.Colliding(projHitbox, targetHitbox);
        }
        public override void AI()
        {
            if (runOnce)
            {
                dir = Projectile.velocity.ToRotation();
                runOnce = false;
            }
            Projectile.velocity = QwertyMethods.PolarVector(4.5f, dir);
            counter++;
            if (counter > 40)
            {
                if (QwertyMethods.ClosestNPC(ref target, 1000, Projectile.Center))
                {
                    dir.SlowRotation((target.Center - Projectile.Center).ToRotation(), (float)Math.PI / 60f);
                }
            }
            Dust d = Dust.NewDustPerfect(Projectile.Center, 135, Vector2.Zero, 100);
            d.noGravity = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Confused, 60 * 10);
        }
    }
}
