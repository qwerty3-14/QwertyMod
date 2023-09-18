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
using QwertyMod.Common.PlayerLayers;
using Microsoft.Xna.Framework.Graphics;

namespace QwertyMod.Content.Items.Weapon.Ranged.Gun.SuperquantumRifle
{
    public class SuperquantumRifle : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 45;
            Item.crit = 10;
            Item.useTime = 7; Item.useAnimation = 7;
            Item.knockBack = 6f;

            Item.width = 58;
            Item.height = 24;
            Item.shootSpeed = 8f;
            Item.value = GearStats.TrueCaeliteWeaponValue;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAmmo = AmmoID.Bullet;
            Item.shoot = ProjectileID.Bullet;
            Item.rare = ItemRarityID.Red;
            Item.DamageType = DamageClass.Ranged;
            Item.UseSound = SoundID.Item43;
            Item.autoReuse = true;
            Item.noMelee = true;
            if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("QwertyMod/Content/Items/Weapon/Ranged/Gun/SuperquantumRifle/SuperquantumRifle_Glow").Value;
            }
        }
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return Main.rand.NextBool(2);
        }
        

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, -4);
        }
        
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position += QwertyMethods.PolarVector(-6 * player.direction, velocity.ToRotation() + MathF.PI / 2f) + QwertyMethods.PolarVector(28, velocity.ToRotation());
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile p = Main.projectile[Projectile.NewProjectile(source, position, (velocity.RotatedByRandom(MathF.PI / 12f)) * Main.rand.NextFloat(0.7f, 1.3f), type, damage, knockback, player.whoAmI)];
            if(Main.netMode != NetmodeID.SinglePlayer)
            {
                ModPacket packet = Mod.GetPacket();
                packet.Write((byte)ModMessageType.AmmoEnchantQuantum);
                packet.Write(p.identity);
                packet.WriteVector2(Main.MouseWorld);
                packet.Send();
            }
            else
            {
                p.GetGlobalProjectile<QuantumProjectile>().isQuantum = true;
                p.GetGlobalProjectile<QuantumProjectile>().quantumCenter = Main.MouseWorld;
            }
            return false;
        }
    }
    public class QuantumProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public bool isQuantum = false;
        public Vector2 quantumCenter = Vector2.Zero;

        public override void AI(Projectile projectile)
        {
            if(isQuantum)
            {
                if(projectile.timeLeft > 10)
                {
                    projectile.timeLeft = 10;
                }
                if(projectile.timeLeft == 1)
                {
                    
                    for(int d = 0; d < 40; d++)
                    {
                        float dRot = ((float)d / 40f) * MathF.PI * 2f;
                        Dust.NewDustPerfect(projectile.Center, ModContent.DustType<DarknessDust>(), QwertyMethods.PolarVector(1, dRot));
                    }
                    for(int i = 0; i < 200; i++)
                    {
                        NPC npc = Main.npc[i];
                        if(npc.active && !npc.friendly && (npc.Center - quantumCenter).Length() < 300f)
                        {
                            if(Main.myPlayer == projectile.owner)
                            {
                                for(int k = 0; k < 100; k++)
                                {
                                    float rot = Main.rand.NextFloat() * MathF.PI * 2f;
                                    if(Collision.CanHitLine(npc.Center + QwertyMethods.PolarVector(-100, rot), projectile.width, projectile.height, npc.Center, projectile.width, projectile.height))
                                    {
                                        if (Main.netMode != NetmodeID.SinglePlayer)
                                        {
                                            ModPacket packet = Mod.GetPacket();
                                            packet.Write((byte)ModMessageType.SpawnQuantumRing);
                                            packet.WriteVector2(npc.Center + QwertyMethods.PolarVector(-100, rot));
                                            packet.Send();
                                        }
                                        else
                                        {
                                            for(int d = 0; d < 40; d++)
                                            {
                                                float dRot = ((float)d / 40f) * MathF.PI * 2f;
                                                Dust.NewDustPerfect(npc.Center + QwertyMethods.PolarVector(-100, rot), ModContent.DustType<DarknessDust>(), QwertyMethods.PolarVector(1, dRot));
                                            }
                                        }
                                        
                                        Projectile.NewProjectile(projectile.GetSource_FromThis(), npc.Center + QwertyMethods.PolarVector(-100, rot), QwertyMethods.PolarVector(projectile.velocity.Length(), rot), projectile.type, projectile.damage, projectile.knockBack, projectile.owner);
                                        break;
                                    }
                                }
                                
                                //float rot = projectile.velocity.ToRotation();
                                //Projectile.NewProjectile(projectile.GetSource_FromThis(), npc.Center + QwertyMethods.PolarVector(-100, rot), QwertyMethods.PolarVector(projectile.velocity.Length(), rot), projectile.type, projectile.damage, projectile.knockBack, projectile.owner);
                            }
                        }
                    }
                }
            }
        }
        public override bool? Colliding(Projectile projectile, Rectangle projHitbox, Rectangle targetHitbox)
        {
            if(isQuantum)
            {
                return false;
            }
            return null;
        }
    }
    
}