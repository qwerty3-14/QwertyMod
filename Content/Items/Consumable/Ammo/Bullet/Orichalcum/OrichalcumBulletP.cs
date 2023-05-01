﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Consumable.Ammo.Bullet.Orichalcum
{
    public class OrichalcumBulletP : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = 2;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.extraUpdates = 2;
        }

        public bool runOnce = true;
        private float maxSpeed;

        public override void AI()
        {
            if (runOnce)
            {
                maxSpeed = Projectile.velocity.Length();
                runOnce = false;
            }
        }

        public bool firstHit = true;

        private NPC ConfirmedTarget;

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[Projectile.owner] = 0;

            if (QwertyMethods.ClosestNPC(ref ConfirmedTarget, 300, Projectile.Center, specialCondition: delegate (NPC possibleTarget) { return Projectile.localNPCImmunity[possibleTarget.whoAmI] == 0; }))
            {
                Projectile.velocity = QwertyMethods.PolarVector(maxSpeed, (ConfirmedTarget.Center - Projectile.Center).ToRotation());
            }
            else
            {
                Projectile.Kill();
            }
        }

        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
        }
    }
}
