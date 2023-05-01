using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.ID;
using System;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace QwertyMod.Content.Items.Equipment.Vanity.ScarletBallGown
{
    [AutoloadEquip(EquipType.Balloon)]
    public class ScarletFan : ModItem
    {
        public override void SetStaticDefaults()
        {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ArmorIDs.Balloon.Sets.UsesTorsoFraming[Item.balloonSlot] = true;
            ArmorIDs.Balloon.Sets.DrawInFrontOfBackArmLayer[Item.balloonSlot] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 30;
            Item.rare = ItemRarityID.Blue;
            Item.vanity = true;
            Item.value = Item.buyPrice(gold: 3);
            Item.accessory = true;
            Item.value = Item.buyPrice(gold: 20);
        }
    }
    public class Fan : ModPlayer
    {
        public int idleTimer = 0;
        public bool drawAnimatedFan = false;
        
        public override void PostUpdate()
        {
            drawAnimatedFan = false;
            //Main.NewText(Player.balloon + "/" + EquipLoader.GetEquipSlot(Mod, "ScarletFan", EquipType.Balloon));
            if(Player.compositeBackArm.enabled && Player.balloonFront == EquipLoader.GetEquipSlot(Mod, "ScarletFan", EquipType.Balloon))
            {
                //Main.NewText("hmm");
                Player.balloonFront = -1;
            }
            idleTimer++;
            if(Player.ZoneJungle || Player.ZoneDesert || Player.ZoneUnderworldHeight || Player.ZoneBeach)
            {
                idleTimer++;
            }
            if(Player.itemAnimation > 0 || Player.ZoneSnow || Player.wet || (Player.velocity.Length() > 0.1f) || Player.gravDir == -1)
            {
                idleTimer = 0;
            }
            if(idleTimer > 60 && Player.balloonFront == EquipLoader.GetEquipSlot(Mod, "ScarletFan", EquipType.Balloon))
            {
                drawAnimatedFan = true;
                Player.balloonFront = -1;
                Player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Quarter, Player.direction * -2f * MathF.PI / 8f);
            }
        }
        
        public override void PostUpdateMiscEffects()
        {

        }
    }
    
    public class FanLayer : PlayerDrawLayer
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return true;
        }
        public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.ArmOverItem);

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            if( drawPlayer.TryGetModPlayer<Fan>(out Fan fanPlayer) && fanPlayer.drawAnimatedFan)
            {
                Vector2 offset = drawPlayer.GetBackHandPosition(Player.CompositeArmStretchAmount.Full, drawPlayer.direction * -2f * MathF.PI / 8f) - drawPlayer.MountedCenter;
                Vector2 drawAt = drawPlayer.MountedCenter + offset;
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
                Texture2D texture = ModContent.Request<Texture2D>("QwertyMod/Content/Items/Equipment/Vanity/ScarletBallGown/ScarletFan_Hold").Value;
                float rotation = drawPlayer.direction * -1f * (2f * MathF.PI / 8f);
                drawAt += new Vector2(drawPlayer.direction * 1, -1);//QwertyMethods.PolarVector(2, rotation + drawPlayer.direction * -1f * MathF.PI / 2f);
                Vector2 origin = new Vector2(9, 10);
                float scale = 1f + 0.1f * MathF.Sin(drawPlayer.GetModPlayer<Fan>().idleTimer * MathF.PI / 20f);
                DrawData drawData = new DrawData(texture, drawAt - Main.screenPosition, null, drawInfo.colorArmorBody, rotation, origin, new Vector2(1f, scale), drawInfo.playerEffect, 0)
                {
                    shader = drawPlayer.cBalloonFront
                };
                drawInfo.DrawDataCache.Add(drawData);
            }
        }
    }
}