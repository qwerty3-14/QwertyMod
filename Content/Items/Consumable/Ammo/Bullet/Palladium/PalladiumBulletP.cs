using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace QwertyMod.Content.Items.Consumable.Ammo.Bullet.Palladium
{
    public class PalladiumBulletP : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            Projectile.extraUpdates = 1;

            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 300;
        }

        public bool runOnce = true;
        public bool HasRightClicked = false;

        public float targetRotation;

        public override void AI()
        {
            
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(HasRightClicked);
            writer.Write(runOnce);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            HasRightClicked = reader.ReadBoolean();
            runOnce = reader.ReadBoolean();
        }
        
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if(target.life < 0)
            {
                Item.NewItem(target.GetSource_DropAsItem(), target.position, target.Size, ItemID.Heart);
                Item.NewItem(target.GetSource_DropAsItem(), target.position, target.Size, ItemID.Heart);
            }
        }
        public override void OnKill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
        }
    }
}
