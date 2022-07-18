using Microsoft.Xna.Framework;
using QwertyMod.Content.Buffs;
using QwertyMod.Content.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Consumable.Ammo.Bullet.Caelite
{
    public class CaeliteBulletP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Caelite Bullet");
        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.light = 0.5f;
            Projectile.scale = 1.2f;
            Projectile.timeLeft = 600;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.extraUpdates = 1;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }

        public override void AI()
        {
            for (int d = 0; d < 1; d++)
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustType<CaeliteDust>(), Vector2.Zero);
                dust.frame.Y = 0;
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 16; i++)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, DustType<CaeliteDust>());
                d.frame.Y = 0;
                d.velocity *= 2;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffType<PowerDown>(), 300);
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffType<PowerDown>(), 300);
        }
    }
}
