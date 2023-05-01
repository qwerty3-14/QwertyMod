using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common.PlayerLayers;
using QwertyMod.Common.RuneBuilder;
using QwertyMod.Content.Dusts;
using QwertyMod.Content.Items.MiscMaterials;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Magic.PursuitRune
{
    public class PursuitRuneStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Pursuit Rune Staff");
            //Tooltip.SetDefault("Fires explosive Pursuit Runes!");
            Item.staff[Item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }


        public override void SetDefaults()
        {
            Item.damage = 160;
            Item.DamageType = DamageClass.Magic;

            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 2;
            Item.value = 500000;
            Item.rare = ItemRarityID.Cyan;
            Item.autoReuse = true;
            Item.width = 72;
            Item.height = 72;
            if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("QwertyMod/Content/Items/Weapon/Magic/PursuitRune/PursuitRuneStaff_Glow").Value;
            }
            Item.mana = ModLoader.HasMod("TRAEProject") ? 14 : 7;
            Item.shoot = ProjectileType<PursuitRuneMissile>();
            Item.shootSpeed = 9;
            Item.noMelee = true;
            //Item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/AncientItems/AncientWave_Glow");
        }

        public override void UseAnimation(Player player)
        {
            if (player.statMana > Item.mana)
            {
                SoundEngine.PlaySound(SoundID.MaxMana, player.position);
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemType<CraftingRune>(), 15)
                .AddIngredient(ItemType<AncientMissile.AncientMissileStaff>(), 1)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
    public class PursuitRuneMissile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;

            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.alpha = 0;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 720;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.usesLocalNPCImmunity = true;
        }

        public int runeTimer;
        public NPC target;

        public float runeSpeed = 10;
        public float runeDirection;
        public float runeTargetDirection;
        public bool runOnce = true;
        public int f;
        int timer = 0;
        public override void AI()
        {
            timer++;
            Player player = Main.player[Projectile.owner];
            if (runOnce)
            {
                Projectile.rotation = (Projectile.velocity).ToRotation();
                runOnce = false;
            }
            if (QwertyMethods.ClosestNPC(ref target, 1000, Projectile.Center))
            {
                Projectile.rotation.SlowRotation((target.Center - Projectile.Center).ToRotation(), MathHelper.ToRadians(1));
            }
            Projectile.velocity = new Vector2((float)(Math.Cos(Projectile.rotation) * runeSpeed), (float)(Math.Sin(Projectile.rotation) * runeSpeed));
        }

        public override bool PreDraw(ref Color lightColor)
        {
            float c = (timer / 40f);
            if (c > 1f)
            {
                c = 1f;
            }
            int frame = timer / 2;
            if (frame > 19)
            {
                frame = 19;
            }
            Main.EntitySpriteDraw(RuneSprites.runeTransition[(int)Runes.Pursuit][frame], Projectile.Center - Main.screenPosition, null, new Color(c, c, c, c), Projectile.rotation, new Vector2(10, 5), Vector2.One * 2, 0, 0);
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[Projectile.owner] = 0;
            Projectile e = Main.projectile[Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, ProjectileType<PursuitRuneBlast>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 1f)];
            e.localNPCImmunity[target.whoAmI] = -1;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, ProjectileType<PursuitRuneBlast>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            return true;
        }
    }
    public class PursuitRuneBlast : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Rune Blast");
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.width = 225;
            Projectile.height = 225;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 2;
            Projectile.usesLocalNPCImmunity = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.width = 225;
            Projectile.height = 225;

            SoundEngine.PlaySound(SoundID.Item62, Projectile.position);

            for (int i = 0; i < 600; i++)
            {
                float theta = Main.rand.NextFloat(-MathF.PI, MathF.PI);
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustType<PursuitRuneDeath>(), QwertyMethods.PolarVector(Main.rand.Next(2, 30), theta));
                dust.noGravity = true;
            }
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
