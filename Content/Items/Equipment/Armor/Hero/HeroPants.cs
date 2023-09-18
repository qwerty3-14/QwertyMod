using QwertyMod.Content.Items.Consumable.Tiles.Bars;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Equipment.Armor.Hero
{
    [AutoloadEquip(EquipType.Legs)]
    public class HeroPants : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }

        public override void SetDefaults()
        {
            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ItemRarityID.Orange;
            Item.width = 22;
            Item.height = 12;
            Item.defense = 6;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.2f;
        }

        public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
        {
            if (male) equipSlot = QwertyMod.HeroPantsMale;
            if (!male) equipSlot = QwertyMod.HeroPantsFemale;
        }
    }
}