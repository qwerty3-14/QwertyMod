using QwertyMod.Common;
using Terraria;
using Terraria.ModLoader;



namespace QwertyMod.Content.Items.Weapon.Minion.LuneArcherMinion
{
    class LuneArcherB : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            MinionManager modPlayer = player.GetModPlayer<MinionManager>();
            if (player.ownedProjectileCounts[ ModContent.ProjectileType<LuneArcher>()] > 0)
            {
                modPlayer.LuneArcher = true;
            }
            if (!modPlayer.LuneArcher)
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
