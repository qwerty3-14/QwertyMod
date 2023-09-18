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


namespace QwertyMod.Content.NPCs.Bosses.InvaderBattleship
{
    public class EtimsSphere : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 72;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 10 * 2 * 60;
            Projectile.extraUpdates = 1;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
        }
        bool runOnce = true;
        float rotSpeed = MathF.PI / 90f;
        float rotDir = 0;
        int blashTime = 0;
        float blashRadius = 300;
        List<Point> cleanUp = new List<Point>();
        void blash()
        {
            SoundEngine.PlaySound(SoundID.Item92, Projectile.Center);
            int sideLength = (int)(blashRadius / 8f);
            Projectile.velocity = Vector2.Zero;
            blashTime = 10;
            Point topLeft = (Projectile.Center + new Vector2(-blashRadius, -blashRadius)).ToTileCoordinates();
            for(int i = topLeft.X + 1; i < topLeft.X + sideLength + 1; i++)
            {
                for(int j = topLeft.Y + 1; j < topLeft.Y + sideLength + 1; j++)
                {
                    Point loc = new Point(i, j);
                    if(Main.tile[i, j].HasTile && ((loc.ToVector2() * 16) - Projectile.Center).Length() < blashRadius)
                    {
                        Wiring.DeActive(i, j);
                        WorldGen.paintTile(i, j, PaintID.ShadowPaint, true);
                        cleanUp.Add(loc);
                    }
                }
            }
        }
        public override void OnKill(int timeLeft)
        {
            foreach(Point loc in cleanUp)
            {
                Wiring.ReActive(loc.X, loc.Y);
                Main.tile[loc.X, loc.Y].ClearBlockPaintAndCoating();
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            //blash();
            return false;
        }
        public override void AI()
        {
            if(runOnce)
            {
                runOnce = false;
                rotDir = Projectile.velocity.ToRotation();
                //blash();
            }
            if(Projectile.timeLeft < 60)
            {
                Projectile.damage = 0;
                Projectile.scale = Projectile.timeLeft / 60f;
            }
            else
            {
                Entity target = FindTarget(Projectile);
                if(target != null)
                {
                    rotDir.SlowRotation((target.Center - Projectile.Center).ToRotation(), rotSpeed);
                    
                }
                float angDiffRatio = QwertyMethods.AngularDifference(rotDir, Projectile.velocity.ToRotation()) / MathF.PI;
                float acc = 4f / 30f;
                if(blashTime > 0)
                {
                    Projectile.velocity = Vector2.Zero;
                    blashTime--;
                }
                else
                {
                    Projectile.velocity += QwertyMethods.PolarVector(angDiffRatio * 3f * acc, (-Projectile.velocity).ToRotation());
                    Projectile.velocity += QwertyMethods.PolarVector((1f - angDiffRatio) * acc, rotDir);
                }
                Projectile.frameCounter++;
                if(Projectile.frameCounter % 20 == 0)
                {
                    Projectile.frame++;
                }
                if(Projectile.frame > 4)
                {
                    Projectile.frame = 0;
                }
            }
        }
        public override void PostDraw( Color lightColor)
        {
            if(blashTime > 0)
            {
                Texture2D blash = Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/EtimsBlash").Value;
                Main.EntitySpriteDraw(blash, Projectile.Center - Main.screenPosition,
                        null, Color.White, Projectile.rotation,
                        blash.Size() * 0.5f, (blashRadius * 2f) / blash.Width, SpriteEffects.None, 0);
                Point topLeft = (Projectile.Center + new Vector2(-blashRadius, -blashRadius)).ToTileCoordinates();
                Texture2D texture = Terraria.GameContent.TextureAssets.Extra[2].Value;
                int sideLength = (int)(blashRadius / 8f);
                /*
                for(int i = topLeft.X + 1; i < topLeft.X + sideLength + 1; i++)
                {
                    for(int j = topLeft.Y + 1; j < topLeft.Y + sideLength + 1; j++)
                    {
                        Point loc = new Point(i, j);
                        Main.EntitySpriteDraw(texture, (loc.ToVector2() * 16) - Main.screenPosition, new Rectangle(0, 0, 16, 16), Color.White, 0, Vector2.Zero, Vector2.One, 0, 0);
                    }
                }
                */
                //Main.EntitySpriteDraw(texture, (topLeft.ToVector2() * 16) - Main.screenPosition, new Rectangle(0, 0, 16, 16), Color.White, 0, Vector2.Zero, Vector2.One, 0, 0);
            }
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
    }
}