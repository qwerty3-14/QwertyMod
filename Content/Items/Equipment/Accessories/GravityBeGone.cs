using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common.PlayerLayers;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.ID;

namespace QwertyMod.Content.Items.Equipment.Accessories
{
    public class GravityBeGone : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.value = GearStats.InvaderGearValue;
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