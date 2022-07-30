
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using QwertyMod.Common.PlayerLayers;
using Terraria;
using Microsoft.Xna.Framework.Graphics;

namespace QwertyMod.Content.Items.MiscMaterials
{
    public class InvaderPlating : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Invader Scrap");
            Tooltip.SetDefault("Leftover invader parts");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }


        public override void SetDefaults()
        {
            Item.value = 1000;
            Item.width = Item.height = 32;
            Item.maxStack = 999;
            Item.rare = 8;
            if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = ModContent.Request<Texture2D>("QwertyMod/Content/Items/MiscMaterials/InvaderPlating_Glow").Value;
            }
        }
    }
}