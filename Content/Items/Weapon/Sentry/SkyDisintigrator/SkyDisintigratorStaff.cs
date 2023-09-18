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
using Microsoft.Xna.Framework.Graphics;
using System;
using QwertyMod.Content.Items.MiscMaterials;

namespace QwertyMod.Content.Items.Weapon.Sentry.SkyDisintigrator
{
    public class SkyDisintigratorStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Sky Spiral Staff");
            //Tooltip.SetDefault("Higher beings will punish all enemies near this sentry!");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 40;
            Item.mana = 20;
            Item.width = 48;
            Item.height = 48;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 0f;
            Item.value = GearStats.TrueCaeliteWeaponValue;
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item44;
            Item.shoot = ProjectileType<SkyDisintigrator>();
            Item.DamageType = DamageClass.Summon;
            Item.sentry = true;
        }
        
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemType<SkySpiral.CaeliteSentryStaff>())
            .AddIngredient(ItemType<SoulOfHeight>(), 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.SpawnMinionOnCursor(source, player.whoAmI, type, Item.damage, knockback);
            return false;
        }

    }

    public class SkyDisintigrator : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Sky bound spiral");
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = 5000;
            ProjectileID.Sets.SentryShot[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.sentry = true;
            Projectile.width = 56;
            Projectile.height = 56;
            Projectile.hostile = false;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = Projectile.SentryLifeTime;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.sentry = true;
            Projectile.DamageType = DamageClass.Summon;
        }
        private int timer;
        NPC target = null;
        int chargeTime = 60;
        int attackCooldown = 10;
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.ArmorPenetration += 20;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            player.UpdateMaxTurrets();
            if(target != null && (!target.active || !target.CanBeChasedBy(Projectile)))
            {
                target = null;
                timer = 0;
            }
            if(target != null)
            {
                timer++;
                if(timer > chargeTime && timer % attackCooldown == 0)
                {
                    QwertyMethods.PokeNPCMinion(Main.player[Projectile.owner], target, Projectile.GetSource_FromThis(), Projectile.damage, 0);
                }
            }
            else
            {
                if(QwertyMethods.ClosestNPC(ref target, 4000, Projectile.Center, false, player.MinionAttackTargetNPC))
                {
                }
                else
                {
                    target = null;
                }
            }
            Dust dust2 = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<CaeliteDust>())];
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
        
        public override bool PreDraw(ref Color lightColor)
        {
            if(target != null)
            {
                if(timer > chargeTime)
                {
                    
                    float dist = (target.Center - Projectile.Center).Length();
                    float toward = (target.Center - Projectile.Center).ToRotation();
                    int frameHeight = 32;
                    Texture2D texture = ModContent.Request<Texture2D>("QwertyMod/Content/Items/Weapon/Sentry/SkyDisintigrator/DivineBolt").Value;
                    for(int i =0; i < (dist / frameHeight); i++)
                    {
                        int locHeight = frameHeight;
                        if(frameHeight * (i + 1) > dist)
                        {
                            locHeight = (int)(dist - (locHeight * i));
                        }
                        int frame = (i + (timer % 12) / 4 ) % 4;
                        Main.EntitySpriteDraw(texture, Projectile.Center + QwertyMethods.PolarVector(i * frameHeight, toward ) - Main.screenPosition, new Rectangle(0, frameHeight * frame, 16, locHeight), Color.White, toward + MathF.PI / 2f, new Vector2(8, locHeight), 1f, SpriteEffects.None, 0);
                    }
                }
                else if(timer > chargeTime / 2)
                {
                    Vector2 distToProj = target.Center - Projectile.Center;
                    float projRotation = distToProj.ToRotation();
                    

                    Main.EntitySpriteDraw(Request<Texture2D>("QwertyMod/Content/Items/Weapon/Sentry/RhuthiniumGuardian/laser").Value, Projectile.Center - Main.screenPosition,
                        null, Color.Yellow, projRotation,
                        new Vector2(0, 0.5f), new Vector2(distToProj.Length() * 0.5f, 0.5f), SpriteEffects.None, 0);
                }
            }
            
            return true;
        }
    }
}