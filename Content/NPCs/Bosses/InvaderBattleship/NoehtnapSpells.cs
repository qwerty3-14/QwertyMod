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
using QwertyMod.Content.Dusts;
using Terraria.Audio;
using QwertyMod.Content.Buffs;


namespace QwertyMod.Content.NPCs.Bosses.InvaderBattleship
{
    public class NoehtnapSpells : ModSystem
    {
        private const float greaterPupilRadius = 18;
        private const float lesserPupilRadius = 6;
        private static float pulseCounter = 0f;
        private static float scale = 1f;
        public override void PreUpdateWorld()
        {
            pulseCounter += (float)Math.PI / 30;
            scale = 1f + .05f * (float)Math.Sin(pulseCounter);
        }
        public static void DrawBody(SpriteBatch spriteBatch, Vector2 drawHere, Color drawColor, float pupilDirection, float pupilStareOutAmount, bool passenger = false)
        {
            if(passenger)
            {
                Texture2D texture = Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/PassengerNoehtnap").Value;
                spriteBatch.Draw(texture, drawHere,
                    null, drawColor, 0,
                    texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
                
            }
            else
            {
                
            }
            Vector2 pupilOffset = new Vector2((float)Math.Cos(pupilDirection) * greaterPupilRadius * pupilStareOutAmount, (float)Math.Sin(pupilDirection) * lesserPupilRadius) * scale;
            Texture2D Pupil = Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/CloakedDarkBoss/Pupil").Value;
            spriteBatch.Draw(Pupil, drawHere + pupilOffset,
                    Pupil.Frame(), drawColor, 0,
                    Pupil.Size() * .5f, scale, SpriteEffects.None, 0f);
            
            Texture2D Monocol = Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/NoehtnapMonocol").Value;
            Rectangle monocolFrame = new Rectangle(Monocol.Width / 2 - 47 - (int)pupilOffset.X, 0, 94, 32);
            spriteBatch.Draw(Monocol, drawHere + pupilOffset,
                    monocolFrame, drawColor, 0,
                    new Vector2(47 + (int)pupilOffset.X, Monocol.Height / 2),  1f, SpriteEffects.None, 0f);
            Texture2D MonocolGlow = Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/NoehtnapMonocol_Glow").Value;
            spriteBatch.Draw(MonocolGlow, drawHere + pupilOffset,
                    monocolFrame, Color.White, 0,
                    new Vector2(47 + (int)pupilOffset.X, Monocol.Height / 2),  1f, SpriteEffects.None, 0f);
        }
        public static Vector2 PupilPosition(float pupilDirection, float pupilStareOutAmount)
        {
            return new Vector2((float)Math.Cos(pupilDirection) * greaterPupilRadius * pupilStareOutAmount, (float)Math.Sin(pupilDirection) * lesserPupilRadius) * scale;
        }
        public static int Start(Spell spell)
        {
            switch(spell)
            {
                case Spell.DistruptGravity:
                return 120;
                case Spell.DistruptVision:
                return 120;
                case Spell.DistruptControls:
                return 120;
                case Spell.DistruptCamera:
                return 120;
                case Spell.Beam:
                return 10 * 60;
                case Spell.AimedShots:
                return 10 * 60;
                case Spell.ShadowMissiles:
                return 10 * 60;
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
        public static void UpdateSpell(Spell spell, Vector2 position, int time, out float pupilDirection, out float pupilStareOutAmount)
        {
            pupilDirection = 0;
            pupilStareOutAmount = 0;
            int maxTime = 120;
            Player player = FindTarget(position);
            switch(spell)
            {
                case Spell.DistruptGravity:
                int part = time % 20;
                pupilDirection = (float)Math.PI / 2;
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
                pupilDirection = (float)Math.PI * 2 * ((float)time / maxTime);
                pupilStareOutAmount = 0.8f;
                int amt = 4;
                if(time == 1)
                {
                    InflictAllPlayers(ModContent.BuffType<Darkness>());
                    amt = 20;
                }
                for(int i = 0; i < amt; i++)
                {
                    Dust d2 = Dust.NewDustPerfect(position + PupilPosition(pupilDirection, pupilStareOutAmount), ModContent.DustType<DarknessDust>(), QwertyMethods.PolarVector(time == 1 ? 10 : 3, (float)Math.PI * 2 * ((float)i / amt)));
                    d2.noGravity = true;
                    d2.frame.Y = 0;
                    d2.scale *= 2;
                }
                return;
                case Spell.DistruptControls:
                pupilDirection = (float)Math.PI * 10 * ((float)time / maxTime);
                pupilStareOutAmount = 0.8f;
                if(time == 1)
                {
                    InflictAllPlayers(ModContent.BuffType<PeriodicConfusion>());
                }
                return;
                case Spell.DistruptCamera:
                pupilDirection = (float)Math.PI * -10 * ((float)time / maxTime);
                pupilStareOutAmount = 0.8f;
                if(time == 1)
                {
                    InflictAllPlayers(ModContent.BuffType<CameraIssues>());
                }
                return;
                case Spell.Beam:
                maxTime = 6 * 60;
                pupilDirection = PlayerDirection(position, player);
                pupilStareOutAmount = 0.8f;
                return;
                case Spell.AimedShots:
                pupilStareOutAmount = 0.3f;
                pupilDirection = QwertyMethods.PredictiveAim(position, 6 * 3, player.Center, player.velocity);
                if(float.IsNaN(pupilDirection))
                {
                    pupilDirection = PlayerDirection(position, player);
                }
                maxTime = 6 * 60;
                if(time % 120 == 0)
                {
                    Projectile.NewProjectile(new EntitySource_Misc(""), position + PupilPosition(pupilDirection, pupilStareOutAmount), QwertyMethods.PolarVector(6, pupilDirection), ModContent.ProjectileType<InvaderRay>(), Main.expertMode ? InvaderBattleship.expertDamage : InvaderBattleship.normalDamage, 0);
                }
                return;
                case Spell.ShadowMissiles:
                maxTime = 10 * 60;
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
                    allPlayer.AddBuff(buffID, 30 * 60);
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
                case Spell.DistruptControls:
                return;
                case Spell.DistruptCamera:
                return;
                case Spell.Beam:
                return;
                case Spell.AimedShots:
                return;
                case Spell.ShadowMissiles:
                return;
            }
        }
        
    }
    public enum Spell : byte
    {
        DistruptGravity,
        DistruptVision,
        DistruptControls,
        DistruptCamera,
        Beam,
        AimedShots,
        ShadowMissiles
    }
    public class SpellBeam : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Invader Battleship");
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 4;
            Projectile.timeLeft = 60 + 5 * 60;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
        }
        float length = 0;
        float beamWidth = 0;
        public override void AI()
        {
            Projectile.velocity = Vector2.Zero;
            for (length = 0; length < 1000; length++)
            {
                if (!Collision.CanHitLine(Projectile.Center, 1, 1, Projectile.Center + QwertyMethods.PolarVector(length, Projectile.rotation), 1, 1))
                {
                    break;
                }
            }
            if(Projectile.timeLeft <= 60)
            {
                if (Projectile.timeLeft > 50)
                {
                    beamWidth = 60 - Projectile.timeLeft;
                }
                if (Projectile.timeLeft < 10)
                {
                    beamWidth = Projectile.timeLeft;
                }
            }
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {

            if(Projectile.timeLeft <= 60)
            {
                float point = 0;
                return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + QwertyMethods.PolarVector(length, Projectile.rotation), beamWidth, ref point);
            }
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if(Projectile.timeLeft <= 60)
            {
                Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
                Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, 26, 22), Color.White, Projectile.rotation - (float)Math.PI / 2f, new Vector2(13, 11), new Vector2(beamWidth / 10f, 1f), SpriteEffects.None, 0);
                float subLength = length - (11 + 22);
                int midBeamHieght = 30;
                Main.EntitySpriteDraw(texture, Projectile.Center + QwertyMethods.PolarVector(11, Projectile.rotation) - Main.screenPosition, new Rectangle(0, 24, 26, midBeamHieght), Color.White, Projectile.rotation - (float)Math.PI / 2f, new Vector2(13, 0), new Vector2(beamWidth / 10f, subLength / (float)midBeamHieght), SpriteEffects.None, 0);
                Main.EntitySpriteDraw(texture, Projectile.Center + QwertyMethods.PolarVector(length - 22, Projectile.rotation) - Main.screenPosition, new Rectangle(0, 56, 26, 22), Color.White, Projectile.rotation - (float)Math.PI / 2f, new Vector2(13, 0), new Vector2(beamWidth / 10f, 1f), SpriteEffects.None, 0);
            }
            else
            {
                Texture2D beamWarning = Request<Texture2D>("QwertyMod/Content/NPCs/Invader/InvaderZap").Value;
                Main.EntitySpriteDraw(beamWarning, Projectile.Center - Main.screenPosition, null, Color.White,  Projectile.rotation, Vector2.UnitY * 1, new Vector2(length / 2f, 1), SpriteEffects.None, 0);
            }
            return false;
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<CriticalFailure>(), 10 * 60);
        }

    }
}