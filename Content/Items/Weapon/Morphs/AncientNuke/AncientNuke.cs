using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common.PlayerLayers;
using QwertyMod.Content.Buffs;
using QwertyMod.Content.Dusts;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Morphs.AncientNuke
{
    public class AncientNuke : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Shape shift: Ancient Nuke");
            //Tooltip.SetDefault("Breifly turns you into an ancient nuke that causes a massive explosion when you collide with something... don't worry you'll live");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public const int dmg = 300;
        public const float kb = 9f;
        public const int def = -1;

        public override void SetDefaults()
        {
            Item.damage = dmg;
            Item.knockBack = kb;
            Item.DamageType = DamageClass.Generic;
            Item.GetGlobalItem<ShapeShifterItem>().morphCooldown = 27;
            Item.noMelee = true;

            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.useStyle = ItemUseStyleID.Shoot;

            Item.value = 150000;
            Item.rare = ItemRarityID.Blue;

            Item.noUseGraphic = true;
            Item.width = 30;
            Item.height = 30;

            //Item.autoReuse = true;
            if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("QwertyMod/Content/Items/Weapon/Morphs/AncientNuke/AncientNuke_Glow").Value;
            }
            Item.shoot = ProjectileType<AncientNukeMorph>();
            Item.shootSpeed = 0f;
            Item.channel = true;
        }


        public override bool CanUseItem(Player player)
        {
            for (int i = 0; i < 1000; ++i)
            {
                if ((Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == Item.shoot))
                {
                    return false;
                }
            }

            return true;
        }
    }

    public class AncientNukeMorph : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Ancient Nuke");
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 600;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.extraUpdates = 1;
        }

        private float dustYoffset;

        public override void AI()
        {
            //Projectile.timeLeft = 2;

            Player player = Main.player[Projectile.owner];
            player.Center = Projectile.Center;
            player.immune = true;
            player.immuneTime = 120;
            player.statDefense.FinalMultiplier *= 0;
            player.itemAnimation = 2;
            player.itemTime = 2;
            player.fallStart = (int)player.Bottom.Y;
            player.GetModPlayer<ShapeShifterPlayer>().noDraw = true;
            player.AddBuff(BuffType<HealingHalt>(), 10);
            player.AddBuff(BuffType<MorphCooldown>(), (int)((27 * player.GetModPlayer<ShapeShifterPlayer>().coolDownDuration) * 60f));
            //Projectile.timeLeft = 2;
            //player.mount = null;
            //player.noItems = true;
            //player.buffTime[mod.BuffType("MorphCooldown")]++;
            if (player.controlLeft)
            {
                Projectile.rotation -= MathF.PI / 60;
            }
            if (player.controlRight)
            {
                Projectile.rotation += MathF.PI / 60;
            }
            Projectile.velocity = QwertyMethods.PolarVector(10, Projectile.rotation - MathF.PI / 2);
            dustYoffset = 20;
            for (int i = 0; i < 2; i++)
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center + QwertyMethods.PolarVector(dustYoffset, Projectile.rotation + MathF.PI / 2) + QwertyMethods.PolarVector(Main.rand.Next(-9, 9), Projectile.rotation), DustType<AncientGlow>());
            }
        }

        public override void OnKill(int timeLeft)
        {
            if (timeLeft == 0)
            {
                Projectile e = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ProjectileType<AncientFallout>(), Projectile.damage, Projectile.knockBack, Projectile.owner)];
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[Projectile.owner] = 0;
            Projectile e = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ProjectileType<AncientFallout>(), Projectile.damage, Projectile.knockBack, Projectile.owner)];
            e.localNPCImmunity[target.whoAmI] = -1;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile e = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ProjectileType<AncientFallout>(), Projectile.damage, Projectile.knockBack, Projectile.owner)];
            return true;
        }

        public override bool PreDraw(ref Color drawColor)
        {
            Main.EntitySpriteDraw(Request<Texture2D>("QwertyMod/Content/Items/Weapon/Morphs/AncientNuke/AncientNukeMorph").Value, new Vector2(Projectile.Center.X - Main.screenPosition.X, Projectile.Center.Y - Main.screenPosition.Y),
                        new Rectangle(0, 0, 38, 56), drawColor, Projectile.rotation,
                        new Vector2(46 * 0.5f, 56 * 0.5f), 1f, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(Request<Texture2D>("QwertyMod/Content/Items/Weapon/Morphs/AncientNuke/AncientNukeMorph_Glow").Value, new Vector2(Projectile.Center.X - Main.screenPosition.X, Projectile.Center.Y - Main.screenPosition.Y),
                        new Rectangle(0, 0, 38, 56), Color.White, Projectile.rotation,
                        new Vector2(46 * 0.5f, 56 * 0.5f), 1f, SpriteEffects.None, 0);
            return false;
        }
    }

    public class AncientFallout : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Ancient Fallout");
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.width = 1000;
            Projectile.height = 1000;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.DamageType = DamageClass.Summon;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            SoundEngine.PlaySound(SoundID.Item62, Projectile.position);

            for (int i = 0; i < 1600; i++)
            {
                float theta = Main.rand.NextFloat(-MathF.PI, MathF.PI);
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustType<AncientGlow>(), QwertyMethods.PolarVector(Main.rand.Next(2, 120), theta));
                dust.noGravity = true;
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (Collision.CanHit(Projectile.Center, 1, 1, targetHitbox.Location.ToVector2(), targetHitbox.Width, targetHitbox.Height))
            {
                return base.Colliding(projHitbox, targetHitbox);
            }
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[Projectile.owner] = 0;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
}