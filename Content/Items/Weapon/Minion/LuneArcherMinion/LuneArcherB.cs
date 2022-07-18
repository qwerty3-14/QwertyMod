using QwertyMod.Common;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace QwertyMod.Content.Items.Weapon.Minion.LuneArcherMinion
{
    class LuneArcherB : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lune Archer");
            Description.SetDefault("Will shoot your enemies in the knee!");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            MinionManager modPlayer = player.GetModPlayer<MinionManager>();
            if (player.ownedProjectileCounts[ProjectileType<LuneArcher>()] > 0)
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
