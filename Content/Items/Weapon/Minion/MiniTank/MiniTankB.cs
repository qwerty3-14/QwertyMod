using QwertyMod.Common;
using Terraria;
using Terraria.ModLoader;


namespace QwertyMod.Content.Items.Weapon.Minion.MiniTank
{
    class MiniTankB : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            MinionManager modPlayer = player.GetModPlayer<MinionManager>();
            if (player.ownedProjectileCounts[ ModContent.ProjectileType<MiniTank>()] > 0)
            {
                modPlayer.miniTank = true;
            }
            if (!modPlayer.miniTank)
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
