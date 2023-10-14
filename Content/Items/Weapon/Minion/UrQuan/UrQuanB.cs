using QwertyMod.Common;
using Terraria;
using Terraria.ModLoader;


namespace QwertyMod.Content.Items.Weapon.Minion.UrQuan
{
    class UrQuanB : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            MinionManager modPlayer = player.GetModPlayer<MinionManager>();
            if (player.ownedProjectileCounts[ ModContent.ProjectileType<Dreadnought>()] > 0)
            {
                modPlayer.Dreadnought = true;
            }
            if (!modPlayer.Dreadnought)
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
