using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Content.Buffs;
using QwertyMod.Content.Dusts;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;



namespace QwertyMod.Content.NPCs.Bosses.InvaderBattleship
{
    public class NoehtnapSpells : ModSystem
    {
        private const float greaterPupilRadius = 18;
        private const float lesserPupilRadius = 6;
        private static float pulseCounter = 0f;
        public static float scale = 1f;
        public override void PreUpdateWorld()
        {
            pulseCounter += MathF.PI / 30;
            scale = 1f + .05f * MathF.Sin(pulseCounter);
        }
        public static void DrawBody(SpriteBatch spriteBatch, Vector2 drawHere, Color drawColor, float pupilDirection, float pupilStareOutAmount, bool passenger = false)
        {
            if(!passenger)
            {
                Texture2D front = ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/InvaderNoehtnap_Back").Value;
                spriteBatch.Draw(front, drawHere,
                    null, drawColor, 0,
                    front.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
            }
            if(passenger)
            {
                Texture2D texture = ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/PassengerNoehtnap").Value;
                spriteBatch.Draw(texture, drawHere,
                    null, drawColor, 0,
                    texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
                
            }
            else
            {
                Texture2D texture = ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/InvaderNoehtnap").Value;
                spriteBatch.Draw(texture, drawHere,
                    new Rectangle(0, 0, texture.Width, texture.Height / 5), drawColor, 0,
                    new Vector2(texture.Width, texture.Height / 5) * 0.5f, scale, SpriteEffects.None, 0f);
            }
            Vector2 pupilOffset = PupilPosition(pupilDirection, pupilStareOutAmount);
            Texture2D Pupil = ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/CloakedDarkBoss/Pupil").Value;
            spriteBatch.Draw(Pupil, drawHere + pupilOffset,
                    Pupil.Frame(), drawColor, 0,
                    Pupil.Size() * .5f, scale, SpriteEffects.None, 0f);
            
            Texture2D Monocol = ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/NoehtnapMonocol").Value;
            Rectangle monocolFrame = new Rectangle(Monocol.Width / 2 - 47 - (int)pupilOffset.X, 0, 94, 32);
            spriteBatch.Draw(Monocol, drawHere + pupilOffset,
                    monocolFrame, drawColor, 0,
                    new Vector2(47 + (int)pupilOffset.X, Monocol.Height / 2),  1f, SpriteEffects.None, 0f);
            if(!passenger)
            {
                Texture2D front = ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/InvaderNoehtnap_Front").Value;
                spriteBatch.Draw(front, drawHere,
                    null, drawColor, 0,
                    front.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
            }
            Texture2D MonocolGlow = ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/NoehtnapMonocol_Glow").Value;
            spriteBatch.Draw(MonocolGlow, drawHere + pupilOffset,
                    monocolFrame, Color.White, 0,
                    new Vector2(47 + (int)pupilOffset.X, Monocol.Height / 2),  1f, SpriteEffects.None, 0f);
        }
        public static Vector2 PupilPosition(float pupilDirection, float pupilStareOutAmount)
        {
            return new Vector2(MathF.Cos(pupilDirection) * greaterPupilRadius * pupilStareOutAmount, MathF.Sin(pupilDirection) * lesserPupilRadius * pupilStareOutAmount) * scale;
        }
        public static int Start(Spell spell)
        {
            switch(spell)
            {
                case Spell.DistruptGravity:
                return 120;
                case Spell.DistruptVision:
                return 120;
                case Spell.DistruptCamera:
                return 120;
                case Spell.AimedShot:
                return 120;
                case Spell.Dizy:
                return 240;
            }
            return 0;
        }
        public static Player FindTarget(Vector2 position)
        {
            Player target = null;
            float maxRange = 10000;
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                if (Main.player[i].active && (Main.player[i].Center - position).Length() - Main.player[i].aggro < maxRange)
                {
                    target = Main.player[i];
                    maxRange = (Main.player[i].Center - position).Length() - Main.player[i].aggro;
                }
            }
            return target;
        }
        public static float PlayerDirection(Vector2 position, Player player)
        {
            return (player.Center - position).ToRotation();
        }
        public static void UpdateSpell(IEntitySource source, Spell spell, Vector2 position, int time, out float pupilDirection, out float pupilStareOutAmount)
        {
            pupilDirection = 0;
            pupilStareOutAmount = 0;
            int maxTime = 120;
            Player player = FindTarget(position);
            switch(spell)
            {
                case Spell.DistruptGravity:
                int part = time % 20;
                pupilDirection = MathF.PI / 2;
                pupilStareOutAmount = (float)part/ 20;
                Dust d = Dust.NewDustPerfect(position + PupilPosition(pupilDirection, pupilStareOutAmount), ModContent.DustType<InvaderGlow>(), Vector2.UnitX * 3 * (time % 2 == 0 ? -1 : 1));
                d.noGravity = true;
                d.frame.Y = 0;
                d.scale *= 2;
                if(time == 1)
                {
                    InflictAllPlayers(ModContent.BuffType<GravityFlipped>());
                }
                return;
                case Spell.DistruptVision:
                pupilDirection = MathF.PI * 2 * ((float)time / maxTime);
                pupilStareOutAmount = 0.8f;
                int amt = 4;
                if(time == 1)
                {
                    InflictAllPlayers(ModContent.BuffType<Darkness>());
                    amt = 20;
                }
                for(int i = 0; i < amt; i++)
                {
                    Dust d2 = Dust.NewDustPerfect(position + PupilPosition(pupilDirection, pupilStareOutAmount), ModContent.DustType<DarknessDust>(), QwertyMethods.PolarVector(time == 1 ? 10 : 3, MathF.PI * 2 * ((float)i / amt)));
                    d2.noGravity = true;
                    d2.frame.Y = 0;
                    d2.scale *= 2;
                }
                return;
                case Spell.DistruptCamera:
                pupilDirection = MathF.PI * -10 * ((float)time / maxTime);
                pupilStareOutAmount = 0.8f;
                if(time == 1)
                {
                    InflictAllPlayers(ModContent.BuffType<CameraIssues>());
                }
                return;
                case Spell.AimedShot:
                pupilStareOutAmount = (1f - ((time % 90) / 90f)) * 0.8f;
                pupilDirection = QwertyMethods.PredictiveAim(position, 4.5f * 3, player.Center, player.velocity);
                if(float.IsNaN(pupilDirection))
                {
                    pupilDirection = PlayerDirection(position, player);
                }
                if(time % 90 == 89)
                {
                    SoundEngine.PlaySound(new SoundStyle("QwertyMod/Assets/Sounds/invbattleship_turret"), position);
                    if(Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(source, position + PupilPosition(pupilDirection, pupilStareOutAmount), QwertyMethods.PolarVector(4.5f, pupilDirection), ModContent.ProjectileType<InvaderRay>(), Main.expertMode ? InvaderBattleship.expertDamage : InvaderBattleship.normalDamage, 0);
                    }
                }
                return;
                case Spell.Dizy:
                pupilDirection = MathF.PI * 6 * ((float)time / maxTime);
                pupilStareOutAmount = 1f;
                return;
            }
        }
        static void InflictAllPlayers(int buffID)
        {

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player allPlayer = Main.player[i];
                if (allPlayer.active && !allPlayer.dead)
                {
                    allPlayer.AddBuff(buffID, 10 * 60);
                }
            }
        }
        public static void SpellEnd(Spell spell)
        {
            switch(spell)
            {
                case Spell.DistruptGravity:
                return;
                case Spell.DistruptVision:
                return;
                case Spell.DistruptCamera:
                return;
            }
        }
        
    }
    public enum Spell : byte
    {
        DistruptGravity,
        DistruptVision,
        DistruptCamera,
        AimedShot,
        Dizy
    }
    
}