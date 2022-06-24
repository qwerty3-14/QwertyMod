using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.NPCs.Bosses.Hydra
{
    // The following laser shows a channeled ability, after charging up the laser will be fired
    // Using custom drawing, dust effects, and custom collision checks for tiles
    public class HydraBeamT : ModProjectile
    {
        //The distance charge particle from the player center
        private const float MoveDistance = 70f;

        // The actual distance is stored in the ai0 field
        // By making a property to handle this it makes our life easier, and the accessibility more readable
        public float Distance;

        public NPC shooter;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydra Beam");
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.hostile = true;
            Projectile.hide = false;
        }

        // The AI of the projectile
        public bool runOnce = true;

        public override void AI()
        {
            float rOffset = 0;
            shooter = Main.npc[(int)Projectile.ai[0]];

            Vector2 mousePos = Main.MouseWorld;
            Player player = Main.player[Projectile.owner];
            if (!shooter.active || shooter.life <= 0)
            {
                Projectile.Kill();
            }

            #region Set projectile position

            Vector2 diff = new Vector2((float)Math.Cos(shooter.rotation + rOffset) * 14f, (float)Math.Sin(shooter.rotation + rOffset) * 14f);
            diff.Normalize();
            Projectile.velocity = diff;
            Projectile.direction = Projectile.Center.X > shooter.Center.X ? 1 : -1;
            Projectile.netUpdate = true;

            Projectile.position = new Vector2(shooter.Center.X, shooter.Center.Y) + Projectile.velocity * MoveDistance;
            Projectile.timeLeft = 2;
            int dir = Projectile.direction;
            /*
            player.ChangeDir(dir);
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = (float)Math.Atan2(Projectile.velocity.Y * dir, Projectile.velocity.X * dir);
            */

            #endregion Set projectile position

            #region Charging process

            // Kill the projectile if the player stops channeling

            // Do we still have enough mana? If not, we kill the projectile because we cannot use it anymore

            Vector2 offset = Projectile.velocity;
            offset *= MoveDistance - 20;
            Vector2 pos = new Vector2(shooter.Center.X, shooter.Center.Y) + offset - new Vector2(10, 10);

            #endregion Charging process

            Vector2 start = new Vector2(shooter.Center.X, shooter.Center.Y);
            Vector2 unit = Projectile.velocity;
            unit *= -1;
            for (Distance = MoveDistance; Distance <= 2200f; Distance += 5f)
            {
                start = new Vector2(shooter.Center.X, shooter.Center.Y) + Projectile.velocity * Distance;
            }
        }

        public int colorCounter;
        public Color lineColor;

        public override bool PreDraw(ref Color lightColor)
        {
            DrawLaser(TextureAssets.Projectile[Projectile.type].Value, shooter.Center,
                Projectile.velocity, 10, Projectile.damage, -1.57f, 1f, 4000f, Color.White, (int)MoveDistance);

            return false;
        }

        // The core function of drawing a laser
        public void DrawLaser(Texture2D texture, Vector2 start, Vector2 unit, float step, int damage, float rotation = 0f, float scale = 1f, float maxDist = 4000f, Color color = default(Color), int transDist = 50)
        {
            Vector2 origin = start;
            float r = unit.ToRotation() + rotation;

            #region Draw laser body

            for (float i = transDist; i <= Distance; i += step)
            {
                Color c = Color.White;
                origin = start + i * unit;
                Main.EntitySpriteDraw(texture, origin - Main.screenPosition,
                    new Rectangle(0, 26, 28, 26), i < transDist ? Color.Transparent : c, r,
                    new Vector2(28 * .5f, 26 * .5f), scale, 0, 0);
            }

            #endregion Draw laser body

            #region Draw laser tail

            Main.EntitySpriteDraw(texture, start + unit * (transDist - step) - Main.screenPosition,
                new Rectangle(0, 0, 28, 26), Color.White, r, new Vector2(28 * .5f, 26 * .5f), scale, 0, 0);

            #endregion Draw laser tail

            #region Draw laser head

            Main.EntitySpriteDraw(texture, start + (Distance + step) * unit - Main.screenPosition,
                new Rectangle(0, 52, 28, 26), Color.White, r, new Vector2(28 * .5f, 26 * .5f), scale, 0, 0);

            #endregion Draw laser head
        }

        // Change the way of collision check of the projectile
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            // We can only collide if we are at max charge, which is when the laser is actually fired

            Player player = Main.player[Projectile.owner];
            Vector2 unit = Projectile.velocity;
            float point = 0f;
            // Run an AABB versus Line check to look for collisions, look up AABB collision first to see how it works
            // It will look for collisions on the given line using AABB
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), new Vector2(shooter.Center.X, shooter.Center.Y),
                new Vector2(shooter.Center.X, shooter.Center.Y) + unit * Distance, 22, ref point);
        }

        // Set custom immunity time on hitting an NPC
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 5;
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        /*
        public override void CutTiles()
        {
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Vector2 unit = Projectile.velocity;
            Utils.PlotTileLine(Projectile.Center, Projectile.Center + unit * Distance, (Projectile.width + 16) * Projectile.scale, DelegateMethods.CutTiles);
        }
        */
    }
}