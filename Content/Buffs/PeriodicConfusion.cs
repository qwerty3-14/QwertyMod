using Terraria;
using Terraria.ModLoader;

namespace QwertyMod.Content.Buffs
{
    public class PeriodicConfusion : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            int time = player.buffTime[buffIndex];
            if(time % (20 * 60) < 10 * 60)
            {
                player.confused = true;
            }
        }
    }
}