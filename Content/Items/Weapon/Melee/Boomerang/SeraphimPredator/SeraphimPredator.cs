using Microsoft.Xna.Framework;
using QwertyMod.Content.Buffs;
using QwertyMod.Content.Items.Consumable.Tiles.Bars;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Content.Items.MiscMaterials;

namespace QwertyMod.Content.Items.Weapon.Melee.Boomerang.SeraphimPredator
{
    public class SeraphimPredator : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Angelic Tracker");
            //Tooltip.SetDefault("Higher beings will guide your boomerang!");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 300;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.noMelee = true;

            Item.useTime = 26;
            Item.useAnimation = 26;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 2;
            Item.value = GearStats.TrueCaeliteWeaponValue;
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item1;
            Item.noUseGraphic = true;
            Item.width = 18;
            Item.height = 32;

            Item.autoReuse = true;
            Item.shoot = ProjectileType<SeraphimPredatorP>();
            Item.shootSpeed = 15;
            Item.channel = true;
        }
        
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemType<AngelicTracker.CaeliteBoomerang>())
            .AddIngredient(ItemType<SoulOfHeight>(), 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    public class SeraphimPredatorP : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 16;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            //Projectile.aiStyle = ProjectileID.WoodenBoomerang;
            //aiType = 52;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.width = 18;
            Projectile.height = 32;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.extraUpdates = 2;
        }

        private float speed;
        private float maxSpeed;
        private bool runOnce = true;
        private float decceleration = 1f / 3f;
        private int spinDirection;
        private bool returnToPlayer;
        private NPC ConfirmedTarget;
        private int timerAfterReturning;
        float acc = 1f;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (runOnce)
            {
                spinDirection = player.direction;
                speed = Projectile.velocity.Length();
                maxSpeed = speed;
                runOnce = false;
            }
            Projectile.rotation += MathHelper.ToRadians(maxSpeed * spinDirection);
            if (returnToPlayer)
            {
                timerAfterReturning++;
                if (timerAfterReturning == 30)
                {
                    for (int k = 0; k < 200; k++)
                    {
                        Projectile.localNPCImmunity[k] = 0;
                    }
                }

                if (Collision.CheckAABBvAABBCollision(player.position, player.Size, Projectile.position, Projectile.Size))
                {
                    Projectile.Kill();
                }
                Projectile.tileCollide = false;
                //Projectile.friendly = false;
                Projectile.velocity = QwertyMethods.PolarVector(speed, (player.Center - Projectile.Center).ToRotation());
                speed += decceleration;
                if (speed > maxSpeed)
                {
                    speed = maxSpeed;
                }
            }
            else
            {
                if (QwertyMethods.ClosestNPC(ref ConfirmedTarget, 700, Projectile.Center, specialCondition: delegate (NPC possibleTarget) { return Projectile.localNPCImmunity[possibleTarget.whoAmI] == 0; }))
                {
                    float angDiffRatio = QwertyMethods.AngularDifference((ConfirmedTarget.Center - Projectile.Center).ToRotation(), Projectile.velocity.ToRotation()) / MathF.PI;
                    Projectile.velocity += QwertyMethods.PolarVector(angDiffRatio * 0.5f * acc, (-Projectile.velocity).ToRotation());
                    Projectile.velocity += QwertyMethods.PolarVector(acc, (ConfirmedTarget.Center - Projectile.Center).ToRotation());
                    
                    if(Projectile.velocity.Length() > maxSpeed)
                    {
                        Projectile.velocity = Projectile.velocity.SafeNormalize(-Vector2.UnitY) * maxSpeed;
                    }
                    speed = Projectile.velocity.Length();
                }
                else
                {
                    Projectile.velocity = Projectile.velocity.SafeNormalize(-Vector2.UnitY) * speed;
                    speed -= decceleration;
                    if (speed < 1f)
                    {
                        returnToPlayer = true;
                    }
                }
            }
            //Main.NewText("MaxSpeed: " + maxSpeed);
            //Main.NewText("speed: " + speed);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(10))
            {
                target.AddBuff(BuffType<PowerDown>(), 120);
            }
            Projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[Projectile.owner] = 0;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y;
            }
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, drawPos, new Rectangle(0, Projectile.frame * Projectile.height, Projectile.width, Projectile.height), color, Projectile.oldRot[k], drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }
            
            return base.PreDraw(ref lightColor);
        }
    }
}