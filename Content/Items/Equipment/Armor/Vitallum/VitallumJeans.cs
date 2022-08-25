using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;

namespace QwertyMod.Content.Items.Equipment.Armor.Vitallum
{
    [AutoloadEquip(EquipType.Legs)]
    public class VitallumJeans : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vitallum Jeans");
            Tooltip.SetDefault("Increases max life by 100 \n6% increased damage \nRegenerate 2 life/sec when on the ground");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
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
            Main.LocalPlayer.QuickSpawnItem(new EntitySource_Misc(""), ItemType<VitallumCoreUncharged>(), 1);
        }
    }

}