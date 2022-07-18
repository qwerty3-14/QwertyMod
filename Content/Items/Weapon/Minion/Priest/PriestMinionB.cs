using QwertyMod.Common;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Minion.Priest
{
    class PriestMinionB : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Priest Minion");
            Description.SetDefault("Higher beings fight for you!");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            MinionManager modPlayer = player.GetModPlayer<MinionManager>();
            if (player.ownedProjectileCounts[ProjectileType<PriestMinion>()] > 0)
            {
                modPlayer.PriestMinion = true;
            }
            if (!modPlayer.PriestMinion)
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
