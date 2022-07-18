using QwertyMod.Common;
using QwertyMod.Content.Items.Consumable.Tiles.Bars;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Equipment.Accessories
{
    [AutoloadEquip(EquipType.HandsOn, EquipType.HandsOff)]
    public class RhuthiniumBracelet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rhuthinium Bracelet");
            Tooltip.SetDefault("Increases grappling hook pull speed");
        }

        public override void SetDefaults()
        {
            Item.value = 10000;
            Item.rare = 3;

            Item.width = 28;
            Item.height = 22;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<CommonStats>().hookSpeed += 1f;
            base.UpdateAccessory(player, hideVisual);
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemType<RhuthiniumBar>(), 4)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}