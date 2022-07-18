using QwertyMod.Content.Items.Weapon.Morphs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ID.ArmorIDs;

namespace QwertyMod.Content.Items.Equipment.Armor.Bionic
{
    [AutoloadEquip(EquipType.Head)]
    public class BionicEye : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bionic Eye");
            Tooltip.SetDefault("30% reduced cooldown on morphs\n10% increased critical strike chance\nGrants enhanced vision");
            Head.Sets.DrawFullHair[Item.headSlot] = true;
        }

        public override void SetDefaults()
        {
            Item.rare = 5;
            Item.value = Item.sellPrice(gold: 5);
            Item.defense = 7;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<ShapeShifterPlayer>().coolDownDuration *= 0.7f;
            player.nightVision = true;
            player.dangerSense = true;
            player.detectCreature = true;
            player.findTreasure = true;
            player.GetCritChance(DamageClass.Generic) += 10;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.SoulofMight, 8)
                .AddIngredient(ItemID.HallowedBar, 3)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

    }
}
