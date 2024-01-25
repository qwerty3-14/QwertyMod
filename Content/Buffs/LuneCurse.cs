using QwertyMod.Content.Dusts;
using Terraria;
using Terraria.ModLoader;

namespace QwertyMod.Content.Buffs
{
    public class LuneCurse : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<LuneDust>());
        }
    }
}