using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Weapon.Minion
{
    public abstract class BowMinionBase : ModProjectile
    {

        protected float shootSpeed = 12;
        protected int cycleTIme = 80;
        protected float holdOffset = 20;
        protected float rotSpeed = (float)Math.PI / 30f;

        private NPC target;
        private int timer;
        private int arrow = 1;
        Projectile loadedArrow;
        private bool giveTileCollision = false;
        float aimRotation = 0;
        float targetRotation = 0;
        bool arrowFired = true;
        int safetyIdCheck = -1;


        protected void BowAI()
        {
            Player player = Main.player[Projectile.owner];
            int identity = 0;
            int bowCount = 0;
            bool foundIdentiy = false;
            for (int p = 0; p < 1000; p++)
            {
                if (Main.projectile[p].owner == Projectile.owner && Main.projectile[p].active && Main.projectile[p].ModProjectile != null && Main.projectile[p].ModProjectile is BowMinionBase)
                {
                    bowCount++;
                    if (!foundIdentiy)
                    {
                        if (p == Projectile.whoAmI)
                        {
                            foundIdentiy = true;
                        }
                        else
                        {

                            identity++;
                        }
                    }
                }
            }

            if (bowCount != 0)
            {
                timer++;
                Projectile.velocity = Vector2.Zero;
                Vector2 RestSpot = player.Center + QwertyMethods.PolarVector(50f + 8f * bowCount, -(float)Math.PI * ((float)(identity + 1) / (bowCount + 1))) + Vector2.UnitY * 10;
                if (timer >= cycleTIme)
                {
                    Projectile.Center = RestSpot;
                    Aim();
                }
                else
                {
                    IdleTurn();
                    int dist = Math.Abs(timer - cycleTIme / 2);
                    Projectile.Center = player.Center + ((RestSpot - player.Center) / (cycleTIme / 2)) * dist;
                }
                if (timer >= (cycleTIme / 2) && arrowFired)
                {
                    LoadArrow();
                }
                if (!arrowFired && loadedArrow != null)
                {
                    HoldArrow();
                }
                else if (loadedArrow != null)
                {
                    FloatArrow();
                }
            }
        }
        void LoadArrow()
        {
            int weaponDamage = Projectile.damage;
            float weaponKnockback = Projectile.knockBack;
            if (Projectile.owner == Main.myPlayer && Projectile.UseAmmo(AmmoID.Arrow, ref arrow, ref shootSpeed, ref weaponDamage, ref weaponKnockback, Main.rand.Next(2) == 0))
            {
                ChangeArrow(ref arrow);
                loadedArrow = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, QwertyMethods.PolarVector(shootSpeed, Projectile.rotation), arrow, weaponDamage, weaponKnockback, Main.myPlayer)];
                arrowFired = false;
                safetyIdCheck = loadedArrow.type;
            }
        }
        protected virtual void ChangeArrow(ref int arrowType)
        {

        }
        void HoldArrow()
        {
            loadedArrow.velocity = QwertyMethods.PolarVector(0.01f, Projectile.rotation - (float)Math.PI / 2);
            loadedArrow.Center = Projectile.Center + QwertyMethods.PolarVector(holdOffset - loadedArrow.velocity.Length() * (loadedArrow.extraUpdates + 1), Projectile.rotation - (float)Math.PI / 2);
            loadedArrow.friendly = false;
            loadedArrow.rotation = Projectile.rotation;
            loadedArrow.timeLeft += loadedArrow.extraUpdates + 1;
            loadedArrow.ai[0] = 0;
            if (loadedArrow.tileCollide)
            {
                giveTileCollision = true;
                loadedArrow.tileCollide = false;
            }
        }
        void Fire()
        {
            if (loadedArrow != null && loadedArrow.active && loadedArrow.type == safetyIdCheck)
            {
                if (giveTileCollision)
                {
                    loadedArrow.tileCollide = true;
                    giveTileCollision = false;
                }
                SoundEngine.PlaySound(SoundID.Item5, Projectile.Center);
                loadedArrow.velocity = QwertyMethods.PolarVector(shootSpeed, Projectile.rotation - (float)Math.PI / 2);
                loadedArrow.friendly = true;
            }
            arrowFired = true;
            timer = 0;
            shootSpeed = 12;

        }
        void Aim()
        {
            Player player = Main.player[Projectile.owner];
            if (loadedArrow != null)
            {
                float range = Math.Min(1200, loadedArrow.timeLeft * shootSpeed * (loadedArrow.extraUpdates + 1) - 40);
                if (QwertyMethods.ClosestNPC(ref target, range, Projectile.Center, !giveTileCollision, player.MinionAttackTargetNPC))
                {
                    float rot = QwertyMethods.PredictiveAim(Projectile.Center, shootSpeed * (loadedArrow.extraUpdates + 1), target.Center, target.velocity);
                    if (!float.IsNaN(rot))
                    {
                        targetRotation = rot;
                    }
                    else
                    {
                        targetRotation = (target.Top - Projectile.Center).ToRotation();
                    }
                    aimRotation.SlowRotation(targetRotation, rotSpeed);

                    Projectile.rotation = aimRotation + (float)Math.PI / 2;
                    if (aimRotation == targetRotation)
                    {
                        Fire();
                    }
                }
                else
                {
                    IdleTurn();
                }
            }
        }
        void IdleTurn()
        {
            Player player = Main.player[Projectile.owner];
            targetRotation = (Projectile.Center - player.Center).ToRotation();
            if (timer - (cycleTIme / 2) < 0)
            {
                targetRotation += (float)Math.PI;
            }
            aimRotation.SlowRotation(targetRotation, rotSpeed);
            Projectile.rotation = aimRotation + (float)Math.PI / 2;
        }
        void FloatArrow()
        {
            if (loadedArrow != null && loadedArrow.active && loadedArrow.type == safetyIdCheck)
            {
                loadedArrow.ai[0] = 0;
            }
        }
        public override void Kill(int timeLeft)
        {
            Fire();
        }
    }
}
