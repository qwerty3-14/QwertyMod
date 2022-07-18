using Terraria.ModLoader;
using Terraria;
using QwertyMod.Content.NPCs.Fortress;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.WorldBuilding;
using System;
using System.Collections.Generic;
using Terraria.DataStructures;
using QwertyMod.Content.Dusts;

namespace QwertyMod.Content.NPCs.Invader
{
    public class InvaderNPCGeneral : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public bool invaderNPC = false;
        public bool contactDamageToNatives = false;
        public static Entity FindTarget(NPC npc, bool allowFlip = false)
        {
            npc.TargetClosest(false);
            float maxDist = (Main.player[npc.target].Center - npc.Center).Length() - Main.player[npc.target].aggro;
            NPC npcTarget = null;
            if(QwertyMethods.ClosestNPC(ref npcTarget, maxDist, npc.Center, false, -1, delegate (NPC possibleTarget) { return possibleTarget.GetGlobalNPC<FortressNPCGeneral>().fortressNPC; }))
            {
                if(allowFlip)
                {
                    npc.direction = Math.Sign(npcTarget.Center.X - npc.Center.X);
                }
                return npcTarget;
            }
            if(Main.player[npc.target].dead)
            {
                return null;
            }
            if(allowFlip && Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0) )
            {
                npc.direction = Math.Sign(Main.player[npc.target].Center.X - npc.Center.X);
            }
            return Main.player[npc.target];
        }
        public static void SpawnIn(NPC npc)
        {
            Point origin = npc.Bottom.ToTileCoordinates();

            while (!WorldUtils.Find(origin, Searches.Chain(new Searches.Down(2), new GenCondition[]
            {
                                        new Terraria.WorldBuilding.Conditions.IsSolid()
            }), out _))
            {
                npc.position.Y++;
                origin = npc.Bottom.ToTileCoordinates();
            }
        }

        public static void SpawnAnimation(NPC npc)
        {
            int width = npc.width / 2;
            int height = npc.height / 4;
            int dustCount = width;
            for(int i =0; i < dustCount; i++)
            {
                float rot = (float)Math.PI * 2f * ((float)i / dustCount);
                Vector2 unitVector = QwertyMethods.PolarVector(1f, rot);
                Dust d = Dust.NewDustPerfect(npc.Top + new Vector2( unitVector.X*width, unitVector.Y * height), ModContent.DustType<InvaderGlow>(), Vector2.UnitY * npc.height * 0.09f);
                d.noGravity = true;
                d.frame.Y = 0;
                d.scale *= 2;
            }
        }
        int idleWalkTimer = 0;
        public static void WalkerIdle(NPC npc, float speed)
        {
            if(npc.direction == 0)
            {
                npc.direction = 1;
            }
            npc.GetGlobalNPC<InvaderNPCGeneral>().idleWalkTimer--;
            if(npc.GetGlobalNPC<InvaderNPCGeneral>().idleWalkTimer <= -60)
            {
                npc.GetGlobalNPC<InvaderNPCGeneral>().idleWalkTimer = 120;
            }
            if(npc.GetGlobalNPC<InvaderNPCGeneral>().idleWalkTimer > 0)
            {
                
                if(npc.collideX)
                {
                    npc.direction *= -1;
                }
                
                if(!WalkerWalk(npc, speed))
                {
                    npc.direction *= -1;
                }
            }
        }
        public static bool WalkerWalk(NPC npc, float speed)
        {
            Point checkHere = (npc.Bottom + Vector2.UnitY * 8 + Vector2.UnitX * npc.direction * 16).ToTileCoordinates();
            if(!Main.tile[checkHere.X, checkHere.Y].HasTile && !Main.tile[checkHere.X, checkHere.Y+1].HasTile)
            {
                npc.velocity.X = 0;
                return false;
            }
            npc.velocity.X = npc.direction * speed;
            return true;
        }

        int contactDamageCooldown = 0;
        public override void PostAI(NPC npc)
        {
            if(contactDamageToNatives)
            {
                if(contactDamageCooldown <=0)
                {
                    for(int i =0; i < 200; i++)
                    {
                        if( Main.npc[i].TryGetGlobalNPC<FortressNPCGeneral>(out FortressNPCGeneral gNPC))
                        {
                            if(gNPC.fortressNPC && Collision.CheckAABBvAABBCollision(npc.position, npc.Size, Main.npc[i].position, Main.npc[i].Size))
                            {
                                QwertyMethods.PokeNPC(Main.LocalPlayer, Main.npc[i], new EntitySource_Misc(""), npc.damage, DamageClass.Default, 0);
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
    public class InvaderProjectile : GlobalProjectile
    {
        public bool isInvaderProjectile;
        public override bool InstancePerEntity => true;
        public override void SetDefaults(Projectile projectile)
        {
            if(isInvaderProjectile)
            {
                projectile.friendly = true;
            }
        }
        public override bool? CanHitNPC(Projectile projectile, NPC target)
        {
            if(isInvaderProjectile)
            {
                return target.GetGlobalNPC<FortressNPCGeneral>().fortressNPC ? null : false;
            }
            return null;
        }
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if(isInvaderProjectile)
            {
                damage *= 4;
            }
        }
        public static Entity FindTarget(Projectile projectile, float maxRange)
        {
            Entity target = null;
            for(int i =0; i <255; i++)
            {
                if(Main.player[i].active && (Main.player[i].Center - projectile.Center).Length() - Main.player[i].aggro < maxRange )
                {
                    target = Main.player[i];
                    maxRange = (Main.player[i].Center - projectile.Center).Length() - Main.player[i].aggro;
                }
            }
            NPC npcTarget = null;
            if(QwertyMethods.ClosestNPC(ref npcTarget, maxRange, projectile.Center, false, -1, delegate (NPC possibleTarget) { return possibleTarget.GetGlobalNPC<FortressNPCGeneral>().fortressNPC; }))
            {
                return npcTarget;
            }
            return target;
        }
    }
}