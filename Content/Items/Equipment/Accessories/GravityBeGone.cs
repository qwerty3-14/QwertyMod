using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using QwertyMod.Common.PlayerLayers;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;

namespace QwertyMod.Content.Items.Equipment.Accessories
{
    public class GravityBeGone : ModItem
    {

        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Gravity Be Gone");
            //Tooltip.SetDefault("Makes you uneffected by gravity\nGrants immunity to fall damage\nMethod of going down not included.");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.value = QwertyMod.InvaderGearValue;
            Item.rare = ItemRarityID.Yellow;

            Item.width = 20;
            Item.height = 20;
            Item.accessory = true;

            if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = ModContent.Request<Texture2D>("QwertyMod/Content/Items/Equipment/Accessories/GravityBeGone_Glow").Value;
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.gravity = 0;
            player.noFallDmg = true;
        }
    }
}