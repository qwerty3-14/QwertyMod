using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common;
using QwertyMod.Common.Fortress;
using QwertyMod.Content.Dusts;
using QwertyMod.Content.Items.Consumable.BossSummon;
using QwertyMod.Content.Items.MiscMaterials;
using QwertyMod.Content.Items.Weapon.Ranged.Gun.SoEF;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertyMod.Content.NPCs.Invader
{
    public class InvaderElite : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 5;
        }
        public override void SetDefaults()
        {
            NPC.width = 48;
            NPC.height = 48;
            NPC.aiStyle = -1;
            NPC.damage = 100;
            NPC.defense = 32;
            NPC.lifeMax = 4000;
            NPC.value = 6000;
            //NPC.alpha = 100;
            NPC.HitSound = new SoundStyle("QwertyMod/Assets/Sounds/InvaderEliteHurt");
            NPC.DeathSound = new SoundStyle("QwertyMod/Assets/Sounds/InvaderDestroy");;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.buffImmune[BuffID.Confused] = true;
            NPC.GetGlobalNPC<InvaderNPCGeneral>().invaderNPC = true;
        }
        int armFrame = 0;
        int losCounter = 0;
        Entity target = null;
        float throwSpeed = 10;
        bool thrownSpear = false;
        Projectile spear;
        bool spinSpear = false;
        float spun = 0;
        int dustTimer = 0;
        public override void AI()
        {
            NPC.damage = 0;
            target = InvaderNPCGeneral.FindTarget(NPC, true);
            if(Main.netMode != NetmodeID.Server && Main.rand.NextBool(6000))
            {
                SoundEngine.PlaySound(new SoundStyle("QwertyMod/Assets/Sounds/InvaderEliteIdle"), NPC.Center);
            }
            if(spear != null && !spear.active)
            {
                spear = null;
                thrownSpear = false;
            }
            //if (target != null)
            {
                if(target != null)
                {
                    NPC.direction = Math.Sign(target.Center.X - NPC.Center.X);
                    if(Collision.CanHitLine(NPC.Center, 1, 1, target.Center, 1, 1))
                    {
                        losCounter++;
                    }
                    else
                    {
                        losCounter = 0;
                    }
                }
                else
                {
                    spinSpear = false;
                    spun = 0;
                }
               
                if(!thrownSpear)
                {
                    if(target != null)
                    {
                        float dist = (target.Center - NPC.Center).Length();
                        if(losCounter > 60 && dist > 200)
                        {
                            spinSpear = false;
                            spun = 0;
                            NPC.velocity = Vector2.Zero;
                            if(armFrame < 2)
                            {
                                NPC.frameCounter++;
                                if(NPC.frameCounter > 10)
                                {
                                    NPC.frameCounter = 0;
                                    armFrame++;
                                    
                                }
                            }
                            if(armFrame == 2 && losCounter > 120 && !thrownSpear)
                            {
                                thrownSpear = true;
                                HeresyOffset(out Vector2 holdOffset, out float rotation);
                                spear = Main.projectile[Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + holdOffset, QwertyMethods.PolarVector(throwSpeed, rotation), ModContent.ProjectileType<Heresy>(), 40, 0, Main.myPlayer)];
                                spear.ai[0] = NPC.whoAmI;
                                armFrame = 1;
                            }
                        }
                        else
                        {
                            if(armFrame != 0)
                            {
                                NPC.frameCounter++;
                                if(NPC.frameCounter > 10)
                                {
                                    NPC.frameCounter = 0;
                                    armFrame = armFrame == 2 ? 3 : 0;
                                }
                            }
                            if(armFrame == 0 && dist < 200 && losCounter > 0)
                            {
                                spinSpear = true;
                                spun += MathF.PI / 10f * NPC.direction;
                                if(losCounter % 10 == 0)
                                {
                                    HeresyOffset(out Vector2 holdOffset, out _);
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + holdOffset, Vector2.Zero, ModContent.ProjectileType<SpinSpearHitbox>(), 40, 0, Main.myPlayer);
                                }
                                
                            }
                            else
                            {
                                spinSpear = false;
                                spun = 0;
                            }
                            if(dist > 20)
                            {
                                float? dir = Pathfinder.PathFindWithNodeSize(NPC.Center, target.Center, 3, 2);
                                if(dir != null)
                                {
                                    NPC.velocity = QwertyMethods.PolarVector(4, (float)dir);
                                }
                                else
                                {
                                    NPC.velocity = Vector2.Zero;
                                }
                            }
                            
                        }
                    }
                    else
                    {
                        NPC.velocity = Vector2.Zero;
                        if(armFrame != 0)
                        {
                            NPC.frameCounter++;
                            if(NPC.frameCounter > 10)
                            {
                                NPC.frameCounter = 0;
                                armFrame = armFrame == 2 ? 3 : 0;
                            }
                        }
                    }
                }
                else
                {
                    
                    if(spear != null && spear.active)
                    {
                        
                        if(target != null)
                        {
                            //NPC.velocity = target.velocity * 0.5f;
                            if((spear.Center - NPC.Center).Length() > (target.Center - NPC.Center).Length())
                            {
                                Vector2 attemptToPathFindTo = target.Center + QwertyMethods.PolarVector( MathF.Max((target.Center - NPC.Center).Length(), 100), (target.Center - spear.Center).ToRotation());
                                NPC.velocity = (attemptToPathFindTo - NPC.Center).SafeNormalize(-Vector2.UnitY) * 4;
                            }
                            else
                            {
                                NPC.velocity = Vector2.Zero;
                            }
                            /*
                            float? dir = Pathfinder.PathFindWithNodeSize(NPC.Center, attemptToPathFindTo, 3, 2);
                            if(dir != null)
                            {
                                NPC.velocity = QwertyMethods.PolarVector(10, (float)dir);
                            }
                            else
                            {
                                NPC.velocity = Vector2.Zero;
                            }
                            */
                            if((spear.Center - NPC.Center).Length() > (target.Center - NPC.Center).Length())
                            {
                                if(armFrame < 2)
                                {
                                    NPC.frameCounter++;
                                    if(NPC.frameCounter > 10)
                                    {
                                        NPC.frameCounter = 0;
                                        armFrame++;
                                        
                                    }
                                }
                                if(armFrame == 2)
                                {
                                    spear.ai[1] = 1;
                                }
                            }
                        }
                        else
                        {
                            NPC.velocity = Vector2.Zero;
                            if(armFrame < 2)
                            {
                                NPC.frameCounter++;
                                if(NPC.frameCounter > 10)
                                {
                                    NPC.frameCounter = 0;
                                    armFrame++;
                                    
                                }
                            }
                            if(armFrame == 2)
                            {
                                spear.ai[1] = 1;
                            }
                        }
                        if(spear.velocity == Vector2.Zero)
                        {
                            if(armFrame < 2)
                            {
                                NPC.frameCounter++;
                                if(NPC.frameCounter > 10)
                                {
                                    NPC.frameCounter = 0;
                                    armFrame++;
                                    
                                }
                            }
                            if(armFrame == 2)
                            {
                                spear.ai[1] = 1;
                            }
                        }
                        if(spear.ai[1] == 1)
                        {
                            
                            if((spear.Center - NPC.Center).Length() < 20)
                            {
                                spear.Kill();
                                thrownSpear = false;
                                spear = null;
                                losCounter = 0;
                            }
                        }
                        else if( spear.velocity != Vector2.Zero && target != null && (spear.Center - NPC.Center).Length() <= (target.Center - NPC.Center).Length())
                        {
                            if(armFrame != 0)
                            {
                                NPC.frameCounter++;
                                if(NPC.frameCounter > 10)
                                {
                                    NPC.frameCounter = 0;
                                    armFrame = armFrame == 2 ? 3 : 0;
                                }
                            }
                        }
                    }
                }
                
            }
            if(NPC.velocity != Vector2.Zero)
            {
                dustTimer++;
                if(dustTimer % 10 == 0)
                {
                    int width = 10;
                    int height = 20;
                    int dustCount = width;
                    for (int i = 0; i < dustCount; i++)
                    {
                        float rot = MathF.PI * 2f * ((float)i / dustCount);
                        Vector2 unitVector = QwertyMethods.PolarVector(1f, rot);
                        Dust d = Dust.NewDustPerfect(NPC.Center + new Vector2(unitVector.X * width, unitVector.Y * height).RotatedBy((NPC.velocity * -1f).ToRotation()), ModContent.DustType<InvaderGlow>(), NPC.velocity * -1f);
                        d.noGravity = true;
                        d.frame.Y = 0;
                        d.scale *= 2;
                    }
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Y = armFrame * frameHeight;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Sets the description of this NPC that is listed in the bestiary
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
                new FlavorTextBestiaryInfoElement("The most complex infantry the invaders deploy.")
            });
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<InvaderPlating>(), 1, 5, 9));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<GodSealKeycard>(), 1, 1, 1));
        }


        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.InModBiome(ModContent.GetInstance<FortressBiome>()) && SkyFortress.beingInvaded)
            {
                return 12f;
            }
            return 0f;
        }
        void HeresyOffset(out Vector2 holdOffset, out float rotation)
        {
            holdOffset = Vector2.Zero;
            holdOffset.X = NPC.direction == 1 ? 18 : -18;
            switch(armFrame)
            {
                case 0:
                    holdOffset.Y = 8;
                break;
                case 1:
                    holdOffset.Y = -4;
                break;
                case 2:
                    holdOffset.Y = -24;
                break;
                case 3:
                    holdOffset.Y = 11;
                break;
                case 4:
                    holdOffset.Y = 10;
                break;
            }
            rotation = NPC.direction == 1 ? (MathF.PI / 8f) : (7f * MathF.PI / 8f);
            if(losCounter > 60 && target != null && !spinSpear)
            {
                rotation = QwertyMethods.PredictiveAim(NPC.Center + holdOffset, throwSpeed * 2, target.Center, target.velocity);
                if(float.IsNaN(rotation))
                {
                    rotation = (target.Center - (NPC.Center + holdOffset)).ToRotation();
                }
            }
            if(spinSpear)
            {
                rotation += spun;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if(!thrownSpear)
            {
                HeresyOffset(out Vector2 holdOffset, out float rotation);
            spriteBatch.Draw(ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Invader/Heresy").Value, NPC.Center + holdOffset - screenPos,
            null, drawColor, NPC.rotation + rotation,
            new Vector2(44, 10), 1f, SpriteEffects.None, 0f);
            }
            
            
            Vector2 origin = new Vector2(24, 42);
            spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - screenPos,
            NPC.frame, drawColor, NPC.rotation,
            origin, 1f, NPC.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            spriteBatch.Draw(ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Invader/InvaderElite_Glow").Value, NPC.Center - screenPos,
            NPC.frame, Color.White, NPC.rotation,
            origin, 1f, NPC.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

            

            return false;
        }
    }
    public class Heresy : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 20;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.friendly = true;
            Projectile.hostile = true;
            Projectile.GetGlobalProjectile<InvaderProjectile>().isInvaderProjectile = true;
            Projectile.GetGlobalProjectile<InvaderProjectile>().EvEMultiplier = 5f;
            Projectile.GetGlobalProjectile<EtimsProjectile>().effect = true;
        }
        
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center + QwertyMethods.PolarVector(Projectile.ai[1] == 1 ? -46 : 16, Projectile.rotation), Projectile.Center + QwertyMethods.PolarVector(46, Projectile.rotation));
        }
        public override void AI()
        {
            if(Projectile.ai[1] == 1)
            {
                NPC parent = Main.npc[(int)Projectile.ai[0]];
                if(!parent.active || parent.type != ModContent.NPCType<InvaderElite>())
                {
                    Projectile.Kill();
                    return;
                }
                Projectile.rotation += MathF.PI / 30f;
                Projectile.tileCollide = false;
                Projectile.velocity = (parent.Center - Projectile.Center).SafeNormalize(-Vector2.UnitY) * 6;
                return;
            }
            if(Projectile.velocity == Vector2.Zero) return;
            Projectile.rotation = Projectile.velocity.ToRotation();
        }
        public override bool OnTileCollide(Vector2 velocityChange)
        {
            Projectile.velocity = Vector2.Zero;
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition,
                        new Rectangle(0, Projectile.frame * texture.Height, texture.Width, texture.Height), lightColor, Projectile.rotation,
                        texture.Size() * 0.5f, 1f, SpriteEffects.None, 0);
            return false;
        }

    }
    public class SpinSpearHitbox : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.tileCollide = false;
            Projectile.width = 92;
            Projectile.height = 92;
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