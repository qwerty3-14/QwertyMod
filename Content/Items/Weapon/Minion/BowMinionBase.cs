using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;


namespace QwertyMod.Content.Items.Weapon.Minion
{
    public abstract class BowMinionBase : ModProjectile
    {

        protected float shootSpeed = 12;
        protected int cycleTIme = 80;
        protected float holdOffset = 20;
        protected float rotSpeed = MathF.PI / 30f;

        private NPC target;
        private int timer;
        private int arrow = 1;
        Projectile loadedArrow = null;
        private bool giveTileCollision = false;
        float aimRotation = 0;
        float targetRotation = 0;
        bool arrowFired = true;
        int safetyIdCheck = -1;
        int arrowIndex = -1;


        protected void BowAI()
        {
            Player player = Main.player[Projectile.owner];
            if(arrowIndex != -1)
            {
                loadedArrow = Main.projectile.FirstOrDefault(x => x.identity == arrowIndex);
            }
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
                Vector2 RestSpot = player.Center + QwertyMethods.PolarVector(50f + 8f * bowCount, -MathF.PI * ((float)(identity + 1) / (bowCount + 1))) + Vector2.UnitY * 10;
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
            if (Projectile.UseAmmo(AmmoID.Arrow, ref arrow, ref shootSpeed, ref weaponDamage, ref weaponKnockback, Main.rand.NextBool(2)))
            {
                ChangeArrow(ref arrow);
                if(Projectile.owner == Main.myPlayer)
                {
                    arrowIndex = Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, QwertyMethods.PolarVector(shootSpeed, Projectile.rotation), arrow, weaponDamage, weaponKnockback, Projectile.owner);
                    arrowIndex = Main.projectile[arrowIndex].identity;
                    safetyIdCheck = arrow;
                    Projectile.netUpdate = true;
                }
                arrowFired = false;
            }
            if(arrowIndex != -1)
            {
                loadedArrow = Main.projectile.FirstOrDefault(x => x.identity == arrowIndex);
            }
        }
        protected virtual void ChangeArrow(ref int arrowType)
        {

        }
        void HoldArrow()
        {
            if(loadedArrow != null)
            {
                loadedArrow.velocity = QwertyMethods.PolarVector(0.01f, Projectile.rotation - MathF.PI / 2);
                loadedArrow.Center = Projectile.Center + QwertyMethods.PolarVector(holdOffset - loadedArrow.velocity.Length() * (loadedArrow.extraUpdates + 1), Projectile.rotation - MathF.PI / 2);
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
                loadedArrow.velocity = QwertyMethods.PolarVector(shootSpeed, Projectile.rotation - MathF.PI / 2);
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

                    Projectile.rotation = aimRotation + MathF.PI / 2;
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
                targetRotation += MathF.PI;
            }
            aimRotation.SlowRotation(targetRotation, rotSpeed);
            Projectile.rotation = aimRotation + MathF.PI / 2;
        }
        void FloatArrow()
        {
            if (loadedArrow != null && loadedArrow.active && loadedArrow.type == safetyIdCheck)
            {
                loadedArrow.ai[0] = 0;
            }
        }
        public override void OnKill(int timeLeft)
        {
            Fire();
        }
        
        public override void PostDraw(Color lightColor)
        {
            /*
            if(loadedArrow != null)
            {
                float lineLength = (loadedArrow.Center - Projectile.Center).Length();
                Vector2 center = Projectile.Center;
                Vector2 distToProj = loadedArrow.Center - center;
                float projRotation = distToProj.ToRotation() - 1.57f;
                distToProj.Normalize();                 //get unit vector
                distToProj *= 12f;                      //speed = 12
                center += distToProj;                   //update draw position
                distToProj = loadedArrow.Center - center;    //update distance
                Color drawColor = lightColor;

                Main.EntitySpriteDraw(ModContent.Request<Texture2D>("QwertyMod/Content/Items/Weapon/Sentry/RhuthiniumGuardian/laser").Value, new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
                    new Rectangle(0, 0, 1, (int)lineLength - 10), Color.Red, projRotation,
                    new Vector2(0, 0), 1f, SpriteEffects.None, 0);
            }
            */
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(arrowIndex);
            writer.Write(safetyIdCheck);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            arrowIndex = reader.ReadInt32();
            safetyIdCheck = reader.ReadInt32();
        }
    }
    /*
    public class DebugArrow : GlobalProjectile
    {
        public override void AI(Projectile projectile)
        {
            if(projectile.ai[2] == 0 && projectile.type == ProjectileID.FrostburnArrow)
            {
                projectile.ai[2] = 1;
                QwertyMethods.ServerClientCheck("whoAmI: " + projectile.whoAmI);
            }
        }
    }
    */
}
