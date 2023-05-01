using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Weapon.Minion.GlassSpike
{
    public class GlassSpike : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Glass Spike");
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
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Main.projFrames[Projectile.type] = 1;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.minion = true;
            Projectile.minionSlots = 1;
            Projectile.timeLeft = 2;
            Projectile.aiStyle = -1;
            Projectile.usesIDStaticNPCImmunity = true;
        }

        private float orientation = 0;
        private bool orientationSet = false;
        private bool spin = true;

        public override void AI()
        {
            if (spin)
            {
                Projectile.rotation += Projectile.velocity.Length() * MathF.PI / 60 * (Projectile.velocity.X > 0 ? 1 : -1);
            }
            spin = true;
            Player player = Main.player[Projectile.owner];
            if (player.GetModPlayer<MinionManager>().GlassSpike)
            {
                Projectile.timeLeft = 2;
            }

            if ((player.Center - Projectile.Center).Length() > 400)
            {
                Projectile.tileCollide = false;
            }

            if (Projectile.tileCollide)
            {
                Projectile.velocity.Y = 7;
                Projectile.velocity.X *= .9f;
            }
            else
            {
                Vector2 flyTo = player.Center + new Vector2(orientation, -20);
                if ((Projectile.Center - flyTo).Length() < 20)
                {
                    Projectile.tileCollide = true;
                }
                Vector2 vel = (flyTo - Projectile.Center) * .07f;
                if (vel.Length() < 3)
                {
                    vel = vel.SafeNormalize(-Vector2.UnitY) * 3;
                }
                Projectile.velocity = vel;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(texture, new Vector2(Projectile.Center.X - Main.screenPosition.X, Projectile.Center.Y - Main.screenPosition.Y + 2),
                        new Rectangle(0, 0, texture.Width, texture.Height), lightColor, Projectile.rotation,
                        new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), 1f, SpriteEffects.None, 0);
            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = Vector2.Zero;
            if (!orientationSet)
            {
                orientation = Projectile.Center.X - Main.player[Projectile.owner].Center.X;
                orientationSet = true;
            }
            spin = false;
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.perIDStaticNPCImmunity[Projectile.type][target.whoAmI] = (uint)(Main.GameUpdateCount + 10);
        }
    }
}
