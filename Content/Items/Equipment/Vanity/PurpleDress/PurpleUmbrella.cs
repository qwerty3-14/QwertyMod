using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.GameContent.Golf;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Equipment.Vanity.PurpleDress
{
    public class PurpleUmbrella : ModItem
    {
        public override void SetStaticDefaults()
        {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 54;
            Item.rare = ItemRarityID.White;
            Item.accessory = true;
            Item.value = Item.sellPrice(silver: 30);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.slowFall = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1)
				.AddIngredient(ItemID.Silk, 10)
				.AddIngredient(ItemID.ShadowScale, 3)
                .AddTile(TileID.Loom)
                .Register();
        }
        
    }
    public class DrawUmbrella : PlayerDrawLayer
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return true;
        }
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.BackAcc);

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            DU(ref drawInfo, false);
        }
        public static void DU(ref PlayerDrawSet drawInfo, bool top)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            int useShader = -1;
            if (drawPlayer.HeldItem.type != ItemID.FairyQueenMagicItem)
            {
                Item umbrella = null;
                for (int i = 3; i < 10; i++)
                {
                    if (!drawPlayer.hideVisibleAccessory[i] && (drawPlayer.armor[i].type == ModContent.ItemType<PurpleUmbrella>()))
                    {
                        umbrella = drawPlayer.armor[i];
                        useShader = i;
                    }
                }
                for (int i = 13; i < 20; i++)
                {
                    if (drawPlayer.armor[i].type == ModContent.ItemType<PurpleUmbrella>())
                    {
                        umbrella = drawPlayer.armor[i];
                        useShader = i - 10;
                    }
                }
                if (umbrella != null)
                {
                    Texture2D texture = null;
                    if (umbrella.type == ModContent.ItemType<PurpleUmbrella>())
                    {
                        if (top)
                        {
                            texture = ModContent.Request<Texture2D>("QwertyMod/Content/Items/Equipment/Vanity/PurpleDress/PurpleUmbrella_Top").Value;
                        }
                        else
                        {
                            texture = ModContent.Request<Texture2D>("QwertyMod/Content/Items/Equipment/Vanity/PurpleDress/PurpleUmbrella_Handle").Value;
                        }
                    }
                    bool holdingUp = true;
                    if (!drawPlayer.slowFall || drawPlayer.controlDown || drawPlayer.velocity.Y <= 0)
                    {
                        holdingUp = false;
                    }
                    float turnArmAmt = MathF.PI * -3f / 5f;
                    if(!holdingUp)
                    {
                        turnArmAmt = MathF.PI * -1f / 5f;
                    }
                    if (!top)
                    {
                        drawPlayer.SetCompositeArmBack(enabled: true, Player.CompositeArmStretchAmount.ThreeQuarters, turnArmAmt * (float)drawPlayer.direction);
                    }
                    Vector2 offset = new Vector2(14 * drawPlayer.direction, drawPlayer.gravDir == -1  ? 6 : -6);
                    if(!holdingUp)
                    {
                        offset = new Vector2(8 * drawPlayer.direction, 0);
                    }
                    Vector2 drawAt = drawInfo.Position + new Vector2(drawPlayer.width * 0.5f, drawPlayer.height * 0.5f) + offset;
                    float rotation = drawPlayer.bodyRotation;
                    if(!holdingUp)
                    {
                        rotation += MathF.PI * 0.2f * drawPlayer.direction * -1 * drawPlayer.gravDir;
                    }

                    Vector2 origin = new Vector2(texture.Width * 0.5f, drawPlayer.gravDir == -1 ? 10 : texture.Height-10);
                    int fHeight = 56;
                    if (drawPlayer.bodyFrame.Y == 7 * fHeight || drawPlayer.bodyFrame.Y == 8 * fHeight || drawPlayer.bodyFrame.Y == 9 * fHeight || drawPlayer.bodyFrame.Y == 14 * fHeight || drawPlayer.bodyFrame.Y == 15 * fHeight || drawPlayer.bodyFrame.Y == 16 * fHeight)
                    {
                        if (drawPlayer.gravDir == -1)
                        {
                            drawAt.Y += 2;
                        }
                        else
                        {
                            drawAt.Y -= 2;
                        }
                    }

                    DrawData drawData = new DrawData(texture, drawAt - Main.screenPosition, null, drawInfo.colorArmorBody, rotation, origin, 1f, drawInfo.playerEffect, 0)
                    {
                        shader = drawPlayer.dye[useShader].dye
                    };
                    drawInfo.DrawDataCache.Add(drawData);
                }
            }
        }

    }
    public class DrawUmbrellaTop : PlayerDrawLayer
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return true;
        }
        public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.HeldItem);

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            DrawUmbrella.DU(ref drawInfo, true);
        }
    }
    class HoldUmbrella : ModPlayer
    {
        public override void PostItemCheck()
        {
            if (Player.HeldItem.type != ItemID.FairyQueenMagicItem && !GolfHelper.IsPlayerHoldingClub(Player) && Player.HeldItem.holdStyle != 5)
            {
                Item umbrella = null;
                for (int i = 3; i < 10; i++)
                {
                    if (!Player.hideVisibleAccessory[i] && (Player.armor[i].type == ModContent.ItemType<PurpleUmbrella>()))
                    {
                        umbrella = Player.armor[i];
                    }
                }
                for (int i = 13; i < 20; i++)
                {
                    if ((Player.armor[i].type == ModContent.ItemType<PurpleUmbrella>()))
                    {
                        umbrella = Player.armor[i];
                    }
                }
                if (umbrella != null)
                {
                    bool holdingUp = true;
                    if (!Player.slowFall || Player.controlDown || Player.velocity.Y <= 0)
                    {
                        holdingUp = false;
                    }
                    float turnArmAmt = MathF.PI * -3f / 5f;
                    if (!holdingUp)
                    {
                        turnArmAmt = MathF.PI * -1f / 5f;
                    }
                    Player.SetCompositeArmBack(enabled: true, Player.CompositeArmStretchAmount.ThreeQuarters, turnArmAmt * (float)Player.direction);
                }
            }
        }
    }
}