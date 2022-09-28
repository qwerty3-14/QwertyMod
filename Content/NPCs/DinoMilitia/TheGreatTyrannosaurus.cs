using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Content.Items.Weapon.Magic.ExtinctionGun;
using QwertyMod.Content.Items.Weapon.Melee.Flail.Ankylosaurus;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.NPCs.DinoMilitia
{
    [AutoloadBossHead]
    public class TheGreatTyrannosaurus : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Great Tyrannosaurus");
            Main.npcFrameCount[NPC.type] = 4;
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                CustomTexturePath = "QwertyMod/Content/NPCs/DinoMilitia/TheGreatTyrannosaurus_Bestiary",
                PortraitScale = 0.6f,
                PortraitPositionYOverride = 40f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }

        public override void SetDefaults()
        {
            NPC.width = 166;
            NPC.height = 154;
            NPC.damage = 100;
            NPC.defense = 22;
            NPC.lifeMax = 28000;

            NPC.boss = true;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.value = 60f;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = 3;

            AIType = 27;
            AnimationType = -1;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/OldDinosNewGuns");
            }

        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Sets the description of this NPC that is listed in the bestiary

            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
            {
                new MoonLordPortraitBackgroundProviderBestiaryInfoElement(), //Plain black background
                new FlavorTextBestiaryInfoElement("They made fun of his arms... so he got better ones!")
            });
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.6f * bossLifeScale);
            NPC.damage = (int)(NPC.damage * .6f);
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (DinoEvent.EventActive && !NPC.AnyNPCs(NPCType<Velocichopper>()) && !NPC.AnyNPCs(NPCType<TheGreatTyrannosaurus>()))
            {
                if (DinoEvent.DinoKillCount >= 140)
                {
                    return 50f;
                }
                return 3f;
            }
            return 0f;
        }
        public override void OnKill()
        {
            if (DinoEvent.EventActive)
            {
                DinoEvent.DinoKillCount += 10;
                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
            }
        }
        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.GreaterHealingPotion;
        }

        public override bool CheckActive()
        {
            Player player = Main.player[NPC.target];
            float playerDistance = (float)Math.Sqrt((NPC.Center.X - player.Center.X) * (NPC.Center.X - player.Center.X) + (NPC.Center.Y - player.Center.Y) * (NPC.Center.Y - player.Center.Y));
            if (playerDistance > 2000f)
            {
                return true;
            }
            return false;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
            }
            for (int i = 0; i < 10; i++)
            {
                int dustType = 148;
                int dustIndex = Dust.NewDust(NPC.position, NPC.width, NPC.height, dustType);
                Dust dust = Main.dust[dustIndex];
                dust.velocity.X = dust.velocity.X + Main.rand.Next(-50, 51) * 0.01f;
                dust.velocity.Y = dust.velocity.Y + Main.rand.Next(-50, 51) * 0.01f;
                dust.scale *= 1f + Main.rand.Next(-30, 31) * 0.01f;
            }
        }

        private const int moveFrameType = 0;
        private const int attackFrameType = 1;
        private const int launchFrameType = 4;
        public int timer = 0;
        public int Pos = 1;
        public int damage = 35;
        public int walkTime = 300;
        public int moveCount = 0;
        public int fireCount = 0;
        public int frameType = 0;
        public int attack = 0;
        public bool meteorsLaunched = false;
        public int multiplayerAttackCycle = 1;
        private int[] attackreloadTimes = new int[] { 10, 4, 30 };
        private Vector2 gunOffset = new Vector2(78, 76);
        private float gunRot = (float)Math.PI;
        private int meteorTime;
        private int gunFrame = 0;

        public override void AI()
        {
            if (Main.expertMode)
            {
                damage = 25;
            }

            Player player = Main.player[NPC.target];
            NPC.TargetClosest(true);
            if (!player.active || player.dead)
            {
                NPC.TargetClosest(false);
                player = Main.player[NPC.target];
                if (!player.active || player.dead)
                {
                    NPC.velocity = new Vector2(0f, -100f);
                    if (NPC.timeLeft > 10)
                    {
                        NPC.timeLeft = 10;
                    }
                    return;
                }
            }

            timer++;
            NPC.frameCounter++;
            gunOffset = NPC.spriteDirection == 1 ? new Vector2(78, 76) : new Vector2(166 - 78, 76);
            gunFrame = 0;
            if (timer > walkTime)
            {
                NPC.aiStyle = -1;
                NPC.directionY = -Math.Sign(player.Center.X - NPC.Center.X);
                meteorsLaunched = attack == 2;
                NPC.velocity.X = 0;
                NPC.velocity.Y += 4.3f;

                if (attack == 2)
                {
                    gunRot = NPC.spriteDirection == 1 ? 0f : (float)Math.PI;
                }
                else
                {
                    gunRot = (player.Center - (NPC.position + gunOffset)).ToRotation();
                    if (NPC.frameCounter % 4 > 1)
                    {
                        gunFrame = 1;
                    }
                }
                if ((timer - walkTime) % attackreloadTimes[attack] == 0)
                {
                    SoundEngine.PlaySound(SoundID.DoubleJump, NPC.position + gunOffset);
                    if (Main.netMode != 1)
                    {
                        float spread = MathHelper.ToRadians(Main.rand.Next(-15, 15));

                        switch (attack)
                        {
                            case 0:
                                Projectile.NewProjectile(new EntitySource_Misc(""), NPC.position + gunOffset + QwertyMethods.PolarVector(56, gunRot), QwertyMethods.PolarVector(10f, gunRot + spread), ProjectileType<SnowFlake>(), damage, 3f, Main.myPlayer);
                                break;

                            case 1:
                                NPC.NewNPC(new EntitySource_Misc(""), (int)(NPC.position + gunOffset + QwertyMethods.PolarVector(56, gunRot)).X, (int)(NPC.position + gunOffset + QwertyMethods.PolarVector(56, gunRot)).Y, NPCType<Mosquitto>(), 0, spread, NPC.direction);
                                break;

                            case 2:
                                Projectile.NewProjectile(new EntitySource_Misc(""), NPC.Center + new Vector2(-24 * NPC.direction, -74f), Vector2.UnitY * -40f, ProjectileType<MeteorLaunch>(), damage, 3f, Main.myPlayer);

                                break;
                        }
                    }
                }
                if (timer >= walkTime * 2)
                {
                    timer = 0;
                    attack = Main.rand.Next(3);
                }
            }
            else
            {
                if (NPC.frameCounter >= 10)
                {
                    gunOffset.Y += 2;
                }
                NPC.aiStyle = 3;
                gunRot = NPC.spriteDirection == 1 ? 0f : (float)Math.PI;
                if (meteorsLaunched)
                {
                    meteorTime++;
                    if (meteorTime > 10)
                    {
                        if (Main.netMode != 1)
                        {
                            int Xvar = Main.rand.Next(-750, 750);
                            Projectile.NewProjectile(new EntitySource_Misc(""), player.Center.X + Xvar * 1.0f, player.Center.Y - 800f, 0f, 10f, ProjectileType<MeteorFall>(), damage, 3f, Main.myPlayer);
                        }
                        meteorTime = 0;
                    }
                }
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemType<DinoFlail>(), 4, 1, 1));
            npcLoot.Add(ItemDropRule.Common(ItemType<TheTyrantsExtinctionGun>(), 4, 1, 1));

            

			// ItemDropRule.MasterModeCommonDrop for the relic
			npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<Items.Consumable.Tiles.Relics.TyrantRelic>()));
        }
        public int moveFrame = 0;
        public int moveFrame2 = 1;

        public int launchFrame = 3;
        public int launchFrame2 = 4;

        public override void FindFrame(int frameHeight)
        {
            // This makes the sprite flip horizontally in conjunction with the NPC.direction.
            NPC.spriteDirection = NPC.direction;

            if (frameType == moveFrameType)
            {
                if (NPC.frameCounter < 10)
                {
                    NPC.frame.Y = (moveFrame * frameHeight);
                }
                else if (NPC.frameCounter < 20)
                {
                    NPC.frame.Y = (moveFrame2 * frameHeight);
                }
                else
                {
                    NPC.frameCounter = 0;
                }
            }
            if (frameType == attackFrameType)
            {
                NPC.frame.Y = (moveFrame * frameHeight);
            }
            if (frameType == launchFrameType)
            {
                if (NPC.frameCounter < 10)
                {
                    NPC.frame.Y = (moveFrame * frameHeight);
                }
                else if (NPC.frameCounter < 20)
                {
                    NPC.frame.Y = (launchFrame * frameHeight);
                }
                else if (NPC.frameCounter < 30)
                {
                    NPC.frame.Y = (launchFrame2 * frameHeight);
                }
                else
                {
                    NPC.frameCounter = 0;
                }
            }
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            int frameHeight = 28;
            bool flip = NPC.spriteDirection == 1;
            Texture2D gun = Request<Texture2D>("QwertyMod/Content/NPCs/DinoMilitia/TheTyrantsExtinctionGun").Value;
            spriteBatch.Draw(gun, NPC.position + gunOffset - screenPos,
                       new Rectangle(0, frameHeight * gunFrame, 80, frameHeight), drawColor, gunRot + (float)Math.PI,
                       new Vector2(70, 14), NPC.scale, flip ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
        }
    }

    public class SnowFlake : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("SnowFlake");
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 0;

            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 120;
            Projectile.tileCollide = true;
        }

        public override void AI()
        {
            Projectile.rotation += 1.5f;
        }

        public override bool OnTileCollide(Vector2 velocityChange)
        {
            if (Projectile.velocity.X != velocityChange.X)
            {
                Projectile.velocity.X = -velocityChange.X;
            }
            if (Projectile.velocity.Y != velocityChange.Y)
            {
                Projectile.velocity.Y = -velocityChange.Y;
            }
            return false;
        }
    }

    public class MeteorLaunch : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Meteor");
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 0;

            Projectile.width = 36;
            Projectile.height = 36;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 120;
            Projectile.tileCollide = true;
        }

        public bool runOnce = true;

        public override void AI()
        {
            if (runOnce)
            {
                SoundEngine.PlaySound(SoundID.Item62, Projectile.position);
                for (int i = 0; i < 50; i++)
                {
                    int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 31, 0f, 0f, 100, default(Color), 2f);
                    Main.dust[dustIndex].velocity *= 1.4f;
                }
                // Fire Dust spawn
                for (int i = 0; i < 80; i++)
                {
                    int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default(Color), 3f);
                    Main.dust[dustIndex].noGravity = true;
                    Main.dust[dustIndex].velocity *= 5f;
                    dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default(Color), 2f);
                    Main.dust[dustIndex].velocity *= 3f;
                }
                runOnce = false;
            }
            Projectile.rotation += 1.5f;
        }
    }

    public class MeteorFall : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Meteor");
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 0;

            Projectile.width = 36;
            Projectile.height = 36;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 1200;
            Projectile.tileCollide = true;
        }

        public override void AI()
        {
            Projectile.rotation += 1.5f;
        }

        public override bool OnTileCollide(Vector2 velocityChange)
        {
            if (Main.netMode != 1)
            {
                SoundEngine.PlaySound(SoundID.Item62, Projectile.position);
                for (int i = 0; i < 50; i++)
                {
                    int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 31, 0f, 0f, 100, default(Color), 2f);
                    Main.dust[dustIndex].velocity *= 1.4f;
                }
                // Fire Dust spawn
                for (int i = 0; i < 80; i++)
                {
                    int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default(Color), 3f);
                    Main.dust[dustIndex].noGravity = true;
                    Main.dust[dustIndex].velocity *= 5f;
                    dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default(Color), 2f);
                    Main.dust[dustIndex].velocity *= 3f;
                }
            }
            return true;
        }
    }
}