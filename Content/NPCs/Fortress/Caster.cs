using Microsoft.Xna.Framework;
using QwertyMod.Common.Fortress;
using QwertyMod.Content.Buffs;
using QwertyMod.Content.Dusts;
using QwertyMod.Content.Items.Consumable.BossSummon;
using QwertyMod.Content.Items.Consumable.Tiles.Banners;
using QwertyMod.Content.NPCs.Bosses.FortressBoss;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

using System.IO;

namespace QwertyMod.Content.NPCs.Fortress
{
    public class Caster : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 9;
            NPCID.Sets.NoMultiplayerSmoothingByType[NPC.type] = true;
        }

        public override void SetDefaults()
        {
            NPC.width = 58;
            NPC.height = 64;
            NPC.aiStyle = -1;
            NPC.damage = 20;
            NPC.defense = 6;
            NPC.lifeMax = 100;

            if (SkyFortress.beingInvaded)
            {
                NPC.lifeMax = 1000;
            }
            NPC.value = 500;
            //NPC.alpha = 100;
            NPC.behindTiles = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.buffImmune[BuffID.Confused] = false;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<CasterBanner>();
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Sets the description of this NPC that is listed in the bestiary
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
                new FlavorTextBestiaryInfoElement("The High preists are disciples of Caelin, God of the sky. They manage the sky fortess.")
            });
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FortressBossSummon>(), 1));

        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.InModBiome(ModContent.GetInstance<FortressBiome>()) && !NPC.AnyNPCs(ModContent.NPCType<Caster>()) && !NPC.AnyNPCs(ModContent.NPCType<FortressBoss>()))
            {
                return 10f;
            }
            return 0f;
        }

        private int timer;
        private int GenerateRingTime = 30;
        private int throwRingTime = 150;
        private Projectile ring;
        private float ringSpeed = 6;
        private int ringProjectileCount;
        private bool castingFrames;
        bool aggressive = false;

        public override void AI()
        {
            NPC.damage = 0;
            NPC.chaseable = NPC.life < NPC.lifeMax || SkyFortress.beingInvaded;
            NPC.GetGlobalNPC<FortressNPCGeneral>().fortressNPC = true;
            if (NPC.life < NPC.lifeMax || SkyFortress.beingInvaded || aggressive)
            {
                timer++;
                
                if(!aggressive && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    aggressive = true;
                    NPC.netUpdate = true;
                }
            }

            NPC.spriteDirection = NPC.direction;
            Entity player = FortressNPCGeneral.FindTarget(NPC, true);
            //QwertyMethods.ServerClientCheck("" + SkyFortress.beingInvaded);
            if(player == null && ring == null && Main.netMode != NetmodeID.MultiplayerClient)
            {
                timer = 0;
                NPC.ai[0] += 1f;
                NPC.netUpdate = true;
            }
            //QwertyMethods.ServerClientCheck("" + timer);
            //QwertyMethods.ServerClientCheck("" + (player != null ? "I see" : "no"));
            ringProjectileCount = 2 - (int)((float)NPC.life / (float)NPC.lifeMax * 2) + 4;
            if (timer == GenerateRingTime && Main.netMode != NetmodeID.MultiplayerClient)
            {
                ring = Main.projectile[Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<RingCenter>(), SkyFortress.beingInvaded ? 40 : 11, 0, Main.myPlayer, ringProjectileCount, NPC.direction)];
                ring.ai[0] = ringProjectileCount;
                ring.ai[1] = NPC.direction;
                castingFrames = true;
            }
            if (timer > GenerateRingTime && timer < GenerateRingTime + throwRingTime && Main.netMode != NetmodeID.MultiplayerClient)
            {
                ring.Center = NPC.Center;
            }
            if (timer == GenerateRingTime + throwRingTime && Main.netMode != NetmodeID.MultiplayerClient)
            {
                if(player != null)
                {
                    ring.velocity = ((player.Center - NPC.Center).SafeNormalize(-Vector2.UnitY) * ringSpeed) * (NPC.confused ? -1 : 1);
                    if(Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        ring.netUpdate = true;
                    }
                    ring = null;
                }
                else
                {
                    ring.Kill();
                    ring = null;
                }
                castingFrames = false;
                timer = 0;
            }
            NPC.velocity.X = NPC.velocity.X * 0.93f;
            if ((double)NPC.velocity.X > -0.1 && (double)NPC.velocity.X < 0.1)
            {
                NPC.velocity.X = 0f;
            }
            /*
            if (NPC.ai[0] == 0f)
            {
                NPC.ai[0] = 500f;
            }
            */
            if (NPC.ai[2] != 0f && NPC.ai[3] != 0f)
            {
                SoundEngine.PlaySound(SoundID.Item8, NPC.position);
                for (int num67 = 0; num67 < 50; num67++)
                {
                    int num75 = Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<CaeliteDust>(), 0f, 0f, 100, default(Color), 2.5f);
                    Main.dust[num75].velocity *= 3f;
                    Main.dust[num75].noGravity = true;
                }
                NPC.position.X = NPC.ai[2] * 16f - (float)(NPC.width / 2) + 8f;
                NPC.position.Y = NPC.ai[3] * 16f - (float)NPC.height;
                NPC.velocity.X = 0f;
                NPC.velocity.Y = 0f;
                NPC.ai[2] = 0f;
                NPC.ai[3] = 0f;
                SoundEngine.PlaySound(SoundID.Item8, NPC.position);
                for (int num76 = 0; num76 < 50; num76++)
                {
                    int num84 = Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<CaeliteDust>(), 0f, 0f, 100, default(Color), 2.5f);
                    Main.dust[num84].velocity *= 3f;
                    Main.dust[num84].noGravity = true;
                }
            }

            if (player != null && Math.Abs(NPC.position.X - player.position.X) + Math.Abs(NPC.position.Y - player.position.Y) > 2000f)
            {
                NPC.ai[0] = 650f;
            }
            if (player != null && NPC.ai[0] >= 650f && Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.ai[0] = 1f;
                int playerTilePositionX = (int)player.position.X / 16;
                int playerTilePositionY = (int)player.position.Y / 16;
                int npcTilePositionX = (int)NPC.position.X / 16;
                int npcTilePositionY = (int)NPC.position.Y / 16;
                int playerTargetShift = 40;
                int num90 = 0;

                for (int s = 0; s < 100; s++)
                {
                    num90++;
                    int nearPlayerX = Main.rand.Next(playerTilePositionX - playerTargetShift, playerTilePositionX + playerTargetShift);
                    int nearPlayerY = Main.rand.Next(playerTilePositionY - playerTargetShift, playerTilePositionY + playerTargetShift);
                    for (int num93 = nearPlayerY; num93 < playerTilePositionY + playerTargetShift; num93++)
                    {
                        if ((nearPlayerX < playerTilePositionX - 12 || nearPlayerX > playerTilePositionX + 12) && (num93 < npcTilePositionY - 1 || num93 > npcTilePositionY + 1 || nearPlayerX < npcTilePositionX - 1 || nearPlayerX > npcTilePositionX + 1))
                        {
                            bool flag5 = true;
                            if (Main.tile[nearPlayerX, num93 - 1].LiquidType == LiquidID.Lava)
                            {
                                flag5 = false;
                            }
                            if (flag5 && Main.tileSolid[(int)Main.tile[nearPlayerX, num93].TileType] && !Collision.SolidTiles(nearPlayerX - 1, nearPlayerX + 1, num93 - 4, num93 - 1))
                            {
                                NPC.ai[1] = 20f;
                                NPC.ai[2] = (float)nearPlayerX;
                                NPC.ai[3] = (float)num93 - 1;

                                break;
                            }
                        }
                    }
                }
                NPC.netUpdate = true;
            }
            NPC.netOffset *= 0;
            if (NPC.ai[1] > 0f)
            {
                NPC.ai[1] -= 1f;
            }
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            //timer = 0;
            NPC.ai[0] = 650f;
            NPC.TargetClosest(true);
        }

        private int frameCounter;
        private int frame;

        public override void FindFrame(int frameHeight)
        {
            frameCounter++;
            frame = 0;
            if (frameCounter > 50)
            {
                frameCounter = 0;
            }
            if (frameCounter > 40)
            {
                frame = 3;
            }
            else if (frameCounter > 30)
            {
                frame = 2;
            }
            else if (frameCounter > 20)
            {
                frame = 1;
            }
            if (castingFrames)
            {
                frame += 5;
            }
            NPC.frame.Y = frameHeight * frame;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(castingFrames);
            writer.Write(aggressive);
            writer.Write(timer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            castingFrames = reader.ReadBoolean();
            aggressive = reader.ReadBoolean();
            timer = reader.ReadInt32();
        }
    }

    public class RingCenter : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 8 * 60;
            Projectile.npcProj = true;
        }

        private bool runOnce = true;
        private int projectilesInRing = 6;

        public override void AI()
        {
            //QwertyMethods.ServerClientCheck("" + Projectile.Center);
            if(Projectile.position != Projectile.oldPosition && Main.netMode != NetmodeID.MultiplayerClient)
            {
                Projectile.netUpdate = true;
            }
            projectilesInRing = (int)Projectile.ai[0];
            if (runOnce)
            {
                for (int i = 0; i < projectilesInRing; i++)
                {
                    //Projectile.NewProjectile(Projectile.Center, Vector2.Zero, mod.ProjectileType("RingOuter"), Projectile.damage, Projectile.knockBack, Projectile.owner, (float)i / (float)projectilesInRing * 2 * MathF.PI, Projectile.whoAmI);

                    if (Projectile.ai[1] == 1)
                    {
                        Projectile p = Main.projectile[Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<RingOuter>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, (float)i / (float)projectilesInRing * 2 * MathF.PI, Projectile.whoAmI)];
                        p.ai[0] = (float)i / (float)projectilesInRing * 2 * MathF.PI;
                        p.ai[1] = Projectile.whoAmI;
                    }
                    else
                    {
                        Projectile p = Main.projectile[Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<RingOuter>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, (float)i / (float)projectilesInRing * 2 * MathF.PI, -Projectile.whoAmI)];
                        p.ai[0] = (float)i / (float)projectilesInRing * 2 * MathF.PI;
                        p.ai[1] = -Projectile.whoAmI;
                    }
                }
                runOnce = false;
            }
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            //writer.WritePackedVector2(Projectile.velocity);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            //Projectile.velocity = reader.ReadPackedVector2();
        }
    }

    public class RingOuter : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.GetGlobalProjectile<FortressNPCProjectile>().isFromFortressNPC = true;
            Projectile.GetGlobalProjectile<FortressNPCProjectile>().EvEMultiplier = 4f;
            Projectile.friendly = true;
            Projectile.npcProj = true;
            Projectile.penetrate = -1;
        }

        private bool runOnce = true;
        private Projectile parent;
        private float radius = 60;
        private Projectile clearCheck;
        private int spinDirection = 1;
        private int frameTimer;

        public override void AI()
        {
            Dust dust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<CaeliteDust>())];
            dust.scale = .5f;
            if (runOnce)
            {
                if (Projectile.ai[1] < 0)
                {
                    spinDirection = -1;
                    Projectile.ai[1] = Math.Abs(Projectile.ai[1]);
                }
                runOnce = false;
            }
            Projectile.ai[1] = Math.Abs(Projectile.ai[1]);
            parent = Main.projectile[(int)Projectile.ai[1]];
            Projectile.position.X = parent.Center.X - (int)(Math.Cos(Projectile.ai[0]) * radius) - Projectile.width / 2;
            Projectile.position.Y = parent.Center.Y - (int)(Math.Sin(Projectile.ai[0]) * radius) - Projectile.height / 2;
            Projectile.ai[0] += MathF.PI / 120 * spinDirection;
            for (int p = 0; p < 1000; p++)
            {
                clearCheck = Main.projectile[p];
                if (clearCheck.friendly && !clearCheck.GetGlobalProjectile<FortressNPCProjectile>().isFromFortressNPC && !clearCheck.sentry && clearCheck.minionSlots <= 0 && Collision.CheckAABBvAABBCollision(Projectile.position, Projectile.Size, clearCheck.position, clearCheck.Size))
                {
                    clearCheck.Kill();
                }
            }
            if (!parent.active || parent.type != ModContent.ProjectileType<RingCenter>())
            {
                Projectile.Kill();
            }
            frameTimer++;
            if (frameTimer > 10)
            {
                Projectile.frame++;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
                frameTimer = 0;
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 30; i++)
            {
                Dust dust = Main.dust[Dust.NewDust(Projectile.Center, 0, 0, ModContent.DustType<CaeliteDust>())];
                dust.velocity *= 3;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (Main.rand.NextBool(3))
            {
                target.AddBuff(ModContent.BuffType<PowerDown>(), 600);
            }
        }
    }
}