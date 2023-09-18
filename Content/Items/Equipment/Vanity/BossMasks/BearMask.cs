﻿using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using static Terraria.ID.ArmorIDs;
using Terraria.ID;

namespace QwertyMod.Content.Items.Equipment.Vanity.BossMasks
{
    [AutoloadEquip(EquipType.Head)]
    public class PolarMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Polar Exterminator Mask");
            //Tooltip.SetDefault("");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Head.Sets.DrawHead[Item.headSlot] = false;
        }

        public override void SetDefaults()
        {
            Item.value = 0;
            Item.rare = ItemRarityID.Blue;
            Item.vanity = true;
            Item.width = 22;
            Item.height = 20;
        }
    }
}
