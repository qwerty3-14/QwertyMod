using Terraria;
using Terraria.ModLoader;

namespace QwertyMod.Content.Buffs
{
    public class CriticalFailure : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Critical Failure");
            Description.SetDefault("Critical strike chance reduced by 40%");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetCritChance(DamageClass.Generic) -= 40;
        }
    }
}