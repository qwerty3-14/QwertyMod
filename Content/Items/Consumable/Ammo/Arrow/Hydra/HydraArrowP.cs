using QwertyMod.Content.Dusts;
using System;
using Terraria;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Consumable.Ammo.Arrow.Hydra
{
    public class HydraArrowP : ModProjectile
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
            Projectile.timeLeft = 300;
            Projectile.DamageType = DamageClass.Ranged;
        }

        public override void AI()
        {
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<HydraBeamGlow>());
            if (Projectile.owner == Main.myPlayer && Projectile.ai[1] == 0 && Projectile.timeLeft == 298)
            {
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, QwertyMethods.PolarVector(Projectile.velocity.Length(), Projectile.velocity.ToRotation() + MathF.PI / 8), Type, (int)((float)Projectile.damage * .5f), Projectile.knockBack * .5f, Projectile.owner, 1, 1);
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, QwertyMethods.PolarVector(Projectile.velocity.Length(), Projectile.velocity.ToRotation() - MathF.PI / 8), Type, (int)((float)Projectile.damage * .5f), Projectile.knockBack * .5f, Projectile.owner, 1, 1);
                Projectile.ai[1] = 1;
            }
        }
        public override void OnKill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            for(int i = 0; i < 5; i++)
            { 
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<HydraBeamGlow>(), Scale: 0.5f);
            }
        }


    }
}
