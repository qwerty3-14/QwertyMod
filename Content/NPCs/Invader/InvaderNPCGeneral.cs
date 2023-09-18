using Microsoft.Xna.Framework;
using QwertyMod.Content.Dusts;
using QwertyMod.Content.NPCs.Fortress;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Terraria.ID;
using Terraria.Audio;
using QwertyMod.Common;

namespace QwertyMod.Content.NPCs.Invader
{
    public class InvaderNPCGeneral : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public bool invaderNPC = false;
        public bool contactDamageToNatives = false;
        public static Entity FindTarget(NPC npc, bool allowFlip = false)
        {
            Entity target = null;
            float maxDist = 10000;
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                if (Main.player[i].active && (Main.player[i].Center - npc.Center).Length() - Main.player[i].aggro < maxDist && !Main.player[i].GetModPlayer<CommonStats>().InvaderFiendly)
                {
                    target = Main.player[i];
                    npc.target = i;
                    maxDist = (Main.player[npc.target].Center - npc.Center).Length() - Main.player[npc.target].aggro;
                }
            }
            NPC npcTarget = null;
            if (QwertyMethods.ClosestNPC(ref npcTarget, maxDist, npc.Center, false, -1, delegate (NPC possibleTarget) { return possibleTarget.GetGlobalNPC<FortressNPCGeneral>().fortressNPC; }))
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
        public static void SpawnIn(NPC npc)
        {
            Point origin = npc.Bottom.ToTileCoordinates();
            for(int i = 0; i < 4000; i++)
            {
                if(WorldUtils.Find(origin, Searches.Chain(new Searches.Down(2), new GenCondition[]
                {
                                            new Terraria.WorldBuilding.Conditions.IsSolid()
                }), out _))
                {
                    break;
                }
                npc.position.Y++;
                origin = npc.Bottom.ToTileCoordinates();
            }
            SoundEngine.PlaySound(new SoundStyle("QwertyMod/Assets/Sounds/ZoneIn"), npc.Center);
        }

        public static void SpawnAnimation(NPC npc)
        {
            int width = npc.width / 2;
            int height = npc.height / 4;
            int dustCount = width;
            for (int i = 0; i < dustCount; i++)
            {
                float rot = MathF.PI * 2f * ((float)i / dustCount);
                Vector2 unitVector = QwertyMethods.PolarVector(1f, rot);
                Dust d = Dust.NewDustPerfect(npc.Top + new Vector2(unitVector.X * width, unitVector.Y * height), ModContent.DustType<InvaderGlow>(), Vector2.UnitY * npc.height * 0.09f);
                d.noGravity = true;
                d.frame.Y = 0;
                d.scale *= 2;
            }
        }
        public static void SpawnOut(NPC npc)
        {
            npc.GetGlobalNPC<InvaderNPCGeneral>().invaderNPC = false;
            npc.life = 0;
            npc.active = false;

            int width = npc.width / 2;
            int height = npc.height / 4;
            int dustCount = width;
            for (int i = 0; i < dustCount; i++)
            {
                float rot = MathF.PI * 2f * ((float)i / dustCount);
                Vector2 unitVector = QwertyMethods.PolarVector(1f, rot);
                Dust d = Dust.NewDustPerfect(npc.Bottom + new Vector2(unitVector.X * width, unitVector.Y * height), ModContent.DustType<InvaderGlow>(), Vector2.UnitY * npc.height * -0.09f);
                d.noGravity = true;
                d.frame.Y = 0;
                d.scale *= 2;
            }
            SoundEngine.PlaySound(new SoundStyle("QwertyMod/Assets/Sounds/ZoneOut"), npc.Center);
        }
        public override bool PreAI(NPC npc)
        {
            if(invaderNPC && npc.ai[3] > 0)
            {
                npc.noTileCollide = true;
                npc.ai[3]--;
                if(npc.ai[3] == 0 && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    npc.StrikeInstantKill();
                }
                return false;
            }
            return true;
        }
        int idleWalkTimer = 0;
        public static void WalkerIdle(NPC npc, float speed)
        {
            if (npc.direction == 0)
            {
                npc.direction = 1;
            }
            npc.GetGlobalNPC<InvaderNPCGeneral>().idleWalkTimer--;
            if (npc.GetGlobalNPC<InvaderNPCGeneral>().idleWalkTimer <= -60)
            {
                npc.GetGlobalNPC<InvaderNPCGeneral>().idleWalkTimer = 120;
            }
            if (npc.GetGlobalNPC<InvaderNPCGeneral>().idleWalkTimer > 0)
            {

                if (npc.collideX)
                {
                    npc.direction *= -1;
                }

                if (!WalkerWalk(npc, speed))
                {
                    npc.direction *= -1;
                }
            }
        }
        public static bool WalkerWalk(NPC npc, float speed)
        {
            Point checkHere = (npc.Bottom + Vector2.UnitY * 8 + Vector2.UnitX * npc.direction * 16).ToTileCoordinates();
            if (!Main.tile[checkHere.X, checkHere.Y].HasTile && !Main.tile[checkHere.X, checkHere.Y + 1].HasTile)
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
            if (contactDamageToNatives)
            {
                if (contactDamageCooldown <= 0)
                {
                    for (int i = 0; i < 200; i++)
                    {
                        if (Main.npc[i].TryGetGlobalNPC<FortressNPCGeneral>(out FortressNPCGeneral gNPC))
                        {
                            if (gNPC.fortressNPC && Collision.CheckAABBvAABBCollision(npc.position, npc.Size, Main.npc[i].position, Main.npc[i].Size))
                            {
                                NPC.HitInfo hitInfo = new NPC.HitInfo();
                                hitInfo.Damage = (int)(npc.damage);
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
    public class InvaderProjectile : GlobalProjectile
    {
        public bool isInvaderProjectile;
        public float EvEMultiplier = 1f;
        public override bool InstancePerEntity => true;
        public override bool CanHitPlayer(Projectile projectile, Player target)
        {
            if(isInvaderProjectile && target.GetModPlayer<CommonStats>().InvaderFiendly)
            {
                return false;
            }
            return true;
        }
        public override void SetDefaults(Projectile projectile)
        {
            if (isInvaderProjectile)
            {
                projectile.friendly = true;
                projectile.npcProj = true;
            }
        }
        public override bool? CanHitNPC(Projectile projectile, NPC target)
        {
            if (isInvaderProjectile)
            {
                return target.GetGlobalNPC<FortressNPCGeneral>().fortressNPC ? null : false;
            }
            return null;
        }
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (isInvaderProjectile)
            {
                modifiers.FinalDamage *= 4 * EvEMultiplier;
            }
        }
        public static Entity FindTarget(Projectile projectile, float maxRange)
        {
            Entity target = null;
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                if (Main.player[i].active && (Main.player[i].Center - projectile.Center).Length() - Main.player[i].aggro < maxRange && !Main.player[i].GetModPlayer<CommonStats>().InvaderFiendly)
                {
                    target = Main.player[i];
                    maxRange = (Main.player[i].Center - projectile.Center).Length() - Main.player[i].aggro;
                }
            }
            NPC npcTarget = null;
            if (QwertyMethods.ClosestNPC(ref npcTarget, maxRange, projectile.Center, false, -1, delegate (NPC possibleTarget) { return possibleTarget.GetGlobalNPC<FortressNPCGeneral>().fortressNPC; }))
            {
                return npcTarget;
            }
            return target;
        }
    }
}