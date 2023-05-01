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

namespace QwertyMod.Content.Items.Weapon.Melee.Sword.EtimsSword
{
    public class EtimsSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("The Massacre");
            //Tooltip.SetDefault("Right click on the ground for an uppercut" + "\nRight click in the air to slam down!" + "\nKilling an enemy grants immunity to hostile projectiles for a short time.");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }


        public override void SetDefaults()
        {
            Item.useStyle = 101; //custom use style
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.damage = 50;
            Item.knockBack = 7f;
            Item.width = 36;
            Item.height = 34;
            Item.noUseGraphic = true;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.rare = ItemRarityID.Orange;
            Item.value = 120000;
            Item.useTurn = true;
            Item.scale = 1.8f;
        }


        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemType<Etims>(), 12)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override bool CanUseItem(Player player)
        {
            return player.itemAnimation == 0;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
    }

    public class AltSword : ModPlayer
    {
        private int[] localNPCImmunity = new int[Main.npc.Length];
        private bool uppercut = false;
        private bool slam = false;
        private bool hasRightClicked = false;

        public override void PostItemCheck()
        {
            if (!Player.inventory[Player.selectedItem].IsAir)
            {
                Point origin = Player.Bottom.ToTileCoordinates();
                Point point;
                Item item = Player.inventory[Player.selectedItem];
                //Main.NewText(Player.itemAnimation  + " / " + Player.itemAnimationMax);

                if (item.useStyle == 101)
                {
                    if (Main.mouseRight && !hasRightClicked)
                    {
                        if (WorldUtils.Find(origin, Searches.Chain(new Searches.Down(3), new GenCondition[]
                                            {
                                            new Conditions.IsSolid()
                                            }), out point))
                        {
                            Player.itemAnimation = Player.itemAnimationMax;
                            Player.velocity.Y = -10 - Player.jumpSpeedBoost;
                            uppercut = true;
                            slam = false;
                        }
                        else
                        {
                            Player.velocity.Y = 10;
                            slam = true;
                            uppercut = false;
                        }
                    }
                    float shift = 0f;
                    if (Player.itemAnimation > 0 && uppercut || slam)
                    {
                        if (slam)
                        {
                            //Main.NewText("Slamming");
                            Player.bodyFrame.Y = Player.bodyFrame.Height * 4;
                            shift = MathF.PI / 2;

                            if (Player.velocity.Y != 0)
                            {
                                Player.itemAnimation = 2;
                            }
                            else
                            {
                                Player.itemAnimation = 0;
                                slam = false;
                            }
                            Player.velocity.Y = 10;
                        }
                        else if (uppercut)
                        {
                            shift = MathF.PI / 2 * ((float)Player.itemAnimation / (float)Player.itemAnimationMax) - MathF.PI / 4;

                            if (Player.itemAnimation < Player.itemAnimationMax * .5f)
                            {
                                Player.bodyFrame.Y = Player.bodyFrame.Height * 2;
                            }
                            else if (Player.itemAnimation < Player.itemAnimationMax * .25f)
                            {
                                Player.bodyFrame.Y = Player.bodyFrame.Height * 3;
                            }
                            else
                            {
                                Player.bodyFrame.Y = Player.bodyFrame.Height * 4;
                            }
                            if (Player.itemAnimation < 2)
                            {
                                Player.itemAnimation = 2;
                            }
                            if (Player.velocity.Y >= 0)
                            {
                                Player.itemAnimation = 0;
                                uppercut = false;
                            }
                        }
                    }
                    else
                    {
                        if (Player.itemAnimation < Player.itemAnimationMax * .25f)
                        {
                            shift = MathF.PI / -4 * ((Player.itemAnimation) / (Player.itemAnimationMax * .25f));
                        }
                        else if (Player.itemAnimation < Player.itemAnimationMax * .75f)
                        {
                            shift = MathF.PI / -2 * (1 - (Player.itemAnimation - (Player.itemAnimationMax * .25f)) / (Player.itemAnimationMax * .5f)) + MathF.PI / 4;
                        }
                        else
                        {
                            shift = MathF.PI / 4 * (1 - (Player.itemAnimation - (Player.itemAnimationMax * .75f)) / (Player.itemAnimationMax * .25f));
                        }
                        if (Player.itemAnimation < Player.itemAnimationMax * .15f)
                        {
                            Player.bodyFrame.Y = Player.bodyFrame.Height * 3;
                        }
                        else if (Player.itemAnimation < Player.itemAnimationMax * .35f)
                        {
                            Player.bodyFrame.Y = Player.bodyFrame.Height * 2;
                        }
                        else if (Player.itemAnimation < Player.itemAnimationMax * .65f)
                        {
                            Player.bodyFrame.Y = Player.bodyFrame.Height * 3;
                        }
                        else if (Player.itemAnimation < Player.itemAnimationMax * .85f)
                        {
                            Player.bodyFrame.Y = Player.bodyFrame.Height * 4;
                        }
                        else
                        {
                            Player.bodyFrame.Y = Player.bodyFrame.Height * 3;
                        }
                    }
                    if (Main.mouseRight && !slam && !uppercut)
                    {
                        Player.itemAnimation = 0;
                    }

                    Player.itemRotation = MathF.PI / -4 + Player.direction * (MathF.PI / 2 + shift);
                    //Main.NewText(MathHelper.ToDegrees(Player.itemRotation));

                    Vector2 vector24 = Main.OffsetsPlayerOnhand[Player.bodyFrame.Y / 56] * 2f;
                    if (Player.direction != 1)
                    {
                        vector24.X = (float)Player.bodyFrame.Width - vector24.X;
                    }
                    if (Player.gravDir != 1f)
                    {
                        vector24.Y = (float)Player.bodyFrame.Height - vector24.Y;
                    }
                    vector24 -= new Vector2((float)(Player.bodyFrame.Width - Player.width), (float)(Player.bodyFrame.Height - 42)) / 2f;
                    Player.itemLocation = Player.position + vector24;

                    float swordLength = new Vector2((TextureAssets.Item[item.type].Value).Width, (TextureAssets.Item[item.type].Value).Height).Length();
                    swordLength *= item.scale;
                    for (int n = 0; n < Main.npc.Length; n++)
                    {
                        localNPCImmunity[n]--;
                        if (Main.npc[n].active && !Main.npc[n].dontTakeDamage && (!Main.npc[n].friendly || (Main.npc[n].type == NPCID.Guide && Player.killGuide) || (Main.npc[n].type == NPCID.Clothier && Player.killClothier)) && Player.itemAnimation > 0 && localNPCImmunity[n] <= 0 && Collision.CheckAABBvLineCollision(Main.npc[n].position, Main.npc[n].Size, Player.itemLocation, Player.itemLocation + QwertyMethods.PolarVector(swordLength, Player.itemRotation - MathF.PI / 4)))
                        {
                            localNPCImmunity[n] = item.useAnimation / 3;
                            int damageBeforeVariance = Player.GetWeaponDamage(item);
                            if (slam || uppercut)
                            {
                                damageBeforeVariance *= 2;
                            }
                            if (slam)
                            {
                                Player.immune = true;
                                if (Player.immuneTime < 60)
                                {
                                    Player.immuneTime = 60;
                                }
                            }
                            if (!WorldUtils.Find(origin, Searches.Chain(new Searches.Down(3), new GenCondition[]
                                            {
                                            new Conditions.IsSolid()
                                            }), out point) && Player.GetModPlayer<SkywardHiltEffect>().effect && Player.grappling[0] == -1)
                            {
                                damageBeforeVariance *= 2;
                            }
                            //////////////////////

                            Projectile p = QwertyMethods.PokeNPC(Player, Main.npc[n], Player.GetSource_ItemUse(item), damageBeforeVariance, DamageClass.Melee, item.knockBack);
                            if (item.type == ItemType<EtimsSword>())
                            {
                                p.GetGlobalProjectile<EtimsProjectile>().effect = true;
                                p.GetGlobalProjectile<GiveAntiProjectileOnKill>().yes = true;
                            }
                        }
                    }
                    hasRightClicked = (Main.mouseRight);
                }
            }
        }
        public override bool CanBeHitByProjectile(Projectile proj)
        {
            if (Player.HasBuff(BuffType<AntiProjectile>()))
            {
                return false;
            }
            return base.CanBeHitByProjectile(proj);
        }
    }
    public class GiveAntiProjectileOnKill : GlobalProjectile
    {
        public bool yes = false;
        public override bool InstancePerEntity => true;
        
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (damageDone > target.life && !target.SpawnedFromStatue && projectile.GetGlobalProjectile<GiveAntiProjectileOnKill>().yes)
            {
                Main.player[projectile.owner].AddBuff(BuffType<AntiProjectile>(), 360);
            }
        }
    }
    public class WeirdSword : PlayerDrawLayer
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
            if (!drawPlayer.HeldItem.IsAir && drawPlayer.HeldItem.type == ItemType<EtimsSword>() && drawPlayer.itemAnimation > 0)
            {
                Item item = drawPlayer.HeldItem;
                Texture2D texture = TextureAssets.Item[item.type].Value;
                Vector2 origin = new Vector2(0, texture.Height);
                DrawData value = new DrawData(texture, drawPlayer.itemLocation - Main.screenPosition, texture.Frame(), color12, drawPlayer.itemRotation, origin, item.scale, SpriteEffects.None, 0);
                drawInfo.DrawDataCache.Add(value);
            }
        }
    }
}