﻿using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace QwertyMod.Content.Items.Equipment.Armor.Vitallum
{
    [AutoloadEquip(EquipType.Legs)]
    public class VitallumJeans : ModItem
    {
        public override void SetStaticDefaults()
        {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(gold: 6);
            Item.width = 22;
            Item.height = 18;
        }

        public override void UpdateEquip(Player player)
        {
            player.statLifeMax2 += 100;
            player.GetDamage(DamageClass.Generic) += .06f;
            Point origin = player.Bottom.ToTileCoordinates();
            if (WorldUtils.Find(origin, Searches.Chain(new Searches.Down(3), new GenCondition[] { new Conditions.IsSolid() }), out _))
            {
                player.lifeRegen += 4;
            }
        }

        public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
        {
            if (male) equipSlot = QwertyMod.VitLegMale;
            if (!male) equipSlot = QwertyMod.VitLegFemale;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.ChlorophyteBar, 18)
                .AddIngredient(ItemID.LifeCrystal, 6)
                .AddIngredient(ModContent.ItemType<VitallumCoreCharged>(), 1)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
        [Obsolete]
        public override void OnCraft(Recipe recipe)
        {
            Main.LocalPlayer.QuickSpawnItem(new EntitySource_Misc("Recipe"), ModContent.ItemType<VitallumCoreUncharged>(), 1);
        }
    }

}