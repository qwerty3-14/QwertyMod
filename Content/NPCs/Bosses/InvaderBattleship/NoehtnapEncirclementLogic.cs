using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.Audio;
using QwertyMod.Content.Buffs;
using QwertyMod.Content.Dusts;
using QwertyMod.Common.Fortress;
using QwertyMod.Content.NPCs.Invader;


namespace QwertyMod.Content.NPCs.Bosses.InvaderBattleship
{
    public partial class InvaderNoehtnap : ModNPC
    {
        int encirclementTimer = 0;
        bool doingEncirclement = false;
        NPC[] clones = new NPC[5];
        Vector2 encirclementCenter;
        int timeToCloseIn = 600;
        void EncirclementLogic()
        {
            Player player = Main.player[NPC.target];
            encirclementTimer++;
            float towardCenter = (encirclementCenter - NPC.Center).ToRotation();
            if(encirclementTimer < timeToCloseIn)
            {
                NPC.velocity = QwertyMethods.PolarVector(6f , towardCenter - MathF.PI / 2f) + QwertyMethods.PolarVector((800f / timeToCloseIn), towardCenter);
                NoehtnapSpells.UpdateSpell(NPC.GetSource_FromAI(), Spell.AimedShot, NPC.Center, timeToCloseIn - encirclementTimer, out pupilDirection, out pupilStareOutAmount);
                //if(Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for(int i = 0; i < clones.Length; i++)
                    {
                        float rot = (encirclementCenter - clones[i].Center).ToRotation();
                        clones[i].velocity = QwertyMethods.PolarVector(6f , rot - MathF.PI / 2f) + QwertyMethods.PolarVector((800f / timeToCloseIn), rot);

                        rot = MathF.PI * 2f * ((i + 1) / 6f) + (NPC.Center - encirclementCenter ).ToRotation();
                        clones[i].Center = QwertyMethods.PolarVector((encirclementCenter - NPC.Center).Length(), rot) + encirclementCenter;
                    }
                }
            }
            else
            {
                NPC.velocity = (encirclementCenter - NPC.Center) * 0.1f;
                //if(Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for(int i = 0; i < clones.Length; i++)
                    {
                        clones[i].velocity = (encirclementCenter - clones[i].Center) * 0.1f;
                    }
                }
                if(encirclementTimer == timeToCloseIn + 20)
                {
                    Vector2 unitV = player.velocity.SafeNormalize(-Vector2.UnitY);
                    unitV.X *= 2;
                    teleHere = player.Center + unitV * 400;
                    NPC.netUpdate = true;
                }
                if(encirclementTimer >= timeToCloseIn + 24)
                {
                    timer = timeToTele + 1;
                    encirclementTimer = 0;
                    NPC.velocity = Vector2.Zero;
                    //if(Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        for(int i = 0; i < clones.Length; i++)
                        {
                            clones[i].active = false;
                            clones[i] = null;
                        }
                    }
                }
            }
        }
    }
}