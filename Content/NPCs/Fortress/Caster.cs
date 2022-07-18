using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.NPCs.Fortress
{
    public class Caster : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("High Preist");
            Main.npcFrameCount[NPC.type] = 9;
        }

        public override void SetDefaults()
        {
            NPC.width = 58;
            NPC.height = 64;
            NPC.aiStyle = -1;
            NPC.damage = 20;
            NPC.defense = 6;
            NPC.lifeMax = 100;
            NPC.value = 500;
            //NPC.alpha = 100;
            NPC.behindTiles = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.buffImmune[BuffID.Confused] = false;
            Banner = NPC.type;
            BannerItem = ItemType<CasterBanner>();
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
            npcLoot.Add(ItemDropRule.Common(ItemType<FortressBossSummon>(), 1));

        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.InModBiome(GetInstance<FortressBiome>()) && !NPC.AnyNPCs(NPCType<Caster>()) && !NPC.AnyNPCs(NPCType<FortressBoss>()))
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

        public override void AI()
        {
            NPC.damage = 0;
            NPC.chaseable = NPC.life < NPC.lifeMax;
            NPC.GetGlobalNPC<FortressNPCGeneral>().fortressNPC = true;
            if (NPC.life < NPC.lifeMax)
            {
                timer++;
            }
            
            NPC.spriteDirection = NPC.direction;
            Entity player = FortressNPCGeneral.FindTarget(NPC, true);
            ringProjectileCount = 2 - (int)((float)NPC.life / (float)NPC.lifeMax * 2) + 4;
            if (timer == GenerateRingTime)
            {
                ring = Main.projectile[Projectile.NewProjectile(new EntitySource_Misc(""),  NPC.Center, Vector2.Zero, ProjectileType<RingCenter>(), 11, 0, 0, ringProjectileCount, NPC.direction)];
                ring.ai[0] = ringProjectileCount;
                ring.ai[1] = NPC.direction;
                castingFrames = true;
            }
            if (timer > GenerateRingTime && timer < GenerateRingTime + throwRingTime)
            {
                ring.Center = NPC.Center;
            }
            if (timer == GenerateRingTime + throwRingTime)
            {
                castingFrames = false;
                ring.velocity = ((player.Center - NPC.Center).SafeNormalize(-Vector2.UnitY) * ringSpeed) * (NPC.confused ? -1 : 1);
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
                    int num75 = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustType<CaeliteDust>(), 0f, 0f, 100, default(Color), 2.5f);
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
                    int num84 = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustType<CaeliteDust>(), 0f, 0f, 100, default(Color), 2.5f);
                    Main.dust[num84].velocity *= 3f;
                    Main.dust[num84].noGravity = true;
                }
            }
            //NPC.ai[0] += 1f;

            if (Math.Abs(NPC.position.X - Main.player[NPC.target].position.X) + Math.Abs(NPC.position.Y - Main.player[NPC.target].position.Y) > 2000f)
            {
                NPC.ai[0] = 650f;
            }
            if (NPC.ai[0] >= 650f && Main.netMode != 1)
            {
                NPC.ai[0] = 1f;
                int playerTilePositionX = (int)Main.player[NPC.target].position.X / 16;
                int playerTilePositionY = (int)Main.player[NPC.target].position.Y / 16;
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
                        if ((nearPlayerX < playerTilePositionX - 12 || nearPlayerX > playerTilePositionX + 12) && (num93 < npcTilePositionY - 1 || num93 > npcTilePositionY + 1 || nearPlayerX < npcTilePositionX - 1 || nearPlayerX > npcTilePositionX + 1) )
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
            if (NPC.ai[1] > 0f)
            {
                NPC.ai[1] -= 1f;
            }
        }

        public override void HitEffect(int hitDirection, double damage)
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
        }

        private bool runOnce = true;
        private int projectilesInRing = 6;

        public override void AI()
        {
            projectilesInRing = (int)Projectile.ai[0];
            if (runOnce)
            {
                for (int i = 0; i < projectilesInRing; i++)
                {
                    //Projectile.NewProjectile(Projectile.Center, Vector2.Zero, mod.ProjectileType("RingOuter"), Projectile.damage, Projectile.knockBack, Projectile.owner, (float)i / (float)projectilesInRing * 2 * (float)Math.PI, Projectile.whoAmI);

                    if (Projectile.ai[1] == 1)
                    {
                        Projectile p = Main.projectile[Projectile.NewProjectile(new EntitySource_Misc(""), Projectile.Center, Vector2.Zero, ProjectileType<RingOuter>(), Projectile.damage, Projectile.knockBack, 0, (float)i / (float)projectilesInRing * 2 * (float)Math.PI, Projectile.whoAmI)];
                        p.ai[0] = (float)i / (float)projectilesInRing * 2 * (float)Math.PI;
                        p.ai[1] = Projectile.whoAmI;
                    }
                    else
                    {
                        Projectile p = Main.projectile[Projectile.NewProjectile(new EntitySource_Misc(""), Projectile.Center, Vector2.Zero, ProjectileType<RingOuter>(), Projectile.damage, Projectile.knockBack, 0, (float)i / (float)projectilesInRing * 2 * (float)Math.PI, -Projectile.whoAmI)];
                        p.ai[0] = (float)i / (float)projectilesInRing * 2 * (float)Math.PI;
                        p.ai[1] = -Projectile.whoAmI;
                    }
                }
                runOnce = false;
            }
        }
    }

    public class RingOuter : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Caelite Sphere");
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
            Projectile.friendly = true;
        }

        private bool runOnce = true;
        private int projectilesInRing = 4;
        private Projectile parent;
        private float radius = 60;
        private Projectile clearCheck;
        private int spinDirection = 1;
        private int frameTimer;

        public override void AI()
        {
            Dust dust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<CaeliteDust>())];
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
            Projectile.ai[0] += (float)Math.PI / 120 * spinDirection;
            for (int p = 0; p < 1000; p++)
            {
                clearCheck = Main.projectile[p];
                if (clearCheck.friendly && !clearCheck.GetGlobalProjectile<FortressNPCProjectile>().isFromFortressNPC && !clearCheck.sentry && clearCheck.minionSlots <= 0 && Collision.CheckAABBvAABBCollision(Projectile.position, Projectile.Size, clearCheck.position, clearCheck.Size))
                {
                    clearCheck.Kill();
                }
            }
            if (!parent.active || parent.type != ProjectileType<RingCenter>())
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

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 30; i++)
            {
                Dust dust = Main.dust[Dust.NewDust(Projectile.Center, 0, 0, DustType<CaeliteDust>())];
                dust.velocity *= 3;
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (Main.rand.Next(3) == 0)
            {
                target.AddBuff(BuffType<PowerDown>(), 600);
            }
        }
    }
}