using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Minion.UrQuan
{
    public class Fighter : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fighter");
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projFrames[Projectile.type] = 1;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
        }
        public override bool MinionContactDamage()
        {
            return false;
        }
        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2; 
            Projectile.hostile = false;  
            Projectile.friendly = false;   
            Projectile.ignoreWater = true;   
            Projectile.penetrate = -1; 
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.timeLeft = 2;
        }

        private NPC target;
        private float speed = 16f;
        private int counter;
        private Projectile parent;
        private int startTime = 0;

        public override void AI()
        {
            parent = Main.projectile[(int)Projectile.ai[0]];
            counter--;
            Player player = Main.player[Projectile.owner];
            if (startTime > 20)
            {
                if (QwertyMethods.ClosestNPC(ref target, 2000, Projectile.Center, false, player.MinionAttackTargetNPC))
                {
                    Projectile.rotation = (target.Center - Projectile.Center).ToRotation() + (float)Math.PI / 2;
                    Vector2 offSpot = target.Center + QwertyMethods.PolarVector(-40, (target.Center - Projectile.Center).ToRotation());
                    Projectile.velocity = (offSpot - Projectile.Center);
                    if (Projectile.velocity.Length() > speed)
                    {
                        Projectile.velocity = Projectile.velocity.SafeNormalize(-Vector2.UnitY) * speed;
                    }
                    else
                    {
                        if (counter <= 0)
                        {
                            counter = 30;
                            Projectile p = QwertyMethods.PokeNPCMinion(player, target, Projectile.InheritSource(Projectile), Projectile.damage, 0);
                            p.GetGlobalProjectile<QwertyGlobalProjectile>().ignoresArmor = true;
                            SoundEngine.PlaySound(new SoundStyle("QwertyMod/Assets/Sounds/UrQuan-Fighter"), Projectile.Center);
                        }
                    }
                }
                else
                {
                    Projectile.rotation = (parent.Center - Projectile.Center).ToRotation() + (float)Math.PI / 2;
                    Projectile.velocity = (parent.Center - Projectile.Center);
                    if (Projectile.velocity.Length() > speed)
                    {
                        Projectile.velocity = Projectile.velocity.SafeNormalize(-Vector2.UnitY) * speed;
                    }
                    if (Collision.CheckAABBvAABBCollision(Projectile.position, Projectile.Size, parent.position, parent.Size))
                    {
                        SoundEngine.PlaySound(new SoundStyle("QwertyMod/Assets/Sounds/UrQuan-Recover"), Projectile.Center);
                        Projectile.ai[1] = 1;
                    }
                }
            }
            else
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + (float)Math.PI / 2;
                startTime++;
            }
            for (int k = 0; k < 1000; k++)
            {
                if (Main.projectile[k].type == Projectile.type && k != Projectile.whoAmI)
                {
                    if ((Projectile.Center - Main.projectile[k].Center).Length() < 10)
                    {
                        Projectile.velocity += new Vector2((float)Math.Cos((Projectile.Center - Main.projectile[k].Center).ToRotation()) * .1f, (float)Math.Sin((Projectile.Center - Main.projectile[k].Center).ToRotation()) * .1f);
                    }
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (counter > 25)
            {
                Main.EntitySpriteDraw(Request<Texture2D>("QwertyMod/Content/Items/Weapon/Minion/UrQuan/FighterShot").Value, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, new Vector2(1, 39), new Vector2(1, 1), 0, 0);
            }
            return true;
        }
    }
}
