using QwertyMod.Common;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.ID;

namespace QwertyMod.Content.Items.Equipment.Accessories.Sword
{
    public class SwordEnlarger : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.value = 200000;
            Item.rare = ItemRarityID.Green;
            Item.width = 30;
            Item.height = 30;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<CommonStats>().weaponSize += 0.25f;
        }
    }
}