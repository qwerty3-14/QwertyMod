using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Consumable.Ammo.Bullet.Palladium
{
    public class PalladiumBulletP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Palladium Bullet");
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;

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
            Player player = Main.player[Projectile.owner];

            if (Main.mouseRight && Projectile.timeLeft <= 290 || HasRightClicked)
            {
                Projectile.alpha = 0;
                if (runOnce)
                {
                    HasRightClicked = true;
                    Projectile.timeLeft = 3600;
                    runOnce = false;
                    Projectile.netUpdate = true;
                }

                Projectile.velocity.X = (float)Math.Cos(targetRotation + MathHelper.ToRadians(-90)) * 20f;
                Projectile.velocity.Y = (float)Math.Sin(targetRotation + MathHelper.ToRadians(-90)) * 20f;
            }
            else
            {
                Projectile.alpha = (int)(255f - ((float)Projectile.timeLeft / 300f) * 255f);

                if (Main.LocalPlayer == player)
                {
                    Projectile.ai[0] = Main.MouseWorld.X;
                    Projectile.ai[1] = Main.MouseWorld.Y;
                    Projectile.netUpdate = true;

                    //Projectile.netUpdate = true;
                }
                targetRotation = (new Vector2(Projectile.ai[0], Projectile.ai[1]) - Projectile.Center).ToRotation() + (float)Math.PI / 2;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + (float)Math.PI / 2;
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

        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
        }
    }
}
