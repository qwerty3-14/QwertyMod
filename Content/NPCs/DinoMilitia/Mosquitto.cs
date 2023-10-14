using Microsoft.Xna.Framework;
using QwertyMod.Content.Buffs;
using System;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertyMod.Content.NPCs.DinoMilitia
{
    public class Mosquitto : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4;
        }

        public override void SetDefaults()
        {
            NPC.width = 10;
            NPC.height = 12;
            NPC.damage = 1;
            NPC.defense = 0;
            NPC.lifeMax = 1;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 0f;
            NPC.knockBackResist = 2f;
            NPC.aiStyle = 14;
            AIType = 49;
            AnimationType = NPCID.Bee;
            NPC.npcSlots = 0;

            NPC.noGravity = true;
            NPC.noTileCollide = false;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            database.Entries.Remove(bestiaryEntry);
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0f;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            target.AddBuff(ModContent.BuffType<DinoPox>(), 480);
            if (Main.expertMode)
            {
                target.AddBuff(33, 480);
            }
        }

        public bool runOnce = true;
        public int timer;

        public override void AI()
        {
            timer++;
            if (runOnce)
            {
                NPC.velocity = new Vector2(MathF.Cos(NPC.ai[0]) * 6f * NPC.direction, -MathF.Sin(NPC.ai[0]) * 6f);

                runOnce = false;
            }
            if (timer < 20)
            {
                NPC.aiStyle = -1;
            }
            else
            {
                NPC.aiStyle = 14;
            }
        }
    }
}