using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Equipment.Armor.Shaman
{
    [AutoloadEquip(EquipType.Legs)]
    public class ShamanLegs : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Shaman Pants");
            //Tooltip.SetDefault("+1 max sentries");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
        {
            if (male) equipSlot = QwertyMod.shamaLegMale;
            if (!male) equipSlot = QwertyMod.shamanLegFemale;
        }

        public override void SetDefaults()
        {
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Blue;
            Item.defense = 5;
            Item.width = 20;
            Item.height = 20;
        }

        public override void UpdateEquip(Player player)
        {
            player.maxTurrets++;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.JungleSpores, 4)
                .AddIngredient(ItemID.Bone, 25)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}