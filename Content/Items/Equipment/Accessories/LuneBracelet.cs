using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;
using QwertyMod.Common;
using QwertyMod.Content.Items.Consumable.Tiles.Bars;

namespace QwertyMod.Content.Items.Equipment.Accessories
{
    [AutoloadEquip(EquipType.HandsOn, EquipType.HandsOff)]
    public class LuneBracelet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lune Bracelet");
            Tooltip.SetDefault("Lets you do a weak dash and empowers other dashes slightly");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.value = 10000;
            Item.rare = 1;

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
            CreateRecipe(1).AddIngredient(ItemType<LuneBar>(), 4)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}