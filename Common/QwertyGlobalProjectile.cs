using Terraria;
using Terraria.ModLoader;

namespace QwertyMod.Common
{
    public class QwertyGlobalProjectile : GlobalProjectile
    {
        public bool ignoresArmor = false;
        public override bool InstancePerEntity => true;
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (ignoresArmor)
            {
                modifiers.ArmorPenetration += 10000;
            }
        }
    }
}
