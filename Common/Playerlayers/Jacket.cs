using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using QwertyMod.Content.Items.Equipment.VanityAccessories;

namespace QwertyMod.Common.Playerlayers
{
    class JacketBody : PlayerDrawLayer
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return true;
        }
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.NeckAcc);

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("QwertyMod");
            
            int useShader = -1;
            Item jacket = null;

            for (int i = 3; i < 10; i++)
            {
                if (!drawPlayer.hideVisibleAccessory[i] && (drawPlayer.armor[i].type == ModContent.ItemType<Shrug>() || drawPlayer.armor[i].type == ModContent.ItemType<Jacket>()))
                {
                    jacket = drawPlayer.armor[i];
                    useShader = i;
                }
            }
            for (int i = 13; i < 20; i++)
            {
                if (drawPlayer.armor[i].type == ModContent.ItemType<Shrug>() || drawPlayer.armor[i].type == ModContent.ItemType<Jacket>())
                {
                    jacket = drawPlayer.armor[i];
                    useShader = i - 10;
                }
            }

            if (jacket != null)
            {
                Color color12 = drawInfo.colorArmorBody;

                Texture2D texture = Request<Texture2D>("QwertyMod/Content/Items/Equipment/VanityAccessories/Shrug_Jacket").Value;
                if(jacket.type == ModContent.ItemType<Jacket>())
                {
                    texture = Request<Texture2D>("QwertyMod/Content/Items/Equipment/VanityAccessories/Jacket_Jacket").Value;
                }

                Vector2 vector = new Vector2((int)(drawInfo.Position.X - Main.screenPosition.X - (float)(drawInfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawInfo.drawPlayer.width / 2)), (int)(drawInfo.Position.Y - Main.screenPosition.Y + (float)drawInfo.drawPlayer.height - (float)drawInfo.drawPlayer.bodyFrame.Height + 4f)) + drawInfo.drawPlayer.bodyPosition + new Vector2(drawInfo.drawPlayer.bodyFrame.Width / 2, drawInfo.drawPlayer.bodyFrame.Height / 2);
                Vector2 value = Main.OffsetsPlayerHeadgear[drawInfo.drawPlayer.bodyFrame.Y / drawInfo.drawPlayer.bodyFrame.Height];
                value.Y -= 2f;
                vector += value * -drawInfo.playerEffect.HasFlag(SpriteEffects.FlipVertically).ToDirectionInt();
                float bodyRotation = drawInfo.drawPlayer.bodyRotation;

                DrawData drawData;
                if (!drawInfo.drawPlayer.invis)
                {
                    Rectangle useFrame = new Rectangle(drawInfo.compTorsoFrame.X, drawInfo.compTorsoFrame.Y, drawInfo.compTorsoFrame.Width, 56);
                    drawData = new DrawData(texture, vector, useFrame, color12, bodyRotation, drawInfo.bodyVect, 1f, drawInfo.playerEffect, 0)
                    {
                        shader = drawInfo.cBody
                    };
                    if (useShader != -1)
                    {
                        drawData.shader = drawPlayer.dye[useShader].dye;
                    }
                    DrawCompositeArmorPiece(ref drawInfo, CompositePlayerDrawContext.Torso, drawData);
                }

            }
        }
        public static Vector2 GetCompositeOffset_BackArm(ref PlayerDrawSet drawInfo)
        {
            return new Vector2(6 * ((!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipHorizontally)) ? 1 : (-1)), 2 * ((!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipVertically)) ? 1 : (-1)));
        }
        public static void DrawCompositeArmorPiece(ref PlayerDrawSet drawInfo, CompositePlayerDrawContext context, DrawData data)
        {
            drawInfo.DrawDataCache.Add(data);
            switch (context)
            {
                case CompositePlayerDrawContext.BackShoulder:
                case CompositePlayerDrawContext.BackArm:
                case CompositePlayerDrawContext.FrontArm:
                case CompositePlayerDrawContext.FrontShoulder:
                    {
                        if (drawInfo.armGlowColor.PackedValue == 0)
                        {
                            break;
                        }
                        DrawData item2 = data;
                        item2.color = drawInfo.armGlowColor;
                        Rectangle value3 = item2.sourceRect.Value;
                        value3.Y += 224;
                        item2.sourceRect = value3;
                        drawInfo.DrawDataCache.Add(item2);
                        break;
                    }
                case CompositePlayerDrawContext.Torso:
                    {
                        if (drawInfo.bodyGlowColor.PackedValue == 0)
                        {
                            break;
                        }
                        DrawData item = data;
                        item.color = drawInfo.bodyGlowColor;
                        Rectangle value = item.sourceRect.Value;
                        value.Y += 224;
                        item.sourceRect = value;
                        drawInfo.DrawDataCache.Add(item);
                        break;
                    }
            }
        }
    }
    class JacketArm : PlayerDrawLayer
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return true;
        }
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.ArmOverItem);

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("QwertyMod");
            
            int useShader = -1;
            Item jacket = null;

            for (int i = 3; i < 10; i++)
            {
                if (!drawPlayer.hideVisibleAccessory[i] && (drawPlayer.armor[i].type == ModContent.ItemType<Shrug>() || drawPlayer.armor[i].type == ModContent.ItemType<Jacket>()))
                {
                    jacket = drawPlayer.armor[i];
                    useShader = i;
                }
            }
            for (int i = 13; i < 20; i++)
            {
                if (drawPlayer.armor[i].type == ModContent.ItemType<Shrug>() || drawPlayer.armor[i].type == ModContent.ItemType<Jacket>())
                {
                    jacket = drawPlayer.armor[i];
                    useShader = i - 10;
                }
            }

            if (jacket != null)
            {
                Color color12 = drawInfo.colorArmorBody;

                Texture2D texture = Request<Texture2D>("QwertyMod/Content/Items/Equipment/VanityAccessories/Shrug_Jacket").Value;
                if(jacket.type == ModContent.ItemType<Jacket>())
                {
                    texture = Request<Texture2D>("QwertyMod/Content/Items/Equipment/VanityAccessories/Jacket_Jacket").Value;
                }
            

                Vector2 vector = new Vector2((int)(drawInfo.Position.X - Main.screenPosition.X - (float)(drawInfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawInfo.drawPlayer.width / 2)), (int)(drawInfo.Position.Y - Main.screenPosition.Y + (float)drawInfo.drawPlayer.height - (float)drawInfo.drawPlayer.bodyFrame.Height + 4f)) + drawInfo.drawPlayer.bodyPosition + new Vector2(drawInfo.drawPlayer.bodyFrame.Width / 2, drawInfo.drawPlayer.bodyFrame.Height / 2);
                Vector2 value = Main.OffsetsPlayerHeadgear[drawInfo.drawPlayer.bodyFrame.Y / drawInfo.drawPlayer.bodyFrame.Height];
                value.Y -= 2f;
                vector += value * -drawInfo.playerEffect.HasFlag(SpriteEffects.FlipVertically).ToDirectionInt();
                float bodyRotation = drawInfo.drawPlayer.bodyRotation;
                float rotation = drawInfo.drawPlayer.bodyRotation + drawInfo.compositeFrontArmRotation;
                Vector2 bodyVect = drawInfo.bodyVect;
                Vector2 compositeOffset_FrontArm = GetCompositeOffset_FrontArm(ref drawInfo);
                bodyVect += compositeOffset_FrontArm;
                vector += compositeOffset_FrontArm;
                Vector2 position = vector + drawInfo.frontShoulderOffset;
                if (drawInfo.compFrontArmFrame.X / drawInfo.compFrontArmFrame.Width >= 7)
                {
                    vector += new Vector2((!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipHorizontally)) ? 1 : (-1), (!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipVertically)) ? 1 : (-1));
                }
                int num2 = (drawInfo.compShoulderOverFrontArm ? 1 : 0);
                int num3 = ((!drawInfo.compShoulderOverFrontArm) ? 1 : 0);
                int num4 = ((!drawInfo.compShoulderOverFrontArm) ? 1 : 0);
                bool flag = !drawInfo.hidesTopSkin;
                DrawData drawData;

                if (!drawInfo.drawPlayer.invis)
                {

                    for (int i = 0; i < 2; i++)
                    {
                        if (i == num2 && !drawInfo.hideCompositeShoulders)
                        {
                            drawData = new DrawData(texture, position, drawInfo.compFrontShoulderFrame, color12, bodyRotation, bodyVect, 1f, drawInfo.playerEffect, 0)
                            {
                                shader = drawInfo.cBody
                            };
                            if (useShader != -1)
                            {
                                drawData.shader = drawPlayer.dye[useShader].dye;
                            }
                            JacketBody.DrawCompositeArmorPiece(ref drawInfo, CompositePlayerDrawContext.FrontShoulder, drawData);
                        }
                        if (i == num3)
                        {
                            drawData = new DrawData(texture, vector, drawInfo.compFrontArmFrame, color12, rotation, bodyVect, 1f, drawInfo.playerEffect, 0)
                            {
                                shader = drawInfo.cBody
                            };
                            if (useShader != -1)
                            {
                                drawData.shader = drawPlayer.dye[useShader].dye;
                            }
                            JacketBody.DrawCompositeArmorPiece(ref drawInfo, CompositePlayerDrawContext.FrontArm, drawData);
                        }
                    }
                }
            }

        }
        private static Vector2 GetCompositeOffset_FrontArm(ref PlayerDrawSet drawinfo)
        {
            return new Vector2(-5 * ((!drawinfo.playerEffect.HasFlag(SpriteEffects.FlipHorizontally)) ? 1 : (-1)), 0f);
        }
    }
}
