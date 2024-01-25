using Microsoft.Xna.Framework;
using QwertyMod.Content.Items.MiscMaterials;
using QwertyMod.Content.Items.Weapon.Ranged.Gun.Ancient;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Weapon.Ranged.Gun.RuneSniper
{
    public class RunicSniper : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }


        public override void SetDefaults()
        {
            Item.damage = 260;
            Item.DamageType = DamageClass.Ranged;
            Item.autoReuse = true;
            Item.useTime = 35;
            Item.useAnimation = 35;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 5;
            Item.value = 500000;
            Item.rare = ItemRarityID.Cyan;
            Item.UseSound = SoundID.Item11;

            Item.width = 74;
            Item.height = 30;
            Item.crit = 25;
            Item.shoot = ProjectileID.Bullet;
            Item.useAmmo = AmmoID.Bullet;
            Item.shootSpeed = 36;
            Item.noMelee = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<CraftingRune>(), 15)
                .AddIngredient(ModContent.ItemType<AncientSniper>())
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
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (proj.CountsAsClass(DamageClass.Ranged) && Player.inventory[Player.selectedItem].type == ModContent.ItemType<RunicSniper>())
            {
                if ((target.Center - Player.Center).Length() > 700)
                    modifiers.FinalDamage *= 2;
            }
        }
    }
}