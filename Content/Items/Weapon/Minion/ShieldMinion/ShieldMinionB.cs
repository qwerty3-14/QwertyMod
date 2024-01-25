using QwertyMod.Common;
using Terraria;
using Terraria.ModLoader;


namespace QwertyMod.Content.Items.Weapon.Minion.ShieldMinion
{
    class ShieldMinionB : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            MinionManager modPlayer = player.GetModPlayer<MinionManager>();
            if (player.ownedProjectileCounts[ ModContent.ProjectileType<ShieldMinion>()] > 0)
            {
                modPlayer.ShieldMinion = true;
            }
            if (!modPlayer.ShieldMinion)
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
