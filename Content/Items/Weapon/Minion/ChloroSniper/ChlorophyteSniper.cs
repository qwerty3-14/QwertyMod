using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common;
using QwertyMod.Content.Buffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Minion.ChloroSniper
{
    public class ChlorophyteSniper : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chlorophyte Sniper");
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true; //This is necessary for right-click targeting
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
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

        private Vector2 flyTo;
        private int identity = 0;
        private int sniperCount = 0;

        private NPC target;
        private int timer;
        float hoverOffset = 0;
        float speed = .01f;
        float maxSpeed = 0.1f;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            sniperCount = player.ownedProjectileCounts[ProjectileType<ChlorophyteSniper>()];
            if (player.GetModPlayer<MinionManager>().chlorophyteSniper)
            {
                Projectile.timeLeft = 2;
            }
            for (int p = 0; p < 1000; p++)
            {
                if (Main.projectile[p].type == ProjectileType<ChlorophyteSniper>())
                {
                    if (p == Projectile.whoAmI)
                    {
                        break;
                    }
                    else
                    {
                        identity++;
                    }
                }
            }

            timer++;
            if (sniperCount != 0)
            {
                hoverOffset = (float)Math.Sin(player.GetModPlayer<MinionManager>().mythrilPrismRotation * 4f  + (float)Math.PI* ((float)identity / sniperCount)) * 6;

                Projectile.ai[0] = 40f;
                flyTo = player.Center + QwertyMethods.PolarVector(35f + 7f * sniperCount, -(float)Math.PI * ((float)(identity + 1) / (sniperCount + 1)));

                if (timer > 30)
                {
                    speed = .01f;
                }
                else
                {
                    speed = .005f + .005f * ((timer / 30));
                }
                Projectile.velocity += (flyTo - Projectile.Center) * speed;
                if (timer > 30 && Projectile.velocity.Length() > ((flyTo - Projectile.Center) * (maxSpeed)).Length())
                {
                    Projectile.velocity = (flyTo - Projectile.Center) * (maxSpeed);
                }
                if (QwertyMethods.ClosestNPC(ref target, 100000, Projectile.Center, false, player.MinionAttackTargetNPC) && timer > 240)
                {
                    SoundEngine.PlaySound(SoundID.Item92, Projectile.Center);
                    Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), Projectile.Center + Vector2.UnitY * hoverOffset, QwertyMethods.PolarVector(10, (target.Center - Projectile.Center).ToRotation()), ProjectileType<ChlorophyteSnipe>(), Projectile.damage, Projectile.knockBack, player.whoAmI);
                    Projectile.velocity += QwertyMethods.PolarVector(-6, (target.Center - Projectile.Center).ToRotation());
                    timer = 0;
                }
                float rot = Projectile.velocity.X * (float)Math.PI / 30;
                Projectile.rotation.SlowRotation(rot, (float)Math.PI / 30);
                int dustAmt = Math.Min(timer / 60, 4);
                for(int i =0; i < dustAmt; i++)
                {
                    if (Main.rand.Next(2) == 0)
                    {
                        Dust d = Dust.NewDustPerfect(Projectile.Center + Vector2.UnitY * hoverOffset, 75, QwertyMethods.PolarVector(Main.rand.NextFloat() * dustAmt * .9f, Main.rand.NextFloat() * (float)Math.PI * 2), Projectile.alpha);
                        d.noGravity = true;
                    }
                }
            }

            identity = 0;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition + Vector2.UnitY * hoverOffset,
                        new Rectangle(0, Projectile.frame * texture.Height, texture.Width, texture.Height), lightColor, Projectile.rotation,
                        new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), 1f, SpriteEffects.None, 0);
            return false;
        }
    }
}
