using QwertyMod.Content.NPCs.Bosses.BladeBoss;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Common
{
    class SwordQuakeShake : ModPlayer
    {
        public bool shake = false;
        private int repoX = 0;
        private int repoY = 0;
        private int time = 0;

        public override void ResetEffects()
        {
            shake = false;
        }

        public override void PreUpdate()
        {
        }

        public override void ModifyScreenPosition()
        {
            if (NPC.AnyNPCs(NPCType<Imperious>()))
            {
                for (int i = 0; i < Main.npc.Length; i++)
                {
                    if (Main.npc[i].type == NPCType<Imperious>())
                    {
                        if (Main.npc[i].ai[0] == 1 && Main.npc[i].ai[1] > 0)
                        {
                            shake = true;
                            time = (int)Main.npc[i].ai[1];
                        }
                    }
                }
            }

            if (shake)
            {
                if (time % 3 == 0)
                {
                    repoX = Main.rand.Next(-10, 11);
                    repoY = Main.rand.Next(-10, 11);
                }
            }
            else
            {
                repoX = repoY = 0;
            }
            Main.screenPosition.X += repoX;
            Main.screenPosition.Y += repoY;
        }
    }
}
