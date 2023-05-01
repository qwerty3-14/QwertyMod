using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Content.Items.Equipment.Accessories.Sword;
using QwertyMod.Content.Items.MiscMaterials;
using QwertyMod.Content.Items.Weapon.Ranged.Gun.SoEF;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Melee.Sword.Overkill
{
    public class Overkill : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Overkill");
            //Tooltip.SetDefault("just need to get airborne....");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }


        public override void SetDefaults()
        {
            Item.useStyle = 105;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.damage = 1000;
            Item.knockBack = 20f;
            Item.width = 30;
            Item.height = 112;
            Item.noUseGraphic = true;
            Item.useTime = 45;
            Item.useAnimation = 45;
            Item.rare = ItemRarityID.Yellow;
            Item.value = QwertyMod.InvaderGearValue;
            Item.useTurn = false;
        }

        public override bool CanUseItem(Player player)
        {
            return player.itemAnimation == 0;
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {

            float rotation = 2f * ((float)player.itemAnimation / player.itemAnimationMax) * MathF.PI * -player.direction;
            if(((float)player.itemAnimation / player.itemAnimationMax) > 0.5f)
            {
                rotation =  MathF.PI * -player.direction;
            }
            else
            {
                if(player.velocity.Y < 0)
                {
                    player.velocity.Y = 2f;
                }
                player.velocity.Y += (player.gravity * player.jumpSpeedBoost * 1.25f) ;
            }
            player.SetCompositeArmFront(enabled: true, Player.CompositeArmStretchAmount.Full, rotation);
            player.itemRotation = rotation + MathF.PI / 2;

            Vector2 pointPoisition = player.RotatedRelativePoint(player.MountedCenter, false, true);
            player.itemLocation = pointPoisition + new Vector2(-4 * player.direction, -3);
            player.itemLocation += QwertyMethods.PolarVector(12, rotation + MathF.PI / 2) ;
        }
        public override bool? UseItem(Player player)
        {
            if(player.itemAnimationMax - player.itemAnimation == 0)
            {
                player.velocity.Y = -10 - player.jumpSpeedBoost;
                player.velocity.X += 4 * player.direction;
            }
            return base.UseItem(player);
        }
    }
     public class HeavySwordCollision : ModPlayer
    {
        private int[] localNPCImmunity = new int[Main.npc.Length];

        public override void PostUpdate()
        {
            if (!Player.inventory[Player.selectedItem].IsAir && (float)Player.itemAnimation / Player.itemAnimationMax < 0.5f)
            {
                Item item = Player.inventory[Player.selectedItem];
                if(item.type == ModContent.ItemType<Overkill>())
                {
                    float swordLength = TextureAssets.Item[item.type].Value.Height;
                    swordLength *= item.scale;
                    for (int n = 0; n < Main.npc.Length; n++)
                    {
                        localNPCImmunity[n]--;
                        if (Main.npc[n].active && !Main.npc[n].dontTakeDamage && (!Main.npc[n].friendly || (Main.npc[n].type == NPCID.Guide && Player.killGuide) || (Main.npc[n].type == NPCID.Clothier && Player.killClothier)) && Player.itemAnimation > 0 && localNPCImmunity[n] <= 0 && Collision.CheckAABBvLineCollision(Main.npc[n].position, Main.npc[n].Size, Player.itemLocation, Player.itemLocation + QwertyMethods.PolarVector(swordLength, Player.itemRotation + MathF.PI / 2 * -Player.direction)))
                        {
                            localNPCImmunity[n] = item.useAnimation / 2;
                            Projectile p = QwertyMethods.PokeNPC(Player, Main.npc[n], Player.GetSource_ItemUse(item), 
                            Player.GetWeaponDamage(item), DamageClass.Melee, item.knockBack);
                            p.GetGlobalProjectile<EtimsProjectile>().effect = true;
                            
                        }
                    }
                }
            }
        }
    }
    public class WeirdSwordBig : PlayerDrawLayer
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return true;
        }
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.HeldItem);
        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            if (drawInfo.drawPlayer.JustDroppedAnItem)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)drawInfo.Position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawInfo.Position.Y + (double)drawPlayer.height * 0.5) / 16, Microsoft.Xna.Framework.Color.White), 0f);
            if (!drawPlayer.HeldItem.IsAir && drawPlayer.HeldItem.type == ItemType<Overkill>() && drawPlayer.itemAnimation > 0)
            {
                Item item = drawPlayer.HeldItem;
                Texture2D texture = TextureAssets.Item[item.type].Value;
                Vector2 origin = new Vector2(texture.Width / 2, drawPlayer.direction == 1 ? texture.Height - 12 : 12);
                DrawData value = new DrawData(texture, drawPlayer.itemLocation - Main.screenPosition, texture.Frame(), color12, drawPlayer.itemRotation, origin, item.scale, drawPlayer.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0);
                drawInfo.DrawDataCache.Add(value);
            }
        }
    }
}