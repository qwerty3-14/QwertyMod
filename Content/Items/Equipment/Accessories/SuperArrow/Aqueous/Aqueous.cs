using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;


namespace QwertyMod.Content.Items.Equipment.Accessories.SuperArrow.Aqueous
{
    public class Aqueous : ModItem
    {

        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Aqueous");
            //Tooltip.SetDefault("Shot from your bow alongside normal arrows");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 80;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = .5f;
            Item.value = Item.sellPrice(silver: 54);
            Item.rare = ItemRarityID.Green;
            Item.width = 2;
            Item.height = 2;
            Item.crit = 20;
            Item.shootSpeed = 12f;
            Item.useTime = 180;
            Item.maxStack = 1;
            Item.shoot = ProjectileType<AqueousP>();
            Item.accessory = true;
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
            //DisplayName,SetDefault("Aqueous");
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
            if (Main.rand.NextBool(6))
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.DungeonWater);
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if(Main.rand.Next(100) < ((int)Projectile.localAI[0] + Main.player[Projectile.owner].GetCritChance(DamageClass.Ranged)))
            {
                modifiers.SetCrit();
            }
        }
    }
}