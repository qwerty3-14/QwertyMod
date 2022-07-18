using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Equipment.Accessories.Combined
{
    public class PhantomSphere : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Phantom Sphere");
            Tooltip.SetDefault("Magic attacks pierce 2 extra enemies\nProjectiles that normally don't pierce will use local immunity\nMagic attacks ignore 15 defense\n10% reduced magic damage\nExtra pierces will do reduced damage when hitting the same target multiple times");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 26;
            Item.value = Item.sellPrice(gold: 3);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MagicPierePlayer>().pierceBoost += 2;
            player.GetDamage(DamageClass.Magic) -= .1f;
            player.GetArmorPenetration(DamageClass.Magic);
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient<TheBlueSphere>()
                .AddIngredient<ArcaneArmorBreaker>()
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}
