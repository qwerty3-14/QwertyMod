using Microsoft.Xna.Framework;
using QwertyMod.Content.Dusts;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using QwertyMod.Content.Items.Weapon.Ranged.Bow.HolyExiler;
using QwertyMod.Content.Items.MiscMaterials;

namespace QwertyMod.Content.Items.Weapon.Ranged.Bow.DivineBanishment
{
    public class DivineBanishment : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Divine Banishment");
            Tooltip.SetDefault("Higher beings will help you shoot your enemies!");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 90;
            Item.DamageType = DamageClass.Ranged;

            Item.useTime = 30;
            Item.useAnimation = 30;

            Item.useStyle = 5;
            Item.knockBack = 2f;
            Item.value = 50000;
            Item.rare = 3;
            Item.UseSound = SoundID.Item5;

            Item.width = 20;
            Item.height = 50;

            Item.shoot = 40;
            Item.useAmmo = 40;
            Item.shootSpeed = 16f;
            Item.noMelee = true;
            Item.autoReuse = true;
        }

        public Projectile arrow;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            //arrow = Main.projectile[Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI)];
            //arrow.GetGlobalProjectile<ArrowWarping>().warpedArrow = true;
            arrow = Main.projectile[Projectile.NewProjectile(source, position + QwertyMethods.PolarVector(6, velocity.ToRotation() + (float)Math.PI/2), velocity, type, damage, knockback, player.whoAmI)];
            arrow.GetGlobalProjectile<ArrowWarping>().warpedArrow = true;
            arrow = Main.projectile[Projectile.NewProjectile(source, position + QwertyMethods.PolarVector(-6, velocity.ToRotation() + (float)Math.PI/2), velocity, type, damage, knockback, player.whoAmI)];
            arrow.GetGlobalProjectile<ArrowWarping>().warpedArrow = true;
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemType<HolyExiler.HolyExiler>())
            .AddIngredient(ItemType<SoulOfHeight>(), 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    
}