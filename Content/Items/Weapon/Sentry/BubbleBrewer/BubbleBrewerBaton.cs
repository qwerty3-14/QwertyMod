using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;


namespace QwertyMod.Content.Items.Weapon.Sentry.BubbleBrewer
{
    public class BubbleBrewerBaton : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.damage = 60;
            Item.DamageType = DamageClass.Summon;
            Item.knockBack = 1f;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Yellow;
            Item.width = 22;
            Item.height = 32;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.shootSpeed = 0;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.shoot = ModContent.ProjectileType<BubbleBrewer>();
            Item.noUseGraphic = false;
            Item.noMelee = true;
            Item.UseSound = SoundID.Item1;
            Item.sentry = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            player.SpawnMinionOnCursor(source, player.whoAmI, type, Item.damage, knockback);
            return false;
        }
    }
    public class BubbleBrewer : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 2;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true; //This is necessary for right-click targeting
        }
        public override void SetDefaults()
        {
            Projectile.sentry = true;
            Projectile.timeLeft = Projectile.SentryLifeTime;
            Projectile.width = 56;
            Projectile.height = 84;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Summon;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return false;
        }
        int waterLevel = 0;
        int timer = 0;
        NPC target;
        Vector2 bubbleShooterLocation = new Vector2(27, 73);
        bool runOnce = true;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (runOnce)
            {
                Point point;
                while (!WorldUtils.Find(Projectile.Center.ToTileCoordinates(), Searches.Chain(new Searches.Down(1), new GenCondition[]
                    {
                                            new Conditions.IsSolid()
                    }), out point))
                {
                    Projectile.position.Y++;
                }
                Projectile.position.Y -= 42;
                runOnce = false;
            }
            player.UpdateMaxTurrets();
            timer++;
            if (timer == 1)
            {
                SoundEngine.PlaySound(SoundID.Item46, Projectile.Center);
            }
            if (timer % 60 == 0 && waterLevel < 26)
            {
                waterLevel++;
                SoundEngine.PlaySound(SoundID.Item85, Projectile.Center);
            }
            if (waterLevel > 0 && timer % 3 == 0)
            {
                if (QwertyMethods.ClosestNPC(ref target, 500, Projectile.Center, false, player.MinionAttackTargetNPC))
                {
                    if (waterLevel > 10)
                    {
                        SoundEngine.PlaySound(SoundID.ForceRoarPitched, Projectile.Center);
                    }
                    Projectile.frameCounter = 30;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position + bubbleShooterLocation, (target.Center - (Projectile.position + bubbleShooterLocation)).SafeNormalize(Vector2.UnitY) * 12f, ModContent.ProjectileType<BrewerBubble>(), Projectile.damage, Projectile.knockBack, Projectile.owner, -10f);
                    waterLevel--;
                }
            }
            if (Projectile.frameCounter > 0)
            {
                Projectile.frameCounter--;
                Projectile.frame = 1;
            }
            else
            {
                Projectile.frame = 0;
            }
        }
        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("QwertyMod/Content/Items/Weapon/Sentry/BubbleBrewer/BubbleBrewerGauge").Value;
            for (int i = 0; i < waterLevel; i++)
            {
                Main.EntitySpriteDraw(texture, Projectile.position + new Vector2(24, 36) - Vector2.UnitY * i - Main.screenPosition, null, lightColor, 0, new Vector2(0, 1), 1, 0, 0);
            }

        }
    }
    public class BrewerBubble : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.SentryShot[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.aiStyle = 70;
            AIType = ProjectileID.FlaironBubble;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.alpha = 255;
            Projectile.timeLeft = 240;
        }
    }
}
