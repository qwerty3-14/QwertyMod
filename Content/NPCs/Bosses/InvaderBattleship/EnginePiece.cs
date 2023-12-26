using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Terraria.Audio;
using QwertyMod.Content.NPCs.Invader;



namespace QwertyMod.Content.NPCs.Bosses.InvaderBattleship
{
    public class EnginePiece : ModNPC
    {
        public override void SetDefaults()
        {
            NPC.lifeMax = 20000;
            NPC.width = 127;
            NPC.height = 88;
            NPC.damage = 172;
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
        bool runOnce = true;
        int engineTimer = 0;

        Vector2[] ExhaustOffsets = new Vector2[] 
        {
            new Vector2(1, 13),
            new Vector2(5, 34),
            new Vector2(9, 54),
            new Vector2(13, 74)
        };
        public override void AI()
        {
            NPC.TargetClosest(false);
            if(runOnce)
            {
                runOnce = false;
                NPC.velocity = Vector2.UnitX * MathF.Sign(NPC.Center.X - Main.player[NPC.target].Center.X) * -16;
                SoundEngine.PlaySound(new SoundStyle("QwertyMod/Assets/Sounds/invbattleship_warp"), NPC.Center);
            }
            NPC.direction = MathF.Sign(NPC.velocity.X);
            float relXPos = (NPC.Center.X - Main.player[NPC.target].Center.X) * NPC.direction;
            engineTimer++;
            if(engineTimer % 5 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                for(int i =0; i < 4; i++)
                {
                    Vector2 offset = ExhaustOffsets[i];
                    if(NPC.direction == -1)
                    {
                        offset.X = NPC.width - offset.X;
                    }
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.position + offset, Vector2.Zero, ModContent.ProjectileType<InvaderExhaust>(), 0, 0);
                }
            }
            if(relXPos > 1800)
            {
                NPC.ai[1] = 2;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D fragment = ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/BattleshipDebris_Engine").Value;
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