using QwertyMod.Common;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Minion.ShieldMinion
{
    class ShieldMinionB : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shield Minion");
            Description.SetDefault("You got your own personal Phalax!");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            MinionManager modPlayer = player.GetModPlayer<MinionManager>();
            if (player.ownedProjectileCounts[ProjectileType<ShieldMinion>()] > 0)
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
