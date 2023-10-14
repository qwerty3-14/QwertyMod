using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertyMod.Content.Items.Weapon.Whip.Discipline
{
    public class Discipline : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {

            Item.DefaultToWhip(ModContent.ProjectileType<DisciplineP>(), 67, 3, 4, 20);
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.rare = ItemRarityID.Lime;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.width = 26;
            Item.height = 40;
        }
    }
    public class DisciplineP : WhipProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.IsAWhip[Type] = true;
        }
        public override void WhipDefaults()
        {
            Projectile.WhipSettings.Segments = 20;
            Projectile.WhipSettings.RangeMultiplier = 2f;

            originalColor = new Color(128, 39, 83);
            fallOff = 0.15f;
            tag = ModContent.BuffType<DisciplineTag>();
        }
    }
    public class DisciplineTag : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
        }
    }
}
