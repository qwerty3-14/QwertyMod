using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.GameContent;
using QwertyMod.Content.Items.Equipment.Vanity.ScarletBallGown;

namespace QwertyMod.Content.Items.Equipment.VanityAccessories.Corset
{
    [AutoloadEquip(EquipType.Body, EquipType.Waist)]
    public class Corset : ModItem
    {
        public override void SetStaticDefaults()
        {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ArmorIDs.Waist.Sets.UsesTorsoFraming[Item.waistSlot] = true;
            ArmorIDs.Waist.Sets.UsesTorsoFraming[QwertyMod.CorsetMale] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 30;
            Item.rare = ItemRarityID.Blue;
            Item.vanity = true;
            Item.value = Item.buyPrice(gold: 3);
            Item.accessory = true;
        }
    }
    class CorsetSlimmedBody : PlayerDrawLayer
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return true;
        }
        public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.Skin);

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("QwertyMod");
            if(!drawInfo.hidesTopSkin && (drawPlayer.body == EquipLoader.GetEquipSlot(mod, "Corset", EquipType.Body) || drawPlayer.body == EquipLoader.GetEquipSlot(mod, "ScarletBallGown", EquipType.Body) || drawPlayer.waist == EquipLoader.GetEquipSlot(mod, "Corset", EquipType.Waist)))
            {
                if (drawInfo.usesCompositeTorso) 
                {
				    DrawPlayer_12_Skin_Composite(ref drawInfo);
                    if(drawPlayer.waist == EquipLoader.GetEquipSlot(mod, "Corset", EquipType.Waist))
                    {
                        drawInfo.compTorsoFrame = new Rectangle(drawInfo.compTorsoFrame.X, drawInfo.compTorsoFrame.Y, drawInfo.compTorsoFrame.Width, 36);
                        if(drawPlayer.Male)
                        {
                            drawPlayer.waist = QwertyMod.CorsetMale;
                        }
                    }
				    return;
                }

                drawInfo.hidesTopSkin = true;
                drawInfo.Position.Y += drawInfo.torsoOffset;
                Texture2D texture = ModContent.Request<Texture2D>("QwertyMod/Content/Items/Equipment/VanityAccessories/Corset/CorsetSlimmedBody").Value;
                if(drawInfo.skinVar > 9)
                {
                    texture = ModContent.Request<Texture2D>("QwertyMod/Content/Items/Equipment/VanityAccessories/Corset/SlimmedMannequin").Value;
                }
                DrawData drawData = new DrawData(texture, new Vector2((int)(drawInfo.Position.X - Main.screenPosition.X - (float)(drawInfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawInfo.drawPlayer.width / 2)), (int)(drawInfo.Position.Y - Main.screenPosition.Y + (float)drawInfo.drawPlayer.height - (float)drawInfo.drawPlayer.bodyFrame.Height + 4f)) + drawInfo.drawPlayer.bodyPosition + new Vector2(drawInfo.drawPlayer.bodyFrame.Width / 2, drawInfo.drawPlayer.bodyFrame.Height / 2), drawInfo.drawPlayer.bodyFrame, drawInfo.colorBodySkin, drawInfo.drawPlayer.bodyRotation, drawInfo.bodyVect, 1f, drawInfo.playerEffect, 0);
                drawData.shader = drawInfo.skinDyePacked;
                DrawData item = drawData;
                drawInfo.DrawDataCache.Add(item);
                drawInfo.Position.Y -= drawInfo.torsoOffset;
                if(drawPlayer.waist == EquipLoader.GetEquipSlot(mod, "Corset", EquipType.Waist))
                {
                    drawInfo.compTorsoFrame = new Rectangle(drawInfo.compTorsoFrame.X, drawInfo.compTorsoFrame.Y, drawInfo.compTorsoFrame.Width, 36);
                    if(drawPlayer.Male)
                    {
                        drawPlayer.waist = QwertyMod.CorsetMale;
                    }
                    
                }
                
            }
            //drawInfo.compTorsoFrame = new Rectangle(drawInfo.compTorsoFrame.X, drawInfo.compTorsoFrame.Y, drawInfo.compTorsoFrame.Width, 20);
            
        }
        public static void DrawPlayer_12_Skin_Composite(ref PlayerDrawSet drawinfo) 
        {
			DrawData drawData;
			PlayerDrawLayers.DrawPlayer_12_SkinComposite_BackArmShirt(ref drawinfo);
			if (!drawinfo.drawPlayer.invis) 
            {
				Vector2 vector = new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2);
				vector.Y += drawinfo.torsoOffset;
				Vector2 value = Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height];
				value.Y -= 2f;
				vector += value * -drawinfo.playerEffect.HasFlag(SpriteEffects.FlipVertically).ToDirectionInt();
				float bodyRotation = drawinfo.drawPlayer.bodyRotation;
				if (drawinfo.drawFloatingTube) 
                {
					drawData = new DrawData(TextureAssets.Extra[105].Value, vector, new Rectangle(0, 0, 40, 56), drawinfo.floatingTubeColor, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0) {
						shader = drawinfo.cFloatingTube
					};
                    drawinfo.DrawDataCache.Add(drawData);
				}
                Texture2D texture = ModContent.Request<Texture2D>("QwertyMod/Content/Items/Equipment/VanityAccessories/Corset/CorsetSlimmedBody").Value;
                if(drawinfo.skinVar > 9)
                {
                    texture = ModContent.Request<Texture2D>("QwertyMod/Content/Items/Equipment/VanityAccessories/Corset/SlimmedMannequin").Value;
                }
				drawData = new DrawData(texture, vector, drawinfo.compTorsoFrame, drawinfo.colorBodySkin, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0) 
                {
					shader = drawinfo.skinDyePacked
				};
                drawinfo.DrawDataCache.Add(drawData);
			}
            drawinfo.hidesTopSkin = true;
		}
        private static Vector2 GetCompositeOffset_BackArm(ref PlayerDrawSet drawinfo) => new Vector2(6 * ((!drawinfo.playerEffect.HasFlag(SpriteEffects.FlipHorizontally)) ? 1 : (-1)), 2 * ((!drawinfo.playerEffect.HasFlag(SpriteEffects.FlipVertically)) ? 1 : (-1)));
		private static Vector2 GetCompositeOffset_FrontArm(ref PlayerDrawSet drawinfo) => new Vector2(-5 * ((!drawinfo.playerEffect.HasFlag(SpriteEffects.FlipHorizontally)) ? 1 : (-1)), 0f);
        
    }
    class CorsetReturnArms : PlayerDrawLayer
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return true;
        }
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Skin);

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            drawInfo.hidesTopSkin = false;
        }

    }
    class CorsetShiftGravityFlip : PlayerDrawLayer
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return true;
        }
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Leggings);

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            if(drawInfo.drawPlayer.gravDir == -1 && drawInfo.compTorsoFrame.Height == 36)
            {
                drawInfo.Position.Y += 20;
            }
        }

    }
    class CorsetReverseShiftGravityFlip : PlayerDrawLayer
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return true;
        }
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Torso);

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            if(drawInfo.drawPlayer.gravDir == -1 && drawInfo.compTorsoFrame.Height == 36)
            {
                drawInfo.Position.Y -= 20;
            }
        }

    }
    
}