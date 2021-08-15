using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;
using static Terraria.ModLoader.ModContent;
using QwertyMod.Content.NPCs.Bosses.Hydra;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace QwertyMod.Common
{
    class Dash : ModPlayer
    {
        public float Base = 0;
        public float Bonus = 0;
        public int customDashRam = 0;
        public int hyperRuneTimer = 0;
        public bool hyperRune = false;
        int customDashTime = 0;
        int customDashDelay = 0;
        public override void ResetEffects()
        {
            Base = 0;
            Bonus = 0;
            customDashRam = 0;
            hyperRune = false;
        }
        public void SetDash(int speed)
        {
            if(speed > Base)
            {
                Base = speed;
            }
        }
        public override void PreUpdate()
        {
            if (hyperRune)
            {
                if (hyperRuneTimer == 240)
                {
                }
                if (hyperRuneTimer > 240)
                {
                }
            }
        }
        public override void PostUpdateEquips()
        {
            if (Player.grappling[0] == -1 && !Player.tongued && !Player.pulley)
            {
                customDashSpeedMovement();
            }
        }
        public void customDashSpeedMovement()
        {
            if (hyperRune)
            {
                hyperRuneTimer++;
            }
            if (customDashRam > 0 && Player.eocDash > 0)
            {
                if (Player.eocHit < 0)
                {
                    Rectangle rectangle = new Rectangle((int)((double)Player.position.X + (double)Player.velocity.X * 0.5 - 4.0), (int)((double)Player.position.Y + (double)Player.velocity.Y * 0.5 - 4.0), Player.width + 8, Player.height + 8);
                    for (int i = 0; i < 200; i++)
                    {
                        if (Main.npc[i].active && !Main.npc[i].dontTakeDamage && !Main.npc[i].friendly)
                        {
                            NPC nPC = Main.npc[i];
                            Rectangle rect = nPC.getRect();
                            if (rectangle.Intersects(rect) && (nPC.noTileCollide || Player.CanHit(nPC) && nPC.type != NPCType<Hydra>()))
                            {
                                float num = customDashRam * Player.GetDamage(DamageClass.Melee);
                                float num2 = 9f;
                                bool crit = false;
                                if (Player.kbGlove)
                                {
                                    num2 *= 2f;
                                }
                                if (Player.kbBuff)
                                {
                                    num2 *= 1.5f;
                                }
                                if (Main.rand.Next(100) < Player.GetCritChance(DamageClass.Melee))
                                {
                                    crit = true;
                                }
                                int num3 = Player.direction;
                                if (Player.velocity.X < 0f)
                                {
                                    num3 = -1;
                                }
                                if (Player.velocity.X > 0f)
                                {
                                    num3 = 1;
                                }
                                if (Player.whoAmI == Main.myPlayer)
                                {
                                    Player.ApplyDamageToNPC(nPC, (int)num, num2, num3, crit);
                                }
                                Player.eocDash = 10;
                                customDashDelay = 30;
                                Player.velocity.X = -(float)num3 * 9f;
                                Player.velocity.Y = -4f;
                                Player.immune = true;
                                Player.immuneNoBlink = true;
                                Player.immuneTime = 4;
                                Player.eocHit = i;
                            }
                        }
                    }
                }
            }

            if (Player.dash < 1)
            {
                if (customDashDelay > 0)
                {
                    if (Player.eocDash > 0)
                    {
                        Player.eocDash--;
                    }
                    if (Player.eocDash == 0)
                    {
                        Player.eocHit = -1;
                    }
                    customDashDelay--;
                    return;
                }
                if (customDashDelay < 0)
                {
                    if (hyperRune && hyperRuneTimer > 240)
                    {
                        Player.immune = true;
                    }
                    float num7 = 12f;
                    float num8 = 0.992f;
                    float num9 = Math.Max(Player.accRunSpeed, Player.maxRunSpeed);
                    float num10 = 0.96f;
                    int num11 = 20;
                    if ((Base > 0) && Player.dash < 1)
                    {
                        for (int k = 0; k < 2; k++)
                        {
                            int num12;
                            if (Player.velocity.Y == 0f)
                            {
                                num12 = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + (float)Player.height - 4f), Player.width, 8, 31, 0f, 0f, 100, default(Color), 1.4f);
                            }
                            else
                            {
                                num12 = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + (float)(Player.height / 2) - 8f), Player.width, 16, 31, 0f, 0f, 100, default(Color), 1.4f);
                            }
                            Main.dust[num12].velocity *= 0.1f;
                            Main.dust[num12].scale *= 1f + (float)Main.rand.Next(20) * 0.01f;
                            Main.dust[num12].shader = GameShaders.Armor.GetSecondaryShader(Player.cShoe, Player);
                        }
                    }

                    if ((Base > 0) && Player.dash < 1)
                    {
                        Player.vortexStealthActive = false;
                        if (Player.velocity.X > num7 || Player.velocity.X < -num7)
                        {
                            Player.velocity.X = Player.velocity.X * num8;
                            return;
                        }
                        if (Player.velocity.X > num9 || Player.velocity.X < -num9)
                        {
                            Player.velocity.X = Player.velocity.X * num10;
                            return;
                        }
                        hyperRuneTimer = 0;
                        customDashDelay = num11;
                        //Player.immune = false;
                        if (Player.velocity.X < 0f)
                        {
                            Player.velocity.X = -num9;
                            return;
                        }
                        if (Player.velocity.X > 0f)
                        {
                            Player.velocity.X = num9;
                            return;
                        }
                    }
                }
                else if (Player.dash < 1 && (Base > 0) && !Player.mount.Active)
                {
                    if ((Base > 0))
                    {
                        int num16 = 0;
                        bool flag = false;
                        if (customDashTime > 0)
                        {
                            customDashTime--;
                        }
                        if (customDashTime < 0)
                        {
                            customDashTime++;
                        }
                        if (Player.controlRight && Player.releaseRight)
                        {
                            if (customDashTime > 0)
                            {
                                num16 = 1;
                                flag = true;
                                customDashTime = 0;
                            }
                            else
                            {
                                customDashTime = 15;
                            }
                        }
                        else if (Player.controlLeft && Player.releaseLeft)
                        {
                            if (customDashTime < 0)
                            {
                                num16 = -1;
                                flag = true;
                                customDashTime = 0;
                            }
                            else
                            {
                                customDashTime = -15;
                            }
                        }
                        if (flag)
                        {
                            
                            Player.velocity.X = (Base + 10f + Bonus) * (float)num16;

                            Point point = (Player.Center + new Vector2((float)(num16 * Player.width / 2 + 2), Player.gravDir * -(float)Player.height / 2f + Player.gravDir * 2f)).ToTileCoordinates();
                            Point point2 = (Player.Center + new Vector2((float)(num16 * Player.width / 2 + 2), 0f)).ToTileCoordinates();
                            if (WorldGen.SolidOrSlopedTile(point.X, point.Y) || WorldGen.SolidOrSlopedTile(point2.X, point2.Y))
                            {
                                Player.velocity.X = Player.velocity.X / 2f;
                            }

                            customDashDelay = -1;
                            if (customDashRam > 0)
                            {
                                Player.eocDash = 15;
                            }
                            for (int num17 = 0; num17 < 20; num17++)
                            {
                                int num18 = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y), Player.width, Player.height, 31, 0f, 0f, 100, default(Color), 2f);
                                Dust dust = Main.dust[num18];
                                dust.position.X = dust.position.X + (float)Main.rand.Next(-5, 6);
                                Dust dust2 = Main.dust[num18];
                                dust2.position.Y = dust2.position.Y + (float)Main.rand.Next(-5, 6);
                                Main.dust[num18].velocity *= 0.2f;
                                Main.dust[num18].scale *= 1f + (float)Main.rand.Next(20) * 0.01f;
                                Main.dust[num18].shader = GameShaders.Armor.GetSecondaryShader(Player.cShoe, Player);
                            }
                            int num19 = Gore.NewGore(new Vector2(Player.position.X + (float)(Player.width / 2) - 24f, Player.position.Y + (float)(Player.height / 2) - 34f), default(Vector2), Main.rand.Next(61, 64), 1f);
                            Main.gore[num19].velocity.X = (float)Main.rand.Next(-50, 51) * 0.01f;
                            Main.gore[num19].velocity.Y = (float)Main.rand.Next(-50, 51) * 0.01f;
                            Main.gore[num19].velocity *= 0.4f;
                            num19 = Gore.NewGore(new Vector2(Player.position.X + (float)(Player.width / 2) - 24f, Player.position.Y + (float)(Player.height / 2) - 14f), default(Vector2), Main.rand.Next(61, 64), 1f);
                            Main.gore[num19].velocity.X = (float)Main.rand.Next(-50, 51) * 0.01f;
                            Main.gore[num19].velocity.Y = (float)Main.rand.Next(-50, 51) * 0.01f;
                            Main.gore[num19].velocity *= 0.4f;
                            return;
                        }
                    }
                }
            }
        }
    }
    class HyperRune : PlayerDrawLayer
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return true;
        }
        public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.BackAcc);

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("QwertyMod");
            if (drawPlayer.GetModPlayer<Dash>().hyperRune && drawPlayer.GetModPlayer<Dash>().hyperRuneTimer >= 180)
            {
                float c = (float)(drawPlayer.GetModPlayer<Dash>().hyperRuneTimer - 180) / 60f;
                if(c > 1f)
                {
                    c = 1f;
                }
                Color drawColor = new Color(c, c, c, c);
                float rotation = drawPlayer.GetModPlayer<CommonStats>().genericCounter * (float)Math.PI /60f;
                Texture2D texture = Request<Texture2D>("QwertyMod/Common/SignalRune").Value;
                DrawData d = new DrawData(texture, drawInfo.Position + drawPlayer.Size * 0.5f - Main.screenPosition, null, drawColor, rotation, texture.Size() * 0.5f, 1f, drawInfo.playerEffect, 0);
                drawInfo.DrawDataCache.Add(d);

            }
        }
    }
}
