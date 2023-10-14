using QwertyMod.Common;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Weapon.Minion.UrQuan
{
    public class Fusion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Summon;
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 60;
            Projectile.tileCollide = false;
            Projectile.GetGlobalProjectile<QwertyGlobalProjectile>().ignoresArmor = true;
        }

    }
}
