using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Consumable.Ammo.Dart.Rhuthinium
{
    public class DartBeam : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rhuthinium Dart");
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.width = Projectile.height = 6;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.timeLeft = 350;
            Projectile.extraUpdates = 349;
            Projectile.friendly = true;
        }

        private bool decaying = false;
        private bool cantHit = false;

        private void StartDecay()
        {
            if (!decaying)
            {
                Projectile.extraUpdates = 0;
                Projectile.timeLeft = 30;
                Projectile.velocity = Vector2.Zero;
                decaying = true;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[Projectile.owner] = 0;
            cantHit = true;
            Projectile.velocity = Vector2.Zero;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            cantHit = true;
            Projectile.velocity = Vector2.Zero;
            return false;
        }

        private Vector2 start;
        private bool runOnce = true;

        public override void AI()
        {
            if (runOnce)
            {
                runOnce = false;
                start = Projectile.Center;
            }
            if (Projectile.timeLeft == 2 && !decaying)
            {
                StartDecay();
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (!runOnce)
            {
                for (int d = 0; d < (Projectile.Center - start).Length(); d += 4)
                {
                    float rot = (Projectile.Center - (Vector2)start).ToRotation();
                    int c = decaying ? (int)(255f * Projectile.timeLeft / 30f) : 255;
                    Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, start + QwertyMethods.PolarVector(d, rot) - Main.screenPosition, null, new Color(c, c, c, c), rot, Vector2.UnitY * 3, Vector2.One, SpriteEffects.None, 0);
                }
            }
            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (cantHit)
            {
                return false;
            }
            return base.Colliding(projHitbox, targetHitbox);
        }
    }
}
