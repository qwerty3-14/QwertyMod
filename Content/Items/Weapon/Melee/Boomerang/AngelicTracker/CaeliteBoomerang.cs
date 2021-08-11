using Microsoft.Xna.Framework;
using QwertyMod.Content.Buffs;
using QwertyMod.Content.Items.Consumable.Tile.Bars;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Melee.Boomerang.AngelicTracker
{
    public class CaeliteBoomerang : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Angelic Tracker");
            Tooltip.SetDefault("Higher beings will guide your boomerang!");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 38;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;

            Item.useTime = 34;
            Item.useAnimation = 34;
            Item.useStyle = 5;
            Item.knockBack = 0;
            Item.value = 50000;
            Item.rare = 3;
            Item.UseSound = SoundID.Item1;
            Item.noUseGraphic = true;
            Item.width = 18;
            Item.height = 32;

            Item.autoReuse = true;
            Item.shoot = ProjectileType<CaeliteBoomerangP>();
            Item.shootSpeed = 15;
            Item.channel = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemType<CaeliteBar>(), 12)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override bool CanUseItem(Player player)
        {
            for (int i = 0; i < 1000; ++i)
            {
                if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == Item.shoot)
                {
                    return false;
                }
            }
            return true;
        }
    }

    public class CaeliteBoomerangP : ModProjectile
    {
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
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Caelite Boomerang");
        }

        private float speed;
        private float maxSpeed;
        private bool runOnce = true;
        private float decceleration = 1f / 3f;
        private int spinDirection;
        private bool returnToPlayer;
        private NPC ConfirmedTarget;
        private int timerAfterReturning;

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
                Projectile.velocity = Projectile.velocity.SafeNormalize(-Vector2.UnitY) * speed;
                speed -= decceleration;
                if (speed < 1f)
                {
                    returnToPlayer = true;
                }
            }
            //Main.NewText("MaxSpeed: " + maxSpeed);
            //Main.NewText("speed: " + speed);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.Next(10) == 0)
            {
                target.AddBuff(BuffType<PowerDown>(), 120);
            }
            Projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[Projectile.owner] = 0;
            if (!returnToPlayer)
            {
                if (QwertyMethods.ClosestNPC(ref ConfirmedTarget, 300, Projectile.Center, specialCondition: delegate (NPC possibleTarget) { return Projectile.localNPCImmunity[possibleTarget.whoAmI] == 0; }))
                {
                    Projectile.velocity = QwertyMethods.PolarVector(maxSpeed, (ConfirmedTarget.Center - Projectile.Center).ToRotation());
                    speed = maxSpeed;
                }
                else
                {
                    returnToPlayer = true;
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            returnToPlayer = true;
            return false;
        }
    }
}