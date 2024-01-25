using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertyMod.Content.Items.Weapon.Ranged.SpecialAmmo
{
    public class PenguinLauncher : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.DamageType = DamageClass.Ranged;

            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 5;
            Item.value = 100000;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item11;

            Item.width = 44;
            Item.height = 18;

            Item.shoot = ModContent.ProjectileType<SlidingPenguinRanged>();
            Item.useAmmo = ItemID.Penguin;
            Item.shootSpeed = 6;
            Item.noMelee = true;
            Item.autoReuse = true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-18, -1);
        }
    }

    public class PenguinAmmo : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (item.type == ItemID.Penguin)
            {
                item.ammo = ItemID.Penguin;
                item.shoot = ModContent.ProjectileType<SlidingPenguinRanged>();
            }
        }
    }
}
