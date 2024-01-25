using Microsoft.Xna.Framework;
using QwertyMod.Content.Dusts;
using QwertyMod.Content.Items.Consumable.Tiles.Bars;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertyMod.Content.Items.Weapon.Sentry.SkySpiral
{
    public class CaeliteSentryStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 18;
            Item.mana = 20;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 0f;
            Item.value = 25000;
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item44;
            Item.shoot = ModContent.ProjectileType<CaeliteSentry>();
            Item.DamageType = DamageClass.Summon;
            Item.sentry = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ModContent.ItemType<CaeliteBar>(), 12)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.SpawnMinionOnCursor(source, player.whoAmI, type, Item.damage, knockback);
            return false;
        }

    }

    public class CaeliteSentry : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.SentryShot[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.sentry = true;
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.hostile = false;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = Projectile.SentryLifeTime;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Summon;
        }

        private List<NPC> targets;
        private float maxDistance = 700f;
        private int timer;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            player.UpdateMaxTurrets();
            timer++;
            if (timer % 120 == 0)
            {
                if (QwertyMethods.NPCsInRange(ref targets, maxDistance, Projectile.Center))
                {
                    for (int n = 0; n < targets.Count; n++)
                    {
                        NPC.HitInfo hit = new NPC.HitInfo();
                        hit.Damage = Projectile.damage;
                        hit.DamageType = DamageClass.Summon;
                        targets[n].StrikeNPC(hit);
                        float distance = (targets[n].Center - Projectile.Center).Length();
                        for (int d = 0; d < distance; d += 4)
                        {
                            Dust dust = Dust.NewDustPerfect(Projectile.Center + QwertyMethods.PolarVector(d, (targets[n].Center - Projectile.Center).ToRotation()), ModContent.DustType<CaeliteDust>());
                            dust.frame.Y = 0;
                        }
                    }
                }
            }
            Dust dust2 = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<CaeliteDust>())];
            dust2.scale = .5f;
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 10)
            {
                Projectile.frame++;
                if (Projectile.frame >= 4)
                {
                    Projectile.frame = 0;
                }
                Projectile.frameCounter = 0;
            }
        }
    }
}