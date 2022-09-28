
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
using QwertyMod.Content.Buffs;
using QwertyMod.Content.Dusts;
using QwertyMod.Content.NPCs.Invader;

namespace QwertyMod.Content.NPCs.Bosses.InvaderBattleship
{
    public class BattleshipMissile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Invader Battleship");
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 14;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 480;
            Projectile.extraUpdates = 1;
            Projectile.hostile = true;
        }
        bool exploded = false;
        void explode()
        {
            if (!exploded)
            {
                exploded = true;
                Projectile.timeLeft = 5;
                Projectile.width = 15;
                Projectile.height = 15;
                Projectile.position -= Vector2.One * 12;
                Projectile.tileCollide = false;
                Projectile.velocity = Vector2.Zero;
                for (int i = 0; i < 100; i++)
                {
                    float rot = (float)Math.PI * 2f * ((float)i / 30f);
                    Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<InvaderGlow>(), QwertyMethods.PolarVector(6f, rot));
                }
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            explode();
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            explode();
            return false;
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<CriticalFailure>(), 10 * 60);
            explode();
        }
        bool runOnce = true;
        float rotSpeed = (float)Math.PI / 90f;
        public override void AI()
        {
            if (!exploded)
            {
                if (runOnce)
                {
                    Projectile.rotation = Projectile.velocity.ToRotation();
                    runOnce = false;
                }
                Entity target = FindTarget(Projectile);
                bool avoidTiles = true;
                if (target != null)
                {
                    if(Collision.CanHitLine(Projectile.Center, 1, 1, target.Center, 1, 1))
                    {
                        avoidTiles = false;
                    }
                }
                if(avoidTiles)
                {
                    if(ScanForTiles(0, out _))
                    {
                        ScanForTiles((float)Math.PI / 6, out int leftSearch);
                        ScanForTiles(-(float)Math.PI / 6, out int rightSearch);
                        if(leftSearch > rightSearch)
                        {
                            Projectile.rotation += rotSpeed; 
                        }
                        else
                        {
                            Projectile.rotation += -rotSpeed; 
                        }
                    }
                    else if (target != null)
                    {
                        Projectile.rotation.SlowRotation((target.Center - Projectile.Center).ToRotation(), rotSpeed);
                    }
                }
                else if(target != null)
                {
                    Projectile.rotation.SlowRotation((target.Center - Projectile.Center).ToRotation(), rotSpeed);
                }
                Projectile.velocity = QwertyMethods.PolarVector(4, Projectile.rotation);
                Dust.NewDustPerfect(Projectile.Center + QwertyMethods.PolarVector(-35, Projectile.rotation), ModContent.DustType<InvaderGlow>(), Vector2.Zero, Scale: 0.2f);
                if (Projectile.timeLeft < 5)
                {
                    explode();
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (exploded)
            {
                return false;
            }
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition,
                        new Rectangle(0, Projectile.frame * texture.Height, texture.Width, texture.Height), lightColor, Projectile.rotation,
                        new Vector2(39, 7), 1f, SpriteEffects.None, 0);
                        /*
            Main.EntitySpriteDraw(Request<Texture2D>("QwertyMod/Content/NPCs/Invader/InvaderMicroMissile_Glow").Value, Projectile.Center - Main.screenPosition,
                        new Rectangle(0, Projectile.frame * texture.Height, texture.Width, texture.Height), Color.White, Projectile.rotation,
                        Projectile.Size * 0.5f, 1f, SpriteEffects.None, 0);
                        */
            return false;
        }
        public static Entity FindTarget(Projectile projectile)
        {
            Entity target = null;
            float maxRange = 10000;
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                if (Main.player[i].active && (Main.player[i].Center - projectile.Center).Length() - Main.player[i].aggro < maxRange)
                {
                    target = Main.player[i];
                    maxRange = (Main.player[i].Center - projectile.Center).Length() - Main.player[i].aggro;
                }
            }
            return target;
        }
        public bool ScanForTiles(float rot, out int dist)
        {
            dist = 0;
            for(; dist < 24; dist++)
            {
                if(Collision.CanHitLine(Projectile.Center, 1, 1, Projectile.Center + QwertyMethods.PolarVector(8 * dist, Projectile.rotation + rot), 1, 1))
                {
                    return true;
                }
            }
            return false;
        }
    }
}