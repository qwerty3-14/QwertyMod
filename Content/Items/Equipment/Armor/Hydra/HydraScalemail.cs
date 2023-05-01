using QwertyMod.Content.Items.MiscMaterials;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace QwertyMod.Content.Items.Equipment.Armor.Hydra
{
    [AutoloadEquip(EquipType.Body)]
    public class HydraScalemail : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.value = 50000;
            Item.rare = ItemRarityID.Pink;

            Item.width = 30;
            Item.height = 20;
            Item.defense = 18;
        }

        public override void UpdateEquip(Player player)
        {
            player.lifeRegen += 1;
            player.maxMinions += 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemType<HydraScale>(), 24)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

}