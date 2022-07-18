using Microsoft.Xna.Framework;
using Terraria;
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
            //ItemID.Sets.SummonerWeaponThatScalesWithAttackSpeed[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.autoReuse = false;
            Item.useStyle = 1;
            Item.useTime = Item.useAnimation = 20;
            Item.width = 18;
            Item.height = 18;
            Item.shoot = ProjectileType<DisciplineP>();
            Item.UseSound = SoundID.Item152;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Summon;
            Item.noUseGraphic = true;
            Item.damage = 67;
            Item.knockBack = 3f;
            Item.shootSpeed = 4f;
            Item.rare = 7;
            Item.value = Item.sellPrice(0, 10, 0, 0);
        }
    }
    public class DisciplineP : WhipProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Discipline");
            //ProjectileID.Sets.IsAWhip[Type] = true;
        }
        public override void WhipDefaults()
        {
            originalColor = new Color(128, 39, 83);
            whipRangeMultiplier = 2f;
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
