using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertyMod.Content.NPCs.Bosses.InvaderBattleship
{
    public partial class InvaderNoehtnap : ModNPC
    {
        int armBeamAttackCounter = -1;
        float armsRotated = 0;
        float armOutAmount = 0;
        Vector2[] armPos = new Vector2[4];
        int beamSweepCount = 0;
        int[] beamIndexes = new int[] {-1, -1, -1, -1};
        void BeamLogic(Player player)
        {
            int startTime = 180;
            int startSpin = 30;
            float windUpTime = 60f;
            int spinTime = 60;
            NoehtnapSpells.UpdateSpell(NPC.GetSource_FromAI(), Spell.AimedShot, NPC.Center, startTime + startSpin + (int)windUpTime * 2 + spinTime - armBeamAttackCounter, out pupilDirection, out pupilStareOutAmount);
            
            armBeamAttackCounter++;
            if(armBeamAttackCounter < startTime)
            {
                if(beamSweepCount > 1)
                {
                    armBeamAttackCounter = 0;
                    if(armOutAmount <= 0)
                    {
                        timer++;
                    }
                    else
                    {
                        armOutAmount -= 10f;
                    }
                }
                else if(beamSweepCount > 0)
                {
                    float stretchTo = (player.Center - NPC.Center).Length() - 10;
                    if(Math.Abs(stretchTo - armOutAmount) < 10f)
                    {
                        armOutAmount = stretchTo;
                        armBeamAttackCounter = startTime;
                    }
                    else
                    {
                        armOutAmount += 10f * MathF.Sign(stretchTo - armOutAmount);
                    }
                }
                else if(armOutAmount < 100)
                {
                    armOutAmount++;
                }
            }
            for(int i = 0; i < 4; i++)
            {
                float rot = armsRotated + MathF.PI / 4f + i * MathF.PI / 2f;
                armPos[i] = NPC.Center + QwertyMethods.PolarVector(armOutAmount, rot);
                if(armBeamAttackCounter >= startTime)
                {
                    Vector2 beamPosition = armPos[i] + QwertyMethods.PolarVector(20, rot);
                    if(beamIndexes[i] != -1)
                    {
                        Projectile beam = Main.projectile.FirstOrDefault(x => x.identity == beamIndexes[i]);
                        if(beam != null && beam.timeLeft <= FlybyBeam.openTime && armBeamAttackCounter < startTime + startSpin + windUpTime + spinTime)
                        {
                            beam.timeLeft = FlybyBeam.openTime;
                        }
                        if(beam != null && (!beam.active || beam.type != ModContent.ProjectileType<FlybyBeam>()))
                        {
                            beamIndexes[i] = -1;
                        }
                        else if(beam != null)
                        {
                            beam.ai[0] = 0;
                            beam.Center = beamPosition;
                            beam.rotation = rot;
                        }
                    }
                    if(armBeamAttackCounter < startTime + startSpin + windUpTime + spinTime)
                    {
                        if(beamIndexes[i] == -1 && Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            beamIndexes[i] = Projectile.NewProjectile(NPC.GetSource_FromAI(), beamPosition, QwertyMethods.PolarVector(1, rot), ModContent.ProjectileType<FlybyBeam>(), 2 * (Main.expertMode ? InvaderBattleship.expertDamage : InvaderBattleship.normalDamage), 0);
                            beamIndexes[i] = Main.projectile[beamIndexes[i]].identity;
                            
                            NPC.netUpdate = true;
                        }
                    }
                }
            }
            if(armBeamAttackCounter > startTime + startSpin + windUpTime * 2 + spinTime)
            {
                armBeamAttackCounter = 0;
                beamSweepCount++;
            }
            else if(armBeamAttackCounter > startTime + startSpin)
            {
                float rotSpeed = MathF.Min((armBeamAttackCounter - (startTime + startSpin)) / windUpTime, 1f);
                if(armBeamAttackCounter > startTime + startSpin + windUpTime + spinTime)
                {
                    rotSpeed = MathF.Min(1f - (armBeamAttackCounter - (startTime + startSpin + windUpTime + spinTime)) / windUpTime, 1f);
                }
                armsRotated += rotSpeed * MathF.PI / 60f;
            }
            else
            {

            }
        }
    }
}