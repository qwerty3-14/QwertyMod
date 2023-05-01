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
    public class MissileLauncherPiece : ModNPC
    {
        public override void SetDefaults()
        {
            NPC.lifeMax = 10000;
            NPC.width = 61;
            NPC.height = 22;
            NPC.damage = 10;
            NPC.noGravity = true;
            NPC.knockBackResist = 0;
            NPC.HitSound = new SoundStyle("QwertyMod/Assets/Sounds/invbattleship_hurt1");
            NPC.DeathSound = new SoundStyle("QwertyMod/Assets/Sounds/invbattleship_hurt1");
            NPC.noTileCollide = true;
            NPC.aiStyle = -1;
            NPC.GetGlobalNPC<InvaderNPCGeneral>().invaderNPC = true;
        }
        BattleshipMissileLauncher launcher;
        bool runOnce = true;
        int shotTimer = 0;
        public override void AI()
        {
            NPC.damage = 0;
            NPC.TargetClosest(false);
            if(runOnce)
            {
                runOnce = false;
                launcher = new BattleshipMissileLauncher(NPC, new Vector2(20, 11) - NPC.Size * 0.5f);
                NPC.direction = 1;
                NPC.velocity = new Vector2(7 * MathF.Sign(Main.player[NPC.target].Center.X - NPC.Center.X), -14);
            }
            NPC.velocity.Y += 0.1f;
            NPC.rotation += MathF.PI / 20f;
            if(launcher != null)
            {
                launcher.UpdateRelativePosition();
                if(!launcher.HasFired())
                {
                    if(launcher.AimRelative(MathF.PI / 4f) && ((Collision.CanHitLine(launcher.AbsoluteShootPosition(), 1, 1, Main.player[NPC.target].Center, 1, 1) && (Main.player[NPC.target].Center - NPC.Center).Length() < 800)))
                    {
                        launcher.Fire();
                    }
                }
                else
                {
                    if(launcher.AimHome())
                    {
                        launcher.Reload();
                    }
                }
            }
            if(NPC.velocity.Y > 0 && (NPC.Center.Y - Main.player[NPC.target].Center.Y) > 1000)
            {
                NPC.ai[1] = 2;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D fragment = Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/BattleshipDebris_Launcher").Value;
            if(launcher != null)
            {
                launcher.Draw(spriteBatch, screenPos, drawColor);
            }
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