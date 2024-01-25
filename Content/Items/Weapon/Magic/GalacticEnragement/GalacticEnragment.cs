
using Microsoft.Xna.Framework;
using QwertyMod.Content.Items.MiscMaterials;
using QwertyMod.Content.Items.Weapon.Magic.RestlessSun;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertyMod.Content.Items.Weapon.Magic.GalacticEnragement
{
    public class GalacticEnragment : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.DamageType = DamageClass.Magic;
            Item.knockBack = 1;
            Item.value = GearStats.TrueCaeliteWeaponValue;
            Item.rare = ItemRarityID.Orange;
            Item.width = 38;
            Item.height = 16;
            Item.useStyle = 20;
            Item.shootSpeed = 12f;
            Item.useTime = 5;
            Item.useAnimation = 5;
            Item.mana = ModLoader.HasMod("TRAEProject") ? 7 : 5;
            Item.shoot = ModContent.ProjectileType<CaeliteMagicProjectile>();
            Item.noUseGraphic = false;
            Item.noMelee = true;
            Item.UseSound = SoundID.Item21;
            Item.autoReuse = true;
        }
        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            player.bodyFrame.Y = player.bodyFrame.Height * 5; //force the player to a specific frame
            player.itemRotation = 0;
            Vector2 vector24 = Main.OffsetsPlayerOnhand[player.bodyFrame.Y / 56] * 2f;
            if (player.direction != 1)
            {
                vector24.X = player.bodyFrame.Width - vector24.X;
            }
            if (player.gravDir != 1f)
            {
                vector24.Y = player.bodyFrame.Height - vector24.Y;
            }
            vector24 -= new Vector2(player.bodyFrame.Width - player.width, player.bodyFrame.Height - 42) / 2f;
            vector24.Y -= 3;
            vector24.X += -player.direction * Item.width / 4f; 
            player.itemLocation = player.position + vector24;
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ModContent.ItemType<CaeliteMagicWeapon>())
            .AddIngredient(ModContent.ItemType<SoulOfHeight>(), 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for(int i =0; i <3; i++)
            {
                Vector2 pos =  position + Vector2.UnitY * -1000f;
                pos.X += Main.rand.NextFloat() * 1000f - 500f;
                pos.Y += Main.rand.NextFloat() * 400f - 200f;
                float speed = velocity.Length();
                float direction = (Main.MouseWorld - pos).ToRotation();
                Projectile.NewProjectile(source, pos, QwertyMethods.PolarVector(speed, direction), type, damage, knockback, player.whoAmI);
            }
            return false;
        }
    }
}