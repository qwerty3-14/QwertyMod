using Microsoft.Xna.Framework;
using QwertyMod.Content.Dusts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Consumable.Ammo.Dart.Rhuthinium
{
    public class RhuthiniumDartP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rhuthinium Dart");
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.width = Projectile.height = 14;
            Projectile.tileCollide = false;
            Projectile.penetrate = 3;
        }

        private bool start = true;
        private Vector2 flyOffset;
        private float acceleration = 1f;
        private float maxSpeed = 20f;

        private void SetFlyOffset()
        {
            Player player = Main.player[Projectile.owner];
            flyOffset = QwertyMethods.PolarVector(100, (player.Center - Projectile.Center).ToRotation() + Main.rand.NextFloat(-(float)Math.PI / 2, (float)Math.PI / 2));
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (start)
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + (float)Math.PI / 2;
                Projectile.velocity *= .8f;
                if (Projectile.velocity.Length() < .1f)
                {
                    start = false;
                    SetFlyOffset();
                }
            }
            else
            {
                if (Main.LocalPlayer == player)
                {
                    Projectile.rotation.SlowRotation((Main.MouseWorld - Projectile.Center).ToRotation() + (float)Math.PI / 2, (float)Math.PI / 30);
                }
                Projectile.velocity -= Projectile.velocity.SafeNormalize(Vector2.UnitY) * acceleration / 2;
                Projectile.velocity += (player.Center + flyOffset - Projectile.Center).SafeNormalize(Vector2.UnitY) * acceleration;

                if (Projectile.velocity.Length() > maxSpeed)
                {
                    Projectile.velocity = (player.Center + flyOffset - Projectile.Center).SafeNormalize(Vector2.UnitY) * maxSpeed;
                }
                if ((player.Center + flyOffset - Projectile.Center).Length() < Projectile.velocity.Length())
                {
                    SoundEngine.PlaySound(SoundID.Item12, Projectile.Center);
                    Projectile.Center = player.Center + flyOffset;
                    Projectile.velocity = Vector2.Zero;
                    Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center + QwertyMethods.PolarVector(10, Projectile.rotation - (float)Math.PI / 2), QwertyMethods.PolarVector(4, Projectile.rotation - (float)Math.PI / 2), ProjectileType<DartBeam>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    SetFlyOffset();
                    Projectile.penetrate--;
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 12; i++)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, DustType<RhuthiniumDust>());
                d.velocity *= 2;
                d.noGravity = true;
            }
        }
    }
}
