using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Weapon.Melee.Sword.EtimsSword
{
    class AntiProjectile : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Anti Projectile");
            Description.SetDefault("Somehow they can never hit you!");

        }
    }
}
