using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.WorldBuilding;
using QwertyMod.Content.Dusts;
using Terraria.Audio;
using Terraria.GameContent;
using QwertyMod.Content.Items.MiscMaterials;
using QwertyMod.Common.PlayerLayers;

namespace QwertyMod.Content.Items.Weapon.Minion.InvaderCaster
{
    class InvaderCasterStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Invader Caster Staff");
            //Tooltip.SetDefault("Summons an invader caster to fight for you.");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 64;
            Item.mana = 20;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 3f;
            Item.value = QwertyMod.InvaderGearValue;
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item44;
            Item.shoot = ProjectileType<InvaderCasterMinion>();
            Item.DamageType = DamageClass.Summon;
            Item.buffType = BuffType<InvaderCasterB>();
            Item.buffTime = 3600;

            if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = ModContent.Request<Texture2D>("QwertyMod/Content/Items/Weapon/Minion/InvaderCaster/InvaderCasterStaff_Glow").Value;
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.SpawnMinionOnCursor(source, player.whoAmI, type, Item.damage, knockback);
            return false;
        }
    }
    public class InvaderCasterMinion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Invader Caster");
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            Main.projPet[Projectile.type] = true;
        }
        public override bool MinionContactDamage()
        {
            return false;
        }
        public override void SetDefaults()
        {
            Projectile.width = 52;
            Projectile.height = 32;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.knockBack = 10f;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.minion = true;
            Projectile.minionSlots = 1;
            Projectile.timeLeft = 2;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        const int attackCooldown = 20;
        const int attackRecencyCooldown = 120;
        public override void AI()
		{
			//Main.NewText("AI:" + projectile.ai[0] + ", " + projectile.ai[1] + ", " + projectile.localAI[0] + ", " + projectile.localAI[1]);

			bool moveLeft = false;
			bool moveRight = false;

			ManageMinion(Projectile);
			IdlePositioning(Projectile, ref moveLeft, ref moveRight);
			if (Projectile.localAI[0] == 0f)
			{
				ReturnModeCheck(Projectile);
			}
			if (Projectile.ai[0] != 0f) //return mode
			{
				ReturnMode(Projectile);
			}
			else
			{
				Projectile.rotation = 0f;
				Projectile.tileCollide = true;

				//attack recency countdown
				Projectile.localAI[0] -= 1f;
				if (Projectile.localAI[0] < 0f)
				{
					Projectile.localAI[0] = 0f;
				}

				//attack cooldown countdown
				if (Projectile.ai[1] > 0f)
				{
					Projectile.ai[1] -= 1f;
				}
				TargetEnemies(Projectile, ref moveLeft, ref moveRight);
				

				Movement(Projectile, ref moveLeft, ref moveRight);
			}
		}
		
        static void EnterReturnMode(Projectile projectile)
        {
			projectile.ai[0] = 1f;
			projectile.velocity.Y = 0f;
			Poof(projectile);
		}
		static void Poof(Projectile projectile)
        {
            int width = projectile.width / 2;
            int height = projectile.height / 4;
            int dustCount = width;
            for (int i = 0; i < dustCount; i++)
            {
                float rot = MathF.PI * 2f * ((float)i / dustCount);
                Vector2 unitVector = QwertyMethods.PolarVector(1f, rot);
                Dust d = Dust.NewDustPerfect(projectile.Bottom + new Vector2(unitVector.X * width, unitVector.Y * height), ModContent.DustType<InvaderGlow>(), Vector2.UnitY * projectile.height * -0.09f);
                d.noGravity = true;
                d.frame.Y = 0;
                d.scale *= 2;
            }

            for (int i = 0; i < dustCount; i++)
            {
                float rot = MathF.PI * 2f * ((float)i / dustCount);
                Vector2 unitVector = QwertyMethods.PolarVector(1f, rot);
                Dust d = Dust.NewDustPerfect(Main.player[projectile.owner].Top + new Vector2(unitVector.X * width, unitVector.Y * height), ModContent.DustType<InvaderGlow>(), Vector2.UnitY * projectile.height * 0.09f);
                d.noGravity = true;
                d.frame.Y = 0;
                d.scale *= 2;
            }
		}
		static void ManageMinion(Projectile projectile)
        {
			Player player = Main.player[projectile.owner];
			if (!player.active)
			{
				projectile.active = false;
				return;
			}
			if (player.GetModPlayer<MinionManager>().CasterMinion)
			{
				projectile.timeLeft = 2;
			}
		}
		static bool ReturnModeCheck(Projectile projectile)
        {

			Player player = Main.player[projectile.owner];
			Point origin = player.Bottom.ToTileCoordinates();
			int nearGround = 10;
			if (WorldUtils.Find(origin, Searches.Chain(new Searches.Down(nearGround), new GenCondition[]
			{
			new Conditions.IsSolid()
			}), out _))
            {
				if(!Collision.CanHit(player, projectile) || (player.Center - projectile.Center).Length() > 3000)
				{
					EnterReturnMode(projectile);
					return true;
				}
				else if (projectile.localAI[0] == 0f && Math.Abs(player.Center.Y - projectile.Center.Y) > nearGround * 16)
                {
					EnterReturnMode(projectile);
					return true;
				}
			}
			return false;
		}
		static void IdlePositioning(Projectile projectile, ref bool moveLeft, ref bool moveRight)
        {
			Player player = Main.player[projectile.owner];
			int idleMargin = 10;
			int idleDistanceFromPlayer = (50 * (projectile.minionPos + 1)) * player.direction;
			if (player.Center.X < projectile.Center.X - (float)idleMargin + (float)idleDistanceFromPlayer)
			{
				moveLeft = true;
			}
			else if (player.Center.X > projectile.Center.X + (float)idleMargin + (float)idleDistanceFromPlayer)
			{
				moveRight = true;
			}
		}
		static void ReturnMode(Projectile projectile)
        {
			Player player = Main.player[projectile.owner];
			projectile.tileCollide = false;
			projectile.Center = player.Center + Vector2.UnitX * (8 * projectile.minionPos);
			projectile.velocity = player.velocity;
			projectile.ai[0] = 0;

		}
		static void TargetEnemies(Projectile projectile, ref bool moveLeft, ref bool moveRight)
        {
			float minionOffset = 40 * projectile.minionPos;
			

			float maxDistance = 100000f;
			NPC target = null;
			float sphereShotSpeed = 15;
			if (QwertyMethods.ClosestNPC(ref target, maxDistance, projectile.Center, false, projectile.OwnerMinionAttackTargetNPC != null ? projectile.OwnerMinionAttackTargetNPC.whoAmI : -1))
			{
				float horizontalDist = target.Center.X - (projectile.Center.X);
				float distRequired = 20 + 30 * projectile.minionPos;
				if (horizontalDist > distRequired || horizontalDist < -distRequired)
				{
					if (horizontalDist < -distRequired)
					{
						moveLeft = true;
						moveRight = false;
					}
					else if (horizontalDist > distRequired)
					{
						moveRight = true;
						moveLeft = false;
					}
				}
				if (projectile.owner == Main.myPlayer && projectile.ai[1] <=0)
				{
					Vector2 shootFrom = projectile.Top + Vector2.UnitY * 8;
                    if ((target.Center - shootFrom).Length() < 240)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            SoundEngine.PlaySound(SoundID.Item157, shootFrom);
                            Projectile.NewProjectile(projectile.GetSource_FromThis(), shootFrom, QwertyMethods.PolarVector(3, (target.Center - shootFrom).ToRotation()), ModContent.ProjectileType<MinionInvaderZap>(), projectile.damage, projectile.knockBack, projectile.owner);
                        }
                        projectile.localAI[0] = attackRecencyCooldown;
						projectile.ai[1] = attackCooldown;
                    }
					else if(ReturnModeCheck(projectile))
					{

					}
                    else //if(Math.Abs(target.Center.X - projectile.Center.X) < distRequired + 10)
                    {
                        SoundEngine.PlaySound(SoundID.Item157, shootFrom);
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
							float aimAt = QwertyMethods.PredictiveAim(shootFrom, sphereShotSpeed, target.Center, target.velocity);
							if(!float.IsNaN(aimAt))
							{
								Projectile.NewProjectile(projectile.GetSource_FromThis(), shootFrom, QwertyMethods.PolarVector(sphereShotSpeed, aimAt), ModContent.ProjectileType<MinionSphere>(), projectile.damage, projectile.knockBack, projectile.owner);
							}
                        }
                        projectile.localAI[0] = attackRecencyCooldown + 60;
						projectile.ai[1] = attackCooldown + 60;
                    }
				}
			}
		}

		static void Movement(Projectile projectile, ref bool moveLeft, ref bool moveRight)
		{
			Player player = Main.player[projectile.owner];
			bool aboutToHitWall = false;


			if (projectile.localAI[0] == 0f)
			{
				projectile.direction = player.direction;
			}


			float maxVelocityX = 12f;
			float accX = 0.8f;
			if (maxVelocityX < Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y))
			{
				maxVelocityX = Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y);
				accX = 0.3f;
			}

			if (moveLeft)
			{
				if(projectile.velocity.X > 0)
				{
					projectile.velocity.X = 0;
				}
				if ((double)projectile.velocity.X > -3.5)
				{
					projectile.velocity.X -= accX;
				}
				else
				{
					projectile.velocity.X -= accX * 0.25f;
				}
			}
			else if (moveRight)
			{

				if(projectile.velocity.X < 0)
				{
					projectile.velocity.X = 0;
				}
				if ((double)projectile.velocity.X < 3.5)
				{
					projectile.velocity.X += accX;
				}
				else
				{
					projectile.velocity.X += accX * 0.25f;
				}
			}
			else
			{
				projectile.velocity.X *= 0.9f;
				if (projectile.velocity.X >= 0f - accX && projectile.velocity.X <= accX)
				{
					projectile.velocity.X = 0f;
				}
			}

			if (moveLeft || moveRight)
			{
				int tilePosX = (int)(projectile.Center.X) / 16;
				int tilePosY = (int)(projectile.Center.Y) / 16;
				if (moveLeft)
				{
					tilePosX--;
				}
				if (moveRight)
				{
					tilePosX++;
				}
				tilePosX += (int)projectile.velocity.X;
				if (WorldGen.SolidTile(tilePosX, tilePosY))
				{
					aboutToHitWall = true;
				}
			}

			Collision.StepUp(ref projectile.position, ref projectile.velocity, projectile.width, projectile.height, ref projectile.stepSpeed, ref projectile.gfxOffY);
			if (projectile.velocity.Y == 0f)
			{
				if (aboutToHitWall)
				{
					int tilePosX = (int)(projectile.Center.X) / 16;
					int tilePosY = (int)(projectile.Bottom.Y) / 16;
					if (WorldGen.SolidTileAllowBottomSlope(tilePosX, tilePosY) || Main.tile[tilePosX, tilePosY].IsHalfBlock || Main.tile[tilePosX, tilePosY].Slope > 0)
					{

						try
						{
							tilePosX = (int)(projectile.Center.X) / 16;
							tilePosY = (int)(projectile.Center.Y) / 16;
							if (moveLeft)
							{
								tilePosX--;
							}
							if (moveRight)
							{
								tilePosX++;
							}
							tilePosX += (int)projectile.velocity.X;
							if (!WorldGen.SolidTile(tilePosX, tilePosY - 1) && !WorldGen.SolidTile(tilePosX, tilePosY - 2))
							{
								projectile.velocity.Y = -5.1f;
							}
							else if (!WorldGen.SolidTile(tilePosX, tilePosY - 2))
							{
								projectile.velocity.Y = -7.1f;
							}
							else if (WorldGen.SolidTile(tilePosX, tilePosY - 5))
							{
								projectile.velocity.Y = -11.1f;
							}
							else if (WorldGen.SolidTile(tilePosX, tilePosY - 4))
							{
								projectile.velocity.Y = -10.1f;
							}
							else
							{
								projectile.velocity.Y = -9.1f;
							}
						}
						catch
						{
							projectile.velocity.Y = -9.1f;
						}
					}
				}
			}
			if (projectile.velocity.X > maxVelocityX)
			{
				projectile.velocity.X = maxVelocityX;
			}
			if (projectile.velocity.X < 0f - maxVelocityX)
			{
				projectile.velocity.X = 0f - maxVelocityX;
			}
			if (projectile.velocity.X < 0f)
			{
				projectile.direction = -1;
			}
			if (projectile.velocity.X > 0f)
			{
				projectile.direction = 1;
			}
			if (projectile.velocity.X > accX && moveRight)
			{
				projectile.direction = 1;
			}
			if (projectile.velocity.X < 0f - accX && moveLeft)
			{
				projectile.direction = -1;
			}
			if (projectile.direction != 0)
			{
				projectile.spriteDirection = -projectile.direction;
			}

			bool notWalking = projectile.position.X - projectile.oldPosition.X == 0f;
			if (projectile.velocity.Y == 0f)
			{
				projectile.localAI[1] = 0f;
				if (notWalking)
				{
					projectile.frame = 0;
					projectile.frameCounter = 0;
				}
				else if ((double)projectile.velocity.X < -0.8 || (double)projectile.velocity.X > 0.8)
				{
					projectile.frameCounter += (int)Math.Abs(projectile.velocity.X);
					projectile.frameCounter++;
					if (projectile.frameCounter > 6)
					{
						projectile.frame++;
						projectile.frameCounter = 0;
					}
					if (projectile.frame >= 4)
					{
						projectile.frame = 1;
					}
				}
				else
				{
					projectile.frame = 0;
					projectile.frameCounter = 0;
				}
			}
			else if (projectile.velocity.Y < 0f)
			{
				projectile.frameCounter = 0;
				projectile.frame = 0;
			}
			else if (projectile.velocity.Y > 0f)
			{
				projectile.frameCounter = 0;
				projectile.frame = 0;
			}
			projectile.velocity.Y += 0.4f;
			if (projectile.velocity.Y > 10f)
			{
				projectile.velocity.Y = 10f;
			}
		}
		int pulseCounter = 0;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(texture, new Vector2(Projectile.Center.X - Main.screenPosition.X, Projectile.Center.Y - Main.screenPosition.Y),
                        new Rectangle(0, Projectile.frame * texture.Height / 4, texture.Width, texture.Height / 4), lightColor, Projectile.rotation,
                        new Vector2(texture.Width * 0.5f, (float)Projectile.height * 0.5f), 1f, SpriteEffects.None, 0);
			
			pulseCounter++;
            Main.EntitySpriteDraw(Request<Texture2D>("QwertyMod/Content/Items/Weapon/Minion/InvaderCaster/InvaderCasterMinion_Glow").Value, new Vector2(Projectile.Center.X - Main.screenPosition.X, Projectile.Center.Y - Main.screenPosition.Y),
                        new Rectangle(0, ((pulseCounter % 40) / 10) * texture.Height / 4, texture.Width, texture.Height / 4), Color.White, Projectile.rotation,
                        new Vector2(texture.Width * 0.5f, (float)Projectile.height * 0.5f), 1f, SpriteEffects.None, 0);
            return false;
        }
    }
    public class MinionSphere : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Sphere");
            Main.projFrames[Projectile.type] = 5;
        }
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.MagnetSphereBall);
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 600;
        }
        int counter;
        public override void AI()
        {
            counter++;
            if (counter % 10 == 1)
            {
                NPC target = null;
                if (QwertyMethods.ClosestNPC(ref target, 200, Projectile.Center))
                {
					Projectile.velocity *= 0.4f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, QwertyMethods.PolarVector(3, (target.Center - Projectile.Center).ToRotation()), ModContent.ProjectileType<MinionInvaderZap>(), Projectile.damage, 0, Projectile.owner);
                }
            }
            Projectile.frameCounter++;
            if (Projectile.frameCounter % 4 == 0)
            {
                Projectile.frame++;
                if (Projectile.frame >= 5)
                {
                    Projectile.frame = 0;
                }
            }
            Projectile.rotation += MathF.PI / 240f;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.White;
            return base.PreDraw(ref lightColor);
        }
    }
    public class MinionInvaderZap : ModProjectile
    {
        
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Zap");
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 2;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.timeLeft = 75;
            Projectile.extraUpdates = 74;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Summon;
        }

        private bool decaying = false;
        private bool cantHit = false;

        private void StartDecay()
        {
            if (!decaying)
            {
                Projectile.extraUpdates = 0;
                Projectile.timeLeft = 10;
                Projectile.velocity = Vector2.Zero;
                decaying = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[Projectile.owner] = 0;
            cantHit = true;
            Projectile.velocity = Vector2.Zero;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            cantHit = true;
            Projectile.velocity = Vector2.Zero;
            return false;
        }

        private Vector2 start;
        private bool runOnce = true;

        public override void AI()
        {
            if (runOnce)
            {
                runOnce = false;
                start = Projectile.Center;
            }
            if (Projectile.timeLeft == 2 && !decaying)
            {
                StartDecay();
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (!runOnce)
            {
                float rot = (Projectile.Center - (Vector2)start).ToRotation();
                int c = decaying ? (int)(255f * Projectile.timeLeft / 10f) : 255;
                Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, start - Main.screenPosition, null, new Color(c, c, c, c), rot, Vector2.UnitY * 1, new Vector2((Projectile.Center - start).Length() / 2f, 1), SpriteEffects.None, 0);
            }
            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (cantHit)
            {
                return false;
            }
            return base.Colliding(projHitbox, targetHitbox);
        }
    }
}
