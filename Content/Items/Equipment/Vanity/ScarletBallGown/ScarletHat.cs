using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using static Terraria.ID.ArmorIDs;
using Terraria.ID;
using Terraria;

namespace QwertyMod.Content.Items.Equipment.Vanity.ScarletBallGown
{
    [AutoloadEquip(EquipType.Head)]
    public class ScarletHat : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Head.Sets.DrawHatHair[Item.headSlot] = true;
        }

        public override void SetDefaults()
        {
            Item.value = 0;
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.White;
            Item.vanity = true;
            Item.value = Item.sellPrice(silver: 30);
        }
        public override void AddRecipes()
        {
            CreateRecipe(1)
				.AddIngredient(ItemID.Silk, 10)
				.AddIngredient(ItemID.TissueSample, 3)
                .AddTile(TileID.Loom)
                .Register();
        }
    }
}