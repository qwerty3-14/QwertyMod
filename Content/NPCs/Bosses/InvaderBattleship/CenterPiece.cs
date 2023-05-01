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
    public class CenterPiece : ModNPC
    {
        public override void SetDefaults()
        {
            NPC.lifeMax = 10000;
            NPC.width = 132;
            NPC.height = 90;
            NPC.damage = 200;
            NPC.noGravity = true;
            NPC.knockBackResist = 0;
            NPC.HitSound = new SoundStyle("QwertyMod/Assets/Sounds/invbattleship_hurt1");
            NPC.DeathSound = new SoundStyle("QwertyMod/Assets/Sounds/invbattleship_hurt1");
            NPC.noTileCollide = true;
            NPC.aiStyle = -1;
            NPC.GetGlobalNPC<InvaderNPCGeneral>().invaderNPC = true;
            
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.damage = NPC.damage / 2;
        }
        public override bool CheckActive()
        {
            return false;
        }
        bool runOnce = true;
        int shotTimer = 0;
        int[] beamIndexes = new int[2];
        
        public override void AI()
        {
            NPC.TargetClosest(false);
            if(runOnce)
            {
                runOnce = false;
                NPC.direction = 1;
            }
            NPC.velocity = Vector2.UnitY * -4f;
            for(int i = 0; i < 2; i++)
            {
                float emitY = NPC.position.Y + 39;
                float emitX = NPC.position.X + (i == 0 ? 0 : NPC.width);
                Vector2 beamPosition = new Vector2(emitX, emitY) + NPC.velocity;
                
                if(beamIndexes[i] == -1)
                {
                    beamIndexes[i] = Projectile.NewProjectile(NPC.GetSource_FromAI(), beamPosition, Vector2.UnitX * (i == 0 ? -1 : 1), ModContent.ProjectileType<FlybyBeam>(), 2 * (Main.expertMode ? InvaderBattleship.expertDamage : InvaderBattleship.normalDamage), 0);
                }
                if(beamIndexes[i] != -1 && Main.projectile[beamIndexes[i]].timeLeft <= FlybyBeam.openTime)
                {
                    Main.projectile[beamIndexes[i]].timeLeft = FlybyBeam.openTime;
                }
                if(beamIndexes[i] != -1 && (!Main.projectile[beamIndexes[i]].active || Main.projectile[beamIndexes[i]].type != ModContent.ProjectileType<FlybyBeam>()))
                {
                    beamIndexes[i] = -1;
                }
                else if(beamIndexes[i] != -1)
                {
                    Main.projectile[beamIndexes[i]].Center = beamPosition;
                }
            }
            if((NPC.Center.Y - Main.player[NPC.target].Center.Y) < -1000)
            {
                NPC.ai[1] = 2;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D hauler = Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/DebrisHauler").Value;
            Texture2D fragment = Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/CenterPiece").Value;

            spriteBatch.Draw(hauler, NPC.Center - screenPos, null, drawColor, NPC.rotation, new Vector2(68, 72), Vector2.One, NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            spriteBatch.Draw(fragment, NPC.Center - screenPos, null, drawColor, NPC.rotation, fragment.Size() * 0.5f, Vector2.One, NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            return false;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(NPC.lifeMax);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NPC.lifeMax = reader.ReadInt32();
        }
    }
}