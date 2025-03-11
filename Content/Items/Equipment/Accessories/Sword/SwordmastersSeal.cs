using QwertyMod.Common;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Equipment.Accessories.Sword
{
    public class SwordmastersSeal : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Lime;
            Item.value = Item.sellPrice(0, 50, 0, 0);
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<SkywardHiltEffect>().effect++;
            player.GetModPlayer<CommonStats>().weaponSize += 0.25f;
            player.GetModPlayer<BadgeEffect>().critOnHit++;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ModContent.ItemType<SwordsmanBadge>())
                .AddIngredient(ModContent.ItemType<SkywardHilt>())
                .AddIngredient(ModContent.ItemType<SwordEnlarger>())
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}