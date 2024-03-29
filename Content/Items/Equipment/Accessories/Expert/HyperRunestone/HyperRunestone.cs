using QwertyMod.Common;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.ID;

namespace QwertyMod.Content.Items.Equipment.Accessories.Expert.HyperRunestone
{
    public class HyperRunestone : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 76;
            Item.height = 76;
            Item.maxStack = 1;
            Item.value = 500000;
            Item.rare = ItemRarityID.Orange;

            Item.rare = ItemRarityID.Cyan;
            Item.expert = true;
            Item.accessory = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<Dash>().SetDash(5);
            player.GetModPlayer<Dash>().Bonus += 5;
            player.GetModPlayer<Dash>().hyperRune = true;
        }
    }


}