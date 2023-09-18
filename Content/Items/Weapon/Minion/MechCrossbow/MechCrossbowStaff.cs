using Microsoft.Xna.Framework;
using QwertyMod.Common;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Minion.MechCrossbow
{
    class MechCrossbowStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Mech Crossbow Staff");
            //Tooltip.SetDefault("Summons a mechanised crossbow to shoot arrows from your inventory at enemies\nWooden arrows are converted into tagging bolts");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 24;
            Item.mana = 20;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 1f;
            Item.value = 35000;
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item44;
            Item.shoot = ProjectileType<MechCrossbowMinion>();
            Item.DamageType = DamageClass.Summon;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(ModContent.BuffType<MechCrossbowB>(), 3600);
            player.SpawnMinionOnCursor(source, player.whoAmI, type, Item.damage, knockback);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemType<LuneArcherMinion.LuneArcherStaff>())
                .AddIngredient(ItemID.SoulofFright, 20)
                .AddIngredient(ItemID.HallowedBar, 12)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    public class MechCrossbowMinion : BowMinionBase
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Mech Crossbow");
            Main.projFrames[Projectile.type] = 1;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            Main.projPet[Projectile.type] = true;
        }
        public override bool MinionContactDamage()
        {
            return false;
        }
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.minion = true;
            Projectile.minionSlots = 1;
            Projectile.timeLeft = 2;

            cycleTIme = 40;
            shootSpeed = 13;
            holdOffset = 10;
            rotSpeed = MathF.PI / 15f;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if(player.GetModPlayer<MinionManager>().MechCrossbow)
            {
                Projectile.timeLeft = 2;
            }
            BowAI();
        }
        protected override void ChangeArrow(ref int arrowType)
        {
            if (arrowType == ProjectileID.WoodenArrowFriendly)
            {
                arrowType = ProjectileType<TaggingBolt>();
            }
        }
    }
    public class TaggingBolt : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Tagging Bolt");
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
        }


        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.arrow = true;
            Projectile.extraUpdates = 1;
            Projectile.tileCollide = true;
            AIType = ProjectileID.Bullet;
        }

    }
}
