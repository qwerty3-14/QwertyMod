using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;

namespace QwertyMod.Content.Items.Consumable.Ammo.Arrow.Adamantite
{
    public class AdamantiteArrowP : ModProjectile
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
            Projectile.tileCollide = true;
        }



        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            for(int i = 0; i < 5; i++)
            { 
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Adamantite, Scale: 0.5f);
            }
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.Knockback *= 0;
            if(!target.dontTakeDamage && !target.immortal)
            {
                target.velocity += Projectile.velocity.SafeNormalize(Vector2.UnitY) * 4;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundEngine.PlaySound(new SoundStyle("QwertyMod/Assets/Sounds/PUNCH"), Projectile.Center);
        }
    }
}
