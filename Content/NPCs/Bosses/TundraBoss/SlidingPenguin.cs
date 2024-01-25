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

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.damage = 15;
            NPC.lifeMax = 13;
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
            if (NPC.ai[1] == 1)
            {
                NPC.TargetClosest(false);
                Player player = Main.player[NPC.target];
                NPC.rotation = (player.Center - NPC.Center).ToRotation() + MathF.PI;
                NPC.velocity = QwertyMethods.PolarVector(5, NPC.rotation - MathF.PI);
                if (NPC.velocity.X > 0)
                {
                    NPC.rotation += MathF.PI;
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
                    NPC Penguin = Main.npc[NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Top.X, (int)NPC.Top.Y, NPCID.Penguin)];
                    NPC.active = false;
                }
            }
        }

        public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            NPC.ai[0] *= -1;
            NPC.netUpdate = true;
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