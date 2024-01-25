using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Equipment.Armor.Glass
{
    [AutoloadEquip(EquipType.Legs)]
    public class GlassLimbguards : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ArmorIDs.Legs.Sets.HidesBottomSkin[Item.legSlot] = true;
        }

        public override void SetDefaults()
        {
            Item.value = 10000;
            Item.rare = ItemRarityID.Blue;
            Item.width = 26;
            Item.height = 18;
            Item.defense = 4;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.Glass, 30)
                .AddIngredient(ItemID.SilverBar, 6)
                .AddIngredient(ItemID.ShadowScale, 3)
                .AddTile(TileID.GlassKiln)
                .Register();
            CreateRecipe(1).AddIngredient(ItemID.Glass, 30)
                .AddIngredient(ItemID.TungstenBar, 6)
                .AddIngredient(ItemID.ShadowScale, 3)
                .AddTile(TileID.GlassKiln)
                .Register();
            CreateRecipe(1).AddIngredient(ItemID.Glass, 30)
                .AddIngredient(ItemID.SilverBar, 6)
                .AddIngredient(ItemID.TissueSample, 3)
                .AddTile(TileID.GlassKiln)
                .Register();
            CreateRecipe(1).AddIngredient(ItemID.Glass, 30)
                .AddIngredient(ItemID.TungstenBar, 6)
                .AddIngredient(ItemID.TissueSample, 3)
                .AddTile(TileID.GlassKiln)
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