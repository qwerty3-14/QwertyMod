using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using Terraria.GameContent.Creative;

namespace QwertyMod.Content.Items.Equipment.Accessories
{
    public class ArcaneArmorBreaker : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Arcane Armor Breaker");
            Tooltip.SetDefault("Magic attacks ignore 10 defense");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = Item.height = 32;
            Item.accessory = true;
            Item.value = Terraria.Item.sellPrice(gold: 10);
            Item.rare = ItemRarityID.Green;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {

            player.GetArmorPenetration(DamageClass.Magic) += 10;

        }

    }
}
