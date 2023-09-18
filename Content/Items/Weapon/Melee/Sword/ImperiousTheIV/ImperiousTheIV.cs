using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent;

namespace QwertyMod.Content.Items.Weapon.Melee.Sword.ImperiousTheIV
{
    public class ImperiousTheIV : ModItem
    {
        public override void SetStaticDefaults()
        {
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
            Item.scale = 1f;
            Item.width = 40;
            Item.height = 40;
            Item.crit = 20;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<ImperiousTheIVAura>();
            Item.shootsEveryUse = true;
            Item.shootSpeed = 1;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectileDirect(source, player.MountedCenter - velocity * 2, velocity * 5, type, damage, knockback, Main.myPlayer,
                            player.direction * player.gravDir, player.itemAnimationMax, player.GetAdjustedItemScale(player.HeldItem));
                        NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, player.whoAmI);
            return false;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
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

    public class ImperiousTheIVAura : SwordAura
    {
        public override void AuraDefaults()
        {
            scaleIncrease = 0.2f;

            frontColor = new Color(123, 145, 227);
            middleColor = new Color(163, 211, 225);
            backColor = new Color(242, 253, 255);
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center, Vector2.Zero, ProjectileType<ImperiousTheV>(), Main.player[Projectile.owner].GetWeaponDamage(Main.player[Projectile.owner].HeldItem), 0.1f, Main.player[Projectile.owner].whoAmI, target.whoAmI);
        }

    }
}