using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Magic.Swordpocalypse
{
    public class SwordStormStaff : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Swordpocalypse");
            Tooltip.SetDefault("Unleashes a barrage of swords!");
            Item.staff[Item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 30;
            Item.mana = ModLoader.HasMod("TRAEProject") ? 54 : 10;
            Item.width = 46;
            Item.height = 46;
            Item.useTime = 4;
            Item.useAnimation = 20;
            Item.useStyle = 5;
            Item.noMelee = true;
            Item.knockBack = 1f;
            Item.value = 500000;
            Item.rare = 7;
            Item.UseSound = SoundID.Item43;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<SwordDrop>();
            Item.DamageType = DamageClass.Magic;
            Item.shootSpeed = 12;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-26, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 1; i++)
            {
                float trueSpeed = velocity.Length();
                float rot = velocity.ToRotation();
                Vector2 Rposition = position + QwertyMethods.PolarVector(-1200, rot + Main.rand.NextFloat(-(float)Math.PI / 32, (float)Math.PI / 32));
                Vector2 goHere = Main.MouseWorld + QwertyMethods.PolarVector(Main.rand.NextFloat(-40, 40), rot + (float)Math.PI / 2);
                Vector2 diff = goHere - Rposition;
                float dist = diff.Length();

                Projectile.NewProjectile(source, Rposition, diff.SafeNormalize(-Vector2.UnitY) * trueSpeed, type, damage, knockback, player.whoAmI, dist);
            }

            return false;
        }
    }

    public class SwordDrop : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sworddrop");
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 2;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.localNPCImmunity[target.whoAmI] = Projectile.localNPCHitCooldown; //set local immunity
            target.immune[Projectile.owner] = 0; //disable normal immune mechanic
        }
        public override void AI()
        {
            Projectile.ai[0] -= Projectile.velocity.Length();
            Projectile.tileCollide = Projectile.ai[0] <= 0;
            Projectile.rotation = Projectile.velocity.ToRotation() + (float)Math.PI / 2;
        }
    }
}