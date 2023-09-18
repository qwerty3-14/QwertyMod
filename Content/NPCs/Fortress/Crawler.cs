using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common.Fortress;
using QwertyMod.Content.Buffs;
using QwertyMod.Content.Dusts;
using QwertyMod.Content.Items.Consumable.Ammo.Bullet.Caelite;
using QwertyMod.Content.Items.Consumable.Tiles.Banners;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.NPCs.Fortress
{
    public class Crawler : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Mollusket");
            Main.npcFrameCount[NPC.type] = 1;
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                CustomTexturePath = "QwertyMod/Content/NPCs/Fortress/Crawler_Bestiary",
                PortraitScale = 1f,
                PortraitPositionYOverride = 0f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }

        public override void SetDefaults()
        {
            NPC.width = 36 + 24;
            NPC.height = 36 + 24;
            NPC.aiStyle = -1;
            NPC.damage = 20;
            NPC.defense = 18;
            NPC.lifeMax = 65;

            if (SkyFortress.beingInvaded)
            {
                NPC.lifeMax = 600;
            }
            DrawOffsetY = -4;
            NPC.value = 100;
            //NPC.alpha = 100;
            NPC.behindTiles = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.direction = 1;
            //NPC.dontTakeDamage = true;
            //NPC.scale = 1.2f;
            NPC.buffImmune[BuffType<PowerDown>()] = true;
            Banner = NPC.type;
            BannerItem = ItemType<CrawlerBanner>();
            NPC.noTileCollide = true;
            //NPC.scale = 1.2f;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Sets the description of this NPC that is listed in the bestiary
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
                new FlavorTextBestiaryInfoElement("I have so many questions about this thing and I don't think anyone has an answer for them.")
            });
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 40; i++)
                {
                    int dustType = DustType<FortressDust>(); ;
                    int dustIndex = Dust.NewDust(NPC.position, NPC.width, NPC.height, dustType);
                    Dust dust = Main.dust[dustIndex];
                    dust.velocity.X = dust.velocity.X + Main.rand.Next(-50, 51) * 0.01f;
                    dust.velocity.Y = dust.velocity.Y + Main.rand.Next(-50, 51) * 0.01f;
                    dust.scale *= 1f + Main.rand.Next(-30, 31) * 0.01f;
                }
            }
            for (int i = 0; i < 4; i++)
            {
                int dustType = DustType<FortressDust>(); ;
                int dustIndex = Dust.NewDust(NPC.position, NPC.width, NPC.height, dustType);
                Dust dust = Main.dust[dustIndex];
                dust.velocity.X = dust.velocity.X + Main.rand.Next(-50, 51) * 0.01f;
                dust.velocity.Y = dust.velocity.Y + Main.rand.Next(-50, 51) * 0.01f;
                dust.scale *= 1f + Main.rand.Next(-30, 31) * 0.01f;
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.InModBiome(GetInstance<FortressBiome>()))
            {
                return 60f;
            }
            return 0f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemType<CaeliteBullet>(), 1, 2, 4));
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return preSetTimer <= 0;
        }

        private int timer;
        private bool drawLine;
        private bool alternateColor;
        private int startDirection = -1;
        private Vector2 shootFrom;
        private float gunShift = -5.5f;
        private float gunShiftUp = 11.5f;
        private float aimDirection;
        private int preSetTimer = 120;

        public override void AI()
        {
            NPC.damage = 0;
            NPC.GetGlobalNPC<FortressNPCGeneral>().fortressNPC = true;

            NPC.dontTakeDamage = false;
            if (NPC.ai[0] == 0f)
            {
                NPC.TargetClosest(true);
                NPC.direction = 1;
                Player initPlayer = Main.player[NPC.target];
                if (initPlayer.Center.X < NPC.Center.X)
                {
                    //startDirection = 1;
                }
                NPC.noGravity = true;
                NPC.directionY = 1;
                NPC.ai[0] = 1f;
                Point origin = NPC.Center.ToTileCoordinates();
                Point point;
                for (int s = 0; s < 1000; s++)
                {
                    if (WorldUtils.Find(origin, Searches.Chain(new Searches.Down(3), new GenCondition[]
                    {
                                                new Terraria.WorldBuilding.Conditions.IsSolid()
                    }), out point))
                    {
                        break;
                    }
                    else
                    {
                        NPC.position.Y++;
                        origin = NPC.Center.ToTileCoordinates();
                        if(s == 999)
                        {
                            NPC.active = false;
                        }
                    }
                }
            }
            if (preSetTimer > 0)
            {
                preSetTimer--;
                NPC.dontTakeDamage = true;
                NPC.velocity = Vector2.Zero;
                float d = Main.rand.NextFloat() * MathF.PI * 2;
                Dust dusty = Dust.NewDustPerfect(NPC.position + new Vector2(Main.rand.Next(NPC.width), Main.rand.Next(NPC.height)) + QwertyMethods.PolarVector(30f, d + MathF.PI), DustType<FortressDust>(), QwertyMethods.PolarVector(3f, d), Scale: .5f);
                dusty.noGravity = true;
            }
            else
            {
                shootFrom = NPC.Center + QwertyMethods.PolarVector(gunShift, NPC.rotation) * -startDirection + QwertyMethods.PolarVector(gunShiftUp, NPC.rotation - MathF.PI / 2);
                int speed = 2;
                if (NPC.ai[1] == 0f)
                {
                    //NPC.rotation += (float)(NPC.direction * NPC.directionY) * 0.13f;
                    if (NPC.collideY)
                    {
                        NPC.ai[0] = 2f;
                    }
                    if (!NPC.collideY && NPC.ai[0] == 2f)
                    {
                        NPC.direction = -NPC.direction;
                        NPC.ai[1] = 1f;
                        NPC.ai[0] = 1f;
                    }
                    if (NPC.collideX)
                    {
                        NPC.directionY = -NPC.directionY;
                        NPC.ai[1] = 1f;
                    }
                }
                else
                {
                    //NPC.rotation -= (float)(NPC.direction * NPC.directionY) * 0.13f;
                    if (NPC.collideX)
                    {
                        NPC.ai[0] = 2f;
                    }
                    if (!NPC.collideX && NPC.ai[0] == 2f)
                    {
                        NPC.directionY = -NPC.directionY;
                        NPC.ai[1] = 0f;
                        NPC.ai[0] = 1f;
                    }
                    if (NPC.collideY)
                    {
                        NPC.direction = -NPC.direction;
                        NPC.ai[1] = 0f;
                    }
                }
                //NPC.TargetClosest(true);
                Entity player = FortressNPCGeneral.FindTarget(NPC, false);
                if (player != null && Collision.CanHitLine(shootFrom, 0, 0, player.Center, 0, 0) && (player.Center - NPC.Center).Length() < 1000)
                {
                    NPC.velocity.X = 0;
                    NPC.velocity.Y = 0;
                    aimDirection = (player.Center - shootFrom).ToRotation();
                    timer++;
                    if (timer > 180)
                    {
                        float shootSpeed = 24;

                        Projectile.NewProjectile(NPC.GetSource_FromAI(), shootFrom.X, shootFrom.Y, MathF.Cos(aimDirection) * shootSpeed, MathF.Sin(aimDirection) * shootSpeed, ProjectileType<MollusketSnipe>(), SkyFortress.beingInvaded ? 50 : 15, 0, Main.myPlayer);
                        timer = 0;
                    }
                    if (timer > 60)
                    {
                        alternateColor = true;
                    }
                    else
                    {
                        alternateColor = false;
                    }
                    if (timer > 30)
                    {
                        drawLine = true;
                    }
                    else
                    {
                        drawLine = false;
                    }
                }
                else
                {
                    drawLine = false;
                    alternateColor = false;
                    timer = 0;
                    NPC.velocity.X = (float)(speed * NPC.direction);
                    NPC.velocity.Y = (float)(speed * NPC.directionY);
                    NPC.rotation = NPC.velocity.ToRotation();
                    NPC.rotation += MathF.PI / 4 * startDirection;
                    aimDirection = NPC.rotation;
                    /*
                    if(NPC.velocity.X >0 && NPC.velocity.Y >0)
                    {
                        NPC.rotation = 0;
                    }
                    else if(NPC.velocity.X < 0 && NPC.velocity.Y > 0)
                    {
                        NPC.rotation = MathF.PI;
                    }
                    else if (NPC.velocity.X > 0 && NPC.velocity.Y < 0)
                    {
                        NPC.rotation = MathF.PI;
                    }
                    else if (NPC.velocity.X < 0 && NPC.velocity.Y < 0)
                    {
                        NPC.rotation = MathF.PI;
                    }
                    */
                    //NPC.spriteDirection = NPC.direction;
                }

                float num281 = (float)(270 - (int)Main.mouseTextColor) / 400f;

                NPC.oldVelocity = NPC.velocity;
                NPC.collideX = false;
                NPC.collideY = false;
                Vector2 position = NPC.Center;
                int num = 12;
                int num2 = 12;
                position.X -= (float)(num / 2);
                position.Y -= (float)(num2 / 2);
                NPC.velocity = Collision.noSlopeCollision(position, NPC.velocity, num, num2, true, true);
                if (NPC.oldVelocity.X != NPC.velocity.X)
                {
                    NPC.collideX = true;
                }
                if (NPC.oldVelocity.Y != NPC.velocity.Y)
                {
                    NPC.collideY = true;
                }
                //Lighting.AddLight((int)(NPC.position.X + (float)(NPC.width / 2)) / 16, (int)(NPC.position.Y + (float)(NPC.height / 2)) / 16, 0.9f, 0.3f + num281, 0.2f);
                return;
            }
        }

        private int colorCounter;
        private Color lineColor;
        private float distance;
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (preSetTimer > 0)
            {
                return;
            }
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            spriteBatch.Draw(texture, new Vector2(NPC.Center.X - screenPos.X, NPC.Center.Y - screenPos.Y),
                       NPC.frame, drawColor, NPC.rotation,
                       new Vector2(NPC.width * 0.5f, NPC.height * 0.5f), 1f, 0, 0f);
            Entity player = FortressNPCGeneral.FindTarget(NPC, false);

            if (alternateColor)
            {
                colorCounter++;

                if (colorCounter >= 20)
                {
                    colorCounter = 0;
                }
                else if (colorCounter >= 10)
                {
                    lineColor = Color.White;
                }
                else
                {
                    lineColor = Color.Red;
                }
            }
            else
            {
                lineColor = Color.Red;
            }
            //Draw chain
            if (drawLine && player != null)
            {
                Vector2 center = shootFrom;
                Vector2 distToProj = player.Center - center;
                float projRotation = distToProj.ToRotation() - 1.57f;
                distToProj.Normalize();                 //get unit vector
                distToProj *= 12f;                      //speed = 12
                center += distToProj;                   //update draw position
                distToProj = player.Center - center;    //update distance
                distance = distToProj.Length();
                //Color drawColor = lightColor;

                spriteBatch.Draw(Request<Texture2D>("QwertyMod/Content/NPCs/Fortress/laser").Value, new Vector2(center.X - screenPos.X, center.Y - screenPos.Y),
                    new Rectangle(0, 0, 1, (int)distance - 10), lineColor, projRotation,
                    new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            }

            drawLine = false;
            spriteBatch.Draw(Request<Texture2D>("QwertyMod/Content/NPCs/Fortress/Crawler_Turret").Value, new Vector2(shootFrom.X - screenPos.X, shootFrom.Y - screenPos.Y + 2f),
                        new Rectangle(0, 0, 28, 14), drawColor, aimDirection,
                        new Vector2(5, 9), 1f, 0, 0f);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            return false;
        }
    }

    public class MollusketSnipe : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Mollusket Snipe");
        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = 1;
            Projectile.light = 0.5f;
            //Projectile.alpha = 255;
            Projectile.scale = 1.2f;
            Projectile.timeLeft = 600;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.extraUpdates = 3;
            Projectile.GetGlobalProjectile<FortressNPCProjectile>().isFromFortressNPC = true;
            Projectile.GetGlobalProjectile<FortressNPCProjectile>().EvEMultiplier = 10f;
            Projectile.friendly = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }

        public override void AI()
        {
            for (int d = 0; d < 1; d++)
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustType<CaeliteDust>(), Vector2.Zero);
                dust.frame.Y = 0;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffType<PowerDown>(), 480);
        }
    }
}