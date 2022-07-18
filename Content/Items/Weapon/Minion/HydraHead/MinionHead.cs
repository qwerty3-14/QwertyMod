using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Minion.HydraHead
{
    class MinionHead : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydra Head");
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            Main.projPet[Projectile.type] = true;
        }
        public override bool MinionContactDamage()
        {
            return false;
        }

        public override void SetDefaults()
        {
            Projectile.width = 42;
            Projectile.height = 36;
            Projectile.hostile = false;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Main.projFrames[Projectile.type] = 1;
            Projectile.knockBack = 10f;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.minion = true;
            Projectile.minionSlots = 1;
            Projectile.timeLeft = 2;
        }

        public int varTime;
        public int Yvar = 0;
        public int Xvar = 0;
        public int f = 1;
        public float targetAngle = 90;
        public float s = 1;
        public float tarX;
        public float tarY;
        int cooldown = 0;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (player.GetModPlayer<MinionManager>().HydraHeadMinion)
            {
                Projectile.timeLeft = 2;
            }
            Projectile.rotation = (QwertyMod.GetLocalCursor(Projectile.owner) - Projectile.Center).ToRotation();

            if (cooldown > 0)
            {
                cooldown--;
            }
            if (player.itemTime > 0)
            {
                cooldown = 120;
            }
            varTime++;
            if (varTime == 30 && Projectile.owner == Main.myPlayer && cooldown > 0)
            {

                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, QwertyMethods.PolarVector(10, Projectile.rotation), ProjectileType<MinionBreath>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
            }
            if (varTime >= 60)
            {
                if (Projectile.owner == Main.myPlayer && cooldown > 0)
                {
                    Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, QwertyMethods.PolarVector(10, Projectile.rotation), ProjectileType<MinionBreath>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                }
                varTime = 0;
                if (Main.netMode != NetmodeID.Server && Main.myPlayer == Projectile.owner)
                {
                    Yvar = Main.rand.Next(0, 80);
                    Xvar = Main.rand.Next(-80, 80);
                    Projectile.netUpdate = true;
                }
            }

            Vector2 moveTo = new Vector2(player.Center.X + Xvar, player.Center.Y - Yvar) - Projectile.Center;
            Projectile.velocity = (moveTo) * .04f;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Yvar);
            writer.Write(Xvar);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Yvar = reader.ReadInt32();
            Xvar = reader.ReadInt32();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Main.player[Projectile.owner].active && !Main.player[Projectile.owner].dead)
            {
                Vector2 playerCenter = Main.player[Projectile.owner].MountedCenter;
                Vector2 center = Projectile.Center;
                Vector2 distToProj = playerCenter - Projectile.Center;
                float projRotation = distToProj.ToRotation() - 1.57f;
                float distance = distToProj.Length();
                for (int i = 0; i < 1000; i++)
                {
                    if (distance > 4f && !float.IsNaN(distance))
                    {
                        distToProj.Normalize();                 //get unit vector
                        distToProj *= 8f;                      //speed = 12
                        center += distToProj;                   //update draw position
                        distToProj = playerCenter - center;    //update distance
                        distance = distToProj.Length();
                        Color drawColor = lightColor;

                        //Draw chain
                        Main.EntitySpriteDraw(Request<Texture2D>("QwertyMod/Content/Items/Weapon/Minion/HydraHead/HydraHookChain").Value, new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
                            new Rectangle(0, 0, 14, 8), drawColor, projRotation,
                            new Vector2(14 * 0.5f, 8 * 0.5f), 1f, SpriteEffects.None, 0);
                    }
                }
            }
            return true;
        }

        public override void PostDraw(Color lightColor)
        {
            Main.EntitySpriteDraw(Request<Texture2D>("QwertyMod/Content/Items/Weapon/Minion/HydraHead/MinionHead_Glow").Value, new Vector2(Projectile.Center.X - Main.screenPosition.X, Projectile.Center.Y - Main.screenPosition.Y),
                        new Rectangle(0, 0, Projectile.width, Projectile.height), Color.White, Projectile.rotation,
                        new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f), 1f, SpriteEffects.None, 0);
        }
    }
}
