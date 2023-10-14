using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace QwertyMod.Content.Items.Equipment.Vanity.CocktailDress
{
    [AutoloadEquip(EquipType.Legs)]
    internal class CocktailDressSkirt : ModItem
    {

        public override void SetStaticDefaults()
        {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 20;
            Item.rare = ItemRarityID.Blue;
            Item.vanity = true;
            Item.value = Item.buyPrice(gold: 30);
        }
    }

}