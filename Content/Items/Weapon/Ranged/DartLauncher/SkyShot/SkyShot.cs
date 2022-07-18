using Microsoft.Xna.Framework;
using QwertyMod.Content.Buffs;
using QwertyMod.Content.Dusts;
using QwertyMod.Content.Items.Consumable.Tiles.Bars;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Ranged.DartLauncher.SkyShot
{
    public class SkyShot : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sky Shot");
            Tooltip.SetDefault("Uses darts as ammo\nHigher beings have loaded an additional dart.");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 1;
            Item.value = 50000;
            Item.rare = 3;
            Item.width = 38;
            Item.height = 32;
            Item.useStyle = 5;
            Item.shootSpeed = 12f;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.shoot = 10;
            Item.useAmmo = AmmoID.Dart;
            Item.noUseGraphic = false;
            Item.noMelee = true;
            Item.UseSound = SoundID.Item63;
            Item.autoReuse = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float r = (velocity).ToRotation() - (float)Math.PI / 2f;
            Projectile.NewProjectile(source, position + QwertyMethods.PolarVector(4f * player.direction, r), velocity, type, damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position + QwertyMethods.PolarVector(12f * player.direction, r), velocity, ProjectileType<CaeliteDart>(), damage, knockback, player.whoAmI);
            return false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-4, -4);
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemType<CaeliteBar>(), 12)
                .AddTile(TileID.Anvils)
                .Register();
        }

    }
    public class CaeliteDart : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.alpha = 255;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffType<PowerDown>(), 60 * 30);
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 3; i++)
            {
                Dust.NewDustPerfect(Projectile.Center, DustType<CaeliteDust>(), QwertyMethods.PolarVector(Main.rand.NextFloat() * 4f, Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI)));
            }
        }
        public override void AI()
        {
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 3f)
            {
                Projectile.alpha = 0;
            }
            if (Projectile.ai[0] >= 20f)
            {
                Projectile.ai[0] = 20f;

                Projectile.velocity.Y += 0.075f;

            }
        }
    }
}
