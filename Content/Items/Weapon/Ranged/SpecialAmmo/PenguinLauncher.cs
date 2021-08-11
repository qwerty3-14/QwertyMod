using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Ranged.SpecialAmmo
{
    public class PenguinLauncher : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Penguin Launcher");
            Tooltip.SetDefault("Uses penguins as ammo");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.DamageType = DamageClass.Ranged;

            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = 5;
            Item.knockBack = 5;
            Item.value = 100000;
            Item.rare = 1;
            Item.UseSound = SoundID.Item11;

            Item.width = 82;
            Item.height = 34;

            Item.shoot = ProjectileType<SlidingPenguinRanged>();
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
                item.shoot = ProjectileType<SlidingPenguinRanged>();
            }
        }
    }
}
