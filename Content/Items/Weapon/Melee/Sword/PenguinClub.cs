using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Melee.Sword
{
    public class PenguinClub : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Penguin Club");
            Tooltip.SetDefault("Launches penguins upon hitting an enemy");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 17;
            Item.DamageType = DamageClass.Melee;

            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = 1;
            Item.knockBack = 2;
            Item.value = 100000;
            Item.rare = 1;
            Item.UseSound = SoundID.Item1;

            Item.width = 48;
            Item.height = 48;

            Item.noMelee = false;
            Item.autoReuse = true;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            Projectile penguin = Main.projectile[Projectile.NewProjectile(player.GetProjectileSource_Item(Item), player.Center, (target.Center - player.Center).SafeNormalize(-Vector2.UnitY) * 6, ProjectileType<SlidingPenguinMelee>(), Item.damage, knockBack, player.whoAmI, ai1: 1)];
        }
    }
}
