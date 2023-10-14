using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

using QwertyMod.Content.Items.MiscMaterials;


namespace QwertyMod.Content.Items.Weapon.Ranged.DartLauncher.SuperSkyShot
{
    public class SuperSkyShot : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 40;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 1;
            Item.value = 50000;
            Item.rare = ItemRarityID.Orange;
            Item.width = 38;
            Item.height = 32;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shootSpeed = 12f;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.shoot = ProjectileID.PoisonDart;
            Item.useAmmo = AmmoID.Dart;
            Item.noUseGraphic = false;
            Item.noMelee = true;
            Item.UseSound = SoundID.Item63;
            Item.autoReuse = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float r = (velocity).ToRotation() - MathF.PI / 2f;
            Projectile.NewProjectile(source, position + QwertyMethods.PolarVector(4f * player.direction, r), velocity, type, damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position + QwertyMethods.PolarVector(6f * player.direction, r), velocity * 1.5f, ModContent.ProjectileType<SkyShot.CaeliteDart>(), damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position + QwertyMethods.PolarVector(8f * player.direction, r), velocity, type, damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position + QwertyMethods.PolarVector(10f * player.direction, r), velocity * 1.5f, ModContent.ProjectileType<SkyShot.CaeliteDart>(), damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position + QwertyMethods.PolarVector(12f * player.direction, r), velocity, type, damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position + QwertyMethods.PolarVector(14f * player.direction, r), velocity * 1.5f, ModContent.ProjectileType<SkyShot.CaeliteDart>(), damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position + QwertyMethods.PolarVector(16f * player.direction, r), velocity, type, damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position + QwertyMethods.PolarVector(18f * player.direction, r), velocity * 1.5f, ModContent.ProjectileType<SkyShot.CaeliteDart>(), damage, knockback, player.whoAmI);
            return false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-4, -4);
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ModContent.ItemType<SkyShot.SkyShot>())
            .AddIngredient(ModContent.ItemType<SoulOfHeight>(), 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

    }
}
