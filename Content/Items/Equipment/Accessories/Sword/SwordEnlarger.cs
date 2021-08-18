using QwertyMod.Common;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Equipment.Accessories.Sword
{
    public class SwordEnlarger : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sword Enlarger");
            Tooltip.SetDefault("Greatly increases the size of your sword!");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.value = 200000;
            Item.rare = 2;

            Item.width = 16;
            Item.height = 22;

            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<CommonStats>().weaponSize += 1f;
        }
    }
}