using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Weapon.Morphs
{
    public class MorphCooldown : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Quick Morph Cool down");
            Description.SetDefault("Can't use another quick morph!");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
        }
        public override bool ReApply(Player player, int time, int buffIndex)
        {
            player.GetModPlayer<ShapeShifterPlayer>().morphCooldownTime = time;
            return base.ReApply(player, time, buffIndex);
        }
    }
}
