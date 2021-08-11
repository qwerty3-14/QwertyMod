using Microsoft.Xna.Framework;
using QwertyMod.Content.Buffs;
using QwertyMod.Content.Dusts;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Melee.Javelin.Hydra
{
    public class HydraJavelin : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydra Javelin");
            Tooltip.SetDefault("Throws three at once!");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.shootSpeed = 10f;
            Item.damage = 35;
            Item.knockBack = 5f;
            Item.useStyle = 1;
            Item.useAnimation = 24;
            Item.useTime = 24;
            Item.width = 58;
            Item.height = 58;
            Item.value = 250000;
            Item.rare = 5;
            Item.consumable = false;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Melee;

            Item.UseSound = SoundID.Item1;

            Item.shoot = ProjectileType<HydraJavelinP>();
        }

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float angle = velocity.ToRotation();
            float trueSpeed = velocity.Length();
            Projectile.NewProjectile(source, player.MountedCenter.X, player.MountedCenter.Y, (float)Math.Cos(angle + MathHelper.ToRadians(-5)) * trueSpeed, (float)Math.Sin(angle + MathHelper.ToRadians(-5)) * trueSpeed, type, damage, knockback, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(source, player.MountedCenter.X, player.MountedCenter.Y, (float)Math.Cos(angle + MathHelper.ToRadians(0)) * trueSpeed, (float)Math.Sin(angle + MathHelper.ToRadians(0)) * trueSpeed, type, damage, knockback, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(source, player.MountedCenter.X, player.MountedCenter.Y, (float)Math.Cos(angle + MathHelper.ToRadians(5)) * trueSpeed, (float)Math.Sin(angle + MathHelper.ToRadians(5)) * trueSpeed, type, damage, knockback, Main.myPlayer, 0f, 0f);
            return false;
        }
    }

    public class HydraJavelinP : Javelin
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydra Javelin");
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.GetGlobalProjectile<ImplaingProjectile>().CanImpale = true;
            Projectile.GetGlobalProjectile<ImplaingProjectile>().damagePerImpaler = 1;
            maxStickingJavelins = 100;
            dropItem = ItemType<HydraJavelin>();
        }

        public override void ExtraAI()
        {
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<HydraBeamGlow>());
        }
    }
}