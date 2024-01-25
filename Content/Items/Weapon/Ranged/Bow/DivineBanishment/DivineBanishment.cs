using Microsoft.Xna.Framework;
using QwertyMod.Content.Items.MiscMaterials;
using QwertyMod.Content.Items.Weapon.Ranged.Bow.HolyExiler;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertyMod.Content.Items.Weapon.Ranged.Bow.DivineBanishment
{
    public class DivineBanishment : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 90;
            Item.DamageType = DamageClass.Ranged;

            Item.useTime = 22;
            Item.useAnimation = 22;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 2f;
            Item.value = 50000;
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item5;

            Item.width = 20;
            Item.height = 50;

            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.useAmmo = AmmoID.Arrow;
            Item.shootSpeed = 16f;
            Item.noMelee = true;
            Item.autoReuse = true;
        }

        public Projectile arrow;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            arrow = Main.projectile[Projectile.NewProjectile(source, position + QwertyMethods.PolarVector(6, velocity.ToRotation() + MathF.PI/2), velocity, type, damage, knockback, player.whoAmI)];
            arrow.GetGlobalProjectile<ArrowWarping>().warpedArrow = true;
            if(Main.netMode != NetmodeID.SinglePlayer)
            {
                ModPacket packet = Mod.GetPacket();
                packet.Write((byte)ModMessageType.AmmoEnchantArrowWarping);
                packet.Write(arrow.identity);
                packet.Send();
            }

            arrow = Main.projectile[Projectile.NewProjectile(source, position + QwertyMethods.PolarVector(-6, velocity.ToRotation() + MathF.PI/2), velocity, type, damage, knockback, player.whoAmI)];
            arrow.GetGlobalProjectile<ArrowWarping>().warpedArrow = true;
            if(Main.netMode != NetmodeID.SinglePlayer)
            {
                ModPacket packet = Mod.GetPacket();
                packet.Write((byte)ModMessageType.AmmoEnchantArrowWarping);
                packet.Write(arrow.identity);
                packet.Send();
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ModContent.ItemType<HolyExiler.HolyExiler>())
            .AddIngredient(ModContent.ItemType<SoulOfHeight>(), 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    
}