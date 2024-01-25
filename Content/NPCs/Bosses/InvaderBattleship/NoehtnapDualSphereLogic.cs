using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using QwertyMod.Content.Dusts;


namespace QwertyMod.Content.NPCs.Bosses.InvaderBattleship
{
    public partial class InvaderNoehtnap : ModNPC
    {
        int dualSphereTimer = 0;
        int timeToSplit = 70;
        int timeToMorph = 30;
        Projectile etimsSphere;
        Projectile railSphere;
        void DualSphereLogic()
        {
            if(activeSpellCountdown > 0)
            {
                NPC.Center = teleHere;
                if(activeSpellCountdown == 230)
                {
                    int amt = 240; 
                    SoundEngine.PlaySound(SoundID.Item88, teleHere);
                    for(int i = 0; i < amt; i++)
                    {
                        Dust d2 = Dust.NewDustPerfect(teleHere, (Main.rand.NextBool() ? ModContent.DustType<InvaderGlow>(): ModContent.DustType<DarknessDust>()), QwertyMethods.PolarVector(Main.rand.NextFloat() * 10f, MathF.PI * 2 * ((float)i / amt)));
                        d2.noGravity = true;
                        d2.frame.Y = 0;
                        d2.scale *= 2;
                    }
                }
                dualSphereTimer = 0;
                NoehtnapSpells.UpdateSpell(NPC.GetSource_FromAI(), Spell.Dizy, NPC.Center, activeSpellCountdown, out pupilDirection, out pupilStareOutAmount);
                activeSpellCountdown--;
                if(activeSpellCountdown <= 0)
                {
                    activeSpellCountdown = 0;
                    timer = timeToTele + 1;
                }
            }
            else
            {
                NPC.dontTakeDamage = true;
                dualSphereTimer++;
                if(dualSphereTimer == timeToSplit + timeToMorph)
                {
                    Vector2 leftspawn = NPC.Center + new Vector2(-(80 + 52), 10);
                    Vector2 rightSpawn = NPC.Center + new Vector2((80 + 52), -10);
                    if(Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        etimsSphere = Main.projectile[Projectile.NewProjectile(NPC.GetSource_FromAI(), leftspawn, Vector2.UnitX * -1, ModContent.ProjectileType<EtimsSphere>(), (Main.expertMode ? InvaderBattleship.expertDamage : InvaderBattleship.normalDamage), 0)];
                        railSphere = Main.projectile[Projectile.NewProjectile(NPC.GetSource_FromAI(), rightSpawn, Vector2.Zero, ModContent.ProjectileType<RailSphere>(), (Main.expertMode ? InvaderBattleship.expertDamage : InvaderBattleship.normalDamage), 0)];
                    }
                }
                else if(dualSphereTimer > timeToSplit + timeToMorph)
                {
                    if(Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        if(etimsSphere.timeLeft <= 60 && railSphere.timeLeft <= 30)
                        {
                            if(railSphere.timeLeft == 30)
                            {
                                Player player = Main.player[NPC.target];
                                Vector2 unitV = player.velocity.SafeNormalize(-Vector2.UnitY);
                                unitV.X *= 2;
                                teleHere = player.Center + unitV * 400;
                                NPC.netUpdate = true;
                            }
                            if(railSphere.timeLeft <= 26)
                            {
                                dualSphereTimer = 0;
                                teleportframe = 20;
                                timer = timeToTele + 1;
                                etimsSphere = null;
                                railSphere = null;
                                NPC.netUpdate = true;
                            }
                            
                        }
                        else if(Collision.CheckAABBvAABBCollision(etimsSphere.position, etimsSphere.Size, railSphere.position, railSphere.Size))
                        {
                            activeSpellCountdown = NoehtnapSpells.Start(Spell.Dizy);
                            teleHere = (etimsSphere.Center + railSphere.Center) / 2f;
                            NPC.netUpdate = true;
                            etimsSphere.Kill();
                            railSphere.Kill();
                            
                        }
                    }
                }
            }
        }
    }
}