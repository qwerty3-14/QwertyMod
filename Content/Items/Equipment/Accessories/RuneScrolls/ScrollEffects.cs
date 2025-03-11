using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;


namespace QwertyMod.Content.Items.Equipment.Accessories.RuneScrolls
{
    public class ScrollEffects : ModPlayer
    {
        public int aggro = 0;
        public int ice = 0;
        public int leech = 0;
        int leechCooldown = 0;
        public int pursuit = 0;
        public override void ResetEffects()
        {
            aggro = 0;
            ice = 0;
            leech = 0;
            pursuit = 0;
        }
        public override void PreUpdate()
        {
            if (aggro > 0 && Player.ownedProjectileCounts[ ModContent.ProjectileType<AggroRuneFriendly>()] < aggro)
            {
                Projectile.NewProjectile(new EntitySource_Misc("Accesory_AggroRuneScroll"), Player.Center, Vector2.Zero, ModContent.ProjectileType<AggroRuneFriendly>(), (int)(500f * Player.GetDamage(DamageClass.Magic).Multiplicative), 0, Player.whoAmI);
            }
            if (ice > 0 && Player.ownedProjectileCounts[ ModContent.ProjectileType<IceRuneFreindly>()] < 1)
            {
                Projectile.NewProjectile(new EntitySource_Misc("Accesory_IceRuneScroll"), Player.Center, Vector2.Zero, ModContent.ProjectileType<IceRuneFreindly>(), (int)(300f * Player.GetDamage(DamageClass.Melee).Multiplicative), 0, Player.whoAmI);
            }
            if (leechCooldown > 0)
            {
                leechCooldown -= leech;
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (leech > 0 && proj.CountsAsClass(DamageClass.Ranged) && proj.type != ModContent.ProjectileType<LeechRuneFreindly>() && leechCooldown == 0)
            {
                leechCooldown = 30;
                float theta = MathHelper.ToRadians(Main.rand.Next(0, 360));
                Projectile.NewProjectile(proj.GetSource_FromThis(), target.Center + QwertyMethods.PolarVector(150, theta), QwertyMethods.PolarVector(-10, theta), ModContent.ProjectileType<LeechRuneFreindly>(), (int)(50 * Player.GetDamage(DamageClass.Ranged).Multiplicative), 3f, Main.myPlayer);

            }
        }
    }
}
