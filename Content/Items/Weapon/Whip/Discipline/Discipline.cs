using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Whip.Discipline
{
    public class Discipline : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Discipline");
            Tooltip.SetDefault("Your minions will gain speed when attacking struck enemies");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {

            Item.DefaultToWhip(ModContent.ProjectileType<DisciplineP>(), 67, 3, 4, 20);
            Item.rare = 7;
            Item.value = Item.sellPrice(0, 10, 0, 0);
        }
    }
    public class DisciplineP : WhipProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Discipline");
            ProjectileID.Sets.IsAWhip[Type] = true;
        }
        public override void WhipDefaults()
        {
            Projectile.WhipSettings.Segments = 20;
            Projectile.WhipSettings.RangeMultiplier = 2f;

            originalColor = new Color(128, 39, 83);
            fallOff = 0.15f;
            tag = BuffType<DisciplineTag>();
        }
    }
    public class DisciplineTag : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Disciplined");
            Description.SetDefault("Minions will attack faster");
            Main.debuff[Type] = true;
        }
    }
}
