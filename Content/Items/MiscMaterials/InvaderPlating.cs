using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common.PlayerLayers;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.MiscMaterials
{
    public class InvaderPlating : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }


        public override void SetDefaults()
        {
            Item.value = 1000;
            Item.width  = 24;
            Item.height = 22;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.Yellow;
            if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = ModContent.Request<Texture2D>("QwertyMod/Content/Items/MiscMaterials/InvaderPlating_Glow").Value;
            }
        }
    }
}