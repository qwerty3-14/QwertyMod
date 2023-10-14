using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Content.Dusts;
using QwertyMod.Content.Items.Consumable.BossSummon;
using QwertyMod.Content.NPCs.Bosses.FortressBoss;
using QwertyMod.Content.NPCs.Bosses.InvaderBattleship;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;


namespace QwertyMod.Common.Fortress
{
    public class FortressAltar : ModTile
    {
        public override void SetStaticDefaults()
        {
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.LavaPlacement = LiquidPlacement.Allowed;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 0, 0);
            TileObjectData.addTile(Type);
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            DustType = ModContent.DustType<CaeliteDust>();
            HitSound = QwertyMod.FortressBlocks;
            MinPick = 10000;
        }

        public override bool RightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;
            //QwertyMethods.ServerClientCheck();
            if(SkyFortress.beingInvaded)
            {
                if (!NPC.AnyNPCs(ModContent.NPCType<InvaderBattleship>()) && BattleshipSpawnIn.spawnTimer == -1)
                {
                    for (int b = 0; b < 58; b++) // this searches every invintory slot
                    {
                        if (player.inventory[b].type == ModContent.ItemType<GodSealKeycard>() && player.inventory[b].stack > 0) //this checks if the slot has the valid item
                        {
                            
                            BattleshipSpawnIn.spawnTimer = BattleshipSpawnIn.spawnTime + BattleshipSpawnIn.scanTime + BattleshipSpawnIn.alarmTime;
                            player.inventory[b].stack--;
                            break;
                        }
                    }
                }
                return true;
            }
            if (!NPC.AnyNPCs(ModContent.NPCType<FortressBoss>()))
            {
                for (int b = 0; b < 58; b++) // this searches every invintory slot
                {
                    if (player.inventory[b].type == ModContent.ItemType<FortressBossSummon>() && player.inventory[b].stack > 0) //this checks if the slot has the valid item
                    {
                        if (Main.netMode == NetmodeID.SinglePlayer)
                        {
                            int npcID = NPC.NewNPC(NPC.GetBossSpawnSource(player.whoAmI), i * 16, j * 16 - 200, ModContent.NPCType<FortressBoss>());
                        }
                        else
                        {
                            ModPacket packet = Mod.GetPacket();
                            packet.Write((byte)ModMessageType.DivineCall);
                            packet.WriteVector2(new Vector2(i * 16 + 400, j * 16));
                            packet.Write(player.whoAmI);
                            packet.Send();
                        }

                        player.inventory[b].stack--;
                        break;
                    }
                }
            }
            return true;
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = SkyFortress.beingInvaded ? ModContent.ItemType<GodSealKeycard>() : ModContent.ItemType<FortressBossSummon>();
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.5f;
            g = 0.5f;
            b = 0.5f;
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            if(!SkyFortress.beingInvaded)
            {
                return;
            }
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if(Main.tile[i, j].TileFrameX == 36 && Main.tile[i, j].TileFrameY == 18)
            {
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
                

                Terraria.Graphics.Effects.Filters.Scene["Vortex"].GetShader().UseIntensity(1f).UseProgress(0f);
                DrawData value84 = new DrawData(Main.Assets.Request<Texture2D>("Images/Misc/Perlin").Value, 
                new Vector2((i * 16) - 9, j * 16) + zero - Main.screenPosition, 
                new Microsoft.Xna.Framework.Rectangle(0, 0, 160, 100), 
                Color.Crimson, 0f, 
                new Vector2(80f, 50f), 
                1f, 
                SpriteEffects.None, 0);

                GameShaders.Misc["ForceField"].UseColor(Color.Crimson);
                GameShaders.Misc["ForceField"].Apply(value84);
                value84.Draw(spriteBatch);
                spriteBatch.End();
                
                spriteBatch.Begin();
                return;
            }
            if(Main.tile[i, j].TileFrameX != 0 || Main.tile[i, j].TileFrameY != 0)
            {
                return;
            }
            float offset = 10 * MathF.Sin((float)Main.time * MathF.PI / 180) - 10;
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            Texture2D texture = ModContent.Request<Texture2D>("QwertyMod/Common/Fortress/GodSeal").Value;
            spriteBatch.Draw(texture, new Vector2((i * 16) + 24, j * 16 + offset) + zero - Main.screenPosition, null, Color.White, 0f, new Vector2(34, 106), 1f, SpriteEffects.None, 0f);

            
        }
    }
}