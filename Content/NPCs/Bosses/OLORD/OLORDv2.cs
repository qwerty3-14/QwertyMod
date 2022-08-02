using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common;
using QwertyMod.Content.Buffs;
using QwertyMod.Content.Dusts;
using QwertyMod.Content.Items.Consumable.BossBag;
using QwertyMod.Content.Items.Tool.Mining.TheDevourer;
using QwertyMod.Content.Items.Weapon.Magic.BlackHole;
using QwertyMod.Content.Items.Weapon.Magic.Plasma;
using QwertyMod.Content.Items.Weapon.Minion.UrQuan;
using QwertyMod.Content.Items.Weapon.Ranged.Bow.B4Bow;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.NPCs.Bosses.OLORD
{
    [AutoloadBossHead]
    public class OLORDv2 : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Oversized Laser-emitting Obliteration Radiation-emitting Destroyer");
            Main.npcFrameCount[NPC.type] = 2;

            NPCID.Sets.MPAllowedEnemies[NPC.type] = true; //For allowing use of SpawnOnPlayer in multiplayer
        }


        public override void SetDefaults()
        {
            NPC.width = 320;
            NPC.height = 60;
            NPC.damage = 70;
            NPC.defense = 50;
            NPC.boss = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 1000000;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = -1;

            NPC.noGravity = true;
            NPC.dontTakeDamage = true;
            NPC.noTileCollide = true;
            NPC.netAlways = true;

            NPC.scale = 1f;
            NPC.lifeMax = 120000;
            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/EnergisedPlanetaryIncinerationClimax");
            }
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Sets the description of this NPC that is listed in the bestiary
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
            {
                new MoonLordPortraitBackgroundProviderBestiaryInfoElement(), //Plain black background
				new FlavorTextBestiaryInfoElement("What were the martians after when making such a creature?")
            });
        }
        private int frame = 0;
        private const int guideWidth = 750;
        private const int turretVerticalShift = -30;
        private Vector2[] turret = new Vector2[4] { new Vector2(0, 2), new Vector2(0, 2), new Vector2(0, 2), new Vector2(0, 2) }; //Y is the frame, X is the angle
        private Vector2[] turretPos = new Vector2[4] { new Vector2(-2 * (guideWidth / 3), turretVerticalShift), new Vector2(-1 * (guideWidth / 3), turretVerticalShift), new Vector2(1 * (guideWidth / 3), turretVerticalShift), new Vector2(2 * (guideWidth / 3), turretVerticalShift) };
        private int shotDamage = 35;
        private int quitCount = 0;
        private bool playerDied = false;
        public Projectile[] wall = new Projectile[2];
        private bool activeWalls = false;
        private bool[] shootLaser = new bool[] { false, false, false, false };
        private Projectile[] tLaser = new Projectile[4];
        private Projectile superLaser;
        private bool activeSuperLaser = false;

        /// ///////////
        private int timer;

        private int attack = 0;
        private bool runOnce = true;
        private Vector2 GoTo;
        private float laserDistanceFromCenter = 800;
        private bool didHalfDeadSequence = false;
        private bool halfDeadSqequence = false;

        public override void AI()
        {
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player allPlayer = Main.player[i];
                if (allPlayer.active && !allPlayer.dead)
                {
                    allPlayer.AddBuff(BuffID.ChaosState, 2);
                }
            }
            if (Main.expertMode)
            {
                shotDamage = (int)(NPC.damage / 4 * 1.6f);
                if (NPC.life < (int)(NPC.lifeMax * .3f))
                {
                    NPC.ai[3] = 1;
                }
                else
                {
                    NPC.ai[3] = 1;
                }
            }
            else
            {
                shotDamage = NPC.damage / 2;
                NPC.ai[3] = 1;
            }

            //Start
            //////////////////
            NPC.width = (int)(320 * NPC.scale);
            NPC.height = (int)(60 * NPC.scale);
            // NPC.hide = !(NPC.scale == 1f);
            //NPC.behindTiles = NPC.hide;
            if (runOnce)
            {
                if (Main.netMode != 1)
                {
                    for (int t = 0; t < tLaser.Length; t++)
                    {
                        tLaser[t] = Main.projectile[Projectile.NewProjectile(new EntitySource_Misc(""), (NPC.Center + turretPos[t] * NPC.scale), QwertyMethods.PolarVector(14f, turret[t].X + (float)Math.PI / 2), ProjectileType<TurretLaser2>(), (int)(1.5f * shotDamage), 3f, Main.myPlayer, (NPC.Center + turretPos[t] * NPC.scale).X, (NPC.Center + turretPos[t] * NPC.scale).Y)];
                    }

                    wall[0] = Main.projectile[Projectile.NewProjectile(new EntitySource_Misc(""), NPC.Center.X + laserDistanceFromCenter, NPC.position.Y, 0, 14f, ProjectileType<SideLaser>(), (int)(shotDamage * 1.5f), 3f, Main.myPlayer, NPC.whoAmI, laserDistanceFromCenter)];
                    wall[1] = Main.projectile[Projectile.NewProjectile(new EntitySource_Misc(""), NPC.Center.X + laserDistanceFromCenter, NPC.position.Y, 0, 14f, ProjectileType<SideLaser>(), (int)(shotDamage * 1.5f), 3f, Main.myPlayer, NPC.whoAmI, -laserDistanceFromCenter)];
                    superLaser = Main.projectile[Projectile.NewProjectile(new EntitySource_Misc(""), NPC.Center.X, NPC.Center.Y, 0, 14f, ProjectileType<SuperLaser2>(), 5 * shotDamage, 3f, Main.myPlayer, NPC.whoAmI, NPC.rotation + (float)Math.PI / 2)];
                }
                if (Main.netMode != 1)
                {
                    attack = Main.rand.Next(4);
                    NPC.netUpdate = true;
                }
                runOnce = false;
            }
            if (Main.netMode != 1)
            {
                if (superLaser.type != ProjectileType<SuperLaser2>())
                {
                    superLaser = Main.projectile[Projectile.NewProjectile(new EntitySource_Misc(""), NPC.Center.X, NPC.Center.Y, 0, 14f, ProjectileType<SuperLaser2>(), 5 * shotDamage, 3f, Main.myPlayer, NPC.whoAmI, NPC.rotation + (float)Math.PI / 2)];
                }

                //superLaser.timeLeft = 10;
                for (int t = 0; t < tLaser.Length; t++)
                {
                    if (tLaser[t].type != ProjectileType<TurretLaser2>())
                    {
                        tLaser[t] = Main.projectile[Projectile.NewProjectile(new EntitySource_Misc(""), (NPC.Center + turretPos[t] * NPC.scale), QwertyMethods.PolarVector(14f, turret[t].X + (float)Math.PI / 2), ProjectileType<TurretLaser2>(), (int)(1.5f * shotDamage), 3f, Main.myPlayer, (NPC.Center + turretPos[t] * NPC.scale).X, (NPC.Center + turretPos[t] * NPC.scale).Y)];
                    }
                    tLaser[t].rotation = turret[t].X;
                    tLaser[t].Center = (NPC.Center + turretPos[t] * NPC.scale);
                    tLaser[t].ai[0] = (NPC.Center + turretPos[t] * NPC.scale).X;
                    tLaser[t].ai[1] = (NPC.Center + turretPos[t] * NPC.scale).Y;
                    tLaser[t].timeLeft = 10;

                    if (!shootLaser[t])
                    {
                        tLaser[t].localAI[0] = 0;
                    }
                    else
                    {
                        tLaser[t].localAI[0] += (NPC.ai[3] - 1);
                    }
                    tLaser[t].netUpdate = true;
                }
                if (!activeWalls)
                {
                    for (int w = 0; w < 2; w++)
                    {
                        if (wall[w].type != ProjectileType<SideLaser>() && Main.netMode != 1)
                        {
                            wall[w] = Main.projectile[Projectile.NewProjectile(new EntitySource_Misc(""), NPC.Center.X + laserDistanceFromCenter, NPC.position.Y, 0, 14f, ProjectileType<SideLaser>(), (int)(shotDamage * 1.5f), 3f, Main.myPlayer, NPC.whoAmI, laserDistanceFromCenter * w == 1 ? -1 : 1)];
                        }
                        wall[w].localAI[0] = 0;
                        wall[w].netUpdate = true;
                    }
                }
                if (!activeSuperLaser)
                {
                    superLaser.localAI[0] = 0;
                    superLaser.netUpdate = true;
                }
                else
                {
                    superLaser.localAI[0] += (NPC.ai[3] - 1);
                }
            }
            for (int q = 0; q < (int)NPC.ai[3]; q++)
            {
                activeSuperLaser = false;
                for (int t = 0; t < turret.Length; t++)
                {
                    turret[t].Y = 2;
                    shootLaser[t] = false;
                }

                if (frame == 0)
                {
                    NPC.dontTakeDamage = true;
                }
                else
                {
                    NPC.dontTakeDamage = false;
                }
                NPC.TargetClosest(true);
                Player player = Main.player[NPC.target];
                if (!player.active || player.dead)
                {
                    quitCount++;
                    if (quitCount >= 120)
                    {
                        NPC.position.Y -= 4f;
                        playerDied = true;
                        NPC.active = false;
                    }
                }
                else
                {
                    quitCount = 0;
                    playerDied = false;
                }

                //End
                //////////////////

                timer++;
                frame = 0;
                int startAttacks = 420;

                if ((Math.Abs(player.Center.X - NPC.Center.X) > guideWidth || player.Center.Y < NPC.Center.Y + 50))
                {
                    timer = 0;
                    if (Main.netMode != 1)
                    {
                        NPC.netUpdate = true;
                    }
                }
                if (timer > 300)
                {
                    player.GetModPlayer<OLORDScreenLock>().screenLock = NPC.whoAmI;

                    activeWalls = true;
                    NPC.velocity = Vector2.Zero;
                    if (player.Center.Y - NPC.Center.Y > 1000)
                    {
                        NPC.velocity.Y += 3;
                    }
                }
                else
                {
                    activeWalls = false;
                    player.AddBuff(BuffType<HealingHalt>(), 60);
                    GoTo = new Vector2(player.Center.X, player.Center.Y - 350);
                    NPC.velocity = (GoTo - NPC.Center) * .1f;
                    if (NPC.velocity.Length() > 30)
                    {
                        NPC.velocity = NPC.velocity.SafeNormalize(-Vector2.UnitY) * 30;
                    }
                }

                int attackDuration = 1500;
                if (timer > startAttacks)
                {
                    int startShooting = 60;

                    switch (attack)
                    {
                        case 0:
                            #region pew pew laser pew pew attack

                            int[] laserSwitch = new int[] { 300, 600, 900 };
                            if (timer > attackDuration + startAttacks)
                            {
                                timer = startAttacks;
                                if (Main.netMode != 1)
                                {
                                    attack = Main.rand.Next(1, 4);
                                    NPC.netUpdate = true;
                                }
                            }
                            else if (timer > startAttacks + laserSwitch[2] + 60)
                            {
                                for (int t = 0; t < turret.Length; t++)
                                {
                                    turret[t].Y = 1;
                                    shootLaser[t] = true;
                                }
                                frame = 1;
                                if (timer % 30 == 0)
                                {
                                    if (Main.netMode != 1)
                                    {
                                        if (timer % 60 == 0)
                                        {
                                            for (int p = -5; p < 7; p += 2)
                                            {
                                                Projectile.NewProjectile(new EntitySource_Misc(""), new Vector2(NPC.Center.X, NPC.Center.Y + NPC.height / 2), QwertyMethods.PolarVector(3 * NPC.ai[3], p * (float)Math.PI / 12 + (float)Math.PI / 2), ProjectileType<TurretShot>(), shotDamage, 0, Main.myPlayer);
                                            }
                                        }
                                        else
                                        {
                                            for (int p = -3; p < 4; p++)
                                            {
                                                Projectile.NewProjectile(new EntitySource_Misc(""), new Vector2(NPC.Center.X, NPC.Center.Y + NPC.height / 2), QwertyMethods.PolarVector(3 * NPC.ai[3], p * (float)Math.PI / 6 + (float)Math.PI / 2), ProjectileType<TurretShot>(), shotDamage, 0, Main.myPlayer);
                                            }
                                        }
                                    }
                                }
                            }
                            else if (timer > startShooting + startAttacks)
                            {
                                if (timer > startAttacks + laserSwitch[2])
                                {
                                    shootLaser[0] = true;
                                    shootLaser[3] = true;
                                }
                                if (timer > startAttacks + laserSwitch[1])
                                {
                                    shootLaser[2] = true;
                                }
                                if (timer > startAttacks + laserSwitch[0])
                                {
                                    shootLaser[1] = true;
                                }
                                for (int t = 0; t < turret.Length; t++)
                                {
                                    if (shootLaser[t])
                                    {
                                        turret[t].Y = 1;
                                    }
                                    else
                                    {
                                        turret[t].X = QwertyMethods.SlowRotation(turret[t].X, (player.Center - (NPC.Center + turretPos[t] * NPC.scale)).ToRotation() - (float)Math.PI / 2, 4);
                                        turret[t].Y = 0;
                                        if (timer % 120 == 0)
                                        {
                                            if (Main.netMode != 1)
                                            {
                                                for (int r = -1; r < 2; r++)
                                                {
                                                    Projectile p = Main.projectile[Projectile.NewProjectile(new EntitySource_Misc(""), (NPC.Center + turretPos[t] * NPC.scale), QwertyMethods.PolarVector(3 * NPC.ai[3], turret[t].X + (float)Math.PI / 2 + r * (float)Math.PI / 8), ProjectileType<TurretShot>(), shotDamage, 0, Main.myPlayer)];
                                                    p.scale = NPC.scale;
                                                }
                                                Vector2 center = (NPC.Center + turretPos[t] * NPC.scale);
                                                for (int i = 0; i < 30; i++)
                                                {
                                                    float theta = turret[t].X + (float)Math.PI / 2 + Main.rand.NextFloat(-(float)Math.PI / 3, (float)Math.PI / 3);
                                                    float dist = Main.rand.NextFloat(60f, 100f);
                                                    if (Main.netMode != 1)
                                                    {
                                                        Dust.NewDustPerfect(center, DustType<B4PDust>(), QwertyMethods.PolarVector(dist / 10, theta));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else if (timer > startShooting / 2 + startAttacks)
                            {
                                for (int t = 0; t < turret.Length; t++)
                                {
                                    turret[t].Y = 1;
                                }
                            }

                            #endregion
                            break;

                        case 1:
                            #region Super Laser
                            if (timer > attackDuration + startAttacks)
                            {
                                timer = startAttacks;
                                if (Main.netMode != 1)
                                {
                                    attack = Main.rand.Next(3);
                                    if (attack >= 1)
                                    {
                                        attack++;
                                    }
                                    NPC.netUpdate = true;
                                }
                            }
                            else if (timer > startShooting + startAttacks)
                            {
                                float laserProgress = 1f - (((float)timer - (float)startAttacks) / 960f);
                                if (laserProgress < 0)
                                {
                                    laserProgress = 0;
                                }
                                //Main.NewText(laserProgress);
                                if (timer < startAttacks + 960 + 120)
                                {
                                    shootLaser[1] = true;
                                    shootLaser[2] = true;
                                    for (int t = 0; t < turret.Length; t++)
                                    {
                                        if (shootLaser[t])
                                        {
                                            turret[t].Y = 1;
                                            if (t == 1)
                                            {
                                                turret[t].X = QwertyMethods.SlowRotation(turret[t].X, (float)Math.PI / 2 * laserProgress, 4);
                                            }
                                            else if (t == 2)
                                            {
                                                turret[t].X = QwertyMethods.SlowRotation(turret[t].X, -(float)Math.PI / 2 * laserProgress, 4);
                                            }
                                        }
                                        else
                                        {
                                            turret[t].X = QwertyMethods.SlowRotation(turret[t].X, (player.Center - (NPC.Center + turretPos[t] * NPC.scale)).ToRotation() - (float)Math.PI / 2, 4);
                                            turret[t].Y = 1;
                                            if (timer % 120 < 60 && timer % 10 == 0)
                                            {
                                                if (Main.netMode != 1)
                                                {
                                                    Projectile p = Main.projectile[Projectile.NewProjectile(new EntitySource_Misc(""), (NPC.Center + turretPos[t] * NPC.scale), QwertyMethods.PolarVector(3 * NPC.ai[3], turret[t].X + (float)Math.PI / 2), ProjectileType<TurretShot>(), shotDamage, 0, Main.myPlayer)];
                                                    p.scale = NPC.scale;

                                                    Vector2 center = (NPC.Center + turretPos[t] * NPC.scale);
                                                    for (int i = 0; i < 10; i++)
                                                    {
                                                        float theta = turret[t].X + (float)Math.PI / 2 + Main.rand.NextFloat(-(float)Math.PI / 4, (float)Math.PI / 4);
                                                        float dist = Main.rand.NextFloat(60f, 100f);
                                                        if (Main.netMode != 1)
                                                        {
                                                            Dust.NewDustPerfect(center, DustType<B4PDust>(), QwertyMethods.PolarVector(dist / 10, theta));
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                if (timer > startAttacks + 960)
                                {
                                    activeSuperLaser = true;
                                    laserProgress = 0f;
                                    frame = 1;
                                }
                            }
                            else if (timer > startShooting / 2 + startAttacks)
                            {
                                for (int t = 0; t < turret.Length; t++)
                                {
                                    turret[t].Y = 1;
                                }
                            }

                            #endregion
                            break;

                        case 2:
                            #region gravity attack
                            if (timer > attackDuration + startAttacks)
                            {
                                timer = startAttacks;
                                if (Main.netMode != 1)
                                {
                                    attack = Main.rand.Next(3);
                                    if (attack >= 2)
                                    {
                                        attack++;
                                    }
                                    NPC.netUpdate = true;
                                }
                            }
                            else if (timer > startShooting + startAttacks)
                            {
                                if (timer > startAttacks + 960)
                                {
                                    frame = 1;
                                }
                                if (timer == attackDuration + startAttacks - 480 && Main.netMode != 1)
                                {
                                    Projectile.NewProjectile(new EntitySource_Misc(""), NPC.Center.X, NPC.Center.Y, 0, 14f, ProjectileType<BlackHoleSeed>(), (int)(2.5f * shotDamage), 3f, Main.myPlayer, NPC.ai[3]);
                                }
                                for (int t = 0; t < turret.Length; t++)
                                {
                                    turret[t].Y = 0;
                                    turret[t].X = QwertyMethods.SlowRotation(turret[t].X, 0, 4);
                                    if (timer % 120 == t * 30)
                                    {
                                        if (Main.netMode != 1)
                                        {
                                            Projectile.NewProjectile(new EntitySource_Misc(""), (NPC.Center + turretPos[t] * NPC.scale), QwertyMethods.PolarVector(3 * NPC.ai[3], turret[t].X + (float)Math.PI / 2), ProjectileType<TurretGrav>(), shotDamage, 0, Main.myPlayer);
                                        }
                                        Vector2 center = (NPC.Center + turretPos[t] * NPC.scale);
                                        for (int i = 0; i < 30; i++)
                                        {
                                            float theta = turret[t].X + (float)Math.PI / 2 + Main.rand.NextFloat(-(float)Math.PI / 3, (float)Math.PI / 3);
                                            float dist = Main.rand.NextFloat(60f, 100f);
                                            if (Main.netMode != 1)
                                            {
                                                Dust.NewDustPerfect(center, DustType<B4PDust>(), QwertyMethods.PolarVector(dist / 10, theta));
                                            }
                                        }
                                    }
                                }
                            }
                            else if (timer > startShooting / 2 + startAttacks)
                            {
                                for (int t = 0; t < turret.Length; t++)
                                {
                                    turret[t].Y = 1;
                                }
                            }
                            #endregion
                            break;

                        case 3:
                            #region Large bursts
                            if (timer > attackDuration + startAttacks)
                            {
                                timer = startAttacks;
                                if (Main.netMode != 1)
                                {
                                    attack = Main.rand.Next(3);
                                    NPC.netUpdate = true;
                                }
                            }
                            else if (timer > startAttacks + 960)
                            {
                                frame = 1;
                                if (timer == startAttacks + 1320 && Main.netMode != 1)
                                {
                                    Projectile.NewProjectile(new EntitySource_Misc(""), NPC.Center.X, NPC.Center.Y, 0, 7f * NPC.ai[3], ProjectileType<MegaBurst>(), shotDamage, 3f, Main.myPlayer, NPC.ai[3]);
                                }
                                for (int t = 0; t < turret.Length; t++)
                                {
                                    if (timer % 30 == 0 && turret[t].Y < 2)
                                    {
                                        turret[t].Y++;
                                    }
                                }
                            }
                            else if (timer > startShooting + startAttacks)
                            {
                                for (int t = 0; t < turret.Length; t++)
                                {
                                    if (t == 0 || t == 3)
                                    {
                                        turret[t].Y = 0;
                                    }
                                    else
                                    {
                                        turret[t].Y = 1;
                                    }
                                    turret[t].X = QwertyMethods.SlowRotation(turret[t].X, (player.Center - (NPC.Center + turretPos[t] * NPC.scale)).ToRotation() - (float)Math.PI / 2, 4);
                                    if (timer % 90 == 0)
                                    {
                                        if (Main.netMode != 1)
                                        {
                                            if ((timer % 180 == 0 && t == 0) || (timer % 180 != 0 && t == 3))
                                            {
                                                Projectile p = Main.projectile[Projectile.NewProjectile(new EntitySource_Misc(""), (NPC.Center + turretPos[t] * NPC.scale), QwertyMethods.PolarVector(4.5f * NPC.ai[3], turret[t].X + (float)Math.PI / 2), ProjectileType<BurstShot>(), shotDamage, 0, Main.myPlayer)];
                                                p.scale = NPC.scale;

                                                Vector2 center = (NPC.Center + turretPos[t] * NPC.scale);
                                                for (int i = 0; i < 30; i++)
                                                {
                                                    float theta = turret[t].X + (float)Math.PI / 2 + Main.rand.NextFloat(-(float)Math.PI / 3, (float)Math.PI / 3);
                                                    float dist = Main.rand.NextFloat(60f, 100f);
                                                    if (Main.netMode != 1)
                                                    {
                                                        Dust.NewDustPerfect(center, DustType<B4PDust>(), QwertyMethods.PolarVector(dist / 10, theta));
                                                    }
                                                }
                                            }
                                            if (((timer % 180 == 0 && t == 2) || (timer % 180 != 0 && t == 1)))
                                            {
                                                Projectile p = Main.projectile[Projectile.NewProjectile(new EntitySource_Misc(""), (NPC.Center + turretPos[t] * NPC.scale), QwertyMethods.PolarVector(9, turret[t].X + (float)Math.PI / 2), ProjectileType<MagicMineLayer>(), shotDamage, 0, Main.myPlayer, NPC.whoAmI, (player.Center - NPC.Center).Length())];
                                                p.scale = NPC.scale;

                                                Vector2 center = (NPC.Center + turretPos[t] * NPC.scale);
                                                for (int i = 0; i < 10; i++)
                                                {
                                                    float theta = turret[t].X + (float)Math.PI / 2 + Main.rand.NextFloat(-(float)Math.PI / 4, (float)Math.PI / 4);
                                                    float dist = Main.rand.NextFloat(60f, 100f);
                                                    if (Main.netMode != 1)
                                                    {
                                                        Dust.NewDustPerfect(center, DustType<B4PDust>(), QwertyMethods.PolarVector(dist / 10, theta));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else if (timer > startShooting / 2 + startAttacks)
                            {
                                for (int t = 0; t < turret.Length; t++)
                                {
                                    turret[t].Y = 1;
                                }
                            }
                            #endregion
                            break;
                    }

                    /*
                    if (Main.netMode == 1)
                    {
                        Main.NewText("client: " + attack);
                    }

                    if (Main.netMode == 2) // Server
                    {
                        NetMessage.BroadcastChatMessage(Terraria.Localization.NetworkText.FromLiteral("Server: " + attack), Color.Black);
                    }*/
                }
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (activeWalls)
            {
                Texture2D Walls = TextureAssets.Projectile[ProjectileType<SideLaser>()].Value;
                for (int h = 0; h < 20; h++)
                {
                    spriteBatch.Draw(Walls, new Vector2(NPC.Center.X + laserDistanceFromCenter, NPC.position.Y + 50 + h * 188) - screenPos,
                                  new Rectangle(316, 0, 316, 188), drawColor, NPC.rotation,
                                   new Vector2(316 * .5f, 188 * .5f), NPC.scale, SpriteEffects.None, 0);
                    spriteBatch.Draw(Walls, new Vector2(NPC.Center.X - laserDistanceFromCenter, NPC.position.Y + 50 + h * 188) - screenPos,
                                  new Rectangle(0, 0, 316, 188), drawColor, NPC.rotation,
                                   new Vector2(316 * .5f, 188 * .5f), NPC.scale, SpriteEffects.None, 0);
                }
            }
            Texture2D BK = Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/OLORD/BackGround").Value;
            float backgroundOffset = 100f; //70 for old
            Main.EntitySpriteDraw(BK, new Vector2(NPC.Center.X, NPC.position.Y - (backgroundOffset * NPC.scale)) - Main.screenPosition,
                          BK.Frame(), drawColor, NPC.rotation,
                           BK.Size() / 2, NPC.scale, SpriteEffects.None, 0);
            return true;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            for (int t = 0; t < turret.Length; t++)
            {
                spriteBatch.Draw(Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/OLORD/Turret").Value, NPC.Center + turretPos[t] * NPC.scale - screenPos,
                           new Rectangle(0, (int)turret[t].Y * 78, 142, 78), drawColor, turret[t].X,
                           new Vector2(142 * 0.5f, 78 * 0.5f), NPC.scale, SpriteEffects.None, 0f);
            }
            /*
            int num33 = (int)((NPC.position.X - 8f) / 16f);
            int num34 = (int)((NPC.position.X + (float)NPC.width + 8f) / 16f);
            int num35 = (int)((NPC.position.Y - 8f) / 16f);
            int num36 = (int)((NPC.position.Y + (float)NPC.height + 8f) / 16f);
            for (int l = num33; l <= num34; l++)
            {
                for (int m = num35; m <= num36; m++)
                {
                    if (Lighting.Brightness(l, m) == 0f)
                    {
                        color9 = Microsoft.Xna.Framework.Color.Black;
                    }
                }
            }*/
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Y = frameHeight * frame;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(timer);
            writer.Write(attack);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            timer = reader.ReadInt32();
            attack = reader.ReadInt32();
        }

        /// ////////////////////////////////////////////////////////
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            //Add the treasure bag (automatically checks for expert mode)
            npcLoot.Add(ItemDropRule.BossBag(ItemType<B4Bag>())); //this requires you to set BossBag in SetDefaults accordingly

            //All our drops here are based on "not expert", meaning we use .OnSuccess() to add them into the rule, which then gets added
            LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());


            //Notice we use notExpertRule.OnSuccess instead of npcLoot.Add so it only applies in normal mode
            notExpertRule.OnSuccess(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, ItemType<B4Bow>(), ItemType<BlackHoleStaff>(), ItemType<ExplosivePierce>(), ItemType<DreadnoughtStaff>()));
            //Finally add the leading rule
            npcLoot.Add(notExpertRule);


            notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
            notExpertRule.OnSuccess(ItemDropRule.Common(ItemType<TheDevourer>(), 5));
            npcLoot.Add(notExpertRule);

            base.ModifyNPCLoot(npcLoot);
        }
        public override void OnKill()
        {
            //This sets downedMinionBoss to true, and if it was false before, it initiates a lantern night
            NPC.SetEventFlagCleared(ref DownedBossSystem.downedOLORD, -1);
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(0.75f * NPC.lifeMax * bossLifeScale);
            NPC.damage = NPC.damage / 2;
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.GreaterHealingPotion;
        }

        public override bool CheckActive()
        {
            return playerDied;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0f;
        }
    }

    // The following laser shows a channeled ability, after charging up the laser will be fired
    // Using custom drawing, dust effects, and custom collision checks for tiles
    public class TurretLaser2 : ModProjectile
    {
        // The maximum charge value
        private const float MaxChargeValue = 120f;

        //The distance charge particle from the player center
        private const float MoveDistance = 63f;

        // The actual distance is stored in the ai0 field
        // By making a property to handle this it makes our life easier, and the accessibility more readable
        public float Distance;

        // The actual charge value is stored in the localAI0 field
        public float Charge
        {
            get { return Projectile.localAI[0]; }
            set { Projectile.localAI[0] = value; }
        }

        //public NPC shooter;
        // Are we at max charge? With c#6 you can simply use => which indicates this is a get only property
        public bool AtMaxCharge;

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.hostile = true;
            Projectile.hide = false;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.localAI[0]);
            writer.Write(Projectile.rotation);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.localAI[0] = reader.ReadSingle();
            Projectile.rotation = reader.ReadSingle();
        }

        // The AI of the projectile

        public override void AI()
        {
            //Main.NewText(Projectile.whoAmI +", "+Distance);
            //shooter = Main.npc[(int)Projectile.ai[0]];
            Vector2 mousePos = Main.MouseWorld;
            Player player = Main.player[Projectile.owner];
            /*
            if (Main.netMode == 1)
            {
                Main.NewText("client: " + Projectile.whoAmI + ", "+ Projectile.Center);
            }

            if (Main.netMode == 2) // Server
            {
                NetMessage.BroadcastChatMessage(Terraria.Localization.NetworkText.FromLiteral("Server: " + Projectile.whoAmI + ", " + Projectile.Center), Color.Black);
            }
            */
            #region Set projectile position

            //Vector2 diff = new Vector2((float)Math.Cos(shooter.rotation + (float)Math.PI / 2) * 14f, (float)Math.Sin(shooter.rotation + (float)Math.PI / 2) * 14f);

            Vector2 diff = new Vector2((float)Math.Cos(Projectile.rotation + (float)Math.PI / 2) * 14f, (float)Math.Sin(Projectile.rotation + (float)Math.PI / 2) * 14f);
            diff.Normalize();
            Projectile.velocity = diff;
            //Projectile.direction = Projectile.Center.X > shooter.Center.X ? 1 : -1;
            Projectile.netUpdate = true;

            //Projectile.position = new Vector2(shooter.Center.X, shooter.Center.Y) + Projectile.velocity * MoveDistance;
            Projectile.Center += Projectile.velocity * MoveDistance;

            int dir = Projectile.direction;

            #endregion

            #region Charging process
            // Kill the projectile if the player stops channeling

            // Do we still have enough mana? If not, we kill the projectile because we cannot use it anymore

            Vector2 offset = Projectile.velocity;
            offset *= MoveDistance - 20;
            //Vector2 pos = new Vector2(shooter.Center.X, shooter.Center.Y) + offset - new Vector2(10, 10);
            Vector2 pos = new Vector2(Projectile.ai[0], Projectile.ai[1]) + offset - new Vector2(10, 10);
            if (Charge < MaxChargeValue)
            {
                Charge++;
                Distance = 0;
                AtMaxCharge = false;
            }
            else
            {
                AtMaxCharge = true;
            }

            int chargeFact = (int)(Charge / 20f);

            #endregion
            if (Charge > 10 && !AtMaxCharge)
            {
                Vector2 center = Projectile.Center + QwertyMethods.PolarVector(-60, Projectile.rotation + (float)Math.PI / 2);
                for (int i = 0; i < 6; i++)
                {
                    float theta = Projectile.rotation + (float)Math.PI / 2 + Main.rand.NextFloat(-(float)Math.PI / 4, (float)Math.PI / 4);
                    float dist = Main.rand.NextFloat(30f, 60f);
                    if (Main.netMode != 1)
                    {
                        Dust.NewDustPerfect(center + QwertyMethods.PolarVector(dist, theta), DustType<TurretLaserDust>(), QwertyMethods.PolarVector(-dist / 10, theta));
                    }
                }
            }

            if (Charge < MaxChargeValue) return;
            //Vector2 start = new Vector2(shooter.Center.X, shooter.Center.Y);
            Vector2 start = new Vector2(Projectile.ai[0], Projectile.ai[1]);
            Vector2 unit = Projectile.velocity;
            unit *= -1;
            for (Distance = MoveDistance; Distance <= 2200f; Distance += 5f)
            {
                //start = new Vector2(shooter.Center.X, shooter.Center.Y) + Projectile.velocity * Distance;
                start = new Vector2(Projectile.ai[0], Projectile.ai[1]) + Projectile.velocity * Distance;
            }

            //Add lights
            DelegateMethods.v3_1 = new Vector3(0.8f, 0.8f, 1f);
            Utils.PlotTileLine(Projectile.Center, Projectile.Center + Projectile.velocity * (Distance - MoveDistance), 26,
                DelegateMethods.CastLight);
        }

        public int colorCounter;
        public Color lineColor;

        public override bool PreDraw(ref Color lightColor)
        {
            if (AtMaxCharge)
            {
                DrawLaser(TextureAssets.Projectile[Projectile.type].Value, new Vector2(Projectile.ai[0], Projectile.ai[1]),
                    Projectile.velocity, 48f, Projectile.damage, -1.57f, 1f, 4000f, Color.White, (int)MoveDistance);
            }
            else
            {
                Vector2 center = Projectile.Center + QwertyMethods.PolarVector(-60, Projectile.rotation + (float)Math.PI / 2);

                //float projRotation = shooter.rotation;
                float projRotation = Projectile.rotation;
                //update draw position

                float lineLength = 4000f;
                Color drawColor = lightColor;

                colorCounter++;

                if (colorCounter >= 20)
                {
                    colorCounter = 0;
                }
                else if (colorCounter >= 10)
                {
                    lineColor = Color.White;
                }
                else
                {
                    lineColor = Color.Red;
                }
                if (Charge > 10)
                {
                    Main.EntitySpriteDraw(Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/OLORD/laser").Value, new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
                        new Rectangle(0, 0, 1, (int)lineLength - 10), lineColor, projRotation,
                        new Vector2(0, 0), 1f, SpriteEffects.None, 0);
                }
            }
            return false;
        }

        // The core function of drawing a laser
        private int frame = 0;

        public void DrawLaser(Texture2D texture, Vector2 start, Vector2 unit, float step, int damage, float rotation = 0f, float scale = 1f, float maxDist = 4000f, Color color = default(Color), int transDist = 50)
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter % 4 == 0)
            {
                frame++;
                if (frame > 3)
                {
                    frame = 0;
                }
            }
            Vector2 origin = start;
            float r = unit.ToRotation() + rotation;

            #region Draw laser body
            for (float i = transDist; i <= Distance; i += step)
            {
                Color c = Color.White;
                origin = start + i * unit;
                Main.EntitySpriteDraw(texture, origin - Main.screenPosition,
                    new Rectangle(frame * 50, 50, 50, 48), i < transDist ? Color.Transparent : c, r,
                    new Vector2(50 * .5f, 48 * .5f), scale, 0, 0);
            }
            #endregion

            #region Draw laser tail
            Main.EntitySpriteDraw(texture, start + unit * (transDist - step) - Main.screenPosition,
                new Rectangle(frame * 50, 0, 50, 48), Color.White, r, new Vector2(50 * .5f, 48 * .5f), scale, 0, 0);
            #endregion
        }

        // Change the way of collision check of the projectile
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            // We can only collide if we are at max charge, which is when the laser is actually fired

            Player player = Main.player[Projectile.owner];
            Vector2 unit = Projectile.velocity;
            float point = 0f;
            // Run an AABB versus Line check to look for collisions, look up AABB collision first to see how it works
            // It will look for collisions on the given line using AABB
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), new Vector2(Projectile.ai[0], Projectile.ai[1]),
                new Vector2(Projectile.ai[0], Projectile.ai[1]) + unit * Distance, 50, ref point);
        }

        // Set custom immunity time on hitting an NPC
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 5;
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override void CutTiles()
        {
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Vector2 unit = Projectile.velocity;
            Utils.PlotTileLine(Projectile.Center, Projectile.Center + unit * Distance, (Projectile.width + 16) * Projectile.scale, DelegateMethods.CutTiles);
        }
    }

    public class SuperLaser2 : ModProjectile
    {
        // The maximum charge value
        private const float MaxChargeValue = 270f;

        //The distance charge particle from the player center
        private const float MoveDistance = 80f;

        // The actual distance is stored in the ai0 field
        // By making a property to handle this it makes our life easier, and the accessibility more readable
        public float Distance;

        // The actual charge value is stored in the localAI0 field
        public float Charge
        {
            get { return Projectile.localAI[0]; }
            set { Projectile.localAI[0] = value; }
        }

        public NPC shooter;

        // Are we at max charge? With c#6 you can simply use => which indicates this is a get only property
        public bool AtMaxCharge;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Super Laser");
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.hostile = true;
            Projectile.hide = false;
            Projectile.timeLeft = 2;
        }

        // The AI of the projectile
        public float downFromCenter = 130;

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.localAI[0]);
            writer.Write(Projectile.rotation);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.localAI[0] = reader.ReadSingle();
            Projectile.rotation = reader.ReadSingle();
        }

        public override void AI()
        {
            shooter = Main.npc[(int)Projectile.ai[0]];
            if (!shooter.active)
            {
                Projectile.Kill();
                return;
            }
            #region Set projectile position
            /*
            if (Main.netMode == 1)
            {
                Main.NewText("client: "  + Projectile.timeLeft);
            }

            if (Main.netMode == 2) // Server
            {
                NetMessage.BroadcastChatMessage(Terraria.Localization.NetworkText.FromLiteral("Server: "  + Projectile.timeLeft), Color.Black);
            }
            */
            Vector2 diff = new Vector2(0, 14);
            diff.Normalize();
            Projectile.velocity = diff;
            Projectile.direction = Projectile.Center.X > shooter.Center.X ? 1 : -1;
            Projectile.netUpdate = true;
            Projectile.timeLeft = 2;
            Projectile.Center = new Vector2(shooter.Center.X, shooter.Center.Y) + Projectile.velocity * MoveDistance;

            int dir = Projectile.direction;
            /*
            player.ChangeDir(dir);
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = (float)Math.Atan2(Projectile.velocity.Y * dir, Projectile.velocity.X * dir);
            */
            #endregion

            #region Charging process
            // Kill the projectile if the player stops channeling

            // Do we still have enough mana? If not, we kill the projectile because we cannot use it anymore

            Vector2 offset = Projectile.velocity;
            offset *= MoveDistance - 20;
            Vector2 pos = new Vector2(shooter.Center.X, shooter.Center.Y) + offset - new Vector2(10, 10);

            if (Charge < MaxChargeValue)
            {
                Charge++;
                Distance = 0;
                AtMaxCharge = false;
            }
            else
            {
                AtMaxCharge = true;
            }

            int chargeFact = (int)(Charge / 20f);
            if (Charge > 10 && !AtMaxCharge)
            {
                Vector2 center = Projectile.Center + QwertyMethods.PolarVector(-60, Projectile.rotation + (float)Math.PI / 2);
                for (int i = 0; i < 15; i++)
                {
                    float theta = Projectile.rotation + (float)Math.PI / 2 + Main.rand.NextFloat(-5 * (float)Math.PI / 12, 5 * (float)Math.PI / 12);
                    float dist = Main.rand.NextFloat(30f, 120f);
                    if (Main.netMode != 1)
                    {
                        Dust.NewDustPerfect(center + QwertyMethods.PolarVector(dist, theta), DustType<TurretLaserDust>(), QwertyMethods.PolarVector(-dist / 10, theta));
                    }
                }
            }

            #endregion

            if (Charge < MaxChargeValue) return;

            Main.LocalPlayer.GetModPlayer<OLORDScreenLock>().shake = true;
            Vector2 start = new Vector2(shooter.Center.X, shooter.Center.Y);
            Vector2 unit = Projectile.velocity;
            unit *= -1;
            for (Distance = MoveDistance; Distance <= 2200f; Distance += 5f)
            {
                start = new Vector2(shooter.Center.X, shooter.Center.Y) + Projectile.velocity * Distance;
                /*
                if (!Collision.CanHit(new Vector2(shooter.Center.X, shooter.Center.Y), 1, 1, start, 1, 1))
                {
                    Distance -= 5f;
                    break;
                }
                */
            }

            //Add lights
            DelegateMethods.v3_1 = new Vector3(10f, 10f, 10f);
            Utils.PlotTileLine(Projectile.Center, Projectile.Center + Projectile.velocity * (Distance - MoveDistance), 26,
                DelegateMethods.CastLight);
        }

        public int colorCounter;
        public Color lineColor;

        public override bool PreDraw(ref Color lightColor)
        {
            if (AtMaxCharge)
            {
                DrawLaser(TextureAssets.Projectile[Projectile.type].Value, new Vector2(shooter.Center.X, shooter.Center.Y),
                    Projectile.velocity, 35, Projectile.damage, -1.57f, 1f, 4000f, Color.White, (int)MoveDistance);
            }
            else
            {
                Vector2 center = Projectile.Center;

                float projRotation = 0;
                //update draw position

                float lineLength = 4000f;
                Color drawColor = lightColor;

                colorCounter++;

                if (colorCounter >= 20)
                {
                    colorCounter = 0;
                }
                else if (colorCounter >= 10)
                {
                    lineColor = Color.White;
                }
                else
                {
                    lineColor = Color.Red;
                }
                if (Charge > 10)
                {
                    Main.EntitySpriteDraw(Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/OLORD/laser").Value, new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
                    new Rectangle(0, 0, 1, (int)lineLength - 10), lineColor, projRotation,
                    new Vector2(0, 0), 1f, SpriteEffects.None, 0);
                }
            }

            return false;
        }

        // The core function of drawing a laser
        public void DrawLaser(Texture2D texture, Vector2 start, Vector2 unit, float step, int damage, float rotation = 0f, float scale = 1f, float maxDist = 4000f, Color color = default(Color), int transDist = 50)
        {
            Vector2 origin = start;
            float r = unit.ToRotation() + rotation;

            #region Draw laser body
            for (float i = transDist; i <= Distance; i += step)
            {
                Color c = Color.White;
                origin = start + i * unit;
                origin.Y += 118;
                Main.EntitySpriteDraw(texture, origin - Main.screenPosition,
                    new Rectangle(0, 130, 458, 118), i < transDist ? Color.Transparent : c, r,
                    new Vector2(458 * .5f, 118 * .5f), scale, 0, 0);
            }
            #endregion

            #region Draw laser tail
            Main.EntitySpriteDraw(texture, start + unit * (transDist) - Main.screenPosition,
                new Rectangle(0, 0, 458, 118), Color.White, r, new Vector2(458 * .5f, 118 * .5f), scale, 0, 0);
            #endregion

            #region Draw laser head
            Main.EntitySpriteDraw(texture, start + (Distance + step) * unit - Main.screenPosition,
                new Rectangle(0, 260, 458, 124), Color.White, r, new Vector2(458 * .5f, 124 * .5f), scale, 0, 0);
            #endregion
        }

        // Change the way of collision check of the projectile
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            // We can only collide if we are at max charge, which is when the laser is actually fired

            Player player = Main.player[Projectile.owner];
            Vector2 unit = Projectile.velocity;
            float point = 0f;
            // Run an AABB versus Line check to look for collisions, look up AABB collision first to see how it works
            // It will look for collisions on the given line using AABB
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), new Vector2(shooter.Center.X, shooter.Center.Y),
                new Vector2(shooter.Center.X, shooter.Center.Y) + unit * Distance, 472, ref point);
        }

        // Set custom immunity time on hitting an NPC
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 5;
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override void CutTiles()
        {
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Vector2 unit = Projectile.velocity;
            Utils.PlotTileLine(Projectile.Center, Projectile.Center + unit * Distance, (Projectile.width + 16) * Projectile.scale, DelegateMethods.CutTiles);
        }
    }

    public class OLORDScreenLock : ModPlayer
    {
        public int screenLock = -1;
        public bool shake = false;

        public override void ResetEffects()
        {
            screenLock = -1;
            shake = false;
        }

        public override void ModifyScreenPosition()
        {
            if (screenLock != -1 && Player.active && Player.statLife > 0)
            {
                NPC OLORD = Main.npc[screenLock];
                if (OLORD.active)
                {
                    Main.screenPosition.X = OLORD.Center.X - Main.screenWidth / 2;
                    Main.screenPosition.Y = OLORD.position.Y - 100;
                }
            }
            if (shake)
            {
                Main.screenPosition.X += Main.rand.Next(-20, 21);
                Main.screenPosition.Y += Main.rand.Next(-20, 21);
            }
        }
    }

    public class SideLaser : ModProjectile
    {
        private const float MaxChargeValue = 50f;
        private const float MoveDistance = 60f;
        public float Distance;

        // The actual charge value is stored in the localAI0 field
        public float Charge
        {
            get { return Projectile.localAI[0]; }
            set { Projectile.localAI[0] = value; }
        }

        public NPC shooter;
        public bool AtMaxCharge { get { return Charge == MaxChargeValue; } }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("O.L.O.R.D.'s wall");
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.hostile = true;
            Projectile.hide = true; // Prevents projectile from being drawn normally. Use in conjunction with DrawBehind.
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCs.Add(index);
        }
        // The AI of the projectile
        public float downFromCenter = 0;

        public override void AI()
        {
            shooter = Main.npc[(int)Projectile.ai[0]];

            if (!shooter.active)
            {
                Projectile.Kill();
                return;
            }
            #region Set projectile position

            Vector2 diff = new Vector2(0, 14);
            diff.Normalize();
            Projectile.velocity = diff;
            Projectile.direction = Projectile.Center.X > shooter.Center.X ? 1 : -1;
            Projectile.netUpdate = true;

            Projectile.position = new Vector2(shooter.Center.X + Projectile.ai[1], shooter.Center.Y + downFromCenter) + Projectile.velocity * MoveDistance;
            Projectile.timeLeft = 2;
            int dir = Projectile.direction;

            #endregion

            #region Charging process

            Vector2 offset = Projectile.velocity;
            offset *= MoveDistance - 20;
            Vector2 pos = new Vector2(shooter.Center.X + Projectile.ai[1], shooter.Center.Y + downFromCenter) + offset - new Vector2(10, 10);

            if (Charge < MaxChargeValue)
            {
                Charge++;
                Distance = 0;
            }

            int chargeFact = (int)(Charge / 20f);

            #endregion

            if (Charge < MaxChargeValue) return;
            Vector2 start = new Vector2(shooter.Center.X + Projectile.ai[1], shooter.Center.Y + downFromCenter);
            Vector2 unit = Projectile.velocity;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }

        // The core function of drawing a laser
        public void DrawLaser(SpriteBatch spriteBatch, Texture2D texture, Vector2 start, Vector2 unit, float step, int damage, float rotation = 0f, float scale = 1f, float maxDist = 4000f, Color color = default(Color), int transDist = 50)
        {
            Vector2 origin = start;
            float r = unit.ToRotation() + rotation;

            #region Draw laser body
            if (Projectile.ai[1] > 0)
            {
                for (float i = transDist; i <= Distance; i += step)
                {
                    Color c = Color.White;
                    origin = start + i * unit;
                    spriteBatch.Draw(texture, origin - Main.screenPosition,
                        new Rectangle(316, 0, 631, (int)step), color, r,
                        new Vector2(316 * .5f, step * .5f), scale, 0, 0);
                }
            }
            else
            {
                for (float i = transDist; i <= Distance; i += step)
                {
                    Color c = Color.White;
                    origin = start + i * unit;
                    spriteBatch.Draw(texture, origin - Main.screenPosition,
                        new Rectangle(0, 0, 316, (int)step), color, r,
                        new Vector2(316 * .5f, step * .5f), scale, 0, 0);
                }
            }

            #endregion
        }

        // Change the way of collision check of the projectile
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            // We can only collide if we are at max charge, which is when the laser is actually fired

            Player player = Main.player[Projectile.owner];
            Vector2 unit = Projectile.velocity;
            float point = 0f;
            // Run an AABB versus Line check to look for collisions, look up AABB collision first to see how it works
            // It will look for collisions on the given line using AABB
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), new Vector2(shooter.Center.X + Projectile.ai[1], shooter.Center.Y + downFromCenter),
                new Vector2(shooter.Center.X + Projectile.ai[1], shooter.Center.Y + downFromCenter) + unit * Distance, 316, ref point);
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override void CutTiles()
        {
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Vector2 unit = Projectile.velocity;
            Utils.PlotTileLine(Projectile.Center, Projectile.Center + unit * Distance, (Projectile.width + 16) * Projectile.scale, DelegateMethods.CutTiles);
        }
    }
}