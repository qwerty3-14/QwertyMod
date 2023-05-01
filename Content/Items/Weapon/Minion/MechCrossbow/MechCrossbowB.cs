using QwertyMod.Common;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Minion.MechCrossbow
{
    class MechCrossbowB : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Mech Crossbow");
            //Description.SetDefault("Will shoot your enemies in the knee!");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            MinionManager modPlayer = player.GetModPlayer<MinionManager>();
            if (player.ownedProjectileCounts[ProjectileType<MechCrossbowMinion>()] > 0)
            {
                modPlayer.MechCrossbow = true;
            }
            if (!modPlayer.MechCrossbow)
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
