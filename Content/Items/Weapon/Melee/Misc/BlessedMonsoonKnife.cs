using Microsoft.Xna.Framework;
using QwertyMod.Content.Buffs;
using QwertyMod.Content.Dusts;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Content.Items.MiscMaterials;


namespace QwertyMod.Content.Items.Weapon.Melee.Misc
{
    public class BlessedMonsoonKnife : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Divine Hail Knife");
            //Tooltip.SetDefault("Higher beings will throw these from the sky!");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 24;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.knockBack = 1;
            Item.value = GearStats.TrueCaeliteWeaponValue;
            Item.rare = ItemRarityID.Orange;
            Item.width = 14;
            Item.height = 34;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shootSpeed = 12f;
            Item.useTime = 4;
            Item.useAnimation = 8;
            Item.shoot = ProjectileType<BlessedMonsoonKnifeP>();
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.autoReuse = true;
        }


        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for(int i = 0; i < 4; i++)
            {
                Vector2 newPosition = new Vector2(player.Center.X + -player.direction * 500 + Main.rand.Next(-200, 200), position.Y - 600 + Main.rand.Next(-200, 0));
                float trueSpeed = velocity.Length();
                int shift = Main.rand.Next(-5, 5);
                Vector2 newVelocity = velocity;
                newVelocity.X = MathF.Cos((new Vector2(Main.MouseWorld.X + shift, Main.MouseWorld.Y) - newPosition).ToRotation()) * trueSpeed;
                newVelocity.Y = MathF.Sin((new Vector2(Main.MouseWorld.X + shift, Main.MouseWorld.Y) - newPosition).ToRotation()) * trueSpeed;
                Projectile.NewProjectile(source, newPosition, newVelocity, type, damage, knockback, player.whoAmI);
            }
            return false;
        }
        
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemType<CaeliteRainKnife>())
            .AddIngredient(ItemType<SoulOfHeight>(), 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
        
    }
    public class BlessedMonsoonKnifeP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 3;
        }

        private bool runOnce = true;
        private float outOfPhaseHeight;
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(10))
            {
                target.AddBuff(BuffType<PowerDown>(), 120);
            }
        }

        public override void AI()
        {
            if (Main.rand.NextBool(10))
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustType<CaeliteDust>(), Vector2.Zero);
                dust.frame.Y = 0;
            }
            if (runOnce)
            {
                outOfPhaseHeight = Main.MouseWorld.Y;
                runOnce = false;
            }

            if (Projectile.Center.Y > outOfPhaseHeight)
            {
                Projectile.tileCollide = true;
            }
            //Main.NewText(outOfPhaseHeight);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 6; i++)
            {
                Dust dust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<CaeliteDust>())];
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, drawPos, new Rectangle(0, Projectile.frame * Projectile.height, Projectile.width, Projectile.height), color, Projectile.oldRot[k], drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }
            
            return base.PreDraw(ref lightColor);
        }
    }
}