using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.NPCs.Bosses.TundraBoss
{
    public class FlyingPenguin : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Flying Penguin");
            Main.npcFrameCount[NPC.type] = 4;
        }

        public override void SetDefaults()
        {
            NPC.width = 22;
            NPC.height = 40;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = 0;
            NPC.lifeMax = 10;
            NPC.defense = 4;
            NPC.damage = 0;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0f;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Sets the description of this NPC that is listed in the bestiary
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundSnow,
                new FlavorTextBestiaryInfoElement("Penguins can't fly! Immersion Ruined!")
            }); ;
        }

        private int timer;
        private float flyAboveHeight = 150;
        private float penguinPoliteness = 80;
        private float flySpeed = 10;
        private int frame;

        public override void AI()
        {
            timer++;
            Player player = Main.player[NPC.target];

            Vector2 flyTo = new Vector2(player.Center.X + (penguinPoliteness * NPC.ai[0]), player.Center.Y - flyAboveHeight);
            if (timer > 180)
            {
                bool inGround = NPC.collideX || NPC.collideY;
                NPC.velocity = new Vector2(0, 10);
                NPC.noTileCollide = NPC.Bottom.Y < player.Bottom.Y;
                if (Main.expertMode)
                {
                    NPC.damage = 30;
                }
                else
                {
                    NPC.damage = 20;
                }
                NPC.TargetClosest(false);
                NPC.spriteDirection = -NPC.direction;
                NPC.rotation = MathF.PI;
                if (!inGround && timer % 10 == 0)
                {
                    if (frame == 1)
                    {
                        frame = 0;
                    }
                    else
                    {
                        frame = 1;
                    }
                }
            }
            else if (timer > 120)
            {
                NPC.TargetClosest(true);
                NPC.spriteDirection = NPC.direction;
                if (timer % 10 == 0)
                {
                    if (frame == 3)
                    {
                        frame = 2;
                    }
                    else
                    {
                        frame = 3;
                    }
                }
                NPC.velocity = new Vector2(0, 0);
            }
            else
            {

                NPC.TargetClosest(true);
                NPC.spriteDirection = NPC.direction;
                if (timer % 10 == 0)
                {
                    if (frame == 3)
                    {
                        frame = 2;
                    }
                    else
                    {
                        frame = 3;
                    }
                }
                NPC.velocity = (flyTo - NPC.Center);
                if (NPC.velocity.Length() > flySpeed)
                {
                    NPC.velocity = NPC.velocity.SafeNormalize(-Vector2.UnitY) * flySpeed;
                }
            }
            if (timer > 360)
            {
                NPC Penguin = Main.npc[NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Top.X, (int)NPC.Top.Y, NPCID.Penguin)];
                NPC.active = false;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Y = frame * frameHeight;
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, 160);
                Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X, NPC.position.Y), NPC.velocity, 161);
            }
        }
    }
}