using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common.Fortress;
using QwertyMod.Content.Buffs;
using QwertyMod.Content.Dusts;
using QwertyMod.Content.Items.Consumable.BossSummon;
using QwertyMod.Content.Items.Consumable.Tiles.Banners;
using QwertyMod.Content.NPCs.Bosses.FortressBoss;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent;
using QwertyMod.Content.NPCs.Fortress;
using QwertyMod.Content.Items.Weapon.Ranged.Gun.SoEF;

namespace QwertyMod.Content.NPCs.Invader
{
    public class InvaderBehemoth : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Invader Behemoth");
            Main.npcFrameCount[NPC.type] = 8;
        }
        public override void SetDefaults()
        {
            NPC.width = 48;
            NPC.height = 124;
            NPC.aiStyle = -1;
            NPC.damage = 200;
            NPC.defense = 80;
            NPC.lifeMax = 2000;
            NPC.value = 10000;
            //NPC.alpha = 100;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.knockBackResist = 0f;
            NPC.noGravity = false;
            NPC.buffImmune[BuffID.Confused] = false;
            NPC.GetGlobalNPC<InvaderNPCGeneral>().invaderNPC = true;
        }
        int preTimer = 120;
        int shotCounter = 0;
        int beamChargeup = 0;
        float beamDirection = 0;
        int swordTimer = 0;
        public const float beamSweepAngle = (float)Math.PI / 4;
        public override void AI()
        {
            NPC.damage = 0;
            if(preTimer > 0)
            {
                if(preTimer == 120)
                {
                    InvaderNPCGeneral.SpawnIn(NPC);
                    NPC.dontTakeDamage = true;
                }
                if(preTimer % 20 == 0)
                {
                    InvaderNPCGeneral.SpawnAnimation(NPC);
                }
                preTimer--;
            }
            else
            {
                NPC.dontTakeDamage = false;
                shotCounter--;
                Entity target = InvaderNPCGeneral.FindTarget(NPC, beamChargeup < 60);
                Vector2 shootFrom = NPC.Center - Vector2.UnitY * 35 + NPC.direction * Vector2.UnitX;
                if ((target != null && Collision.CanHitLine(shootFrom, 0, 0, target.Center, 0, 0) && (target.Center - NPC.Center).Length() < 1500) || beamChargeup >= 60 || swordTimer > 0)
                {
                    NPC.velocity.X = 0;
                    float dist = 0;
                    if(target != null)
                    {
                        dist = Math.Abs(target.Center.X - NPC.Center.X);
                    } 
                    if(dist < 60 && swordTimer == 0 && beamChargeup == 0)
                    {
                        NPC.direction *= -1;
                        InvaderNPCGeneral.WalkerWalk(NPC, 3);
                    }
                    else if((dist < 120  && beamChargeup < 60 && Math.Abs(target.Center.Y - NPC.Center.Y) < 52) || swordTimer > 0)
                    {
                        beamChargeup = 0;
                        swordTimer++;
                        if(swordTimer == 40)
                        {
                            Vector2 here = shootFrom + new Vector2(84 * NPC.direction, 9);
                            Projectile.NewProjectile(new EntitySource_Misc(""), here, Vector2.Zero, ModContent.ProjectileType<InvaderSwordHitbox>(), 200, 0, 0);
                        }
                        if(swordTimer >= 60)
                        {
                            swordTimer = 0;
                        }
                    }
                    else
                    {
                        beamChargeup++;
                        if(beamChargeup == 60)
                        {
                            beamDirection = (target.Center - shootFrom).ToRotation();
                        }
                        if(beamChargeup == 120)
                        {
                            Projectile.NewProjectile(new EntitySource_Misc(""), shootFrom, QwertyMethods.PolarVector(1f, beamDirection + beamSweepAngle / 2f), ModContent.ProjectileType<InvaderBeam>(), 80, 0, 0);
                        }
                        if(beamChargeup < 120)
                        {
                            for(int i =0; i < 2; i++)
                            {
                                float rot = (float)Math.PI * i + (float)Math.PI * (float)beamChargeup / 10f; 
                                Dust.NewDustPerfect(shootFrom + QwertyMethods.PolarVector(30, rot), ModContent.DustType<InvaderGlow>(), QwertyMethods.PolarVector(-3f, rot));
                            }
                        }
                        if(beamChargeup >= 180)
                        {
                            beamChargeup = 0;
                        }
                    }
                }
                else
                {
                    beamChargeup = 0;
                    swordTimer = 0;
                    NPC.velocity.X = 0;
                    InvaderNPCGeneral.WalkerIdle(NPC, 1.5f);
                }
                
            }
            NPC.spriteDirection = NPC.direction;
        }
        
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return preTimer <= 0;
        }
        public override void FindFrame(int frameHeight)
        {
            if(swordTimer > 0)
            {
                if(swordTimer < 20)
                {
                    NPC.frame.Y = 0;
                }
                else if(swordTimer < 40)
                {
                    NPC.frame.Y = frameHeight * 4;
                }
                else if(swordTimer < 45)
                {
                    NPC.frame.Y = frameHeight * 5;
                }
                else
                {
                    NPC.frame.Y = frameHeight * 6;
                }
            }
            else if(beamChargeup > 0)
            {
                NPC.frame.Y = frameHeight * 7;
            }
            else
            {
                if(NPC.velocity.X != 0)
                {
                    NPC.frameCounter++;
                    if(NPC.frameCounter % 6 ==0)
                    {
                        NPC.frame.Y += frameHeight;
                        if(NPC.frame.Y >= frameHeight * 4)
                        {
                            NPC.frame.Y = 0;
                        }
                    }
                }
                else
                {
                    NPC.frame.Y = 0;
                }
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if(preTimer > 0)
            {
                return false;
            }
            
            spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - screenPos,
            NPC.frame, drawColor, NPC.rotation,
            new Vector2( (NPC.spriteDirection != 1 ? (TextureAssets.Npc[NPC.type].Value.Width - 63) : 63), (150 - 124) + (124 / 2)), 1f, NPC.spriteDirection != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            spriteBatch.Draw(Request<Texture2D>("QwertyMod/Content/NPCs/Invader/InvaderBehemoth_Glow").Value, NPC.Center - screenPos,
            NPC.frame, Color.White, NPC.rotation,
            new Vector2( (NPC.spriteDirection != 1 ? (TextureAssets.Npc[NPC.type].Value.Width - 63) : 63), (150 - 124) + (124 / 2)), 1f, NPC.spriteDirection != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            if(beamChargeup >= 60 && beamChargeup < 120)
            {
                Vector2 shootFrom = NPC.Center - Vector2.UnitY * 35 + NPC.direction * Vector2.UnitX;
                float rot = beamDirection + ((120 - beamChargeup) / 60f) * beamSweepAngle  - beamSweepAngle / 2f;
                int length = 0;
                for(; length < 1000; length++)
                {
                    if(!Collision.CanHitLine(shootFrom, 1, 1, shootFrom + QwertyMethods.PolarVector(length, rot), 1, 1))
                    {
                        break;
                    }
                }
                Texture2D beamWarning = Request<Texture2D>("QwertyMod/Content/NPCs/Invader/InvaderZap").Value;
                spriteBatch.Draw(beamWarning, shootFrom - Main.screenPosition, null, Color.White, rot, Vector2.UnitY * 1, new Vector2(length / 2f, 1), SpriteEffects.None, 0);
            }
            return false;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Sets the description of this NPC that is listed in the bestiary
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
                new FlavorTextBestiaryInfoElement("The invaders see the benfits of large armies of small expendeble drones... they also understand the value of large expendable drones.")
            });
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {

        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.InModBiome(ModContent.GetInstance<FortressBiome>()) && NPC.downedGolemBoss)
            {
                return 20f;
            }
            return 0f;
        }
    }
    public class InvaderBeam : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Invader Behemoth");
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 4;
            Projectile.timeLeft = 60;
            Projectile.friendly = true;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.GetGlobalProjectile<InvaderProjectile>().isInvaderProjectile = true;
        }
        bool runOnce = true;
        float length = 0;
        float beamWidth = 0;
        public override void AI()
        {
            if(runOnce)
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
                Projectile.velocity = Vector2.Zero;
                runOnce = false;
            }
            for(; length < 1000; length++)
            {
                if(!Collision.CanHitLine(Projectile.Center, 1, 1, Projectile.Center + QwertyMethods.PolarVector(length, Projectile.rotation), 1, 1))
                {
                    break;
                }
            }
            if(Projectile.timeLeft > 50)
            {
                beamWidth = 60 - Projectile.timeLeft;
            }
            if( Projectile.timeLeft < 10)
            {
                beamWidth = Projectile.timeLeft;
            }
            Projectile.rotation -= InvaderBehemoth.beamSweepAngle / 60f;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + QwertyMethods.PolarVector(length, Projectile.rotation), beamWidth, ref point);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, 26, 22), Color.White, Projectile.rotation - (float)Math.PI /2f, new Vector2(13, 11), new Vector2(beamWidth / 10f, 1f), SpriteEffects.None, 0);
            float subLength = length - (11 + 22);
            int midBeamHieght = 30;
            Main.EntitySpriteDraw(texture, Projectile.Center + QwertyMethods.PolarVector(11, Projectile.rotation) - Main.screenPosition, new Rectangle(0, 24, 26, midBeamHieght), Color.White, Projectile.rotation - (float)Math.PI /2f, new Vector2(13, 0), new Vector2(beamWidth / 10f, subLength / (float)midBeamHieght), SpriteEffects.None, 0);
            Main.EntitySpriteDraw(texture, Projectile.Center + QwertyMethods.PolarVector(length - 22, Projectile.rotation) - Main.screenPosition, new Rectangle(0, 56, 26, 22), Color.White, Projectile.rotation - (float)Math.PI /2f, new Vector2(13, 0), new Vector2(beamWidth / 10f, 1f), SpriteEffects.None, 0);
            return false;
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<CriticalFailure>(), 10 * 60);
        }

    }
    public class InvaderSwordHitbox : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Invader Behemoth");
        }
        public override void SetDefaults()
        {
            Projectile.tileCollide = false;
            Projectile.width = 72;
            Projectile.height = 124;
            Projectile.timeLeft = 5;
            Projectile.friendly = true;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.GetGlobalProjectile<InvaderProjectile>().isInvaderProjectile = true;
            Projectile.GetGlobalProjectile<EtimsProjectile>().effect = true;
            Projectile.aiStyle = -1;
        }
        public override void AI()
        {
            /*
            for(int i =0; i < 10; i ++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<InvaderGlow>());
            }
            */
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
}