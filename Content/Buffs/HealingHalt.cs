using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Buffs
{
    public class HealingHalt : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen = 0;
            if (player.HasBuff(BuffID.PotionSickness))
            {
                player.buffTime[player.FindBuffIndex(BuffID.PotionSickness)]++;
            }
        }
    }
}