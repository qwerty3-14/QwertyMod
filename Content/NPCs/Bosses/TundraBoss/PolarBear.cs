using Microsoft.Xna.Framework;
using QwertyMod.Common;
using QwertyMod.Content.Items.Consumable.BossBag;
using QwertyMod.Content.Items.Consumable.Tiles.Trophy.Polar;
using QwertyMod.Content.Items.Equipment.Vanity.BossMasks;
using QwertyMod.Content.Items.Weapon.Magic.PenguinWhistle;
using QwertyMod.Content.Items.Weapon.Melee.Sword;
using QwertyMod.Content.Items.Weapon.Ranged.SpecialAmmo;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.NPCs.Bosses.TundraBoss
{
    public class PolarBear : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Polar Exterminator");
            Main.npcFrameCount[NPC.type] = 5;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
        }

        public override void SetDefaults()
        {
            NPC.width = 68;
            NPC.height = 82;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = -1;
            NPC.lifeMax = 1600;
            NPC.defense = 8;
            NPC.damage = 40;
            NPC.boss = true;
            NPC.noGravity = false;
            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/PolarOpposition");
            }
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Sets the description of this NPC that is listed in the bestiary
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundSnow,
                new FlavorTextBestiaryInfoElement("Wait? Polar bears and penguins interacting? That's not possible! Polar bears are from the south pole and penguins from the north! Immersion ruined!")
            }); ;
        }
        private const int IdleFrame = 4;
        private const int JumpFrame = 1;
        private const int AboutToJumpFrame = 0;
        private const int ShootSliderFrame = 3;
        private const int ShootFlierFrame = 2;

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.damage = 30;
            NPC.lifeMax = (int)(NPC.lifeMax * 0.7 * bossLifeScale);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0f;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            //Add the treasure bag (automatically checks for expert mode)
            npcLoot.Add(ItemDropRule.BossBag(ItemType<TundraBossBag>())); //this requires you to set BossBag in SetDefaults accordingly

            //All our drops here are based on "not expert", meaning we use .OnSuccess() to add them into the rule, which then gets added
            LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());

            //Notice we use notExpertRule.OnSuccess instead of npcLoot.Add so it only applies in normal mode
            notExpertRule.OnSuccess(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, ItemType<PenguinClub>(), ItemType<PenguinLauncher>(), ItemType<PenguinWhistle>()));
            //Finally add the leading rule
            npcLoot.Add(notExpertRule);

            //Boss masks are spawned with 1/7 chance
            notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
            notExpertRule.OnSuccess(ItemDropRule.Common(ItemType<PolarMask>(), 7));
            npcLoot.Add(notExpertRule);

            notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
            notExpertRule.OnSuccess(ItemDropRule.Common(ItemID.Penguin, 1, 35, 60));
            npcLoot.Add(notExpertRule);

            //Trophies are spawned with 1/10 chance
            npcLoot.Add(ItemDropRule.Common(ItemType<PolarTrophy>(), 10));


            base.ModifyNPCLoot(npcLoot);
        }
        public override void OnKill()
        {
            //This sets downedMinionBoss to true, and if it was false before, it initiates a lantern night
            NPC.SetEventFlagCleared(ref DownedBossSystem.downedBear, -1);
        }

        private float timer;
        private int attackDelay = 60;
        private int resetAttacks = 360;
        private int attackCounter;
        private bool landed;
        private int frame = 4;
        int attackCycle = 0;
        int agentCooldown = 0;
        public override void AI()
        {
            NPC.TargetClosest(true);
            Player player = Main.player[NPC.target];

            if (!player.active || player.dead)
            {
                NPC.TargetClosest(false);
                player = Main.player[NPC.target];

                if (!player.active || player.dead)
                {
                    NPC.noTileCollide = true;
                    frame = JumpFrame;
                    NPC.velocity = new Vector2(0f, 10f);

                    if (NPC.timeLeft > 10)
                    {
                        NPC.timeLeft = 10;
                    }
                    return;
                }
            }
            else
            {
                if (Main.expertMode && landed)
                {
                    timer += 1 + 2 * (1f - (float)NPC.life / NPC.lifeMax);
                }
                else
                {
                    timer++;
                }

                if (timer > resetAttacks)
                {
                    NPC.velocity.X = 10 * NPC.direction;
                    NPC.velocity.Y = -10;
                    landed = false;
                    frame = JumpFrame;
                    if (Main.netMode != 1)
                    {
                        timer = 0;
                        NPC.netUpdate = true;
                    }
                    attackCounter = 0;
                    attackCycle++;
                    if (attackCycle == 7)
                    {
                        attackCycle = 0;
                    }
                    if (Main.netMode != 1)
                    {
                        NPC.ai[0] = (attackCycle == 0 || attackCycle == 1 || attackCycle == 3 || attackCycle == 4) ? 0 : 1;
                        NPC.netUpdate = true;
                    }
                }
                else if (timer > resetAttacks - 60)
                {
                    frame = AboutToJumpFrame;
                }
                else if (timer > attackDelay)
                {
                    if (NPC.ai[0] == 0)
                    {
                        frame = ShootSliderFrame;
                        if (timer > (attackCounter + 1) * 90 + attackDelay && attackCounter < 2)
                        {
                            attackCounter++;
                            if (Main.netMode != 1)
                            {
                                NPC.NewNPC(new EntitySource_Misc(""), (int)NPC.Center.X + 30 * NPC.direction, (int)NPC.Center.Y + 14, NPCType<SlidingPenguin>(), ai0: NPC.direction, ai1: (player.Bottom.Y < NPC.Center.Y + 14) ? 1 : 0);
                            }
                            SoundEngine.PlaySound(SoundID.Item11, NPC.position);
                            for (int i = 0; i < 8; i++)
                            {
                                Dust.NewDustPerfect(new Vector2((int)NPC.Center.X + 30 * NPC.direction, (int)NPC.Center.Y + 14), DustID.Ice, new Vector2(NPC.direction * (2 + Main.rand.NextFloat() * 2f), 0).RotatedByRandom(Math.PI / 8));
                            }
                        }
                    }
                    else if (NPC.ai[0] == 1)
                    {
                        frame = ShootFlierFrame;
                        if (timer > attackDelay + 90 && attackCounter == 0)
                        {
                            attackCounter++;
                            for (int i = -2; i < 3; i++)
                            {
                                if (Main.netMode != 1)
                                {
                                    NPC.NewNPC(new EntitySource_Misc(""), (int)NPC.Center.X + 34 * NPC.direction, (int)NPC.Center.Y, NPCType<FlyingPenguin>(), 0, i);
                                }
                                SoundEngine.PlaySound(SoundID.Item11, NPC.position);
                            }
                            for (int i = 0; i < 8; i++)
                            {
                                Dust.NewDustPerfect(new Vector2((int)NPC.Center.X + 34 * NPC.direction, (int)NPC.Center.Y), DustID.Ice, new Vector2(NPC.direction * (2 + Main.rand.NextFloat() * 2f), 0).RotatedByRandom(Math.PI / 8));
                            }
                        }
                    }
                }
                if (NPC.collideY && NPC.velocity.Y > 0)
                {
                    if (!landed)
                    {
                        frame = IdleFrame;
                        for (int i = 0; i < 60; i++)
                        {
                            Dust.NewDust(NPC.BottomLeft, Main.rand.Next(NPC.width), 1, DustID.Ice);
                        }
                        landed = true;
                    }
                    NPC.velocity.X = 0;
                }
                /*
                if (Main.netMode == 1)
                {
                    Main.NewText("client: " + timer);
                }

                if (Main.netMode == 2) // Server
                {
                    NetMessage.BroadcastChatMessage(Terraria.Localization.NetworkText.FromLiteral("Server: " + timer), Color.Black);
                }
                */
                //Main.NewText(NPC.collideY);
                NPC.noTileCollide = false;
                if (NPC.velocity.Y < 0 || player.Bottom.Y > NPC.Bottom.Y + 32)
                {
                    NPC.noTileCollide = true;
                    NPC.collideY = false;
                }
                else
                {
                    NPC.noTileCollide = true;
                    Point bottomLeft = NPC.BottomLeft.ToTileCoordinates();
                    for (int i = 0; i < (NPC.width / 16) + 1; i++)
                    {
                        Vector2 p = bottomLeft.ToVector2() + new Vector2(i, 0);
                        if (Main.tileSolid[Main.tile[(int)p.X, (int)p.Y].TileType] && !Main.tileSolidTop[Main.tile[(int)p.X, (int)p.Y].TileType])
                        {
                            NPC.noTileCollide = false;
                        }
                    }
                }
                if (Main.netMode != 1)
                {
                    if (Main.expertMode && (float)NPC.life / (float)NPC.lifeMax < .5f && agentCooldown <= 0)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            float x = Main.rand.NextFloat(7, 24) * (i == 0 ? 1 : -1);
                            int denLength = 101;
                            int denUpperHeight = 40;
                            int ceilingHeight = (int)((float)Math.Sin(((float)(x + (denLength / 2)) / (float)denLength) * (float)Math.PI) * (float)denUpperHeight);
                            Vector2 spawnPos = FrozenDen.BearSpawn + new Vector2(x * 16, ceilingHeight * -16);
                            NPC.NewNPC(new EntitySource_Misc(""), (int)spawnPos.X, (int)spawnPos.Y, NPCType<AgentPenguin>());

                        }
                        agentCooldown = 600;
                    }
                    if (agentCooldown > 0)
                    {
                        agentCooldown--;
                    }
                }

            }
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Y = frame * frameHeight;
            NPC.spriteDirection = NPC.direction;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(timer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            timer = reader.ReadSingle();
        }
        /*
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Point bottomLeft = NPC.BottomLeft.ToTileCoordinates();
            Texture2D texture = Main.extraTexture[2];
            for (int i = 0; i < (NPC.width / 16)+1; i++)
            {
                spriteBatch.Draw(texture, (bottomLeft.ToVector2() * 16) + Vector2.UnitX * i * 16 - Main.screenPosition, new Rectangle(0, 0, 16, 16), Color.White, 0, Vector2.Zero, Vector2.One, 0, 0);
            }
            return base.PreDraw(spriteBatch, drawColor);
        }
        */
    }
}