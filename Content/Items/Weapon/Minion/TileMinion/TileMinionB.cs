using QwertyMod.Common;
using Terraria;
using Terraria.ModLoader;


namespace QwertyMod.Content.Items.Weapon.Minion.TileMinion
{
    public class TileMinionB : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            MinionManager modPlayer = player.GetModPlayer<MinionManager>();
            if (player.ownedProjectileCounts[ ModContent.ProjectileType<TileMinion>()] > 0)
            {
                modPlayer.TileMinion = true;
            }
            if (!modPlayer.TileMinion)
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
