using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common;
using QwertyMod.Content.Dusts;
using QwertyMod.Content.Items.Consumable.BossBag;
using QwertyMod.Content.Items.Consumable.Tiles.Trophy.Ancient;
using QwertyMod.Content.Items.Tool.Mining.Ancient;
using QwertyMod.Content.Items.Weapon.Magic.AncientMissile;
using QwertyMod.Content.Items.Weapon.Magic.AncientWave;
using QwertyMod.Content.Items.Weapon.Melee.Sword.AncientBlade;
using QwertyMod.Content.Items.Weapon.Melee.Yoyo.AncientThrow;
using QwertyMod.Content.Items.Weapon.Minion.AncientMinion;
using QwertyMod.Content.Items.Weapon.Morphs.AncientNuke;
using QwertyMod.Content.Items.Weapon.Ranged.Bow.Ancient;
using QwertyMod.Content.Items.Weapon.Ranged.Gun.Ancient;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

using System.IO;
using QwertyMod.Content.Items.Equipment.Vanity.BossMasks;

namespace QwertyMod.Content.NPCs.Bosses.AncientMachine
{
    [AutoloadBossHead]
    public class AncientMachine : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.NoMultiplayerSmoothingByType[NPC.type] = true;
            Main.npcFrameCount[NPC.type] = 4;

