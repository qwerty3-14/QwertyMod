using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Equipment.Armor.Vitallum
{
    [AutoloadEquip(EquipType.Legs)]
    public class VitallumJeans : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vitallum Jeans");
            Tooltip.SetDefault("Increases max life by 100 \n6% increased damage \nRegenerate 2 life/sec when on the ground");
        }

        public override void SetDefaults()
        {
            Item.rare = 8;
            Item.value = Item.sellPrice(gold: 6);
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
                .AddIngredient(ItemType<VitallumCoreCharged>(), 1)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
        public override void OnCraft(Recipe recipe)
        {
            Main.LocalPlayer.QuickSpawnItem(ItemType<VitallumCoreUncharged>(), 1);
        }
    }
    
}