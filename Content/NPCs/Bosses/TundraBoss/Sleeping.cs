using QwertyMod.Common;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace QwertyMod.Content.NPCs.Bosses.TundraBoss
{
    public class Sleeping : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("ZZZZZ...");
            Main.npcFrameCount[NPC.type] = 2;
        }

        public override void SetDefaults()
        {
            NPC.width = 94;
            NPC.height = 70;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.chaseable = false;
            //NPC.value = 6000f;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = 0;
            NPC.lifeMax = 200;
            NPC.defense = 0;
            NPC.noGravity = false;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            database.Entries.Remove(bestiaryEntry);
        }
        public override bool CheckActive()
        {
            return false;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0f;
        }

        public override void OnKill()
        {
            FrozenDen.activeSleeper = false;
            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
            NPC.NewNPC(new EntitySource_Misc(""), (int)NPC.Center.X, (int)NPC.Center.Y, NPCType<PolarBear>());
        }

        private int frame;

        public override void FindFrame(int frameHeight)
        {
            NPC.spriteDirection = 1;
            NPC.frameCounter++;
            if (NPC.frameCounter > 10)
            {
                frame++;
                if (frame >= 2)
                {
                    frame = 0;
                }
                NPC.frameCounter = 0;
            }
            NPC.frame.Y = frameHeight * frame;
        }
    }
}