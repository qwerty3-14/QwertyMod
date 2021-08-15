using QwertyMod.Content.Items.Consumable.Tile.Bars;
using QwertyMod.Content.Items.MiscMaterials;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Equipment.Armor.Caelite
{
    [AutoloadEquip(EquipType.Head)]
    public class CaeliteHelm : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Caelite Helm");
            Tooltip.SetDefault("Enemies killed by melee or magic attacks drop more money!");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.value = 30000;
            Item.rare = 3;

            Item.width = 22;

            Item.defense = 7;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<CaeliteHelmEffect>().hasEffect = true;
        }

        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawHair = false;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemType<CaeliteBar>(), 8)
                .AddIngredient(ItemType<CaeliteCore>(), 4)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class CaeliteHelmEffect : ModPlayer
    {
        public bool hasEffect;

        public override void ResetEffects()
        {
            hasEffect = false;
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (damage > target.life && (proj.CountsAsClass(DamageClass.Magic) || proj.CountsAsClass(DamageClass.Melee)))
            {
                target.value *= 2;
            }
        }
    }
}