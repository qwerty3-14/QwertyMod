using Microsoft.Xna.Framework;
using QwertyMod.Content.Items.Consumable.Tiles.Banners;
using QwertyMod.Content.Items.Equipment.Accessories;
using QwertyMod.Content.Items.Weapon.Sentry.AntiAir;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.NPCs.DinoMilitia
{
    public class AntiAir : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Anti Air");
            Main.npcFrameCount[NPC.type] = 5;
        }

        public override void SetDefaults()
        {
            NPC.width = 94;
            NPC.height = 118;
            NPC.damage = 20;
            NPC.defense = 20;

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
            NPC.lifeMax = 1800;
            Banner = NPC.type;
            BannerItem = ItemType<AntiAirBanner>();
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Sets the description of this NPC that is listed in the bestiary

            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
            {
                new MoonLordPortraitBackgroundProviderBestiaryInfoElement(), //Plain black background
                new FlavorTextBestiaryInfoElement("The majority of the dino militia is grounded. So the velociraptors inveted the anti air missiles battery")
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (DinoEvent.EventActive)
            {
                return 10f;
            }
            else
            {
                return 0f;
            }
        }
        public override void OnKill()
        {
            if (DinoEvent.EventActive)
            {
                DinoEvent.DinoKillCount += 2;
                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
            }
        }
        public override void HitEffect(int hitDirection, double damage)
        {
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

        public int AI_Timer = 0;
        public int Pos = 1;
        public int damage = 40;
        public int walkTime = 300;
        public int moveCount = 0;
        public int fireCount = 0;
        public int frameType = 0;
        public int ReloadTime = 20;
        public int attackTime = 0;
        public bool secondShot = true;

        public override void AI()
        {
            Player player = Main.player[NPC.target];
            NPC.TargetClosest(true);

            if (frameType == attackFrameType)

            {
                attackTime++;
                NPC.velocity.X = (0);
                NPC.velocity.Y = (0);

                if (attackTime > ReloadTime)
                {
                    if (secondShot)
                    {
                        if (Main.netMode != 1)
                        {
                            Projectile.NewProjectile(new EntitySource_Misc(""), NPC.Center.X - (17f * NPC.direction), NPC.Center.Y - 40f, 0f, -10f, ProjectileType<AntiAirRocket>(), damage, 3f, Main.myPlayer);
                        }
                        //Projectile.NewProjectile(NPC.Center.X-(17f*NPC.direction), NPC.Center.Y-40f, 0f, 0f, 102, damage, 3f, Main.myPlayer);
                        secondShot = false;
                        attackTime = 0;
                    }
                    else
                    {
                        if (Main.netMode != 1)
                        {
                            Projectile.NewProjectile(new EntitySource_Misc(""), NPC.Center.X + (23f * NPC.direction), NPC.Center.Y - 40f, 0f, -10f, ProjectileType<AntiAirRocket>(), damage, 3f, Main.myPlayer);
                        }
                        //Projectile.NewProjectile(NPC.Center.X+(23f*NPC.direction), NPC.Center.Y-40f, 0f, 0f, 102, damage, 3f, Main.myPlayer);
                        secondShot = true;
                        attackTime = 0;
                    }
                }
            }

            float playerPositionSummery = player.Center.Y - NPC.Center.Y;
            Point origin = player.Center.ToTileCoordinates();
            Point point;
            if (playerPositionSummery < -200f && !WorldUtils.Find(origin, Searches.Chain(new Searches.Down(12), new GenCondition[]
                                        {
                                            new Terraria.WorldBuilding.Conditions.IsSolid()
                                        }), out point))
            {
                frameType = attackFrameType;
            }
            else
            {
                frameType = moveFrameType;
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemType<DinoTooth>(), 100, 1, 1));
            npcLoot.Add(ItemDropRule.Common(ItemType<AntiAirWrench>(), 6, 1, 1));
        }

        public int moveFrame = 0;
        public int moveFrame2 = 1;

        public int attackFrameLeft = 2;
        public int attackFrameRight = 3;
        public int attackFrameAlternation = 4;

        public override void FindFrame(int frameHeight)
        {
            // This makes the sprite flip horizontally in conjunction with the NPC.direction.
            NPC.spriteDirection = -NPC.direction;
            NPC.frameCounter++;
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
                if (NPC.frameCounter < 10)
                {
                    NPC.frame.Y = (attackFrameRight * frameHeight);
                }
                else if (NPC.frameCounter < 20)
                {
                    NPC.frame.Y = (attackFrameAlternation * frameHeight);
                }
                else if (NPC.frameCounter < 30)
                {
                    NPC.frame.Y = (attackFrameLeft * frameHeight);
                }
                else if (NPC.frameCounter < 40)
                {
                    NPC.frame.Y = (attackFrameAlternation * frameHeight);
                }
                else
                {
                    NPC.frameCounter = 0;
                }
            }
        }
    }

    public class AntiAirRocket : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Anti Air Rocket");
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 240;
            Projectile.tileCollide = true;
        }

        public bool runOnce = true;
        public int dustTimer;
        private float direction;
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
                if (Main.netMode != 1)
                {
                    for (int i = 0; i < 255; i++)
                    {
                        if (Main.player[i].active && (Projectile.Center - Main.player[i].Center).Length() < closest)
                        {
                            closest = (Projectile.Center - Main.player[i].Center).Length();
                            Projectile.ai[0] = (Main.player[i].Center - Projectile.Center).ToRotation();
                            Projectile.netUpdate = true;
                        }
                    }
                }
                Projectile.velocity += new Vector2((float)Math.Cos(Projectile.ai[0]) * missileAcceleration, (float)Math.Sin(Projectile.ai[0]) * missileAcceleration);
                if (Projectile.velocity.Length() > topSpeed)
                {
                    Projectile.velocity = Projectile.velocity.SafeNormalize(-Vector2.UnitY) * 10;
                }
            }
            //int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, mod.DustType("AncientGlow"), 0, 0, 0, default(Color), .4f);
            Dust dust = Dust.NewDustPerfect(Projectile.Center + QwertyMethods.PolarVector(26, Projectile.rotation + (float)Math.PI / 2) + QwertyMethods.PolarVector(Main.rand.Next(-6, 6), Projectile.rotation), 6);
            closest = 10000;
        }
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item62, Projectile.position);
            for (int i = 0; i < 14; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 31, 0f, 0f, 100, default(Color), 2f);
                Main.dust[dustIndex].velocity *= .6f;
            }
            // Fire Dust spawn
            for (int i = 0; i < 20; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default(Color), 3f);
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].velocity *= 2f;
                dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default(Color), 2f);
                Main.dust[dustIndex].velocity *= 1f;
            }
        }
    }
}