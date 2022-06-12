using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace QwertyMod.Common
{
    public class QwertyGlobalProjectile : GlobalProjectile
    {
        public bool ignoresArmor = false;
        public override bool InstancePerEntity => true;
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (ignoresArmor)
            {
                Player player = Main.player[projectile.owner];
                int finalDefense = target.defense - (int)player.GetArmorPenetration(DamageClass.Generic);
                target.ichor = false;
                target.betsysCurse = false;
                if (finalDefense < 0)
                {
                    finalDefense = 0;
                }
                damage += finalDefense / 2;
            }
        }
    }
}
