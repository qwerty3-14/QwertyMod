using Microsoft.Xna.Framework;
using QwertyMod.Common;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Minion.SpaceFighter
{
    public class SpaceFighter : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Space Fighter");
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true; //This is necessary for right-click targeting
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.hostile = false;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Main.projFrames[Projectile.type] = 1;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.minion = true;
            Projectile.minionSlots = 1;
            Projectile.timeLeft = 2;
            Projectile.aiStyle = -1;
        }

        private NPC target;
        private const float maxSpeed = 12f;
        private int shotCounter = 0;

        private void Thrust()
        {
            Projectile.velocity += QwertyMethods.PolarVector(-.1f, Projectile.velocity.ToRotation());
            Projectile.velocity += QwertyMethods.PolarVector(.2f, Projectile.rotation);
            Dust d = Dust.NewDustPerfect(Projectile.Center + QwertyMethods.PolarVector(-8, Projectile.rotation) + QwertyMethods.PolarVector(12, Projectile.rotation + (float)Math.PI / 2), 6);
            d.noGravity = true;
            d.noLight = true;
            d = Dust.NewDustPerfect(Projectile.Center + QwertyMethods.PolarVector(-8, Projectile.rotation) + QwertyMethods.PolarVector(-12, Projectile.rotation + (float)Math.PI / 2), 6);
            d.noGravity = true;
            d.noLight = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            shotCounter++;
            if (player.GetModPlayer<MinionManager>().SpaceFighter)
            {
                Projectile.timeLeft = 2;
            }
            if (QwertyMethods.ClosestNPC(ref target, 1000, Projectile.Center, false, player.MinionAttackTargetNPC) && (player.Center - Projectile.Center).Length() < 1000)
            {
                Projectile.rotation = QwertyMethods.SlowRotation(Projectile.rotation, (target.Center - Projectile.Center).ToRotation(), 6);
                if ((target.Center - Projectile.Center).Length() < 300)
                {
                    if (shotCounter >= 30)
                    {
                        shotCounter = 0;
                        Projectile l = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, QwertyMethods.PolarVector(12f, Projectile.rotation), ProjectileType<FighterLaser>(), Projectile.damage, Projectile.knockBack, Projectile.owner)];
                        SoundEngine.PlaySound(SoundID.Item12, Projectile.position);
                    }
                }
                else
                {
                    Thrust();
                }
            }
            else
            {
                Projectile.rotation = QwertyMethods.SlowRotation(Projectile.rotation, (player.Center - Projectile.Center).ToRotation(), 6);
                if ((player.Center - Projectile.Center).Length() < 300)
                {
                }
                else
                {
                    Thrust();
                }
            }
            for (int k = 0; k < 1000; k++)
            {
                if (Main.projectile[k].type == Projectile.type && k != Projectile.whoAmI)
                {
                    if (Collision.CheckAABBvAABBCollision(Projectile.position + new Vector2(Projectile.width / 4, Projectile.height / 4), new Vector2(Projectile.width / 2, Projectile.height / 2), Main.projectile[k].position + new Vector2(Main.projectile[k].width / 4, Main.projectile[k].height / 4), new Vector2(Main.projectile[k].width / 2, Main.projectile[k].height / 2)))
                    {
                        Projectile.velocity += new Vector2((float)Math.Cos((Projectile.Center - Main.projectile[k].Center).ToRotation()) * .1f, (float)Math.Sin((Projectile.Center - Main.projectile[k].Center).ToRotation()) * .1f);
                    }
                }
            }
            if (Projectile.velocity.Length() > maxSpeed)
            {
                Projectile.velocity = Projectile.velocity.SafeNormalize(-Vector2.UnitY) * maxSpeed;
            }
            if ((player.Center - Projectile.Center).Length() > 2000)
            {
                Projectile.rotation = (player.Center - Projectile.Center).ToRotation();
                Projectile.Center = player.Center;
            }
        }
    }
}
