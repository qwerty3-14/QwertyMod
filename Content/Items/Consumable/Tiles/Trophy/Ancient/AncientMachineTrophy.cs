using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common.PlayerLayers;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

using Terraria.ID;

namespace QwertyMod.Content.Items.Consumable.Tiles.Trophy.Ancient
{
    public class AncientMachineTrophy : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }


        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = 50000;
            Item.createTile = ModContent.TileType<AncientMachineTrophyT>();
            Item.placeStyle = 0;
            if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = ModContent.Request<Texture2D>("QwertyMod/Content/Items/Consumable/Tiles/Trophy/Ancient/AncientMachineTrophy_Glow").Value;
            }
        }


    }
}