using QwertyMod.Content.Items.Consumable.Tiles.Bars;
using QwertyMod.Content.Items.MiscMaterials;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertyMod.Content.Items.Equipment.Armor.Caelite
{
    [AutoloadEquip(EquipType.Head)]
    public class CaeliteHelm : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
        }

        public override void SetDefaults()
        {
            Item.value = 30000;
            Item.rare = ItemRarityID.Orange;
            Item.width = 22;
            Item.height = 18;
            Item.defense = 7;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<CaeliteHelmEffect>().hasEffect = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<CaeliteBar>(), 8)
                .AddIngredient(ModContent.ItemType<CaeliteCore>(), 4)
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

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Player.GetModPlayer<CaeliteHelmEffect>().hasEffect && damageDone > target.life && (proj.CountsAsClass(DamageClass.Magic) || proj.CountsAsClass(DamageClass.Melee)))
            {
                target.value = (int)(target.value * 2f);
            }
        }
    }
}