using QwertyMod.Common;
using Terraria;
using Terraria.ModLoader;



namespace QwertyMod.Content.Items.Weapon.Minion.GlassSpike
{
    class GlassSpikeB : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            MinionManager modPlayer = player.GetModPlayer<MinionManager>();
            if (player.ownedProjectileCounts[ ModContent.ProjectileType<GlassSpike>()] > 0)
            {
                modPlayer.GlassSpike = true;
            }
            if (!modPlayer.GlassSpike)
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
