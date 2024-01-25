using Microsoft.Xna.Framework;
using QwertyMod.Content.Items.Consumable.Tiles.Bars;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertyMod.Content.Items.Weapon.Ranged.Gun.Lune
{
    public class LuneTrickshooter : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 32;
            Item.DamageType = DamageClass.Ranged;
            Item.crit = 15;
            Item.useTime = 28;
            Item.useAnimation = 28;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 1;
            Item.value = 20000;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item11;

            Item.width = 54;
            Item.height = 30;

            Item.shoot = ProjectileID.Bullet;
            Item.useAmmo = AmmoID.Bullet;
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
                type = ModContent.ProjectileType<Trickshot>();
            }
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<LuneBar>(), 12)
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

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            bounceCounter = 3;
        }
    }
}