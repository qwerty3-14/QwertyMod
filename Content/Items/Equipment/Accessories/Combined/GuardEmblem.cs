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
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 15);
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.defense = 8;
            if(ModLoader.HasMod("TRAEProject"))
            {
                Item.defense = 10;
            }
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
            player.aggro += 400;
        }
    }
}
