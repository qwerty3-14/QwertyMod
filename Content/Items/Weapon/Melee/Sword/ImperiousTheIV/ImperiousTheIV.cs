using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Melee.Sword.ImperiousTheIV
{
    public class ImperiousTheIV : ModItem
    {


        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Imperious The IV");
            Tooltip.SetDefault("Hitting enemies launches richoching swords");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 80;
            Item.DamageType = DamageClass.Melee;

            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = 1;
            Item.knockBack = 3;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = 7;
            Item.UseSound = SoundID.Item1;
            Item.scale = 1.8f;
            Item.width = 40;
            Item.height = 40;
            Item.crit = 20;
            Item.autoReuse = true;
            //Item.scale = 5;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            if (player.whoAmI == Main.myPlayer && !target.immortal && player.ownedProjectileCounts[ProjectileType<ImperiousTheV>()] < 40)
            {
                Projectile.NewProjectile(player.GetProjectileSource_Item(Item), target.Center, Vector2.Zero, ProjectileType<ImperiousTheV>(), damage, 0.1f, player.whoAmI, target.whoAmI);
            }
        }
    }

    public class ImperiousTheV : ModProjectile
    {


        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Imperious The V");
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Main.projFrames[Projectile.type] = 1;
            Projectile.knockBack = 10f;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            //Projectile.minionSlots = 1;
            Projectile.timeLeft = 120;
            Projectile.aiStyle = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.extraUpdates = 1;
        }

        private NPC target = null;
        private bool runOnce = true;

        public override void AI()
        {
            if (runOnce)
            {
                Projectile.localNPCImmunity[(int)Projectile.ai[0]] = -1;
                runOnce = false;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + (float)Math.PI / 2;
            if (QwertyMethods.ClosestNPC(ref target, 400, Projectile.Center, true, specialCondition: delegate (NPC possibleTarget) { return Projectile.localNPCImmunity[possibleTarget.whoAmI] == 0; }))
            {
                Projectile.velocity = (target.Center - Projectile.Center).SafeNormalize(-Vector2.UnitY) * 10f;
            }
            else
            {
                Projectile.Kill();
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[Projectile.owner] = 0;
        }
    }
}