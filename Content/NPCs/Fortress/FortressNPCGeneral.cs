using QwertyMod.Content.NPCs.Invader;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace QwertyMod.Content.NPCs.Fortress
{
    public class FortressNPCGeneral : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public bool fortressNPC = false;
        public bool contactDamageToInvaders = false;
        public static Entity FindTarget(NPC npc, bool allowFlip = false)
        {
            npc.TargetClosest(false);
            float maxDist = (Main.player[npc.target].Center - npc.Center).Length() - Main.player[npc.target].aggro;
            NPC npcTarget = null;
            if (QwertyMethods.ClosestNPC(ref npcTarget, maxDist, npc.Center, false, -1, delegate (NPC possibleTarget) { return possibleTarget.GetGlobalNPC<InvaderNPCGeneral>().invaderNPC; }))
            {
                if (allowFlip)
                {
                    npc.direction = Math.Sign(npcTarget.Center.X - npc.Center.X);
                }
                return npcTarget;
            }
            if (allowFlip && Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0))
            {
                npc.TargetClosest(true);
            }
            return Main.player[npc.target];
        }
        int contactDamageCooldown = 0;
        public override void PostAI(NPC npc)
        {
            if (contactDamageToInvaders)
            {
                if (contactDamageCooldown <= 0)
                {
                    for (int i = 0; i < 200; i++)
                    {
                        if (Main.npc[i].TryGetGlobalNPC<InvaderNPCGeneral>(out InvaderNPCGeneral gNPC))
                        {
                            if (gNPC.invaderNPC && Collision.CheckAABBvAABBCollision(npc.position, npc.Size, Main.npc[i].position, Main.npc[i].Size))
                            {
                                QwertyMethods.PokeNPC(Main.LocalPlayer, Main.npc[i], npc.GetSource_FromAI(), npc.damage, DamageClass.Default, 0);
                                contactDamageCooldown = 10;
                                break;
                            }
                        }
                    }
                }
                contactDamageCooldown--;
            }
        }
    }
    public class FortressNPCProjectile : GlobalProjectile
    {
        public bool isFromFortressNPC = false;
        public override bool InstancePerEntity => true;
        public override void SetDefaults(Projectile projectile)
        {
            if (isFromFortressNPC)
            {
                projectile.friendly = true;
            }
        }
        public override bool? CanHitNPC(Projectile projectile, NPC target)
        {
            if (isFromFortressNPC)
            {
                return target.GetGlobalNPC<InvaderNPCGeneral>().invaderNPC ? null : false;
            }
            return null;
        }

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (isFromFortressNPC)
            {
                modifiers.FinalDamage *= 4;
            }
        }
    }

}