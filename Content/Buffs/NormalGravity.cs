using QwertyMod.Common;
using Terraria;
using Terraria.ModLoader;

namespace QwertyMod.Content.Buffs
{
    public class NormalGravity : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<CommonStats>().normalGravity = 2;
        }
    }
}