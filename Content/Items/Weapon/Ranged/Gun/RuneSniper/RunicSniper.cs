using Microsoft.Xna.Framework;
using QwertyMod.Content.Items.MiscMaterials;
using QwertyMod.Content.Items.Weapon.Ranged.Gun.Ancient;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
namespace QwertyMod.Content.Items.Weapon.Ranged.Gun.RuneSniper
{
    public class RunicSniper : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Runic Sniper");
            Tooltip.SetDefault("x2 damage to enemies far away from you" + "\nRight click to zoom");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }


        public override void SetDefaults()
        {
            Item.damage = 260;
            Item.DamageType = DamageClass.Ranged;
            Item.autoReuse = true;
            Item.useTime = 35;
            Item.useAnimation = 35;
            Item.useStyle = 5;
            Item.knockBack = 5;
            Item.value = 500000;
            Item.rare = 9;
            Item.UseSound = SoundID.Item11;

            Item.width = 74;
            Item.height = 30;
            Item.crit = 25;
            Item.shoot = 97;
            Item.useAmmo = AmmoID.Bullet;
            Item.shootSpeed = 36;
            Item.noMelee = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemType<CraftingRune>(), 15)
                .AddIngredient(ItemType<AncientSniper>())
                .AddTile(TileID.Anvils)
                .Register();
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, -6);
        }

        public override void HoldItem(Player player)
        {
            player.scope = true;
        }
    }

    public class DoubleSnipeDamage : ModPlayer
    {
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (proj.CountsAsClass(DamageClass.Ranged) && Player.inventory[Player.selectedItem].type == ItemType<RunicSniper>())
            {
                if ((target.Center - Player.Center).Length() > 700)
                    damage *= 2;
            }
        }
    }
}