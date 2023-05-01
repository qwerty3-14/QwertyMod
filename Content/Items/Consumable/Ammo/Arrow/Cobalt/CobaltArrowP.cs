using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Consumable.Ammo.Arrow.Cobalt
{
    public class CobaltArrowP : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.arrow = true;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = true;
            Projectile.alpha = 0;
        }

        public bool HasRightClicked = false;
        public bool runOnce = true;
        public float targetRotation;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if ((Main.mouseRight && Projectile.timeLeft <= 290 && Main.myPlayer == Projectile.owner || HasRightClicked))
            {
                Projectile.alpha = 0;
                if (runOnce)
                {
                    HasRightClicked = true;

                    Projectile.timeLeft = 3600;
                    runOnce = false;
                    Projectile.netUpdate = true;
                }

                Projectile.velocity.X = MathF.Cos(targetRotation + MathHelper.ToRadians(-90)) * 10f;
                Projectile.velocity.Y = MathF.Sin(targetRotation + MathHelper.ToRadians(-90)) * 10f;
            }
            else
            {
                Projectile.alpha = (int)(255f - ((float)Projectile.timeLeft / 300f) * 255f);
                Projectile.velocity.X = 0;
                Projectile.velocity.Y = 0;
                if (Main.LocalPlayer == player)
                {
                    Projectile.ai[0] = (Main.MouseWorld - Projectile.Center).ToRotation();
                    if (Projectile.ai[1] == 1)
                    {
                        Projectile.ai[0] += MathF.PI;
                    }
                    Projectile.netUpdate = true;
                }
                targetRotation = Projectile.ai[0] + MathF.PI / 2;
                Projectile.rotation = targetRotation;
            }
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
