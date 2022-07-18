using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace QwertyMod.Content.Items.Weapon.Ranged.SuperArrow.Aqueous
{
    public class Aqueous : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aqueous");
            Tooltip.SetDefault("Shot from your bow alongside normal arrows");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = .5f;
            Item.value = Item.sellPrice(silver: 54);
            Item.rare = 2;
            Item.width = 2;
            Item.height = 2;
            Item.crit = 20;
            Item.shootSpeed = 12f;
            Item.useTime = 100;
            Item.maxStack = 1;
            Item.shoot = ProjectileType<AqueousP>();
        }

        public override bool CanUseItem(Player player)
        {
            return false;
        }

    }

    public class AqueousP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aqueous");
        }

        protected int assosiatedItemID = -1;

        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.arrow = true;

            Projectile.tileCollide = true;
        }


        public override void AI()
        {
            if (Main.rand.Next(6) == 0)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 172);
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            crit = Main.rand.Next(100) < ((int)Projectile.localAI[0] + Main.player[Projectile.owner].GetCritChance(DamageClass.Ranged));
        }
    }
}