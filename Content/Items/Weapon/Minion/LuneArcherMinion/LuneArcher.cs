using QwertyMod.Common;
using Terraria;
using Terraria.ID;

namespace QwertyMod.Content.Items.Weapon.Minion.LuneArcherMinion
{
    class LuneArcher : BowMinionBase
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            Main.projPet[Projectile.type] = true;
        }
        public override bool MinionContactDamage()
        {
            return false;
        }
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.minion = true;
            Projectile.minionSlots = 1;
            Projectile.timeLeft = 2;

            cycleTIme = 80;
            shootSpeed = 12;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (player.GetModPlayer<MinionManager>().LuneArcher)
            {
                Projectile.timeLeft = 2;
            }
            BowAI();
        }
    }
}
