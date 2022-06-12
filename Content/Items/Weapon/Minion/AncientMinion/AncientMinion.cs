using QwertyMod.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace QwertyMod.Content.Items.Weapon.Minion.AncientMinion
{
    class AncientMinionB : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Minion");
            Description.SetDefault("The Ancient Minion will fight for you!");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            MinionManager modPlayer = player.GetModPlayer<MinionManager>();
            if (player.ownedProjectileCounts[ProjectileType<AncientMinionFreindly>()] > 0)
            {
                modPlayer.AncientMinion = true;
            }
            if (!modPlayer.AncientMinion)
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
