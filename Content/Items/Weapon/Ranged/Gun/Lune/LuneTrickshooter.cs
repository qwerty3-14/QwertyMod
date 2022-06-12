using Microsoft.Xna.Framework;
using QwertyMod.Content.Items.Consumable.Tiles.Bars;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Ranged.Gun.Lune
{
    public class LuneTrickshooter : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lune Trickshooter");
            Tooltip.SetDefault("Musket balls are converted to Lune trick shots!" + "\nTrick shots can bounce off walls 3 times and gain significant damage if they do so");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 32;
            Item.DamageType = DamageClass.Ranged;
            Item.crit = 15;
            Item.useTime = 28;
            Item.useAnimation = 28;
            Item.useStyle = 5;
            Item.knockBack = 1;
            Item.value = 20000;
            Item.rare = 1;
            Item.UseSound = SoundID.Item11;

            Item.width = 54;
            Item.height = 30;

            Item.shoot = 97;
            Item.useAmmo = 97;
            Item.shootSpeed = 9f;
            Item.noMelee = true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-4, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (type == ProjectileID.Bullet)
            {
                type = ProjectileType<Trickshot>();
            }
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemType<LuneBar>(), 12)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class Trickshot : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.light = 0.5f;
            //Projectile.alpha = 255;
            Projectile.scale = 1.2f;
            Projectile.timeLeft = 600;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.extraUpdates = 1;
        }

        private int bounceCounter = 3;

        public override bool OnTileCollide(Vector2 velocityChange)
        {
            if (bounceCounter > 0)
            {
                if (Projectile.velocity.X != velocityChange.X)
                {
                    Projectile.velocity.X = -velocityChange.X;
                }
                if (Projectile.velocity.Y != velocityChange.Y)
                {
                    Projectile.velocity.Y = -velocityChange.Y;
                }
                Projectile.damage = (int)(Projectile.damage * 1.5f);
                bounceCounter--;
                return false;
            }

            return true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            bounceCounter = 3;
        }
    }
}