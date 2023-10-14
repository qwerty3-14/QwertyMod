using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;


namespace QwertyMod.Common.Playerlayers
{
    public class HeadArmorExtra : PlayerDrawLayer
    {
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return true;
        }
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Head);

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            if (OnHeadDraw.HeadDictionary.ContainsKey(drawPlayer.head))
            {
                if (!OnHeadDraw.HeadDictionary[drawPlayer.head].lowered)
                {
                    DrawHead(drawPlayer, ref drawInfo);
                }
            }
        }
        public static void DrawHead(Player drawPlayer, ref PlayerDrawSet drawInfo)
        {
            //Main.NewText("yes");
            Color color12 = drawInfo.colorArmorHead;
            bool glowmask = OnHeadDraw.HeadDictionary[drawPlayer.head].glowmask;
            if (glowmask)
            {
                color12 = Color.White;
            }
            Texture2D texture = OnHeadDraw.HeadDictionary[drawPlayer.head].texture;
            int useShader = OnHeadDraw.HeadDictionary[drawPlayer.head].useShader;

            Vector2 Position = drawInfo.Position;
            Vector2 origin = new Vector2((float)drawPlayer.legFrame.Width * 0.5f, (float)drawPlayer.legFrame.Height * 0.5f);
            Vector2 pos = new Vector2((float)((int)(Position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(Position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2));

            Rectangle useFrame = drawPlayer.bodyFrame;
            int frameCount = OnHeadDraw.HeadDictionary[drawPlayer.head].cycleFrameCount;
            if (frameCount > 1)
            {
                int f = (drawPlayer.GetModPlayer<CommonStats>().genericCounter / 10) % (frameCount + (frameCount - 2));
                if (f >= frameCount)
                {
                    f = (frameCount - 1) - (f - frameCount);
                }
                useFrame.X = 40 * f;
            }

            DrawData data = new DrawData(texture, pos, useFrame, color12, 0f, origin, 1f, drawInfo.playerEffect, 0);
            if (useShader == -1)
            {
                data.shader = drawInfo.cHead;
            }
            else
            {
                data.shader = drawPlayer.dye[useShader].dye;
            }

            drawInfo.DrawDataCache.Add(data);
        }
    }
    public class HeadArmorExtraLower : PlayerDrawLayer
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return true;
        }
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Head);

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            if (OnHeadDraw.HeadDictionary.ContainsKey(drawPlayer.head))
            {
                if (OnHeadDraw.HeadDictionary[drawPlayer.head].lowered)
                {
                    HeadArmorExtra.DrawHead(drawPlayer, ref drawInfo);
                }
            }
        }
    }
    public class OnHeadDraw
    {
        public static Dictionary<int, OnHeadDraw> HeadDictionary = new Dictionary<int, OnHeadDraw>();
        public Texture2D texture;
        public bool glowmask = true;
        public int useShader = -1;
        public int cycleFrameCount = 1;
        public bool lowered = false;
        public OnHeadDraw(Texture2D texture, bool glowmask = true, int useShader = -1, int cycleFrameCount = 1, bool lowered = false)
        {
            this.texture = texture;
            this.glowmask = glowmask;
            this.useShader = useShader;
            this.cycleFrameCount = cycleFrameCount;
            this.lowered = lowered;
        }
        public static void RegisterHeads()
        {
            var immediate = AssetRequestMode.ImmediateLoad;
            Mod mod = ModLoader.GetMod("QwertyMod");
            OnHeadDraw head = new OnHeadDraw(ModContent.Request<Texture2D>("QwertyMod/Content/Items/Equipment/Armor/Hydra/HydraHelmet_Head_Glow", immediate).Value);
            HeadDictionary.Add(EquipLoader.GetEquipSlot(mod, "HydraHelmet", EquipType.Head), head);
            head = new OnHeadDraw(ModContent.Request<Texture2D>("QwertyMod/Content/Items/Equipment/Armor/Glass/GlassHelm_Head_Glass", immediate).Value, false, 3);
            HeadDictionary.Add(EquipLoader.GetEquipSlot(mod, "GlassHelm", EquipType.Head), head);
            head = new OnHeadDraw(ModContent.Request<Texture2D>("QwertyMod/Content/Items/Equipment/Armor/Vitallum/VitallumHeadress_Head_Vein", immediate).Value, false, 3, 4);
            HeadDictionary.Add(EquipLoader.GetEquipSlot(mod, "VitallumHeadress", EquipType.Head), head);

            //head = new OnHeadDraw(ModContent.Request<Texture2D>("QwertyMod/Content/Items/Equipment/Armor/Bionic/BionicEye_Head_Lower", immediate).Value, lowered: true);
            //HeadDictionary.Add(EquipLoader.GetEquipSlot(mod, "BionicEye", EquipType.Head), head);
            head = new OnHeadDraw(ModContent.Request<Texture2D>("QwertyMod/Content/Items/Equipment/Armor/Bionic/BionicEye_Head_Glow", immediate).Value, lowered: true);
            HeadDictionary.Add(EquipLoader.GetEquipSlot(mod, "BionicEye", EquipType.Head), head);

            head = new OnHeadDraw(ModContent.Request<Texture2D>("QwertyMod/Content/Items/Equipment/Armor/Invader/InvaderPercepticals_Head_Glow", immediate).Value, lowered: true);
            HeadDictionary.Add(EquipLoader.GetEquipSlot(mod, "InvaderPercepticals", EquipType.Head), head);

            head = new OnHeadDraw(ModContent.Request<Texture2D>("QwertyMod/Content/Items/Equipment/Vanity/BossMasks/AncientMachineMask_Head_Glow", immediate).Value, lowered: true);
            HeadDictionary.Add(EquipLoader.GetEquipSlot(mod, "AncientMachineMask", EquipType.Head), head);

            head = new OnHeadDraw(ModContent.Request<Texture2D>("QwertyMod/Content/Items/Equipment/Vanity/BossMasks/InvaderMask_Head_Glow", immediate).Value, lowered: true);
            HeadDictionary.Add(EquipLoader.GetEquipSlot(mod, "InvaderMask", EquipType.Head), head);

            head = new OnHeadDraw(ModContent.Request<Texture2D>("QwertyMod/Content/Items/Equipment/Vanity/BossMasks/HydraMask_Head_Glow", immediate).Value, lowered: true);
            HeadDictionary.Add(EquipLoader.GetEquipSlot(mod, "HydraMask", EquipType.Head), head);


        }
    }
}
