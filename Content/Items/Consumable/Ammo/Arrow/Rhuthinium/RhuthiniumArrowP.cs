using Microsoft.Xna.Framework;
using QwertyMod.Content.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Consumable.Ammo.Arrow.Rhuthinium
{
    public class RhuthiniumArrowP : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.arrow = true;
            Projectile.extraUpdates = 1;
            Projectile.tileCollide = true;
            AIType = ProjectileID.Bullet;
        }

        public override void AI()
        {
            if (Projectile.timeLeft < 3598)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, DustType<RhuthiniumDust>(), Vector2.Zero);
                d.frame.Y = Main.rand.NextBool(2) ? 0 : 10;
                d.noGravity = true;
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 12; i++)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, DustType<RhuthiniumDust>());
                d.velocity *= 2;
                d.noGravity = true;
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            Player player = Main.player[Projectile.owner];
            float distance = (player.Center - target.Center).Length();
            if (distance > 1200)
            {
                distance = 1200;
            }
            modifiers.FinalDamage *= 1 + (distance / 1200f) * 0.5f;
        }
    }
}
