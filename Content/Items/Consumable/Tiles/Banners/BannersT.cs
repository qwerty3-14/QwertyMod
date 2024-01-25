using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Content.NPCs.DinoMilitia;
using QwertyMod.Content.NPCs.Fortress;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;


namespace QwertyMod.Content.Items.Consumable.Tiles.Banners
{
    public class BannersT : ModTile
    {
        public override void SetStaticDefaults()
        {
            DustType = -1;

            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.CoordinateHeights = new int[3]
			{
				16,
				16,
				16
			};
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom | AnchorType.PlanterBox, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.StyleWrapLimit = 111;
			TileObjectData.newTile.DrawYOffset = -2;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.Platform, TileObjectData.newTile.Width, 0);
			TileObjectData.newAlternate.DrawYOffset = -10;
			TileObjectData.addAlternate(0);
			TileObjectData.addTile(Type);
        }
		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
		{
            if((tileFrameY == 0 && Main.tileSolidTop[Main.tile[i, j - 1].TileType]) || (tileFrameY == 18 && Main.tileSolidTop[Main.tile[i, j - 2].TileType]) || (tileFrameY == 36 && Main.tileSolidTop[Main.tile[i, j - 3].TileType]))
            { 
                offsetY -= 8;
            }
			base.SetDrawPositions(i, j, ref width, ref offsetY, ref height, ref tileFrameX, ref tileFrameY);
		}
        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)
            {
                Player player = Main.LocalPlayer;
                int style = Main.tile[i, j].TileFrameX / 18;
                int type;
                switch (style)
                {
                    case 0:
                        type = ModContent.NPCType<Hopper>();
                        break;

                    case 1:
                        type = ModContent.NPCType<Crawler>();
                        break;

                    case 2:
                        type = ModContent.NPCType<GuardTile>();
                        break;

                    case 3:
                        type = ModContent.NPCType<FortressFlier>();
                        break;

                    case 4:
                        type = ModContent.NPCType<Caster>();
                        break;
                    /*
                case 5:
                    type = ModContent.NPCType<Spector>();
                    break;
                    */
                case 6:
                    type = ModContent.NPCType<Triceratank>();
                    break;

                case 7:
                    type = ModContent.NPCType<Utah>();
                    break;

                case 8:
                    type = ModContent.NPCType<Velocichopper>();
                    break;

                case 9:
                    type = ModContent.NPCType<AntiAir>();
                    break;

                    case 10:
                        type = ModContent.NPCType<Swarmer>();
                        break;

                    default:
                        return;
                }
            }
        }

        public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
        {
            if (i % 2 == 1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
        }
    }
}