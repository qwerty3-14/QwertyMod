using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common.Fortress;
using QwertyMod.Content.Dusts;
using QwertyMod.Content.Items.Consumable.Tiles.Banners;
using QwertyMod.Content.Items.Consumable.Tiles.Fortress.BuildingBlocks;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.NPCs.Fortress
{
    public class Hopper : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Enchanted Tile");
            Main.npcFrameCount[NPC.type] = 4;
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                CustomTexturePath = "QwertyMod/Content/NPCs/Fortress/Hopper_Bestiary",
                PortraitScale = 1f,
                PortraitPositionYOverride = 0f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }


        public override void SetDefaults()
        {
            NPC.width = 32;
            NPC.height = 42;
            NPC.aiStyle = -1;
            NPC.damage = 28;
            NPC.defense = 18;
            NPC.lifeMax = 160;
            NPC.value = 100;
            //NPC.alpha = 100;
            //NPC.behindTiles = true;
            NPC.HitSound = SoundID.NPCHit7;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            //NPC.dontTakeDamage = true;
            //NPC.scale = 1.2f;
            NPC.buffImmune[20] = true;
            NPC.buffImmune[24] = true;
            Banner = NPC.type;
            BannerItem = ItemType<HopperBanner>();

            NPC.buffImmune[BuffID.Confused] = false;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Sets the description of this NPC that is listed in the bestiary
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
                new FlavorTextBestiaryInfoElement("The high preists' were unable to defend the fortress on their own, so Caelin gave them the enchanted tile.")
            });
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 40; i++)
                {
                    int dustType = DustType<FortressDust>(); 
                    int dustIndex = Dust.NewDust(NPC.position, NPC.width, NPC.height, dustType);
                    Dust dust = Main.dust[dustIndex];
                    dust.velocity.X = dust.velocity.X + Main.rand.Next(-50, 51) * 0.01f;
                    dust.velocity.Y = dust.velocity.Y + Main.rand.Next(-50, 51) * 0.01f;
                    dust.scale *= 1f + Main.rand.Next(-30, 31) * 0.01f;
                }
            }
            for (int i = 0; i < 4; i++)
            {
                int dustType = DustType<FortressDust>(); 
                int dustIndex = Dust.NewDust(NPC.position, NPC.width, NPC.height, dustType);
                Dust dust = Main.dust[dustIndex];
                dust.velocity.X = dust.velocity.X + Main.rand.Next(-50, 51) * 0.01f;
                dust.velocity.Y = dust.velocity.Y + Main.rand.Next(-50, 51) * 0.01f;
                dust.scale *= 1f + Main.rand.Next(-30, 31) * 0.01f;
            }
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return preSetTimer <= 0;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            return preSetTimer <= 0;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemType<FortressBrick>(), 1, 2, 4));
        }


        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.player.InModBiome(GetInstance<FortressBiome>()))
            {
                return 140f;
            }
            return 0f;
        }

        private int frame;
        private int timer;
        private float jumpSpeedY = -10.5f;
        private float jumpSpeedX = 4;
        private float aggroDistance = 400;
        private float aggroDistanceY = 200;
        private bool jump;
        private float gravity = .3f;
        private bool runOnce = true;
        private bool flipped = false;
        private int preSetTimer = 120;
        private bool spawnChildren = false;

        public override void AI()
        {
            NPC.GetGlobalNPC<FortressNPCGeneral>().fortressNPC = true;
            if (runOnce)
            {
                switch (Main.rand.Next(3))
                {
                    case 0:
                        spawnChildren = true;

                        break;

                    case 1:
                        Point origin = NPC.Center.ToTileCoordinates();
                        Point point;
                        for (int s = 0; s < 200; s++)
                        {
                            if (NPC.Top.ToTileCoordinates().X - 10 < 0)
                            {
                                break;
                            }
                            if (!WorldUtils.Find(origin, Searches.Chain(new Searches.Up(2), new GenCondition[]
                            {
                                            new Terraria.WorldBuilding.Conditions.IsSolid()
                            }), out point))
                            {
                                NPC.position.Y--;
                                origin = NPC.Center.ToTileCoordinates();
                            }
                            else
                            {
                                flipped = true;
                                break;
                            }
                        }
                        break;
                }
                if (!flipped)
                {
                    Point origin = NPC.Center.ToTileCoordinates();
                    Point point;

                    while (!WorldUtils.Find(origin, Searches.Chain(new Searches.Down(4), new GenCondition[]
                    {
                                            new Terraria.WorldBuilding.Conditions.IsSolid()
                    }), out point))
                    {
                        NPC.position.Y++;
                        origin = NPC.Center.ToTileCoordinates();
                    }
                }
                runOnce = false;
            }
            if (preSetTimer > 0)
            {
                preSetTimer--;
                NPC.dontTakeDamage = true;
                NPC.velocity = Vector2.Zero;
                float d = Main.rand.NextFloat() * (float)Math.PI * 2;
                Dust dusty = Dust.NewDustPerfect(NPC.position + new Vector2(Main.rand.Next(NPC.width), Main.rand.Next(NPC.height)) + QwertyMethods.PolarVector(30f, d + (float)Math.PI), DustType<FortressDust>(), QwertyMethods.PolarVector(3f, d), Scale: .5f);
                dusty.noGravity = true;
                if (preSetTimer == 0 && spawnChildren)
                {
                    int children = Main.rand.Next(3);
                    for (int i = 0; i < children; i++)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNPCFromNPCAI(), (int)NPC.Center.X + Main.rand.Next(-40, 41), (int)NPC.Center.Y, NPCType<YoungTile>());
                    }
                }
            }
            else
            {
                if (frame == 0)
                {
                    NPC.dontTakeDamage = true;
                }
                else
                {
                    NPC.dontTakeDamage = false;
                }
                if (flipped)
                {
                    gravity = 0f;
                    NPC.rotation = (float)Math.PI;
                    Player player = Main.player[NPC.target];
                    NPC.TargetClosest(true);
                    if (Collision.CheckAABBvLineCollision(player.position, player.Size, NPC.Center, NPC.Center + new Vector2(0, 1000)) && Collision.CanHit(NPC.Center, 0, 0, player.Center, 0, 0))
                    {
                        flipped = false;
                        timer = 63;
                        jump = true;
                        NPC.velocity.Y = 9;
                    }
                }
                else
                {
                    NPC.rotation = 0f;
                    gravity = .3f;
                    float worldSizeModifier = (float)(Main.maxTilesX / 4200);
                    worldSizeModifier *= worldSizeModifier;
                    //small =1
                    //medium =2.25
                    //large =4
                    float num2 = (float)((double)(NPC.position.Y / 16f - (60f + 10f * worldSizeModifier)) / (Main.worldSurface / 6.0));
                    if ((double)num2 < 0.25)
                    {
                        num2 = 0.25f;
                    }
                    if (num2 > 1f)
                    {
                        num2 = 1f;
                    }
                    gravity *= num2;
                    jumpSpeedY = gravity * -35;
                    //Main.NewText("gravity: " +gravity);
                    //Main.NewText("jump: " +jumpSpeedY);
                    Player player = Main.player[NPC.target];
                    NPC.TargetClosest(true);
                    //Main.NewText(Math.Abs(player.Center.X - NPC.Center.X));
                    if (Math.Abs(player.Center.X - NPC.Center.X) < aggroDistance && Math.Abs(player.Bottom.Y - NPC.Bottom.Y) < aggroDistanceY)
                    {
                        jumpSpeedX = Math.Abs(player.Center.X - NPC.Center.X) / 70 * (NPC.confused ? -1 : 1);
                        timer++;
                        if (timer > 30)
                        {
                            frame = 3;
                            if (!jump)
                            {
                                if (player.Center.X > NPC.Center.X)
                                {
                                    NPC.velocity.X = jumpSpeedX;
                                    NPC.velocity.Y = jumpSpeedY;
                                }
                                else
                                {
                                    NPC.velocity.X = -jumpSpeedX;
                                    NPC.velocity.Y = jumpSpeedY;
                                }
                                jump = true;
                            }
                        }
                        else if (timer > 20)
                        {
                            frame = 1;
                        }
                        else if (timer > 10)
                        {
                            frame = 2;
                        }
                        else
                        {
                            frame = 1;
                        }
                    }
                    else if (!jump)
                    {
                        frame = 0;
                        timer = 0;
                    }
                    if (NPC.collideX)
                    {
                        NPC.velocity.X *= -1;
                    }
                    if (timer > 62 && NPC.collideY)
                    {
                        NPC.velocity.X = 0;
                        NPC.velocity.Y = 0;
                        jump = false;
                        timer = 0;
                    }
                    NPC.velocity.Y += gravity;
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Y = frame * frameHeight;
        }
    }
}