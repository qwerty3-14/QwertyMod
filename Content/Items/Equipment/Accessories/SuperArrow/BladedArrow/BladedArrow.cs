using QwertyMod.Content.Items.MiscMaterials;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Equipment.Accessories.SuperArrow.BladedArrow
{
    public class BladedArrow : ModItem
    {

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 500;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = .5f;
            Item.rare = ItemRarityID.Lime;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.width = 2;
            Item.height = 2;
            Item.crit = 25;
            Item.shootSpeed = 12f;
            Item.useTime = 180;
            Item.shoot = ProjectileType<BladedArrowP>();
            Item.maxStack = 1;
            Item.accessory = true;
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
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.ArmorPenetration += 20;
            if(Main.rand.Next(100) < ((int)Projectile.localAI[0] + Main.player[Projectile.owner].GetCritChance(DamageClass.Ranged)))
            {
                modifiers.SetCrit();
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[Projectile.owner] = 0;
        }
    }
}