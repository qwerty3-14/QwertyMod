using Microsoft.Xna.Framework;
using QwertyMod.Content.Dusts;
using QwertyMod.Content.Items.MiscMaterials;
using QwertyMod.Content.NPCs.Fortress;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Ranged.Gun.SoEF
{
    public class ShotgunOfExcessiveForce : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.DamageType = DamageClass.Ranged;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.damage = 20;
            Item.knockBack = 5f;
            Item.width = 46;
            Item.height = 22;
            Item.useAmmo = AmmoID.Bullet;
            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 8f;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.UseSound = SoundID.Item38;
            Item.rare = ItemRarityID.Orange;
            Item.value = 120000;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-7, -1);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int bulletCount = 3 + Main.rand.Next(2);
            float dir = velocity.ToRotation();
            float speed = velocity.Length();
            for (int b = 0; b < bulletCount; b++)
            {
                Projectile p = Main.projectile[Projectile.NewProjectile(source, position + QwertyMethods.PolarVector(24, dir), QwertyMethods.PolarVector(speed * Main.rand.NextFloat(.7f, 1.4f), dir + Main.rand.NextFloat(-1, 1) * MathF.PI / 18), type, damage, knockback, player.whoAmI)];
                p.extraUpdates++;
                p.GetGlobalProjectile<EtimsProjectile>().effect = true;
                if(Main.netMode != NetmodeID.SinglePlayer)
                {
                    ModPacket packet = Mod.GetPacket();
                    packet.Write((byte)ModMessageType.AmmoEnchantEtims);
                    packet.Write(p.identity);
                    packet.Send();
                }
            }

            player.velocity = QwertyMethods.PolarVector(12f, dir + MathF.PI);
            NetMessage.SendData(MessageID.PlayerControls, number: player.whoAmI);

            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemType<Etims>(), 12)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class EtimsProjectile : GlobalProjectile
    {
        public bool effect = false;
        public override bool InstancePerEntity => true;
        public override void AI(Projectile projectile)
        {
            if (effect)
            {
                if (projectile.timeLeft > 10)
                {
                    Dust.NewDustPerfect(projectile.Center, DustType<BloodforceDust>(), Vector2.Zero);
                }
            }
        }

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (effect)
            {
                if (target.GetGlobalNPC<FortressNPCGeneral>().fortressNPC)
                {
                    for (int i = 0; i < modifiers.FinalDamage.Multiplicative / 3; i++)
                    {
                        Dust d = Dust.NewDustPerfect(projectile.Center, DustType<BloodforceDust>());
                        d.velocity *= 5f;
                    }
                    modifiers.FinalDamage *= 2;
                }
            }
        }
    }
}