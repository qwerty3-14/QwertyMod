using Microsoft.Xna.Framework;
using QwertyMod.Content.Buffs;
using QwertyMod.Content.Dusts;
using QwertyMod.Content.Items.Consumable.Tiles.Bars;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Magic.Lune
{
    public class LuneStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Lune Staff");
            //Tooltip.SetDefault("Fires a Lune crest to zap enemies" + "\nInflicts Lune curse making enemies more vulnerable to critical hits");
            Item.staff[Item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 11;
            Item.mana = ModLoader.HasMod("TRAEProject") ? 100 : 16;
            Item.width = 70;
            Item.height = 70;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 1f;
            Item.value = 20000;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item43;
            Item.shoot = ProjectileType<LuneCrest>();
            Item.DamageType = DamageClass.Magic;
            Item.shootSpeed = 9;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemType<LuneBar>(), 12)
                .AddTile(TileID.Anvils)
                .Register();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 1000; i++)
            {
                if (Main.projectile[i].type == type && Main.projectile[i].owner == player.whoAmI && Main.projectile[i].active)
                {
                    Main.projectile[i].Kill();
                }
            }
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 85f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }
    }

    public class LuneCrest : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.light = .5f;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 4 * 60;
        }

        private bool runOnce = true;
        NPC target;
        int timer = 0;
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.FinalDamage *= 0;
        }
        public override void AI()
        {
            if (runOnce)
            {
                runOnce = false;
            }
            Projectile.rotation += MathF.PI / 15;
            Projectile.velocity *= .95f;
            timer++;
            if (timer % 26 == 0)
            {
                if (QwertyMethods.ClosestNPC(ref target, 400, Projectile.Center, false, -1))
                {
                    QwertyMethods.PokeNPC(Main.player[Projectile.owner], target, Projectile.GetSource_FromThis(), Projectile.damage, DamageClass.Magic, Projectile.knockBack);
                    for (int d = 0; d < (target.Center - Projectile.Center).Length(); d += 4)
                    {
                        Dust.NewDust(Projectile.Center + QwertyMethods.PolarVector(d, (target.Center - Projectile.Center).ToRotation()), 0, 0, DustType<LuneDust>());
                        target.AddBuff(BuffType<LuneCurse>(), 120);
                    }
                }
            }
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 8; i++)
            {
                Dust dust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<LuneDust>())];
                dust.velocity *= 3f;
            }
        }

    }
}