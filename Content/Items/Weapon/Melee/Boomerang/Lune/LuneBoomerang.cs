using Microsoft.Xna.Framework;
using QwertyMod.Content.Buffs;
using QwertyMod.Content.Dusts;
using QwertyMod.Content.Items.Consumable.Tiles.Bars;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Melee.Boomerang.Lune
{
    public class LuneBoomerang : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lune Boomerang");
            Tooltip.SetDefault("Unlimited, pierces enemies" + "\nInflicts Lune curse making enemies more vulnerable to critical hits");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 19;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.noMelee = true;

            Item.useTime = 20;
            Item.useAnimation = 20;

            Item.useStyle = 5;
            Item.knockBack = 0;
            Item.value = 20000;
            Item.rare = 1;
            Item.UseSound = SoundID.Item1;
            Item.noUseGraphic = true;
            Item.width = 18;
            Item.height = 32;

            Item.shoot = ProjectileType<LuneBoomerangP>();
            Item.shootSpeed = 10f;
            Item.channel = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemType<LuneBar>(), 12)
                .AddTile(TileID.Anvils)
                .Register();
        }

        /*
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
		}*/

    }

    public class LuneBoomerangP : ModProjectile
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
            Projectile.localNPCHitCooldown = 12;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lune Boomerang");
        }

        private float speed;
        private float maxSpeed;
        private bool runOnce = true;
        private float decceleration = 1f / 4f;
        private int spinDirection;
        private bool returnToPlayer;

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
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<LuneDust>());
            Projectile.rotation += MathHelper.ToRadians(maxSpeed * spinDirection);
            if (returnToPlayer)
            {
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
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffType<LuneCurse>(), 60);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            returnToPlayer = true;
            return false;
        }
    }
}