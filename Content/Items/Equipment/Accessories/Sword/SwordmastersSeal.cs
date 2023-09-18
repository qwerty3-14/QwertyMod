﻿using QwertyMod.Common;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Equipment.Accessories.Sword
{
    public class SwordmastersSeal : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Swordmasters Seal");
            //Tooltip.SetDefault("Greatly enhances swordplay performance!" + "\nMakes your sword much larger" + "\nHitting things with your sword while airborne does more damage" + "\nStriking with a sword increases critical chance");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Lime;
            Item.value = Item.sellPrice(0, 50, 0, 0);
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<SkywardHiltEffect>().effect = true;
            player.GetModPlayer<CommonStats>().weaponSize += 0.25f;
            player.GetModPlayer<BadgeEffect>().critOnHit = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemType<SwordsmanBadge>())
                .AddIngredient(ItemType<SkywardHilt>())
                .AddIngredient(ItemType<SwordEnlarger>())
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}