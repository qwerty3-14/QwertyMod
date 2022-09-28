using Terraria;
using Terraria.ModLoader;
using QwertyMod.Common;

namespace QwertyMod.Content.Buffs
{
    public class NormalGravity : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Normal gravity");
            Description.SetDefault("Prevents high altitudes from reducing your gravity");
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