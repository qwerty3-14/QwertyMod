using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace QwertyMod.Content.Hair
{
    public class ShortQwertyHair : ModHair
    {
        public static int type;
        public override void SetStaticDefaults()
        {
            type = Type;
        }
    }
    public class ShortHairFront : PlayerDrawLayer
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
            if(drawInfo.drawPlayer.hair == ShortQwertyHair.type)
            {
                //Main.NewText("Hair: " + drawInfo.drawPlayer.hair);
                Texture2D texture = ModContent.Request<Texture2D>("QwertyMod/Content/Hair/ShortQwertyHair_Front").Value;

                Player drawPlayer = drawInfo.drawPlayer;
                Color color12 = drawInfo.colorHair;

                Vector2 Position = drawInfo.Position;
                Vector2 origin = new Vector2((float)drawPlayer.legFrame.Width * 0.5f, (float)drawPlayer.legFrame.Height * 0.5f);
                Vector2 pos = new Vector2((float)((int)(Position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(Position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2));

                Rectangle useFrame = drawPlayer.bodyFrame;
                

                DrawData data = new DrawData(texture, pos, drawInfo.hairFrontFrame, color12, 0f, origin, 1f, drawInfo.playerEffect, 0);
                data.shader = drawInfo.hairDyePacked;

                drawInfo.DrawDataCache.Add(data);
            }
        }
    }
}