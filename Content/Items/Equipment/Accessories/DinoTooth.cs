using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Equipment.Accessories
{
    [AutoloadEquip(EquipType.Neck)]
    public class DinoTooth : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dino Tooth");
            Tooltip.SetDefault("Increases armor penetration by 18");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.value = 10000;
            Item.rare = 6;
            Item.width = 14;
            Item.height = 20;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetArmorPenetration(DamageClass.Generic) += 18;
        }
    }
}