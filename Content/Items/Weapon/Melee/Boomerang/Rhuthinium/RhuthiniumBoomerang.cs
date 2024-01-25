using Microsoft.Xna.Framework;
using QwertyMod.Content.Dusts;
using QwertyMod.Content.Items.Consumable.Tiles.Bars;
using System;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertyMod.Content.Items.Weapon.Melee.Boomerang.Rhuthinium
{
    public class RhuthiniumBoomerang : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }


        public override void SetDefaults()
        {
            Item.damage = 26;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.noMelee = true;
            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 0;
            Item.value = 25000;
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item1;
            Item.noUseGraphic = true;
            Item.width = 28;
            Item.height = 32;
            Item.crit = 5;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<RhuthiniumBoomerangP>();
            Item.shootSpeed = 10;
            Item.channel = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<RhuthiniumBar>(), 8)
                .AddTile(TileID.Anvils)
                .Register();
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] <= 0;
        }
    }

    public class RhuthiniumBoomerangP : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.width = 28;
            Projectile.height = 32;
            Projectile.DamageType = DamageClass.Melee;
        }
        public int timer;
        public bool runOnce = true;
        public int spinDirection;
        public Vector2 origonalVelocity;

        public override void AI()
        {
            if (Main.rand.NextBool(10))
            {
                Dust d = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<RhuthiniumDust>())];
                d.frame.Y = Main.rand.NextBool(2) ? 0 : 10;
                d.noGravity = true;
            }
            Player player = Main.player[Projectile.owner];
            if (runOnce)
            {
                spinDirection = player.direction;
                origonalVelocity = Projectile.velocity;
                runOnce = false;
            }
            Projectile.rotation += MathHelper.ToRadians(20 * spinDirection);
            timer++;
            switch((int)Projectile.ai[2])
            {
                case 0:
                    Projectile.velocity = origonalVelocity;
                break;
                case 1:
                    Projectile.velocity.X = 0;
                    Projectile.velocity.Y = 0;
                break;
                case 2:
                    Projectile.tileCollide = false;
                    float speed = 10;
                    float direction = (player.Center - Projectile.Center).ToRotation();
                    Projectile.velocity.X = speed * MathF.Cos(direction);
                    Projectile.velocity.Y = speed * MathF.Sin(direction);
                    float distance = MathF.Sqrt((player.Center.X - Projectile.Center.X) * (player.Center.X - Projectile.Center.X) + (player.Center.Y - Projectile.Center.Y) * (player.Center.Y - Projectile.Center.Y));
                    if (distance < 10)
                    {
                        Projectile.Kill();
                    }
                break;
            }
            if(Main.myPlayer == Projectile.owner)
            {
                float oldValue = Projectile.ai[2];
                if (Main.mouseRight)
                {
                    Projectile.ai[2] = 1;
                }
                else if (player.channel)
                {
                    Projectile.ai[2] = 0;
                }
                else
                {
                    Projectile.ai[2] = 2;
                }
                if(oldValue != Projectile.ai[2])
                {
                    Projectile.netUpdate = true;
                }
            }
            
        }

        public override bool OnTileCollide(Vector2 velocityChange)
        {
            return false;
        }
    }
}