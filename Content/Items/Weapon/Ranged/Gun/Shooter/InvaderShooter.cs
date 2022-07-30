using Microsoft.Xna.Framework;
using QwertyMod.Content.Items.MiscMaterials;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common.PlayerLayers;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.DataStructures;
using System;

namespace QwertyMod.Content.Items.Weapon.Ranged.Gun.Shooter
{
    public class InvaderShooter : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Invader Shooter");
            Tooltip.SetDefault("Fires 2 bullets at once");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }


        public override void SetDefaults()
        {
            Item.damage = 100;
            Item.DamageType = DamageClass.Ranged;
            Item.autoReuse = true;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = 5;
            Item.knockBack = 5;
            Item.value = QwertyMod.InvaderGearValue;
            Item.rare = 8;
            Item.UseSound = SoundID.Item11;
            if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("QwertyMod/Content/Items/Weapon/Ranged/Gun/Shooter/InvaderShooter_Glow").Value;
            }
            Item.width = 74;
            Item.height = 30;
            Item.shoot = 97;
            Item.useAmmo = AmmoID.Bullet;
            Item.shootSpeed = 36;
            Item.noMelee = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float r = (velocity).ToRotation();
            position += QwertyMethods.PolarVector(0, r) + QwertyMethods.PolarVector(-5 * player.direction, r + (float)Math.PI / 2);
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            position += QwertyMethods.PolarVector(2, r) + QwertyMethods.PolarVector(3 * player.direction, r + (float)Math.PI / 2);
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<InvaderPlating>(), 30)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-12, 0);
        }

        public override void HoldItem(Player player)
        {
            player.scope = true;
        }
    }
}