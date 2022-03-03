using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Weapon.Ranged.DartLauncher.Whirpool
{
    public class Whirlpool : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Whirlpool");
            Tooltip.SetDefault("Uses darts as ammo");
        }

        public override void SetDefaults()
        {
            Item.damage = 75;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 7f;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = 8;
            Item.width = 48;
            Item.height = 30;
            Item.useStyle = 5;
            Item.shootSpeed = 15f;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.shoot = 10;
            Item.useAmmo = AmmoID.Dart;
            Item.noUseGraphic = false;
            Item.noMelee = true;
            Item.UseSound = SoundID.Item95;
            Item.autoReuse = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float r = (velocity).ToRotation();
            position += QwertyMethods.PolarVector(-12f, r) + QwertyMethods.PolarVector(-12f * player.direction, r + (float)Math.PI / 2);
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            int amt = Main.rand.Next(2) + 2;
            for (int i = 0; i < amt; i++)
            {
                Dust.NewDustPerfect(position + QwertyMethods.PolarVector(30f, r), 217, QwertyMethods.PolarVector(Main.rand.NextFloat() * 3f + 1f, r + Main.rand.NextFloat(-(float)Math.PI / 16, (float)Math.PI / 16)), 100);
            }
            return false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-16, -8);
        }
    }
}
