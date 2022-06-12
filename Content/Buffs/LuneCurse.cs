using QwertyMod.Content.Dusts;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Buffs
{
    public class LuneCurse : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Moon Cooldown");
            Description.SetDefault("Can't shoot another moon yet!");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            Dust.NewDust(npc.position, npc.width, npc.height, DustType<LuneDust>());
        }
    }
}