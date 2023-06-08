﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Common.Playerlayers
{
    class LegArmorExtra : PlayerDrawLayer
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
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("QwertyMod");
            if (OnLegDraw.LegDictionary.ContainsKey(drawPlayer.legs))
            {
                Color color12 = drawInfo.colorArmorLegs;
                bool glowmask = OnLegDraw.LegDictionary[drawPlayer.legs].glowmask;
                if (glowmask)
                {
                    color12 = Color.White;
                }
                int defaultColor = OnLegDraw.LegDictionary[drawPlayer.legs].useDefaultColor;
                if(defaultColor != -1)
                {
                    switch(defaultColor)
                    {
                        case 0:
                        color12 = drawPlayer.shirtColor;
                        break;
                        case 1:
                        color12 = drawPlayer.underShirtColor;
                        break;
                        case 2:
                        color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColorClamped((int)((double)drawInfo.Position.X + (double)drawPlayer.width * 0.5) / 16, (int)(((double)drawInfo.Position.Y + (double)drawPlayer.height * 0.75) / 16.0), drawPlayer.shoeColor), drawInfo.shadow);
                        break;
                        case 3:
                        color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColorClamped((int)((double)drawInfo.Position.X + (double)drawPlayer.width * 0.5) / 16, (int)(((double)drawInfo.Position.Y + (double)drawPlayer.height * 0.75) / 16.0), drawPlayer.shoeColor), drawInfo.shadow);
                        break;
                    }
                }
                Texture2D texture = OnLegDraw.LegDictionary[drawPlayer.legs].texture;
                int useShader = OnLegDraw.LegDictionary[drawPlayer.legs].useShader;

                if (!drawPlayer.Male)
                {
                    texture = OnLegDraw.LegDictionary[drawPlayer.legs].femaleTexture;
                }
                Vector2 legsOffset = drawInfo.legsOffset;
                DrawData item;
                if (drawInfo.isSitting)
                {

                    if ((!ShouldOverrideLegs_CheckShoes(ref drawInfo) || drawInfo.drawPlayer.wearsRobe))
                    {
                        if (!drawInfo.drawPlayer.invis)
                        {
                            DrawSittingLegs(ref drawInfo, texture, color12, useShader, glowmask);
                        }
                    }
                }
                else if ((!ShouldOverrideLegs_CheckShoes(ref drawInfo) || drawInfo.drawPlayer.wearsRobe))
                {
                    if (drawInfo.drawPlayer.invis)
                    {
                        return;
                    }
                    item = new DrawData(texture, legsOffset + new Vector2((int)(drawInfo.Position.X - Main.screenPosition.X - (float)(drawInfo.drawPlayer.legFrame.Width / 2) + (float)(drawInfo.drawPlayer.width / 2)), (int)(drawInfo.Position.Y - Main.screenPosition.Y + (float)drawInfo.drawPlayer.height - (float)drawInfo.drawPlayer.legFrame.Height + 4f)) + drawInfo.drawPlayer.legPosition + drawInfo.legVect, drawInfo.drawPlayer.legFrame, color12, drawInfo.drawPlayer.legRotation, drawInfo.legVect, 1f, drawInfo.playerEffect, 0);
                    if (useShader == -1)
                    {
                        item.shader = drawInfo.cLegs;
                    }
                    else
                    {
                        item.shader = drawPlayer.dye[useShader].dye;
                    }
                    drawInfo.DrawDataCache.Add(item);
                }

            }

        }
        static void DrawSittingLegs(ref PlayerDrawSet drawInfo, Texture2D textureToDraw, Color matchingColor, int shaderIndex = -1, bool glowmask = false)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Vector2 legsOffset = drawInfo.legsOffset;
            Vector2 value = new Vector2((int)(drawInfo.Position.X - Main.screenPosition.X - (float)(drawInfo.drawPlayer.legFrame.Width / 2) + (float)(drawInfo.drawPlayer.width / 2)), (int)(drawInfo.Position.Y - Main.screenPosition.Y + (float)drawInfo.drawPlayer.height - (float)drawInfo.drawPlayer.legFrame.Height + 4f)) + drawInfo.drawPlayer.legPosition + drawInfo.legVect;
            Rectangle legFrame = drawInfo.drawPlayer.legFrame;
            value.Y -= 2f;
            value.Y += drawInfo.seatYOffset;
            value += legsOffset;
            int num = 2;
            int num2 = 42;
            int num3 = 2;
            int num4 = 2;
            int num5 = 0;
            int num6 = 0;
            int num7 = 0;
            bool flag = drawInfo.drawPlayer.legs == 101 || drawInfo.drawPlayer.legs == 102 || drawInfo.drawPlayer.legs == 118 || drawInfo.drawPlayer.legs == 99;
            if (drawInfo.drawPlayer.wearsRobe && !flag)
            {
                num = 0;
                num4 = 0;
                num2 = 6;
                value.Y += 4f;
                legFrame.Y = legFrame.Height * 5;
            }
            switch (drawInfo.drawPlayer.legs)
            {
                case 214:
                case 215:
                case 216:
                    num = -6;
                    num4 = 2;
                    num5 = 2;
                    num3 = 4;
                    num2 = 6;
                    legFrame = drawInfo.drawPlayer.legFrame;
                    value.Y += 2f;
                    break;
                case 106:
                case 143:
                case 226:
                    num = 0;
                    num4 = 0;
                    num2 = 6;
                    value.Y += 4f;
                    legFrame.Y = legFrame.Height * 5;
                    break;
                case 132:
                    num = -2;
                    num7 = 2;
                    break;
                case 193:
                case 194:
                    if (drawInfo.drawPlayer.body == 218)
                    {
                        num = -2;
                        num7 = 2;
                        value.Y += 2f;
                    }
                    break;
                case 210:
                    if (glowmask)
                    {
                        Vector2 vector = new Vector2((float)Main.rand.Next(-10, 10) * 0.125f, (float)Main.rand.Next(-10, 10) * 0.125f);
                        value += vector;
                    }
                    break;
            }
            for (int num8 = num3; num8 >= 0; num8--)
            {
                Vector2 position = value + new Vector2(num, 2f) * new Vector2(drawInfo.drawPlayer.direction, 1f);
                Rectangle value2 = legFrame;
                value2.Y += num8 * 2;
                value2.Y += num2;
                value2.Height -= num2;
                value2.Height -= num8 * 2;
                if (num8 != num3)
                {
                    value2.Height = 2;
                }
                position.X += drawInfo.drawPlayer.direction * num4 * num8 + num6 * drawInfo.drawPlayer.direction;
                if (num8 != 0)
                {
                    position.X += num7 * drawInfo.drawPlayer.direction;
                }
                position.Y += num2;
                position.Y += num5;
                DrawData item = new DrawData(textureToDraw, position, value2, matchingColor, drawInfo.drawPlayer.legRotation, drawInfo.legVect, 1f, drawInfo.playerEffect, 0);
                if (shaderIndex == -1)
                {
                    item.shader = drawInfo.cLegs;
                }
                else
                {
                    item.shader = drawPlayer.dye[shaderIndex].dye;
                }
                drawInfo.DrawDataCache.Add(item);
            }
        }
        public static bool ShouldOverrideLegs_CheckShoes(ref PlayerDrawSet drawInfo)
        {
            if (drawInfo.drawPlayer.shoe == 15)
            {
                return true;
            }
            return false;
        }
    }
    public class OnLegDraw
    {
        public static Dictionary<int, OnLegDraw> LegDictionary = new Dictionary<int, OnLegDraw>();
        public Texture2D texture;
        public Texture2D femaleTexture;
        public bool glowmask = true;
        public int useShader = -1;
        public int useDefaultColor = -1;
        public OnLegDraw(Texture2D texture, Texture2D femaleTexture, bool glowmask = true, int useShader = -1, int useDefaultColor = -1)
        {
            this.texture = texture;
            this.femaleTexture = femaleTexture;
            this.glowmask = glowmask;
            this.useShader = useShader;
            this.useDefaultColor = useDefaultColor;
        }
        public static void RegisterLegs()
        {
            var immediate = AssetRequestMode.ImmediateLoad;
            Mod mod = ModLoader.GetMod("QwertyMod");
            OnLegDraw leg = new OnLegDraw(Request<Texture2D>("QwertyMod/Content/Items/Equipment/Armor/Hydra/HydraLeggings_Legs_Glow", immediate).Value, Request<Texture2D>("QwertyMod/Content/Items/Equipment/Armor/Hydra/HydraLeggings_FemaleLegs_Glow", immediate).Value);
            LegDictionary.Add(QwertyMod.hydraLegMale, leg);
            LegDictionary.Add(QwertyMod.hydraLegFemale, leg);
            leg = new OnLegDraw(Request<Texture2D>("QwertyMod/Content/Items/Equipment/Armor/Glass/GlassLimbguards_Legs_Glass", immediate).Value, Request<Texture2D>("QwertyMod/Content/Items/Equipment/Armor/Glass/GlassLimbguards_Legs_Glass", immediate).Value, false, 3);
            LegDictionary.Add(EquipLoader.GetEquipSlot(mod, "GlassLimbguards", EquipType.Legs), leg);
            leg = new OnLegDraw(Request<Texture2D>("QwertyMod/Content/Items/Equipment/Armor/Vitallum/VitallumJeans_Legs_Vein", immediate).Value, Request<Texture2D>("QwertyMod/Content/Items/Equipment/Armor/Vitallum/VitallumJeans_FemaleLegs_Vein", immediate).Value, false, 3);
            LegDictionary.Add(QwertyMod.VitLegMale, leg);
            LegDictionary.Add(QwertyMod.VitLegFemale, leg);
            leg = new OnLegDraw(Request<Texture2D>("QwertyMod/Content/Items/Equipment/Armor/Bionic/BionicLimbs_Legs_Glow", immediate).Value, Request<Texture2D>("QwertyMod/Content/Items/Equipment/Armor/Bionic/BionicLimbs_FemaleLegs_Glow", immediate).Value);
            LegDictionary.Add(QwertyMod.BionicLegMale, leg);
            LegDictionary.Add(QwertyMod.BionicLegFemale, leg);
            //leg = new OnLegDraw(Request<Texture2D>("QwertyMod/Content/Items/Equipment/Vanity/Casual/CasualSkirt_Skirt", immediate).Value, Request<Texture2D>("QwertyMod/Content/Items/Equipment/Vanity/Casual/CasualSkirt_Skirt", immediate).Value, false, useDefaultColor: 2);
            //LegDictionary.Add(EquipLoader.GetEquipSlot(QwertyMod.Instance, "CasualSkirt", EquipType.Legs), leg);
            leg = new OnLegDraw(Request<Texture2D>("QwertyMod/Content/Items/Equipment/Vanity/Casual/CasualSkirt_Shoes", immediate).Value, Request<Texture2D>("QwertyMod/Content/Items/Equipment/Vanity/Casual/CasualSkirt_Shoes", immediate).Value, false, 3, 3);
            LegDictionary.Add(EquipLoader.GetEquipSlot(QwertyMod.Instance, "CasualSkirt", EquipType.Legs), leg);

            leg = new OnLegDraw(Request<Texture2D>("QwertyMod/Content/Items/Equipment/Armor/Invader/InvaderLanders_Legs_Glow", immediate).Value,
            Request<Texture2D>("QwertyMod/Content/Items/Equipment/Armor/Invader/InvaderLanders_FemaleLegs_Glow", immediate).Value);
            LegDictionary.Add(QwertyMod.invaderLanderMale, leg);
            LegDictionary.Add(QwertyMod.invaderLanderFemale, leg);
        }
    }
}
