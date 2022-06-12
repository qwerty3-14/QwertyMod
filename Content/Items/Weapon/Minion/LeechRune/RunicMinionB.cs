using QwertyMod.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace QwertyMod.Content.Items.Weapon.Minion.LeechRune
{
    class RunicMinionB : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Leech Rune");
            Description.SetDefault("The Leech rune will fight for you!");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            MinionManager modPlayer = player.GetModPlayer<MinionManager>();
            if (player.ownedProjectileCounts[ProjectileType<RunicMinionFreindly>()] > 0)
            {
                modPlayer.RuneMinion = true;
            }
            if (!modPlayer.RuneMinion)
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
