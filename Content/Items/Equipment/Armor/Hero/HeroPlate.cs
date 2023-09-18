using QwertyMod.Content.Items.Consumable.Tiles.Bars;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Equipment.Armor.Hero
{
    [AutoloadEquip(EquipType.Body)]
    public class HeroPlate : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }

        public override void SetDefaults()
        {
            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ItemRarityID.Orange;
            Item.width = 28;
            Item.height = 20;
            Item.defense = 7;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Melee) += .08f;
        }
    }
}