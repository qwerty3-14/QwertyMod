using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Content.NPCs.Fortress;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Consumable.Tile.Banners
{
    public class BannersT : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.StyleWrapLimit = 111;
            TileObjectData.addTile(Type);
            DustType = -1;
            //DisableSmartCursor = true;
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Banner");
            AddMapEntry(new Color(13, 88, 130), name);
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            int style = frameX / 18;
            int item;
            switch (style)
            {
                case 0:
                    item = ItemType<HopperBanner>();
                    break;

                case 1:
                    item = ItemType<CrawlerBanner>();
                    break;

                case 2:
                    item = ItemType<GuardTileBanner>();
                    break;

                case 3:
                    item = ItemType<FortressFlierBanner>();
                    break;

                case 4:
                    item = ItemType<CasterBanner>();
                    break;

                case 5:
                    item = ItemType<SpectorBanner>();
                    break;

                case 6:
                    item = ItemType<TriceratankBanner>();
                    break;

                case 7:
                    item = ItemType<UtahBanner>();
                    break;

                case 8:
                    item = ItemType<VelocichopperBanner>();
                    break;

                case 9:
                    item = ItemType<AntiAirBanner>();
                    break;

                case 10:
                    item = ItemType<SwarmerBanner>();
                    break;

                default:
                    return;
            }
            Item.NewItem(i * 16, j * 16, 16, 48, item);
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)
            {
                Player player = Main.LocalPlayer;
                int style = Main.tile[i, j].frameX / 18;
                int type;
                switch (style)
                {
                    case 0:
                        type = NPCType<Hopper>();
                        break;

                    case 1:
                        type = NPCType<Crawler>();
                        break;

                    case 2:
                        type = NPCType<GuardTile>();
                        break;

                    case 3:
                        type = NPCType<FortressFlier>();
                        break;

                    case 4:
                        type = NPCType<Caster>();
                        break;
                        /*
                    case 5:
                        type = NPCType<Spector>();
                        break;

                    case 6:
                        type = NPCType<Triceratank>();
                        break;

                    case 7:
                        type = NPCType<Utah>();
                        break;

                    case 8:
                        type = NPCType<Velocichopper>();
                        break;

                    case 9:
                        type = NPCType<AntiAir>();
                        break;
                        */
                    case 10:
                        type = NPCType<Swarmer>();
                        break;

                    default:
                        return;
                }
                /*
                player.HasNPCBannerBuff[type] = true;
                player.hasBanner = true;
                */
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