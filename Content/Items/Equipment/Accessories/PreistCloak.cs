using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using QwertyMod.Common;
using QwertyMod.Content.Items.MiscMaterials;

namespace QwertyMod.Content.Items.Equipment.Accessories
{
    public class PreistCloak : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.value = 100000;
            Item.rare = ItemRarityID.Blue;
            Item.width = 28;
            Item.height = 32;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<CommonStats>().higherBeingFriendly = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ModContent.ItemType<SoulOfHeight>(), 5)
                .AddIngredient(ItemID.Silk, 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}