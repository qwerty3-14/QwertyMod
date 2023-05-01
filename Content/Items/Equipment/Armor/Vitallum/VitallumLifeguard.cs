using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;
using System;

namespace QwertyMod.Content.Items.Equipment.Armor.Vitallum
{
    [AutoloadEquip(EquipType.Body)]
    public class VitallumLifeguard : ModItem
    {
        public override void SetStaticDefaults()
        {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(gold: 6);
        }

        public override void UpdateEquip(Player player)
        {
            player.statLifeMax2 += 120;
            player.GetModPlayer<LifeGuardEffects>().effect = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.ChlorophyteBar, 24)
                .AddIngredient(ItemID.LifeCrystal, 8)
                .AddIngredient(ItemType<VitallumCoreCharged>(), 1)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        [Obsolete]
        public override void OnCraft(Recipe recipe)
        {
            Main.LocalPlayer.QuickSpawnItem(new EntitySource_Misc("Recipe"), ItemType<VitallumCoreUncharged>(), 1);
        }
    }

    public class LifeGuardEffects : ModPlayer
    {
        public bool effect = false;

        public override void ResetEffects()
        {
            effect = false;
        }

        public override void PostUpdateEquips()
        {
            if (effect)
            {
                int missingHealth = Player.statLifeMax2 - Player.statLife;
                Player.GetDamage(DamageClass.Generic) += (missingHealth / 10) * .01f;
            }
        }
    }
}