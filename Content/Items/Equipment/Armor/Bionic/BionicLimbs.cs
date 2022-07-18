using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ID.ArmorIDs;
using QwertyMod.Content.Items.Weapon.Morphs;

namespace QwertyMod.Content.Items.Equipment.Armor.Bionic
{
    [AutoloadEquip(EquipType.Legs)]
    public class BionicLimbs : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bionic Limbs");
            Tooltip.SetDefault("30% reduced cooldown on morphs\nSignificantly increases horizontal acceleration");
            Legs.Sets.HidesBottomSkin[Item.legSlot] = true;
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
            player.runAcceleration += 0.8f;
            
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
