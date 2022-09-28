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

namespace QwertyMod.Content.NPCs.Bosses.InvaderBattleship
{
    public class BattleshipStrikeDrone : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Strike Drone");
        }
        public override void SetDefaults()
        {
            NPC.width = NPC.height = 42;
            NPC.lifeMax = 4000;
            NPC.damage = 10;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.aiStyle = -1;
            NPC.knockBackResist = 0;
        }
        public const float offFlyX = 1600;
        public const float offFlyY = 1600;
        int aiTimer = 0;
        Vector2 flyTo;
        public const float heightAbove = 400f;
        public override void AI()
        {
            NPC.damage = 0;
            if(aiTimer == 0)
            {
                flyTo = NPC.Center + new Vector2(offFlyX, offFlyY);
            }
            aiTimer++;

            if(aiTimer > 360)
            {
                NPC.velocity.Y -= 0.1f;
                if(aiTimer > 480)
                {
                    NPC.active =false;
                }
            }
            else if(aiTimer > 240)
            {
                NPC.velocity = Vector2.Zero;
            }
            else
            {
                NPC.TargetClosest();
                flyTo.Y = Main.player[NPC.target].Center.Y - heightAbove;
                NPC.velocity = (flyTo - NPC.Center) * (1/30f);
            }
            if(aiTimer == 240)
            {
                Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center + Vector2.UnitY * 19, Vector2.UnitY, ModContent.ProjectileType<StrikeDroneBeam>(), 50, 0);
            }
        }
        
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - screenPos,
            NPC.frame, drawColor, NPC.rotation,
            NPC.Size * 0.5f, 1f, NPC.spriteDirection != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            spriteBatch.Draw(Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/BattleshipStrikeDrone_Glow").Value, NPC.Center - screenPos,
            NPC.frame, Color.White, NPC.rotation,
            NPC.Size * 0.5f, 1f, NPC.spriteDirection != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

            if (aiTimer >= 120 && aiTimer < 240)
            {
                Vector2 shootFrom = NPC.Center + Vector2.UnitY * 19;
                float rot = (float)Math.PI / 2f;
                int length = 0;
                for (; length < 1000; length++)
                {
                    if (!Collision.CanHitLine(shootFrom, 1, 1, shootFrom + QwertyMethods.PolarVector(length, rot), 1, 1))
                    {
                        break;
                    }
                }
                Texture2D beamWarning = Request<Texture2D>("QwertyMod/Content/NPCs/Invader/InvaderZap").Value;
                spriteBatch.Draw(beamWarning, shootFrom - Main.screenPosition, null, Color.White, rot, Vector2.UnitY * 1, new Vector2(length / 2f, 1), SpriteEffects.None, 0);
            }
            return false;
        }
    }
    public class StrikeDroneBeam : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Strike Drone");
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 4;
            Projectile.timeLeft = 60;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
        }
        bool runOnce = true;
        float length = 0;
        float beamWidth = 0;
        public override void AI()
        {
            if (runOnce)
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
                Projectile.velocity = Vector2.Zero;
                runOnce = false;
            }
            for (length = 0; length < 1000; length++)
            {
                if (!Collision.CanHitLine(Projectile.Center, 1, 1, Projectile.Center + QwertyMethods.PolarVector(length, Projectile.rotation), 1, 1))
                {
                    break;
                }
            }
            if (Projectile.timeLeft > 50)
            {
                beamWidth = 60 - Projectile.timeLeft;
            }
            if (Projectile.timeLeft < 10)
            {
                beamWidth = Projectile.timeLeft;
            }
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + QwertyMethods.PolarVector(length, Projectile.rotation), beamWidth, ref point);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, 26, 22), Color.White, Projectile.rotation - (float)Math.PI / 2f, new Vector2(13, 11), new Vector2(beamWidth / 10f, 1f), SpriteEffects.None, 0);
            float subLength = length - (11 + 22);
            int midBeamHieght = 30;
            Main.EntitySpriteDraw(texture, Projectile.Center + QwertyMethods.PolarVector(11, Projectile.rotation) - Main.screenPosition, new Rectangle(0, 24, 26, midBeamHieght), Color.White, Projectile.rotation - (float)Math.PI / 2f, new Vector2(13, 0), new Vector2(beamWidth / 10f, subLength / (float)midBeamHieght), SpriteEffects.None, 0);
            Main.EntitySpriteDraw(texture, Projectile.Center + QwertyMethods.PolarVector(length - 22, Projectile.rotation) - Main.screenPosition, new Rectangle(0, 56, 26, 22), Color.White, Projectile.rotation - (float)Math.PI / 2f, new Vector2(13, 0), new Vector2(beamWidth / 10f, 1f), SpriteEffects.None, 0);
            return false;
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<CriticalFailure>(), 10 * 60);
        }

    }
}