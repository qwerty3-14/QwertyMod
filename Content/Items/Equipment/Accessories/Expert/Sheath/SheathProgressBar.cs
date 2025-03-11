using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Content.NPCs.Bosses.BladeBoss;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;


namespace QwertyMod.Content.Items.Equipment.Accessories.Expert.Sheath
{
    class SheathProgressBar : PlayerDrawLayer
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return true;
        }
        public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.BackAcc);
        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            try
            {
                
                Player drawPlayer = drawInfo.drawPlayer;
                if(!drawPlayer.TryGetModPlayer<ImperiousEffect>(out ImperiousEffect modPlayer)){ return; }
                if(!(modPlayer.effect > 0)){ return; }
                for(int p = 0; p < 1000; p++)
                {
                    if(Main.projectile[p].active && Main.projectile[p].type == ModContent.ProjectileType<ImperiousP>())
                    {
                        return;
                    }
                }
                if(NPC.AnyNPCs(ModContent.NPCType<Imperious>()))
                {
                    return;
                }
                Texture2D texture = ModContent.Request<Texture2D>("QwertyMod/Content/Items/Equipment/Accessories/Expert/Sheath/SheathProgress").Value;

                int drawX = (int)(drawPlayer.position.X - Main.screenPosition.X);
                int drawY = (int)(drawPlayer.position.Y - Main.screenPosition.Y);
                Vector2 Position = drawInfo.Position;
                Vector2 origin = texture.Size() / 2;
                Vector2 pos = new Vector2((float)((int)(Position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(Position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2));
                pos.Y += 50;

                DrawData data = new DrawData(texture, pos, new Rectangle(0, 0, texture.Width, texture.Height), Color.White, 0f, origin, 1f, SpriteEffects.None, 0);
                drawInfo.DrawDataCache.Add(data);


                texture = ModContent.Request<Texture2D>("QwertyMod/Content/Items/Equipment/Accessories/Expert/Sheath/SheathBlip").Value;

                Position = drawInfo.Position;
                origin = new Vector2((texture.Width - 2) / 2, texture.Height / 4);
                pos = new Vector2((float)((int)(Position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(Position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2));
                pos.Y += 50f;

                float amountComplete = (float)(modPlayer.damageTally) / (float)(modPlayer.damageTallyMax);
                int frame = 0;
                if (amountComplete >= 1f)
                {
                    frame = 10;
                }

                data = new DrawData(texture, pos, new Rectangle(0, frame, (int)((texture.Width - 2) * amountComplete), texture.Height / 2), Color.White, 0f, origin, 1f, SpriteEffects.None, 0);
                drawInfo.DrawDataCache.Add(data);
            }
            catch
            {

            }
        }
    }
}
