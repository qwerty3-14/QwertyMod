using QwertyMod.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Minion.UrQuan
{
    class UrQuanB : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ur-Quan Dreadnought");
            Description.SetDefault("Submit or die foolish human!");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            MinionManager modPlayer = player.GetModPlayer<MinionManager>();
            if (player.ownedProjectileCounts[ProjectileType<Dreadnought>()] > 0)
            {
                modPlayer.Dreadnought = true;
            }
            if (!modPlayer.Dreadnought)
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
