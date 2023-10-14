using QwertyMod.Common;
using QwertyMod.Content.Items.Consumable.Tiles.Bars;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Equipment.Accessories
{
    [AutoloadEquip(EquipType.HandsOn, EquipType.HandsOff)]
    public class LuneBracelet : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.value = 10000;
            Item.rare = ItemRarityID.Blue;

            Item.width = 28;
            Item.height = 22;

            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<Dash>().SetDash(3);
            player.GetModPlayer<Dash>().Bonus = 2;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<LuneBar>(), 4)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}