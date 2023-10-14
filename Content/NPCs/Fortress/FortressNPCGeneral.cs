using QwertyMod.Content.NPCs.Invader;
using System;
using Terraria;
using Terraria.ModLoader;
using QwertyMod.Common;

namespace QwertyMod.Content.NPCs.Fortress
{
    public class FortressNPCGeneral : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public bool fortressNPC = false;
        public float contactDamageToInvaders = 0;
        public static Entity FindTarget(NPC npc, bool allowFlip = false)
        {
            Entity target = null;
            float maxDist = 10000;
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                if (Main.player[i].active && (Main.player[i].Center - npc.Center).Length() - Main.player[i].aggro < maxDist && !Main.player[i].GetModPlayer<CommonStats>().higherBeingFriendly)
                {
                    target = Main.player[i];
                    npc.target = i;
                    maxDist = (Main.player[npc.target].Center - npc.Center).Length() - Main.player[npc.target].aggro;
                }
            }
            NPC npcTarget = null;
            if (QwertyMethods.ClosestNPC(ref npcTarget, maxDist, npc.Center, false, -1, delegate (NPC possibleTarget) { return possibleTarget.GetGlobalNPC<InvaderNPCGeneral>().invaderNPC; }))
            {
                if (allowFlip)
                {
                    npc.direction = Math.Sign(npcTarget.Center.X - npc.Center.X);
                }
                return npcTarget;
            }
            if (target == null && npcTarget == null)
            {
                return null;
            }
            if (allowFlip && Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0))
            {
                npc.direction = Math.Sign(Main.player[npc.target].Center.X - npc.Center.X);
            }
            return Main.player[npc.target];
        }

        int contactDamageCooldown = 0;
        public override void PostAI(NPC npc)
        {
            if (contactDamageToInvaders > 0)
            {
                if (contactDamageCooldown <= 0)
                {
                    for (int i = 0; i < 200; i++)
                    {
                        if (Main.npc[i].TryGetGlobalNPC<InvaderNPCGeneral>(out InvaderNPCGeneral gNPC))
                        {
                            if (gNPC.invaderNPC && Collision.CheckAABBvAABBCollision(npc.position, npc.Size, Main.npc[i].position, Main.npc[i].Size))
                            {
                                NPC.HitInfo hitInfo = new NPC.HitInfo();
                                hitInfo.Damage = (int)(npc.damage * contactDamageToInvaders);
                                Main.npc[i].StrikeNPC(hitInfo, false, true);
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
        public float EvEMultiplier = 1f;
        public override bool InstancePerEntity => true;
        public override void SetDefaults(Projectile projectile)
        {
            if (isFromFortressNPC)
            {
                projectile.friendly = true;
                projectile.npcProj = true;
            }
        }
        public override bool CanHitPlayer(Projectile projectile, Player target)
        {
            if(isFromFortressNPC && target.GetModPlayer<CommonStats>().higherBeingFriendly)
            {
                return false;
            }
            return true;
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
                modifiers.FinalDamage *= 4 * EvEMultiplier;
            }
        }
    }

}