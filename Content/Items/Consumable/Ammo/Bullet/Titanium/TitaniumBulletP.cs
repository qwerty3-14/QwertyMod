using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Consumable.Ammo.Bullet.Titanium
{
    public class TitaniumBulletP : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.extraUpdates = 1;
            Projectile.aiStyle = 0;
            AIType = ProjectileID.Bullet;
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.penetrate = 3;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.timeLeft = 300;
        }

        public bool runOnce = true;
        public float targetRotation;
        public float speed = .1f;

        public override void AI()
        {
            Projectile.rotation += MathF.PI / 15;
            Projectile.velocity *= .95f;
            if(Projectile.timeLeft > 270)
            {
                
                Projectile.scale = 1 - ((Projectile.timeLeft - 270) / 30f);
            }
            if(Projectile.timeLeft < 6)
            {
                Projectile.scale = (Projectile.timeLeft / 6f);
            }
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.HitDirectionOverride = -1 * MathF.Sign(target.Center.X - Projectile.Center.X);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.damage = (int)(Projectile.damage * .6f);
        }
        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            for(int i = 0; i < 5; i++)
            { 
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Titanium, Scale: 0.5f);
            }
        }
    }
}
