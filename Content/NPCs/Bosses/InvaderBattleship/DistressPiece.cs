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
    public class DistressPiece : ModNPC
    {
        public override void SetDefaults()
        {
            NPC.lifeMax = 6000;
            NPC.width = 22;
            NPC.height = 40;
            NPC.damage = 50;
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
        int distressFrame = 0;
        int distressCounter = 0;
        public override void AI()
        {
            NPC.velocity.Y += 0.075f;
            NPC.rotation += MathF.PI / 120f;
            distressCounter++;
            distressFrame = ((distressCounter / 6) % 4) + 1;
            NPC.TargetClosest(false);
            if(distressCounter == 60)
            {
                SoundEngine.PlaySound(SoundID.Item6, NPC.Center);
                for(int i =0; i < 2; i++)
                {
                    Vector2 spawnHere = Main.player[NPC.target].Center + QwertyMethods.PolarVector(Main.rand.NextFloat() * 700, Main.rand.NextFloat() * MathF.PI * 2f);
                    NPC.NewNPC(NPC.GetSource_FromAI(), (int)spawnHere.X, (int)spawnHere.Y, ModContent.NPCType<Invader.InvaderCaster>());
                }
                if(Main.rand.NextBool(2))
                {
                    Vector2 spawnHere = Main.player[NPC.target].Center;
                    NPC.NewNPC(NPC.GetSource_FromAI(), (int)spawnHere.X, (int)spawnHere.Y, ModContent.NPCType<Invader.InvaderBehemoth>());
                }
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D fragment = Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/InvaderBattleship_Distress").Value;
            spriteBatch.Draw(fragment, NPC.Center - screenPos, new Rectangle(0, distressFrame * 40, 22, 40), drawColor, NPC.rotation, new Vector2(11, 20), Vector2.One, NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

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