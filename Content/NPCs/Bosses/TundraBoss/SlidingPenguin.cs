using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.NPCs.Bosses.TundraBoss
{
    public class SlidingPenguin : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sliding Penguin");
            //Main.npcFrameCount[NPC.type] = 4;
        }

        public override void SetDefaults()
        {
            NPC.width = 42;
            NPC.height = 18;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            //NPC.value = 6000f;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = -1;
            NPC.lifeMax = 25;
            NPC.defense = 4;
            NPC.damage = 20;

            NPC.noGravity = false;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.damage = 30;
            NPC.lifeMax = 25;
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
                new FlavorTextBestiaryInfoElement("This penguin slides a little too well... Immersion Ruined!")
            }); 
        }

        private int timer;
        float speed = 5;
        public override void AI()
        {
            if(NPC.ai[1] == 1)
            {
                NPC.TargetClosest(false);
                Player player = Main.player[NPC.target];
                NPC.rotation = (player.Center - NPC.Center).ToRotation() + (float)Math.PI;
                NPC.velocity = QwertyMethods.PolarVector(5, NPC.rotation - (float)Math.PI);
                if(NPC.velocity.X > 0)
                {
                    NPC.rotation += (float)Math.PI;
                }
                NPC.ai[1] = 2;
                NPC.noTileCollide = true;
                NPC.noGravity = true;
                NPC.spriteDirection = (int)NPC.ai[0];
            }
            else if (NPC.ai[1] == 2)
            {
                NPC.TargetClosest(false);
                Player player = Main.player[NPC.target];
                NPC.noTileCollide = true;
                NPC.noGravity = true;
                if (NPC.Center.Y < player.Center.Y)
                {
                    NPC.ai[1] = 0;
                }
            }
            else
            {
                NPC.noGravity = false;
                NPC.rotation = 0;
                NPC.noTileCollide = false;
                NPC.velocity.X = speed * NPC.ai[0];
                timer++;
                if (NPC.collideX && timer > 5)
                {
                    NPC.ai[0] *= -1;
                    timer = 0;
                }
                NPC.spriteDirection = (int)NPC.ai[0];
                if (timer > 180)
                {
                    speed -= 5f / 180f;
                }
                if (speed <= 0)
                {
                    NPC Penguin = Main.npc[NPC.NewNPC((int)NPC.Top.X, (int)NPC.Top.Y, NPCID.Penguin)];
                    NPC.active = false;
                }
            }
        }

        public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
        {
            NPC.ai[0] *= -1;
            NPC.netUpdate = true;
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            if(NPC.life <=0)
            {
                Gore.NewGore(NPC.position, NPC.velocity, 160);
                Gore.NewGore(new Vector2(NPC.position.X, NPC.position.Y), NPC.velocity, 161);
            }
        }
    }
}