using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Content.Buffs;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Melee.Javelin.Imperium
{
    public class Imperium : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.shootSpeed = 17f;
            Item.damage = 160;
            Item.knockBack = 5f;
            Item.useStyle = 1;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.width = 68;
            Item.height = 68;
            Item.maxStack = 1;
            Item.rare = 7;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.consumable = false;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Melee;

            Item.autoReuse = true;
            Item.UseSound = SoundID.Item1;
            Item.shoot = ProjectileType<ImperiumP>();
        }
    }

    public class ImperiumP : Javelin
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Imperium");
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
            Projectile.GetGlobalProjectile<ImplaingProjectile>().damagePerImpaler = 32;
            maxStickingJavelins = 12;
            dropItem = ItemType<Imperium>();
            rotationOffset = (float)Math.PI / 4;
            maxTicks = 60f;
            maxStickingJavelins = 10;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(texture, new Vector2(Projectile.Center.X - Main.screenPosition.X, Projectile.Center.Y - Main.screenPosition.Y + 2),
                        new Rectangle(0, 0, texture.Width, texture.Height), lightColor, Projectile.rotation,
                        new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f), 1f, SpriteEffects.None, 0);
            return false;
        }
    }
}