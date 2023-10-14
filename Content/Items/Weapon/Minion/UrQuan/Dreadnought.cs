using Microsoft.Xna.Framework;
using QwertyMod.Common;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertyMod.Content.Items.Weapon.Minion.UrQuan
{
    class Dreadnought : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projFrames[Projectile.type] = 1;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            Main.projPet[Projectile.type] = true;
        }
        public override bool MinionContactDamage()
        {
            return false;
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Summon;
            Projectile.width = 80;
            Projectile.height = 66;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.minion = true;
            Projectile.minionSlots = 1;
            Projectile.timeLeft = 2;
        }

        private NPC target;
        private const float maxSpeed = 10f;
        private int shotCounter = 0;
        private int fighterCounter = 0;
        private List<Projectile> fighters = new List<Projectile>();

        private void Thrust()
        {
            Projectile.velocity += QwertyMethods.PolarVector(-.08f, Projectile.velocity.ToRotation());
            Projectile.velocity += QwertyMethods.PolarVector(.15f, Projectile.rotation);
            Dust d = Dust.NewDustPerfect(Projectile.Center + QwertyMethods.PolarVector(-40, Projectile.rotation), 6);
            d.noGravity = true;
            d.noLight = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            shotCounter++;
            fighterCounter--;
            if (player.GetModPlayer<MinionManager>().Dreadnought)
            {
                Projectile.timeLeft = 2;
            }
            if (QwertyMethods.ClosestNPC(ref target, 1000, Projectile.Center, false, player.MinionAttackTargetNPC) && (player.Center - Projectile.Center).Length() < 1000)
            {
                Projectile.rotation = QwertyMethods.SlowRotation(Projectile.rotation, (target.Center - Projectile.Center).ToRotation(), 4);
                if (fighterCounter <= 0 && fighters.Count < 10)
                {
                    fighterCounter = 60;
                    SoundEngine.PlaySound(new SoundStyle("QwertyMod/Assets/Sounds/UrQuan-Launch"), Projectile.Center);
                    fighters.Add(Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center + QwertyMethods.PolarVector(-40, Projectile.rotation) + QwertyMethods.PolarVector(10, Projectile.rotation + MathF.PI / 2), QwertyMethods.PolarVector(4, Projectile.rotation + 3 * MathF.PI / 4), ModContent.ProjectileType<Fighter>(), (int)(Projectile.damage / 6f), 0, Projectile.owner, Projectile.whoAmI)]);
                    fighters.Add(Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center + QwertyMethods.PolarVector(-40, Projectile.rotation) + QwertyMethods.PolarVector(10, Projectile.rotation - MathF.PI / 2), QwertyMethods.PolarVector(4, Projectile.rotation - 3 * MathF.PI / 4), ModContent.ProjectileType<Fighter>(), (int)(Projectile.damage / 6f), 0, Projectile.owner, Projectile.whoAmI)]);
                }
                if ((target.Center - Projectile.Center).Length() < 300)
                {
                    if (shotCounter >= 20)
                    {
                        shotCounter = 0;
                        SoundEngine.PlaySound(new SoundStyle("QwertyMod/Assets/Sounds/UrQuan-Fusion"), Projectile.Center);
                        Projectile l = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center + QwertyMethods.PolarVector(40f, Projectile.rotation), QwertyMethods.PolarVector(12f, Projectile.rotation), ModContent.ProjectileType<Fusion>(), Projectile.damage, Projectile.knockBack, Projectile.owner)];
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
                        Projectile.velocity += new Vector2(MathF.Cos((Projectile.Center - Main.projectile[k].Center).ToRotation()) * .1f, MathF.Sin((Projectile.Center - Main.projectile[k].Center).ToRotation()) * .1f);
                    }
                }
            }
            if (Projectile.velocity.Length() > maxSpeed)
            {
                Projectile.velocity = Projectile.velocity.SafeNormalize(-Vector2.UnitY) * maxSpeed;
            }
            for (int i = 0; i < fighters.Count; i++)
            {
                if (!fighters[i].active || fighters[i].type != ModContent.ProjectileType<Fighter>())
                {
                    fighters.RemoveAt(i);
                }
                else if (fighters[i].ai[1] == 0)
                {
                    fighters[i].timeLeft = 2;
                }
            }
            if ((player.Center - Projectile.Center).Length() > 2000)
            {
                fighters.Clear();
                Projectile.rotation = (player.Center - Projectile.Center).ToRotation();
                Projectile.Center = player.Center;
            }
        }
    }
}
