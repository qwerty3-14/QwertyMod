using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent;

namespace QwertyMod.Common.Playerlayers
{
    class BackArmLayeringBugFix : PlayerDrawLayer
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return true;
        }
        public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.ArmorLongCoat);

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("QwertyMod");
            if (drawInfo.usesCompositeTorso) 
            {
                DrawPlayer_12_Skin_Composite(ref drawInfo);
                return;
            }
                
            
            //drawInfo.compTorsoFrame = new Rectangle(drawInfo.compTorsoFrame.X, drawInfo.compTorsoFrame.Y, drawInfo.compTorsoFrame.Width, 20);
            
        }
        public static void DrawPlayer_12_Skin_Composite(ref PlayerDrawSet drawinfo) 
        {
			DrawData drawData;
			//PlayerDrawLayers.DrawPlayer_12_SkinComposite_BackArmShirt(ref drawinfo);
			if (!drawinfo.drawPlayer.invis && drawinfo.drawPlayer.compositeBackArm.enabled) 
            {

				Vector2 vector = new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2);
				//vector.Y += drawinfo.torsoOffset;
				Vector2 value = Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height];
				value.Y -= 2f;
				vector += value * -drawinfo.playerEffect.HasFlag(SpriteEffects.FlipVertically).ToDirectionInt();
				float bodyRotation = drawinfo.drawPlayer.bodyRotation;
                Mod mod = ModLoader.GetMod("QwertyMod");
                Texture2D texture = TextureAssets.Players[drawinfo.skinVar, 3].Value;

				if(drawinfo.drawPlayer.body == EquipLoader.GetEquipSlot(mod, "Corset", EquipType.Body) || drawinfo.drawPlayer.body == EquipLoader.GetEquipSlot(mod, "ScarletBallGown", EquipType.Body) || drawinfo.drawPlayer.waist == EquipLoader.GetEquipSlot(mod, "Corset", EquipType.Waist))
                {
                    texture = ModContent.Request<Texture2D>("QwertyMod/Content/Items/Equipment/VanityAccessories/Corset/CorsetSlimmedBody").Value;
                }

				drawData = new DrawData(texture, vector, drawinfo.compTorsoFrame, drawinfo.colorBodySkin, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0) 
                {
					shader = drawinfo.skinDyePacked
				};
                drawinfo.DrawDataCache.Add(drawData);
			}
		}
       
    }
}