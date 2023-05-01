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
    public static class CommonGunPiece
    {
        public const int baseShotCooldown = 120;
        public static void AI(NPC NPC, BattleshipTurret turret, ref int shotTimer, bool backGun = false)
        {
            NPC.damage = 0;
            if(turret != null)
            {
                NPC.TargetClosest(false); 
                Player aimAt = Main.player[NPC.target];
                int yHere = -600 + shotTimer * 4;
                if(backGun)
                {
                    yHere = 600 - shotTimer * 4;
                }
                Vector2 goHere = aimAt.Center + new Vector2(MathF.Sign(NPC.Center.X - aimAt.Center.X) * 600, yHere);
                turret.UpdateRelativePosition();
                float shootAngle = QwertyMethods.PredictiveAimWithOffset(turret.AbsolutePosition(), 4.5f * 3, aimAt.Center, aimAt.velocity, 9);
                if(float.NaN != shootAngle)
                {
                    turret.AimAt(shootAngle);
                }
                else
                {
                    turret.AimHome();
                }
                shotTimer++;
                int turretLightCount = (int)(shotTimer % baseShotCooldown) / (baseShotCooldown / 6);
                turret.SetLights(turretLightCount);
                if(shotTimer % baseShotCooldown == 0)
                {
                    turret.Fire();
                }
                if(shotTimer > 300)
                {
                    goHere = aimAt.Center + new Vector2(MathF.Sign(NPC.Center.X - aimAt.Center.X) * 1400, -600 + shotTimer);
                    NPC.ai[1] = 2;
                }
                float scaler = 0.03f;
                if((aimAt.Center - NPC.Center).Length() < 1200)
                {
                    scaler *= ((aimAt.Center - NPC.Center).Length() / 1200f);
                }
                NPC.velocity = (goHere - NPC.Center) * scaler;
                float rotmultiplier = NPC.velocity.X;
                if(Math.Abs(rotmultiplier) > 5)
                {
                    rotmultiplier = Math.Sign(rotmultiplier) * 5f;
                }
                NPC.rotation = rotmultiplier * (float)Math.PI / 20f;

            }
        }
    }
    public class FrontGunPiece : ModNPC
    {
        public override void SetDefaults()
        {
            NPC.lifeMax = 20000;
            NPC.width = 98;
            NPC.height = 92;
            NPC.damage = 200;
            NPC.noGravity = true;
            NPC.knockBackResist = 0;
            NPC.HitSound = new SoundStyle("QwertyMod/Assets/Sounds/invbattleship_hurt1");
            NPC.DeathSound = new SoundStyle("QwertyMod/Assets/Sounds/invbattleship_hurt1");
            NPC.noTileCollide = true;
            NPC.aiStyle = -1;
            NPC.GetGlobalNPC<InvaderNPCGeneral>().invaderNPC = true;
            
        }
        public override bool CheckActive()
        {
            return false;
        }
        BattleshipTurret turret;
        bool runOnce = true;
        int shotTimer = 0;
        
        public override void AI()
        {
            
            if(runOnce)
            {
                runOnce = false;
                turret = new BattleshipTurret(NPC, new Vector2(57, 39) - NPC.Size * 0.5f);
                NPC.direction = 1;
            }
            CommonGunPiece.AI(NPC, turret, ref shotTimer);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D hauler = Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/DebrisHauler").Value;
            Texture2D fragment = Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/BattleshipDebris_Front").Value;

            spriteBatch.Draw(hauler, NPC.Center - screenPos, null, drawColor, NPC.rotation, new Vector2(68, 72), Vector2.One, NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            spriteBatch.Draw(fragment, NPC.Center - screenPos, null, drawColor, NPC.rotation, fragment.Size() * 0.5f, Vector2.One, NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

            if(turret != null)
            {
                turret.Draw(spriteBatch, screenPos, drawColor);
            }
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
    public class BackGunPiece : ModNPC
    {
        public override void SetDefaults()
        {
            NPC.lifeMax = 20000;
            NPC.width = 88;
            NPC.height = 74;
            NPC.damage = 200;
            NPC.noGravity = true;
            NPC.knockBackResist = 0;
            NPC.HitSound = new SoundStyle("QwertyMod/Assets/Sounds/invbattleship_hurt1");
            NPC.DeathSound = new SoundStyle("QwertyMod/Assets/Sounds/invbattleship_hurt1");
            NPC.noTileCollide = true;
            NPC.aiStyle = -1;
            NPC.GetGlobalNPC<InvaderNPCGeneral>().invaderNPC = true;
            
        }
        public override bool CheckActive()
        {
            return false;
        }
        BattleshipTurret turret;
        bool runOnce = true;
        int shotTimer = 0;
        
        public override void AI()
        {
            if(runOnce)
            {
                runOnce = false;
                turret = new BattleshipTurret(NPC, new Vector2(47, 39) - NPC.Size * 0.5f);
                NPC.direction = 1;
            }
            CommonGunPiece.AI(NPC, turret, ref shotTimer, true);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D hauler = Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/DebrisHauler").Value;
            Texture2D fragment = Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/BattleshipDebris_Back").Value;

            spriteBatch.Draw(hauler, NPC.Center - screenPos, null, drawColor, NPC.rotation, new Vector2(68, 72), Vector2.One, NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            spriteBatch.Draw(fragment, NPC.Center - screenPos, null, drawColor, NPC.rotation, fragment.Size() * 0.5f, Vector2.One, NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

            if(turret != null)
            {
                turret.Draw(spriteBatch, screenPos, drawColor);
            }
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