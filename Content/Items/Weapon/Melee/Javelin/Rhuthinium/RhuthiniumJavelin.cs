using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Content.Buffs;
using QwertyMod.Content.Dusts;
using QwertyMod.Content.Items.Consumable.Tiles.Bars;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

//copied from example javelin forom example mod
namespace QwertyMod.Content.Items.Weapon.Melee.Javelin.Rhuthinium
{
    public class RhuthiniumJavelin : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rhuthinium Javelin");
            Tooltip.SetDefault("Each javelin stuck in the enemy causes them to take 1 extra damage\nMax: 10");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }


        public override void SetDefaults()
        {
            // Alter any of these values as you see fit, but you should probably keep useStyle on 1, as well as the noUseGraphic and noMelee bools
            Item.shootSpeed = 10f;
            Item.damage = 20;
            Item.knockBack = 5f;
            Item.useStyle = 1;
            Item.useAnimation = 18;
            Item.useTime = 18;
            Item.width = 68;
            Item.height = 68;
            Item.rare = 5;
            Item.crit = 5;
            Item.value = 25000;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.MeleeNoSpeed;

            Item.UseSound = SoundID.Item1;

            Item.shoot = ProjectileType<RhuthiniumJavelinP>();
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemType<RhuthiniumBar>(), 8)
                .AddTile(TileID.Anvils)
                .Register();
        }
        /*
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float angle = (velocity).ToRotation();
            float trueSpeed = (velocity).Length();
            Projectile.NewProjectile(source, player.MountedCenter.X, player.MountedCenter.Y, (float)Math.Cos(angle + MathHelper.ToRadians(Main.rand.Next(-5, 6))) * trueSpeed, (float)Math.Sin(angle + MathHelper.ToRadians(Main.rand.Next(-5, 6))) * trueSpeed, type, damage, knockback, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(source, player.MountedCenter.X, player.MountedCenter.Y, (float)Math.Cos(angle + MathHelper.ToRadians(Main.rand.Next(-5, 6))) * trueSpeed, (float)Math.Sin(angle + MathHelper.ToRadians(Main.rand.Next(-5, 6))) * trueSpeed, type, damage, knockback, Main.myPlayer, 0f, 0f);
            return false;
        }
        */
    }

    public class RhuthiniumJavelinP : Javelin
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("RhuthiniumJavelin");
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
            Projectile.GetGlobalProjectile<ImplaingProjectile>().damagePerImpaler = 18;
            maxStickingJavelins = 10;
            dropItem = ItemType<RhuthiniumJavelin>();
            rotationOffset = (float)Math.PI / 4;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(texture, new Vector2(Projectile.Center.X - Main.screenPosition.X, Projectile.Center.Y - Main.screenPosition.Y + 2),
                        new Rectangle(0, 0, texture.Width, texture.Height), lightColor, Projectile.rotation,
                        new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f), 1f, SpriteEffects.None, 0);
            return false;
        }

        public override void ExtraKill(int timeLeft)
        {
            for (int i = 0; i < 18; i++)
            {
                Vector2 dustPos = Projectile.Center + QwertyMethods.PolarVector(Main.rand.Next(80), Projectile.rotation + (float)Math.PI / 4);
                Dust d = Main.dust[Dust.NewDust(dustPos, Projectile.width, Projectile.height, DustType<RhuthiniumDust>())];
                d.noGravity = true;
            }
        }
        public override void StuckEffects(NPC victim)
        {
            victim.GetGlobalNPC<JavelinAilments>().ruthJavs++;
        }
    }
}