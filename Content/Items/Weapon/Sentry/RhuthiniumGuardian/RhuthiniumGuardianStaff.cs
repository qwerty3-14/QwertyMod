using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Content.Dusts;
using QwertyMod.Content.Items.Consumable.Tiles.Bars;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Sentry.RhuthiniumGuardian
{
    public class RhuthiniumGuardianStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rhuthinium Guardian Staff");
            Tooltip.SetDefault("Summons an extremely slow guardian to shoot at your enemies");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
        }


        public override void SetDefaults()
        {
            Item.damage = 200;
            Item.mana = 20;
            Item.width = 38;
            Item.height = 38;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = 1;
            Item.noMelee = true;
            Item.knockBack = 10f;
            Item.value = 25000;
            Item.rare = 3;
            Item.UseSound = SoundID.Item44;
            Item.shoot = ProjectileType<RhuthiniumGuardian>();
            Item.DamageType = DamageClass.Summon;
            Item.sentry = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemType<RhuthiniumBar>(), 8)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.SpawnMinionOnCursor(source, player.whoAmI, type, Item.damage, knockback);
            return false;
        }
    }

    public class RhuthiniumGuardian : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rhuthinium Guardian");
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true; //This is necessary for right-click targeting
        }

        public override void SetDefaults()
        {
            Projectile.width = 30; //Set the hitbox width
            Projectile.height = 30;   //Set the hitbox heinght
            Projectile.hostile = false;    //tells the game if is hostile or not.
            Projectile.friendly = false;   //Tells the game whether it is friendly to players/friendly npcs or not
            Projectile.ignoreWater = true;    //Tells the game whether or not projectile will be affected by water
            Main.projFrames[Projectile.type] = 1;  //this is where you add how many frames u'r projectile has to make the animation
            Projectile.knockBack = 10f;
            Projectile.penetrate = -1; //Tells the game how many enemies it can hit before being destroyed  -1 is infinity
            Projectile.tileCollide = true; //Tells the game whether or not it can collide with tiles/ terrain
            Projectile.sentry = true; //tells the game that this is a sentry
            Projectile.timeLeft = Projectile.SentryLifeTime;
        }

        private NPC confirmTarget;
        private int timer;
        private Color lineColor;
        private bool drawLine;
        private bool alternateColor = false;
        private int colorCounter;

        private bool startCountdown;
        private int countdownTimer;
        private float Aim;
        private float shardVelocity = 30f;
        private float lineLength = 0;

        public override void AI()
        {
            Main.player[Projectile.owner].UpdateMaxTurrets();
            Player player = Main.player[Projectile.owner];

            Projectile.rotation += (float)Math.PI / 60;   //this make the projctile to rotate

            if (QwertyMethods.ClosestNPC(ref confirmTarget, 100000, Projectile.Center, false, player.MinionAttackTargetNPC))
            {
                drawLine = true;
                lineLength = (confirmTarget.Center - Projectile.Center).Length();
                Aim = (confirmTarget.Center - Projectile.Center).ToRotation();
                timer++;
                if (timer == 420)
                {
                    startCountdown = true;
                }
                if (timer >= 600)
                {
                    if (Main.netMode != 1)
                        Projectile.NewProjectile(new EntitySource_Misc(""), Projectile.Center, QwertyMethods.PolarVector(shardVelocity, Aim), ProjectileType<RhuthiniumShard>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, 0f, 0f);
                    timer = 0;
                }
            }
            if (startCountdown)
            {
                alternateColor = true;
                countdownTimer++;
                if (countdownTimer == 180)
                {
                    startCountdown = false;
                }
            }
            else
            {
                alternateColor = false;
                countdownTimer = 0;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.EntitySpriteDraw(Request<Texture2D>("QwertyMod/Content/Items/Weapon/Sentry/RhuthiniumGuardian/RhuthiniumGuardianLower").Value, new Vector2(Projectile.Center.X - Main.screenPosition.X, Projectile.Center.Y - Main.screenPosition.Y),
                    new Rectangle(0, Projectile.frame * Projectile.height, Projectile.width, Projectile.height), lightColor, -Projectile.rotation,
                    new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f), 1f, SpriteEffects.None, 0);

            if (alternateColor)
            {
                colorCounter++;

                if (colorCounter >= 20)
                {
                    colorCounter = 0;
                }
                else if (colorCounter >= 10)
                {
                    lineColor = Color.White;
                }
                else
                {
                    lineColor = Color.Red;
                }
            }
            else
            {
                lineColor = Color.Red;
            }
            //Draw chain
            if (drawLine)
            {
                Vector2 center = Projectile.Center;
                Vector2 distToProj = confirmTarget.Center - center;
                float projRotation = distToProj.ToRotation() - 1.57f;
                distToProj.Normalize();                 //get unit vector
                distToProj *= 12f;                      //speed = 12
                center += distToProj;                   //update draw position
                distToProj = confirmTarget.Center - center;    //update distance
                Color drawColor = lightColor;

                Main.EntitySpriteDraw(Request<Texture2D>("QwertyMod/Content/Items/Weapon/Sentry/RhuthiniumGuardian/laser").Value, new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
                    new Rectangle(0, 0, 1, (int)lineLength - 10), lineColor, projRotation,
                    new Vector2(0, 0), 1f, SpriteEffects.None, 0);
            }
            Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, new Vector2(Projectile.Center.X - Main.screenPosition.X, Projectile.Center.Y - Main.screenPosition.Y),
                    new Rectangle(0, Projectile.frame * Projectile.height, Projectile.width, Projectile.height), lightColor, Projectile.rotation,
                    new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f), 1f, SpriteEffects.None, 0);
            drawLine = false;

            return false;
        }
    }

    public class RhuthiniumShard : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rhuthinium Shard");
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.knockBack = 10f;
            Projectile.extraUpdates = 3;
        }

        public override void AI()
        {
            if (Main.rand.Next(10) == 0)
            {
                Dust d = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<RhuthiniumDust>())];
                d.frame.Y = Main.rand.Next(2) == 0 ? 0 : 10;
                d.noGravity = true;
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 12; i++)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, DustType<RhuthiniumDust>());
                d.frame.Y = Main.rand.Next(2) == 0 ? 0 : 10;
                d.noGravity = true;
            }
        }
    }
}