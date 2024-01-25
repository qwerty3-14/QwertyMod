using Terraria;
using Terraria.ModLoader;


namespace QwertyMod.Content.Items.Equipment.Armor.Glass
{
    public class ArcanelyTuned : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            if (Main.rand.NextBool(12))
            {
                Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<DazzleSparkle>());
            }
        }
    }
}