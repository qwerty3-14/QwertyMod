using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace QwertyMod.Content.Items.Weapon.Minion.ChloroSniper
{
    public class ChlorophyteSnipe : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Chlorophyte Snipe");
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 9;
            Projectile.timeLeft = 1200;
        }

        public override void AI()
        {
            for (int num163 = 0; num163 < 5; num163++)
            {
                float x2 = Projectile.position.X - Projectile.velocity.X / 10f * (float)num163;
                float y2 = Projectile.position.Y - Projectile.velocity.Y / 10f * (float)num163;
                int num164 = Dust.NewDust(new Vector2(x2, y2), 1, 1, DustID.CursedTorch, 0f, 0f, 0, default(Color), 1f);
                Main.dust[num164].alpha = Projectile.alpha;
                Main.dust[num164].position.X = x2;
                Main.dust[num164].position.Y = y2;
                Main.dust[num164].velocity *= 0f;
                Main.dust[num164].noGravity = true;
            }
        }
    }
}
