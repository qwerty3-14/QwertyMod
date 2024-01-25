using Microsoft.Xna.Framework;
using QwertyMod.Content.Buffs;
using QwertyMod.Content.Dusts;
using QwertyMod.Content.Items.Consumable.Tiles.Bars;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Weapon.Melee.Boomerang.Lune
{
    public class LuneBoomerang : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 19;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.noMelee = true;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 0;
            Item.value = 20000;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.noUseGraphic = true;
            Item.width = 34;
            Item.height = 34;
            Item.shoot = ModContent.ProjectileType<LuneBoomerangP>();
            Item.shootSpeed = 10f;
            Item.channel = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<LuneBar>(), 12)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class LuneBoomerangP : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.width = 18;
            Projectile.height = 32;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 12;
        }

        private float speed;
        private float maxSpeed;
        private bool runOnce = true;
        private float decceleration = 1f / 4f;
        private int spinDirection;
        private bool returnToPlayer;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (runOnce)
            {
                spinDirection = player.direction;
                speed = Projectile.velocity.Length();
                maxSpeed = speed;
                runOnce = false;
            }
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<LuneDust>());
            Projectile.rotation += MathHelper.ToRadians(maxSpeed * spinDirection);
            if (returnToPlayer)
            {
                if (Collision.CheckAABBvAABBCollision(player.position, player.Size, Projectile.position, Projectile.Size))
                {
                    Projectile.Kill();
                }
                Projectile.tileCollide = false;
                //Projectile.friendly = false;
                Projectile.velocity = QwertyMethods.PolarVector(speed, (player.Center - Projectile.Center).ToRotation());
                speed += decceleration;
                if (speed > maxSpeed)
                {
                    speed = maxSpeed;
                }
            }
            else
            {
                Projectile.velocity = Projectile.velocity.SafeNormalize(-Vector2.UnitY) * speed;
                speed -= decceleration;
                if (speed < 1f)
                {
                    returnToPlayer = true;
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<LuneCurse>(), 60);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            returnToPlayer = true;
            return false;
        }
    }
}