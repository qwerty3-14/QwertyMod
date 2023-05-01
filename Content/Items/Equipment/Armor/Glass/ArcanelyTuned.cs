using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Equipment.Armor.Glass
{
    public class ArcanelyTuned : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Arcanely Tuned");
            //Description.SetDefault("If you can read this you're hacking!");
            Main.debuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            if (Main.rand.NextBool(12))
            {
                Dust.NewDust(npc.position, npc.width, npc.height, DustType<DazzleSparkle>());
            }
        }
    }
}