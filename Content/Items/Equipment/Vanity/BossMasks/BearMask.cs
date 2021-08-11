using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Equipment.Vanity.BossMasks
{
    [AutoloadEquip(EquipType.Head)]
    public class PolarMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Polar Exterminator Mask");
            Tooltip.SetDefault("");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.value = 0;
            Item.rare = 1;
            Item.vanity = true;
            Item.width = 20;
            Item.height = 20;
        }

        public override bool DrawHead()
        {
            return false;
        }
    }
}
