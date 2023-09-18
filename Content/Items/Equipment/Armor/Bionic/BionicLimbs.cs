using QwertyMod.Content.Items.Weapon.Morphs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ID.ArmorIDs;
using Terraria.GameContent.Creative;

namespace QwertyMod.Content.Items.Equipment.Armor.Bionic
{
    [AutoloadEquip(EquipType.Legs)]
    public class BionicLimbs : ModItem
    {
        public override void SetStaticDefaults()
        {
            Legs.Sets.HidesBottomSkin[Item.legSlot] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 5);
            Item.defense = 7;
            Item.width = 22;
            Item.height = 18;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<ShapeShifterPlayer>().coolDownDuration *= 0.7f;
            player.runAcceleration += 0.5f;
            player.runSlowdown += 0.5f;

        }

        public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
        {
            if (male) equipSlot = QwertyMod.BionicLegMale;
            if (!male) equipSlot = QwertyMod.BionicLegFemale;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.SoulofMight, 8)
                .AddIngredient(ItemID.HallowedBar, 4)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
