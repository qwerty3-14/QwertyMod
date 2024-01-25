using QwertyMod.Content.Buffs;
using QwertyMod.Content.Items.Consumable.Tiles.Bars;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertyMod.Content.Items.Weapon.Melee.Top.Lune
{
    public class LuneTop : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 28;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.knockBack = 5;
            Item.value = 20000;
            Item.rare = ItemRarityID.Blue;
            Item.width = 30;
            Item.height = 38;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shootSpeed = 4.5f;
            Item.useTime = 32;
            Item.useAnimation = 32;
            Item.shoot = ModContent.ProjectileType<LuneTopP>();
            Item.noUseGraphic = true;
            Item.noMelee = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<LuneBar>(), 12)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class LuneTopP : Top
    {
        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;

            Projectile.width = 30;
            Projectile.height = 38;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;

            Projectile.tileCollide = true;
            friction = .004f;
            enemyFriction = .08f;
        }
        public override void TopHit(NPC target)
        {
            target.AddBuff(ModContent.BuffType<LuneCurse>(), 180);
        }
    }
}