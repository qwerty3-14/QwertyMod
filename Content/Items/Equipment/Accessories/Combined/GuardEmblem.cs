using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Equipment.Accessories.Combined
{
    class GuardEmblem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Guard Emblem");
            Tooltip.SetDefault("8 defense \n10% increased damage \nEnemies are more likely to target you");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 15);
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.AvengerEmblem)
                .AddIngredient(ItemID.FleshKnuckles)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Generic) += 0.1f;
            player.statDefense += 8;
            player.aggro += 400;
        }
    }
}
