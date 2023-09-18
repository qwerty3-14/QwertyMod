using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.ID;

namespace QwertyMod.Content.Items.Equipment.VanityAccessories
{
    [AutoloadEquip(EquipType.Beard)]
    public class FaceMask : ModItem
    {
        public override void SetStaticDefaults()
        {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 8;
            Item.rare = ItemRarityID.White;
            Item.vanity = true;
            Item.value = Item.sellPrice(silver: 3);
            Item.accessory = true;
        }
		public override void AddRecipes()
        {
            CreateRecipe(1)
				.AddIngredient(ItemID.Silk, 10)
                .AddTile(TileID.Loom)
                .Register();
        }
    }
}