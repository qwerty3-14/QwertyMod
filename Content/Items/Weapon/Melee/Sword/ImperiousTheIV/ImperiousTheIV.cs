using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
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
            //DisplayName,SetDefault("Imperious The IV");
            //Tooltip.SetDefault("Hitting enemies launches richoching swords");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 80;
            Item.DamageType = DamageClass.Melee;

            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 3;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ItemRarityID.Lime;
            Item.UseSound = SoundID.Item1;
            Item.scale = 1.8f;
            Item.width = 40;
            Item.height = 40;
            Item.crit = 20;
            Item.autoReuse = true;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (player.whoAmI == Main.myPlayer && !target.immortal && player.ownedProjectileCounts[ProjectileType<ImperiousTheV>()] < 40)
            {
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center, Vector2.Zero, ProjectileType<ImperiousTheV>(), player.GetWeaponDamage(Item), 0.1f, player.whoAmI, target.whoAmI);
            }
        }
    }

    public class ImperiousTheV : ModProjectile
    {


        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Imperious The V");
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 120;
            Projectile.aiStyle = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
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
            Projectile.rotation = Projectile.velocity.ToRotation() + MathF.PI / 2;
            if (QwertyMethods.ClosestNPC(ref target, 400, Projectile.Center, true, specialCondition: delegate (NPC possibleTarget) { return Projectile.localNPCImmunity[possibleTarget.whoAmI] == 0; }))
            {
                Projectile.velocity = (target.Center - Projectile.Center).SafeNormalize(-Vector2.UnitY) * 10f;
            }
            else
            {
                Projectile.Kill();
            }
        }
    }
}