            NPCID.Sets.MPAllowedEnemies[NPC.type] = true; //For allowing use of SpawnOnPlayer in multiplayer

            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                PortraitScale = 0.1f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }

        public override void SetDefaults()
        {
            NPC.width = 392;
            NPC.height = 348;
            NPC.damage = 50;
            NPC.defense = 14;
            NPC.boss = true;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.value = 60f;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = -1;
            //aiType = 10;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            //music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/BuiltToDestroy");
            NPC.lifeMax = 6000;
            NPC.buffImmune[20] = true;
            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/BuiltToDestroy");
            }
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Sets the description of this NPC that is listed in the bestiary
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
            {
                new MoonLordPortraitBackgroundProviderBestiaryInfoElement(), //Plain black background
                new FlavorTextBestiaryInfoElement("Built long ago to destroy the great conquoer. Once complete its creators feared failure and retaliation and so sealed it away.")
            });
        }
        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.HealingPotion;
        }
        
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.6f * bossAdjustment);
            NPC.damage = (int)(NPC.damage * .6f);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0f;
        }

        private void ADI(int amount, Vector2 position)
        {
            for (int i = 0; i < amount; i++)
            {
                float theta = Main.rand.NextFloat(-MathF.PI, MathF.PI);
                Dust dust = Dust.NewDustPerfect(position, ModContent.DustType<AncientGlow>(), QwertyMethods.PolarVector(Main.rand.Next(amount / 200, amount / 20), theta));
                dust.noGravity = true;
            }
        }
        /*
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                Vector2 pos = NPC.Center + QwertyMethods.PolarVector(98, NPC.rotation) + QwertyMethods.PolarVector(120, NPC.rotation + MathF.PI / 2);
                Gore gore = Main.gore[Gore.NewGore(pos, NPC.velocity, mod.GetGoreSlot("Gores/Debris_1"), 1f)];
                gore.rotation = NPC.rotation;

                pos = NPC.Center + QwertyMethods.PolarVector(98, NPC.rotation) + QwertyMethods.PolarVector(120, NPC.rotation + MathF.PI / 2);
                gore = Main.gore[Gore.NewGore(pos, NPC.velocity, mod.GetGoreSlot("Gores/Debris_2"), 1f)];
                gore.rotation = NPC.rotation;

                pos = NPC.Center + QwertyMethods.PolarVector(144, NPC.rotation) + QwertyMethods.PolarVector(67, NPC.rotation + MathF.PI / 2);
                gore = Main.gore[Gore.NewGore(pos, NPC.velocity, mod.GetGoreSlot("Gores/Debris_3"), 1f)];
                gore.rotation = NPC.rotation;
                pos = NPC.Center + QwertyMethods.PolarVector(144, NPC.rotation) + QwertyMethods.PolarVector(-67, NPC.rotation + MathF.PI / 2);
                gore = Main.gore[Gore.NewGore(pos, NPC.velocity, mod.GetGoreSlot("Gores/Debris_4"), 1f)];
                gore.rotation = NPC.rotation;

                pos = NPC.Center + QwertyMethods.PolarVector(-15, NPC.rotation) + QwertyMethods.PolarVector(102, NPC.rotation + MathF.PI / 2);
                gore = Main.gore[Gore.NewGore(pos, NPC.velocity, mod.GetGoreSlot("Gores/Debris_5"), 1f)];
                gore.rotation = NPC.rotation;
                pos = NPC.Center + QwertyMethods.PolarVector(-15, NPC.rotation) + QwertyMethods.PolarVector(-102, NPC.rotation + MathF.PI / 2);
                gore = Main.gore[Gore.NewGore(pos, NPC.velocity, mod.GetGoreSlot("Gores/Debris_6"), 1f)];
                gore.rotation = NPC.rotation;

                pos = NPC.Center + QwertyMethods.PolarVector(-15, NPC.rotation) + QwertyMethods.PolarVector(0, NPC.rotation + MathF.PI / 2);
                gore = Main.gore[Gore.NewGore(pos, NPC.velocity, mod.GetGoreSlot("Gores/Debris_7"), 1f)];
                gore.rotation = NPC.rotation;
                pos = NPC.Center + QwertyMethods.PolarVector(-15, NPC.rotation) + QwertyMethods.PolarVector(0, NPC.rotation + MathF.PI / 2);
                gore = Main.gore[Gore.NewGore(pos, NPC.velocity, mod.GetGoreSlot("Gores/Debris_8"), 1f)];
                gore.rotation = NPC.rotation;
                pos = NPC.Center + QwertyMethods.PolarVector(-154, NPC.rotation) + QwertyMethods.PolarVector(0, NPC.rotation + MathF.PI / 2);
                gore = Main.gore[Gore.NewGore(pos, NPC.velocity, mod.GetGoreSlot("Gores/Debris_9"), 1f)];
                gore.rotation = NPC.rotation;
                pos = NPC.Center + QwertyMethods.PolarVector(77, NPC.rotation) + QwertyMethods.PolarVector(0, NPC.rotation + MathF.PI / 2);
                gore = Main.gore[Gore.NewGore(pos, NPC.velocity, mod.GetGoreSlot("Gores/Debris_10"), 1f)];
                gore.rotation = NPC.rotation;
                pos = NPC.Center + QwertyMethods.PolarVector(166, NPC.rotation) + QwertyMethods.PolarVector(0, NPC.rotation + MathF.PI / 2);
                gore = Main.gore[Gore.NewGore(pos, NPC.velocity, mod.GetGoreSlot("Gores/Debris_11"), 1f)];
                gore.rotation = NPC.rotation;

                pos = NPC.Center + QwertyMethods.PolarVector(-65, NPC.rotation) + QwertyMethods.PolarVector(79, NPC.rotation + MathF.PI / 2);
                gore = Main.gore[Gore.NewGore(pos, NPC.velocity, mod.GetGoreSlot("Gores/Debris_12"), 1f)];
                gore.rotation = NPC.rotation;
                pos = NPC.Center + QwertyMethods.PolarVector(-65, NPC.rotation) + QwertyMethods.PolarVector(-79, NPC.rotation + MathF.PI / 2);
                gore = Main.gore[Gore.NewGore(pos, NPC.velocity, mod.GetGoreSlot("Gores/Debris_13"), 1f)];
                gore.rotation = NPC.rotation;
            }
        }
        */
        private Vector2 MissileOffset = new Vector2();
        private int frame = 0;
        private const int defaultFrameX = 22;
        private const int defaultFrameY = 148;

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;

            if (NPC.frameCounter > 10)
            {
                frame++;
                if (frame >= 7)
                {
                    frame = 0;
                }
                NPC.frameCounter = 0;
            }
            if (frame > 3)
            {
                NPC.frame.Y = (frame - 4) * frameHeight;
                NPC.frame.X = NPC.width;
                NPC.frame.Width = NPC.width;
            }
            else
            {
                NPC.frame.Y = frame * frameHeight;
                NPC.frame.X = 0;
                NPC.frame.Width = NPC.width;
            }
            switch (frame)
            {
                case 0:
                    MissileOffset = new Vector2(defaultFrameX, defaultFrameY);
                    break;

                case 1:
                    MissileOffset = new Vector2(defaultFrameX - 2, defaultFrameY + 2);
                    break;

                case 2:
                    MissileOffset = new Vector2(defaultFrameX - 2, defaultFrameY + 6);
                    break;

                case 3:
                    MissileOffset = new Vector2(defaultFrameX - 2, defaultFrameY + 8);
                    break;

                case 4:
                    MissileOffset = new Vector2(defaultFrameX - 4, defaultFrameY + 10);
                    break;

                case 5:
                    MissileOffset = new Vector2(defaultFrameX + 2, defaultFrameY + 6);
                    break;

                case 6:
                    MissileOffset = new Vector2(defaultFrameX, defaultFrameY);
                    break;
            }
        }

        public const int RingRadius = 300;
        public const int RingDustQty = 400;
        public int damage = 30;
        public int switchTime = 140;
        public int moveCount = -1;
        public int fireCount = 0;
        public int attackType = 1;
        public int AI_Timer = 0;
        public bool runOnce = true;
        private Vector2 moveTo;
        private float orbSpeed = 12;
        private bool angry;
        private bool justTeleported;
        private int missileReloadCounter;
        private int missileFlashCounter;
        private int missileGlowFrame = 0;
        private float angle = MathF.PI / 6;
        bool AttemptTeleport(ref Vector2 moveTo)
        {
            moveTo = moveTo + new Vector2(MathF.Cos(NPC.ai[0]) * 1050, MathF.Sin(NPC.ai[0]) * 600);
            for(int i =0; i< Main.player.Length; i++)
            {
                Player curPlayer = Main.player[i];
                if(Collision.CheckAABBvAABBCollision(Main.player[i].position, Main.player[i].Size, NPC.position, NPC.Size))
                {
                    return false;
                }
            }
            return true;
        }

        public override void AI()
        {
            missileFlashCounter++;
            if (missileFlashCounter > 60)
            {
                missileFlashCounter = 0;
            }
            else if (missileFlashCounter > 30)
            {
                missileGlowFrame = 1;
            }
            else
            {
                missileGlowFrame = 0;
            }
            if (missileReloadCounter > 0)
            {
                missileReloadCounter--;
            }

            if (NPC.life < NPC.lifeMax / 2 && Main.expertMode)
            {
                angry = true;
            }
            switchTime = (int)(((float)NPC.life / (float)NPC.lifeMax) * 60) + 90;
            Player player = Main.player[NPC.target];
            NPC.TargetClosest(true);
            if (runOnce)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.ai[0] = Main.rand.NextFloat(-MathF.PI, MathF.PI);
                    NPC.netUpdate = true;
                }
                NPC.ai[2] = NPC.Center.X;
                NPC.ai[3] = NPC.Center.Y;
                runOnce = false;
                moveTo = new Vector2(player.Center.X + MathF.Cos(NPC.ai[0]) * 700, player.Center.Y + MathF.Sin(NPC.ai[0]) * 400);
            }
            AI_Timer++;

            if (Main.expertMode)
            {
                damage = 18;
            }

            if (!player.active || player.dead)
            {
                NPC.TargetClosest(false);
                player = Main.player[NPC.target];
                if (!player.active || player.dead)
                {
                    NPC.velocity = new Vector2(0f, 10f);
                    if (NPC.timeLeft > 10)
                    {
                        NPC.timeLeft = 10;
                    }
                    return;
                }
            }

            float targetAngle = new Vector2(player.Center.X - NPC.Center.X, player.Center.Y - NPC.Center.Y).ToRotation();

            NPC.rotation = targetAngle;

            if (AI_Timer > switchTime)
            {
                moveCount++;
                //Main.NewText(moveCount);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.ai[0] = Main.rand.NextFloat(-MathF.PI, MathF.PI);
                    NPC.netUpdate = true;
                }
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    moveTo = player.Center;
                    for(int i = 0; i < 100; i++)
                    {
                        if(AttemptTeleport(ref moveTo))
                        {
                            break;
                        }
                    }
                    NPC.ai[2] = moveTo.X;
                    NPC.ai[3] = moveTo.Y;
                    AI_Timer = 0;
                    NPC.netUpdate = true;
                    justTeleported = true;
                }
            }
            if (moveCount >= 3 || (Main.expertMode && ((float)NPC.life / NPC.lifeMax) < 0.1f))
            {
                #region special attacks

                NPC.velocity = new Vector2(0, 0);

                if (AI_Timer == switchTime / 2)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        NPC.ai[1] = Main.rand.Next(3);
                        NPC.netUpdate = true;
                    }

                    if (NPC.ai[1] == 0)
                    {
                        SoundEngine.PlaySound(SoundID.MaxMana, NPC.Center);
                        for (int r = 0; r < 5; r++)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, MathF.Cos((NPC.rotation + r * MathF.PI / 8) - MathF.PI / 4) * orbSpeed, MathF.Sin((NPC.rotation + r * MathF.PI / 8) - MathF.PI / 4) * orbSpeed, ModContent.ProjectileType<AncientEnergy>(), damage, 3f, Main.myPlayer);
                            }
                        }
                    }
                    if (NPC.ai[1] == 1)
                    {
                        SoundEngine.PlaySound(SoundID.MaxMana, NPC.Center);
                        missileReloadCounter = 60;
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + QwertyMethods.PolarVector(MissileOffset.X, NPC.rotation) + QwertyMethods.PolarVector(MissileOffset.Y, NPC.rotation + MathF.PI / 2), QwertyMethods.PolarVector(orbSpeed, NPC.rotation + angle), ModContent.ProjectileType<AncientMissile>(), damage, 3f, Main.myPlayer);
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + QwertyMethods.PolarVector(MissileOffset.X, NPC.rotation) + QwertyMethods.PolarVector(-MissileOffset.Y, NPC.rotation + MathF.PI / 2), QwertyMethods.PolarVector(orbSpeed, NPC.rotation - angle), ModContent.ProjectileType<AncientMissile>(), damage, 3f, Main.myPlayer);
                        }
                    }
                    if (NPC.ai[1] == 2)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            float d = new Vector2(player.Center.X - NPC.Center.X, player.Center.Y - NPC.Center.Y).ToRotation();
                            Vector2 pos = NPC.Center + QwertyMethods.PolarVector(200, NPC.rotation) + QwertyMethods.PolarVector(100, NPC.rotation + MathF.PI / 2);
                            NPC.NewNPC(NPC.GetSource_FromAI(), (int)pos.X, (int)pos.Y, ModContent.NPCType<AncientMinion>(), 0, NPC.whoAmI);
                            pos = NPC.Center + QwertyMethods.PolarVector(200, NPC.rotation) + QwertyMethods.PolarVector(-100, NPC.rotation + MathF.PI / 2);
                            NPC.NewNPC(NPC.GetSource_FromAI(), (int)pos.X, (int)pos.Y, ModContent.NPCType<AncientMinion>(), 0, NPC.whoAmI);
                            if (angry)
                            {
                                pos = NPC.Center + QwertyMethods.PolarVector(100, NPC.rotation) + QwertyMethods.PolarVector(-200, NPC.rotation + MathF.PI / 2);
                                NPC.NewNPC(NPC.GetSource_FromAI(), (int)pos.X, (int)pos.Y, ModContent.NPCType<AncientMinion>(), 0, NPC.whoAmI);
                                pos = NPC.Center + QwertyMethods.PolarVector(100, NPC.rotation) + QwertyMethods.PolarVector(200, NPC.rotation + MathF.PI / 2);
                                NPC.NewNPC(NPC.GetSource_FromAI(), (int)pos.X, (int)pos.Y, ModContent.NPCType<AncientMinion>(), 0, NPC.whoAmI);
                            }
                        }
                    }
                }
                if (AI_Timer == 3 * switchTime / 4)
                {
                    if (angry)
                    {
                        if (NPC.ai[1] == 0)
                        {
                            SoundEngine.PlaySound(SoundID.MaxMana, NPC.Center);
                            for (int r = 0; r < 4; r++)
                            {
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, MathF.Cos((NPC.rotation + r * MathF.PI / 6) - MathF.PI / 4) * orbSpeed, MathF.Sin((NPC.rotation + r * MathF.PI / 6) - MathF.PI / 4) * orbSpeed, ModContent.ProjectileType<AncientEnergy>(), damage, 3f, Main.myPlayer);
                                }
                            }
                        }
                        if (NPC.ai[1] == 1)
                        {
                            SoundEngine.PlaySound(SoundID.MaxMana, NPC.Center);
                            missileReloadCounter = 60;
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + QwertyMethods.PolarVector(MissileOffset.X, NPC.rotation) + QwertyMethods.PolarVector(MissileOffset.Y, NPC.rotation + MathF.PI / 2), QwertyMethods.PolarVector(orbSpeed, NPC.rotation + angle), ModContent.ProjectileType<AncientMissile>(), damage, 3f, Main.myPlayer);
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + QwertyMethods.PolarVector(MissileOffset.X, NPC.rotation) + QwertyMethods.PolarVector(-MissileOffset.Y, NPC.rotation + MathF.PI / 2), QwertyMethods.PolarVector(orbSpeed, NPC.rotation - angle), ModContent.ProjectileType<AncientMissile>(), damage, 3f, Main.myPlayer);
                            }
                        }
                    }
                    moveCount = -1;
                }

                #endregion special attacks
            }
            else
            {
                if (AI_Timer == switchTime / 2)
                {
                    SoundEngine.PlaySound(SoundID.MaxMana, NPC.Center);
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(MathF.Cos((NPC.rotation)), MathF.Sin(NPC.rotation)) * orbSpeed, ModContent.ProjectileType<AncientEnergy>(), damage, 3f, Main.myPlayer);
                    }
                }
                if (AI_Timer == 3 * switchTime / 4 && angry)
                {
                    SoundEngine.PlaySound(SoundID.MaxMana, NPC.Center);
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(MathF.Cos((NPC.rotation)), MathF.Sin(NPC.rotation)) * orbSpeed, ModContent.ProjectileType<AncientEnergy>(), damage, 3f, Main.myPlayer);
                    }
                }
            }
            //NPC.velocity = (moveTo - NPC.Center) * .02f;
            //QwertyMethods.ServerClientCheck("" + NPC.ai[2] + ", " + NPC.ai[3]);
            if(AI_Timer > 1)
            {
                if(justTeleported)
                {    
                    for (int i = 0; i < RingDustQty; i++)
                    {
                        float theta = Main.rand.NextFloat(-MathF.PI, MathF.PI);

                        Dust dust = Dust.NewDustPerfect(NPC.Center + QwertyMethods.PolarVector(RingRadius, theta), ModContent.DustType<AncientGlow>(), QwertyMethods.PolarVector(-RingRadius / 10, theta));
                        dust.noGravity = true;
                    }
                NPC.netOffset *= 0;
                }
                NPC.Center = new Vector2(NPC.ai[2], NPC.ai[3]);
                if (justTeleported)
                {
                    SoundEngine.PlaySound(SoundID.MaxMana, NPC.Center);
                    for (int i = 0; i < RingDustQty; i++)
                    {
                        float theta = Main.rand.NextFloat(-MathF.PI, MathF.PI);
                        Dust dust = Dust.NewDustPerfect(NPC.Center, ModContent.DustType<AncientGlow>(), QwertyMethods.PolarVector(RingRadius / 10, theta));
                        dust.noGravity = true;
                    }
                    justTeleported = false;
                }
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Rectangle mF = new Rectangle(0, missileGlowFrame * 36, 20, 36);

            spriteBatch.Draw(ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/AncientMachine/AncientMissile").Value, NPC.Center - screenPos + QwertyMethods.PolarVector(MissileOffset.X, NPC.rotation) + QwertyMethods.PolarVector(MissileOffset.Y, NPC.rotation + MathF.PI / 2) + QwertyMethods.PolarVector(-missileReloadCounter / 2, NPC.rotation + angle),
                        mF, drawColor, NPC.rotation + MathF.PI / 2 + angle,
                        new Vector2(mF.Width * 0.5f, mF.Height * 0.5f), 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/AncientMachine/AncientMissile").Value, NPC.Center - screenPos + QwertyMethods.PolarVector(MissileOffset.X, NPC.rotation) + QwertyMethods.PolarVector(-MissileOffset.Y, NPC.rotation + MathF.PI / 2) + QwertyMethods.PolarVector(-missileReloadCounter / 2, NPC.rotation - angle),
                        mF, drawColor, NPC.rotation + MathF.PI / 2 - angle,
                        new Vector2(mF.Width * 0.5f, mF.Height * 0.5f), 1f, SpriteEffects.None, 0f);

            spriteBatch.Draw(ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/AncientMachine/AncientMissile_Glow").Value, NPC.Center - screenPos + QwertyMethods.PolarVector(MissileOffset.X, NPC.rotation) + QwertyMethods.PolarVector(MissileOffset.Y, NPC.rotation + MathF.PI / 2) + QwertyMethods.PolarVector(-missileReloadCounter / 2, NPC.rotation + angle),
                        mF, Color.White, NPC.rotation + MathF.PI / 2 + angle,
                        new Vector2(mF.Width * 0.5f, mF.Height * 0.5f), 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/AncientMachine/AncientMissile_Glow").Value, NPC.Center - screenPos + QwertyMethods.PolarVector(MissileOffset.X, NPC.rotation) + QwertyMethods.PolarVector(-MissileOffset.Y, NPC.rotation + MathF.PI / 2) + QwertyMethods.PolarVector(-missileReloadCounter / 2, NPC.rotation - angle),
                        mF, Color.White, NPC.rotation + MathF.PI / 2 - angle,
                        new Vector2(mF.Width * 0.5f, mF.Height * 0.5f), 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/AncientMachine/AncientMachine").Value, NPC.Center - screenPos,
                        NPC.frame, drawColor, NPC.rotation,
                        new Vector2(NPC.width * 0.5f, NPC.height * 0.5f), 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/AncientMachine/AncientMachine_Glow").Value, NPC.Center - screenPos,
                        NPC.frame, Color.White, NPC.rotation,
                        new Vector2(NPC.width * 0.5f, NPC.height * 0.5f), 1f, SpriteEffects.None, 0f);

            return false;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(AI_Timer);
            writer.Write(justTeleported);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            AI_Timer = reader.ReadInt32();
            justTeleported = reader.ReadBoolean();
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            //Add the treasure bag (automatically checks for expert mode)
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<AncientMachineBag>())); //this requires you to set BossBag in SetDefaults accordingly

            //All our drops here are based on "not expert", meaning we use .OnSuccess() to add them into the rule, which then gets added
            LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());

            //Notice we use notExpertRule.OnSuccess instead of npcLoot.Add so it only applies in normal mode
            notExpertRule.OnSuccess(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, ModContent.ItemType<AncientBlade>(), ModContent.ItemType<AncientSniper>(), ModContent.ItemType<AncientWave>(), ModContent.ItemType<AncientThrow>(), ModContent.ItemType<AncientMinionStaff>(), ModContent.ItemType<AncientMissileStaff>(), ModContent.ItemType<AncientLongbow>(), ModContent.ItemType<AncientNuke>()));
            //Finally add the leading rule
            npcLoot.Add(notExpertRule);

            //Boss masks are spawned with 1/7 chance
            //notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
            //notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<PolarMask>(), 7));
            //npcLoot.Add(notExpertRule);

            notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<AncientMiner>(), 7));
            npcLoot.Add(notExpertRule);

            //Boss masks are spawned with 1/7 chance
            notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<AncientMachineMask>(), 7));
            npcLoot.Add(notExpertRule);

            //Trophies are spawned with 1/10 chance
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<AncientMachineTrophy>(), 10));
            
            

			// ItemDropRule.MasterModeCommonDrop for the relic
			npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<Items.Consumable.Tiles.Relics.AncientMachineRelic>()));


            base.ModifyNPCLoot(npcLoot);
        }
        public override void OnKill()
        {
            //This sets downedMinionBoss to true, and if it was false before, it initiates a lantern night
            NPC.SetEventFlagCleared(ref DownedBossSystem.downedAncient, -1);
        }

        public class AncientEnergy : ModProjectile
        {
            public override void SetStaticDefaults()
            {
                //DisplayName,SetDefault("Ancient Energy");
                Main.projFrames[Projectile.type] = 1;
            }

            public override void SetDefaults()
            {
                Projectile.aiStyle = 1;
                AIType = ProjectileID.Bullet;
                Projectile.width = 62;
                Projectile.height = 62;
                Projectile.friendly = false;
                Projectile.hostile = true;
                Projectile.penetrate = -1;
                Projectile.timeLeft = 120;
                Projectile.tileCollide = false;
                Projectile.alpha = 255;
            }

            public int dustTimer;

            public override void AI()
            {
                if (Projectile.alpha > 0)
                {
                    //Projectile.alpha -= (int)(255f / 180f);
                    Projectile.alpha -= 2;
                }
                else
                {
                    Projectile.alpha = 0;
                }
                Projectile.scale = .5f + (.5f * 1 - (Projectile.alpha / 255f));
                for (int d = 0; d < Projectile.alpha / 10; d++)
                {
                    float theta = Main.rand.NextFloat(-MathF.PI, MathF.PI);
                    Dust dust = Dust.NewDustPerfect(Projectile.Center + QwertyMethods.PolarVector(70, theta), ModContent.DustType<AncientGlow>(), QwertyMethods.PolarVector(-10, theta) + Projectile.velocity);

                    dust.alpha = 255;
                }
                //Main.NewText(Projectile.alpha);
                dustTimer++;
                if (dustTimer > 2)
                {
                    int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<AncientGlow>(), 0, 0, 0, default(Color), .4f);

                    dustTimer = 0;
                }
                Projectile.frameCounter++;
            }

            public override bool PreDraw(ref Color drawColor)
            {
                Main.EntitySpriteDraw(ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/AncientMachine/AncientEnergy").Value, new Vector2(Projectile.Center.X - Main.screenPosition.X, Projectile.Center.Y - Main.screenPosition.Y),
                        new Rectangle(0, Projectile.frame * Projectile.height, Projectile.width, Projectile.height), Color.Lerp(new Color(1f, 1f, 1f, 1f), new Color(0, 0, 0, 0), (float)Projectile.alpha / 255f), Projectile.rotation,
                        new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f), Projectile.scale, SpriteEffects.None, 0);
                return false;
            }
        }
    }
    public class AncientMissile : ModProjectile
    {
        public override void SetStaticDefaults()
        {

            Main.projFrames[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.width = 20;
            Projectile.height = Projectile.width;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 240;
            Projectile.tileCollide = false;
        }

        private float missileAcceleration = .5f;
        private float topSpeed = 10f;
        private int timer;
        private float closest = 10000;

        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter % 30 == 0)
            {
                Projectile.frame++;
                if (Projectile.frame >= 2)
                {
                    Projectile.frame = 0;
                }
            }
            timer++;
            if (timer > 30)
            {
                //Player player = Main.player[Projectile.owner];
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for (int i = 0; i < Main.maxPlayers; i++)
                    {
                        if (Main.player[i].active && !Main.player[i].dead && (Projectile.Center - Main.player[i].Center).Length() < closest)
                        {
                            closest = (Projectile.Center - Main.player[i].Center).Length();
                            Projectile.ai[0] = (Main.player[i].Center - Projectile.Center).ToRotation();
                            Projectile.netUpdate = true;
                        }
                    }
                }
                Projectile.velocity += new Vector2(MathF.Cos(Projectile.ai[0]) * missileAcceleration, MathF.Sin(Projectile.ai[0]) * missileAcceleration);
                if (Projectile.velocity.Length() > topSpeed)
                {
                    Projectile.velocity = Projectile.velocity.SafeNormalize(-Vector2.UnitY) * 10;
                }
            }
            //int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, mod.DustType("AncientGlow"), 0, 0, 0, default(Color), .4f);
            Dust dust = Dust.NewDustPerfect(Projectile.Center + QwertyMethods.PolarVector(26, Projectile.rotation + MathF.PI / 2) + QwertyMethods.PolarVector(Main.rand.Next(-6, 6), Projectile.rotation), ModContent.DustType<AncientGlow>());
            closest = 10000;
        }

        public override void OnKill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<AncientBlast>(), Projectile.damage, Projectile.knockBack, player.whoAmI);
        }

        public override bool PreDraw(ref Color drawColor)
        {
            Main.EntitySpriteDraw(ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/AncientMachine/AncientMissile").Value, new Vector2(Projectile.Center.X - Main.screenPosition.X, Projectile.Center.Y - Main.screenPosition.Y),
                    new Rectangle(0, Projectile.frame * 36, Projectile.width, 36), drawColor, Projectile.rotation,
                    new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f), 1f, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/AncientMachine/AncientMissile_Glow").Value, new Vector2(Projectile.Center.X - Main.screenPosition.X, Projectile.Center.Y - Main.screenPosition.Y),
                    new Rectangle(0, Projectile.frame * 36, Projectile.width, 36), Color.White, Projectile.rotation,
                    new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f), 1f, SpriteEffects.None, 0);
            return false;
        }
    }

    public class AncientBlast : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.width = 150;
            Projectile.height = 150;
            Projectile.friendly = true;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 2;
            Projectile.usesLocalNPCImmunity = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.width = 150;
            Projectile.height = 150;

            SoundEngine.PlaySound(SoundID.Item62, Projectile.position);

            for (int i = 0; i < 400; i++)
            {
                float theta = Main.rand.NextFloat(-MathF.PI, MathF.PI);
                Dust dust = Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<AncientGlow>(), QwertyMethods.PolarVector(Main.rand.Next(2, 20), theta));
                dust.noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[Projectile.owner] = 0;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
}