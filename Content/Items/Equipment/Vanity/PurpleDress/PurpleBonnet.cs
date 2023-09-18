using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using static Terraria.ID.ArmorIDs;
using Terraria.ID;
using Terraria;

namespace QwertyMod.Content.Items.Equipment.Vanity.PurpleDress
{
    [AutoloadEquip(EquipType.Head)]
    public class PurpleBonnet : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Head.Sets.DrawHatHair[Item.headSlot] = true;
        }

        public override void SetDefaults()
        {
            Item.value = 0;
            Item.width = 24;
            Item.height = 22;
            Item.rare = ItemRarityID.White;
            Item.vanity = true;
            Item.value = Item.sellPrice(silver: 30);
        }
        public override void AddRecipes()
        {
            CreateRecipe(1)
				.AddIngredient(ItemID.Silk, 10)
				.AddIngredient(ItemID.ShadowScale, 3)
                .AddTile(TileID.Loom)
                .Register();
        }
    }
}