using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Consumable.Ammo.Dart.Venom
{
    public class VenomCloud : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
        }
        public override void SetDefaults()
        {
            Projectile.tileCollide = false;
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.scale = 1.1f;
            Projectile.penetrate = -1;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.timeLeft = 30;
        }
        public override void AI()
        {
            int num322 = 6;

            Projectile.velocity *= 0.96f;
            Projectile.alpha += 4;
            if (Projectile.alpha > 255)
            {
                Projectile.Kill();
            }

            if (++Projectile.frameCounter >= num322)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.perIDStaticNPCImmunity[Projectile.type][target.whoAmI] = Main.GameUpdateCount + 10;
            target.immune[Projectile.owner] = 0;
            target.AddBuff(BuffID.Venom, 60 * 30);
        }
    }
}
