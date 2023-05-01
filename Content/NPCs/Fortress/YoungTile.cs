using Microsoft.Xna.Framework;
using QwertyMod.Content.Dusts;
using QwertyMod.Content.Items.Consumable.Tiles.Fortress.BuildingBlocks;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.NPCs.Fortress
{
    public class YoungTile : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Young Tile");
            Main.npcFrameCount[NPC.type] = 4;
        }


        public override void SetDefaults()
        {
            NPC.width = 16;
            NPC.height = 22;
            NPC.aiStyle = -1;
            NPC.damage = 28;
            NPC.defense = 18;
            NPC.lifeMax = 30;

            if (NPC.downedGolemBoss)
            {
                NPC.lifeMax = 200;
                NPC.damage = 80;
            }
            NPC.value = 50;
            //NPC.alpha = 100;
            //NPC.behindTiles = true;
            NPC.HitSound = SoundID.NPCHit7;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            //NPC.dontTakeDamage = true;
            //NPC.scale = 1.2f;
            NPC.npcSlots = 0.00f;
            NPC.buffImmune[20] = true;
            NPC.buffImmune[24] = true;
            //banner = NPC.type;
            //bannerItem = mod.ItemType("HopperBanner");
            NPC.buffImmune[BuffID.Confused] = false;
            NPC.GetGlobalNPC<FortressNPCGeneral>().contactDamageToInvaders = true;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Sets the description of this NPC that is listed in the bestiary
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
                new FlavorTextBestiaryInfoElement("Even at a young age, the tile's insticts to defend to fortress are strong.")
            });
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    int dustType = DustType<FortressDust>();
                    int dustIndex = Dust.NewDust(NPC.position, NPC.width, NPC.height, dustType);
                    Dust dust = Main.dust[dustIndex];
                    dust.velocity.X = dust.velocity.X + Main.rand.Next(-50, 51) * 0.01f;
                    dust.velocity.Y = dust.velocity.Y + Main.rand.Next(-50, 51) * 0.01f;
                    dust.scale *= 1f + Main.rand.Next(-30, 31) * 0.01f;
                }
            }
            for (int i = 0; i < 1; i++)
            {
                int dustType = DustType<FortressDust>();
                int dustIndex = Dust.NewDust(NPC.position, NPC.width, NPC.height, dustType);
                Dust dust = Main.dust[dustIndex];
                dust.velocity.X = dust.velocity.X + Main.rand.Next(-50, 51) * 0.01f;
                dust.velocity.Y = dust.velocity.Y + Main.rand.Next(-50, 51) * 0.01f;
                dust.scale *= 1f + Main.rand.Next(-30, 31) * 0.01f;
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemType<FortressBrick>(), 2));
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

        public override void AI()
        {
            NPC.GetGlobalNPC<FortressNPCGeneral>().fortressNPC = true;
            if (runOnce)
            {
                if (NPC.ai[3] == 0)
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

            if (frame == 0)
            {
                NPC.dontTakeDamage = true;
            }
            else
            {
                NPC.dontTakeDamage = false;
            }
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

            Entity player = FortressNPCGeneral.FindTarget(NPC, true);

            //Main.NewText(Math.Abs(player.Center.X - NPC.Center.X));
            if (Math.Abs(player.Center.X - NPC.Center.X) < aggroDistance && Math.Abs(player.Bottom.Y - NPC.Bottom.Y) < aggroDistanceY)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    jumpSpeedX = Math.Abs((player.Center.X + Main.rand.Next(-100, 100)) - NPC.Center.X) / 70 * (NPC.confused ? -1 : 1);
                    NPC.netUpdate = true;
                }
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
            else if (timer > 0)
            {
                timer++;
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

        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Y = frame * frameHeight;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(jumpSpeedX);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            jumpSpeedX = reader.ReadSingle();
        }
    }
}