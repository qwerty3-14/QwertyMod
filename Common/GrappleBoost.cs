using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace QwertyMod.Common
{
    public class GrappleBoost : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override void GrapplePullSpeed(Projectile projectile, Player player, ref float speed)
        {
            if (player.GetModPlayer<CommonStats>().hookSpeed > 1f)
            {
                speed *= player.GetModPlayer<CommonStats>().hookSpeed;
            }
        }


    }
}
