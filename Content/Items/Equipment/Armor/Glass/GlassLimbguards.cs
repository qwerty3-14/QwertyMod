using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ID.ArmorIDs;

namespace QwertyMod.Content.Items.Equipment.Armor.Glass
{
    [AutoloadEquip(EquipType.Legs)]
    public class GlassLimbguards : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Glass Limbguards");
            Tooltip.SetDefault("Walk right for 12% increased ranged damage\nWalk left for 12% increased magic damage");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Legs.Sets.HidesBottomSkin[Item.legSlot] = true;
        }

        public override void SetDefaults()
        {
            Item.value = 10000;
            Item.rare = 1;

            Item.width = 22;
            Item.height = 12;
            Item.defense = 4;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.Glass, 30)
                .AddIngredient(ItemID.SilverBar, 6)
                .AddTile(TileID.Anvils)
                .Register();
            CreateRecipe(1).AddIngredient(ItemID.Glass, 30)
                .AddIngredient(ItemID.TungstenBar, 6)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void UpdateEquip(Player player)
        {
            if (player.velocity.X > 0)
            {
                player.GetDamage(DamageClass.Ranged) += .12f;
            }
            else if (player.velocity.X < 0)
            {
                player.GetDamage(DamageClass.Magic) += .12f;
            }
        }
    }
}