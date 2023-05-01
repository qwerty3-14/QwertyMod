using QwertyMod.Common;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace QwertyMod.Content.Items.Weapon.Minion.GlassSpike
{
    class GlassSpikeB : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Glass Spike");
            //Description.SetDefault("Way worse than stepping on legos!");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            MinionManager modPlayer = player.GetModPlayer<MinionManager>();
            if (player.ownedProjectileCounts[ProjectileType<GlassSpike>()] > 0)
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
