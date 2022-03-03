using Microsoft.Xna.Framework;
using QwertyMod.Content.Dusts;
using QwertyMod.Content.Items.Consumable.Tiles.Bars;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Sentry.SkySpiral
{
    public class CaeliteSentryStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sky Spiral Staff");
            Tooltip.SetDefault("Higher beings will punish all enemies near this sentry!");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 18;
            Item.mana = 20;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = 1;
            Item.noMelee = true;
            Item.knockBack = 0f;
            Item.value = 25000;
            Item.rare = 3;
            Item.UseSound = SoundID.Item44;
            Item.shoot = ProjectileType<CaeliteSentry>();
            Item.DamageType = DamageClass.Summon;
            Item.sentry = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemType<CaeliteBar>(), 12)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.SpawnMinionOnCursor(source, player.whoAmI, type, Item.damage, knockback);
            return false;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool? UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                player.MinionNPCTargetAim(false);
            }
            return base.UseItem(player);
        }
    }

    public class CaeliteSentry : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sky bound spiral");
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projFrames[Projectile.type] = 4;
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
            Projectile.knockBack = 10f;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.sentry = true;
            Projectile.minion = true;
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
                    for(int n =0; n < targets.Count; n++)
                    {
                        targets[n].StrikeNPC(Projectile.damage, Projectile.knockBack, 0, false, false);
                        float distance = (targets[n].Center - Projectile.Center).Length();
                        for (int d = 0; d < distance; d += 4)
                        {
                            Dust dust = Dust.NewDustPerfect(Projectile.Center + QwertyMethods.PolarVector(d, (targets[n].Center - Projectile.Center).ToRotation()), DustType<CaeliteDust>());
                            dust.frame.Y = 0;
                        }
                    }
                }
            }
            Dust dust2 = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<CaeliteDust>())];
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