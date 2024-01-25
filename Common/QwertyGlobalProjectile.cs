using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace QwertyMod.Common
{
    public class QwertyGlobalProjectile : GlobalProjectile
    {
        public bool ignoresArmor = false;
        public override bool InstancePerEntity => true;
        public NPC npcOwner = null;
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (ignoresArmor)
            {
                modifiers.ArmorPenetration += 10000;
            }
        }
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if(source is EntitySource_Parent parent && parent.Entity is NPC npc)
            {
                npcOwner = npc;
            }
        }
    }
}
