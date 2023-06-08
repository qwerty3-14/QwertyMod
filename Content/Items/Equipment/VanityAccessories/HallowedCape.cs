using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.ID;

namespace QwertyMod.Content.Items.Equipment.VanityAccessories
{
    [AutoloadEquip(EquipType.Back)]
    public class HallowedCape : ModItem
    {
        public override void SetStaticDefaults()
        {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 30;
            Item.rare = ItemRarityID.White;
            Item.vanity = true;
            Item.value = Item.sellPrice(silver: 3);
            Item.accessory = true;
        }
		public override void AddRecipes()
        {
            CreateRecipe(1)
				.AddIngredient(ItemID.Silk, 10)
				.AddIngredient(ItemID.HallowedBar, 2)
                .AddTile(TileID.Loom)
                .Register();
        }
    }
}