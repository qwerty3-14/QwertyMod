using QwertyMod.Common;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
namespace QwertyMod.Content.Items.Weapon.Minion.ChloroSniper
{
    class ChlorophyteSniperB : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Chlorophyte Sniper");
            //Description.SetDefault("The Chlorophyte Sniper will fight for you!");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            MinionManager modPlayer = player.GetModPlayer<MinionManager>();
            if (player.ownedProjectileCounts[ProjectileType<ChlorophyteSniper>()] > 0)
            {
                modPlayer.chlorophyteSniper = true;
            }
            if (!modPlayer.chlorophyteSniper)
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
