using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Weapon.Minion.SpaceFighter
{
    public class FighterLaser : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.light = 0.75f;
            Projectile.alpha = 255;
            Projectile.extraUpdates = 2;
            Projectile.scale = 1f;
            Projectile.timeLeft = 600;
            Projectile.DamageType = DamageClass.Summon;
            AIType = 20;
        }

    }
}
