using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace QwertyMod.Common
{
    public class QwertyGlobalItem : GlobalItem
    {
        public override bool ConsumeAmmo(Item item, Player player)
        {
            return Main.rand.NextFloat() > player.GetModPlayer<CommonStats>().ammoReduction;
        }
    }
}
