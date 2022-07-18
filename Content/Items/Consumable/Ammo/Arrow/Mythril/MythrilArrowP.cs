using Terraria;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Consumable.Ammo.Arrow.Mythril
{
    public class MythrilArrowP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mythril Arrow");
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.arrow = true;
            Projectile.timeLeft = 20;
            Projectile.tileCollide = false;
            Projectile.alpha = 0;
            Projectile.light = 0f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void AI()
        {
            Projectile.alpha = (int)(200 * (20f - (float)Projectile.timeLeft) / 20);
            Projectile.light = (float)Projectile.alpha / 255;
        }
    }
}
