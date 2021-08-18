﻿using QwertyMod.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Minion.HydraHead
{
    class HydraHeadB : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydra Head");
            Description.SetDefault("The Hydra Head will assist your firepower!");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            MinionManager modPlayer = player.GetModPlayer<MinionManager>();
            if (player.ownedProjectileCounts[ProjectileType<MinionHead>()] > 0)
            {
                modPlayer.HydraHeadMinion = true;
            }
            if (!modPlayer.HydraHeadMinion)
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