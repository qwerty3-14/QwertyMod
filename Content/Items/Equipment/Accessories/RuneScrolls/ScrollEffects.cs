using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Equipment.Accessories.RuneScrolls
{
    public class ScrollEffects : ModPlayer
    {
        public bool aggro = false;
        public bool ice = false;
        public bool leech = false;
        int leechCooldown = 0;
        public bool pursuit = false;
        public override void ResetEffects()
        {
            aggro = false;
            ice = false;
            leech = false;
            pursuit = false;
        }
        public override void PreUpdate()
        {
            if (aggro && Player.ownedProjectileCounts[ProjectileType<AggroRuneFriendly>()] < 1)
            {
                Projectile.NewProjectile(Player.GetProjectileSource_Misc(0), Player.Center, Vector2.Zero, ProjectileType<AggroRuneFriendly>(), (int)(500f * Player.GetDamage(DamageClass.Magic).Multiplicative), 0, Player.whoAmI);
            }
            if (ice && Player.ownedProjectileCounts[ProjectileType<IceRuneFreindly>()] < 1)
            {
                Projectile.NewProjectile(Player.GetProjectileSource_Misc(0), Player.Center, Vector2.Zero, ProjectileType<IceRuneFreindly>(), (int)(300f * Player.GetDamage(DamageClass.Melee).Multiplicative), 0, Player.whoAmI);
            }
            if(leechCooldown > 0)
            {
                leechCooldown--;
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (leech && proj.CountsAsClass(DamageClass.Ranged) && proj.type != ProjectileType<LeechRuneFreindly>() && leechCooldown == 0)
            {
                leechCooldown = 30;
                float theta = MathHelper.ToRadians(Main.rand.Next(0, 360));
                Projectile.NewProjectile(Player.GetProjectileSource_Misc(0), target.Center + QwertyMethods.PolarVector(150, theta), QwertyMethods.PolarVector(-10, theta), ProjectileType<LeechRuneFreindly>(), (int)(50 * Player.GetDamage(DamageClass.Ranged).Multiplicative), 3f, Main.myPlayer);

            }
        }
    }
}
