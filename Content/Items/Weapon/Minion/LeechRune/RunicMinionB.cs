using QwertyMod.Common;
using Terraria;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Weapon.Minion.LeechRune
{
    class RunicMinionB : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            MinionManager modPlayer = player.GetModPlayer<MinionManager>();
            if (player.ownedProjectileCounts[ ModContent.ProjectileType<RunicMinionFreindly>()] > 0)
            {
                modPlayer.RuneMinion = true;
            }
            if (!modPlayer.RuneMinion)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
            else
            {
                player.buffTime[buffIndex] = 18000;
            }
        }
    }
}
