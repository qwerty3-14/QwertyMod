using QwertyMod.Content.Items.MiscMaterials;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Ranged.SuperArrow.BladedArrow
{
    public class BladedArrow : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bladed Arrow");
            Tooltip.SetDefault("Shot from your bow alongside normal arrows");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 50;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = .5f;
            Item.rare = 7;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.width = 2;
            Item.height = 2;
            Item.crit = 25;
            Item.shootSpeed = 12f;
            Item.useTime = 80;
            Item.shoot = ProjectileType<BladedArrowP>();
            Item.maxStack = 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemType<Aqueous.Aqueous>())
                .AddIngredient(ItemType<BladedArrowShaft>())
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

   
    public class BladedArrowP : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            Projectile.width = Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.arrow = true;
            Projectile.tileCollide = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            crit = Main.rand.Next(100) < ((int)Projectile.localAI[0] + Main.player[Projectile.owner].GetCritChance(DamageClass.Ranged));
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[Projectile.owner] = 0;
        }
    }
}