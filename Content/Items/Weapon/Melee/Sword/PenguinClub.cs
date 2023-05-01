using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
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
            //DisplayName,SetDefault("Penguin Club");
            //Tooltip.SetDefault("Launches penguins upon hitting an enemy");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 17;
            Item.DamageType = DamageClass.Melee;

            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 2;
            Item.value = 100000;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;

            Item.width = 48;
            Item.height = 48;

            Item.noMelee = false;
            Item.autoReuse = true;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile penguin = Main.projectile[Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center, (target.Center - player.Center).SafeNormalize(-Vector2.UnitY) * 6, ProjectileType<SlidingPenguinMelee>(), Item.damage, hit.Knockback, player.whoAmI, ai1: 1)];
        }
    }
}
