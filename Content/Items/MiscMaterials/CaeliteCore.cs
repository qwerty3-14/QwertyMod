using QwertyMod.Content.Dusts;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.MiscMaterials
{
    public class CaeliteCore : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.maxStack = 9999;
            Item.value = 25000;
            Item.rare = ItemRarityID.Orange;

            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            gravity = 0;
            Item.velocity.X = Item.velocity.X * 0.95f;
            if ((double)Item.velocity.X < 0.1 && (double)Item.velocity.X > -0.1)
            {
                Item.velocity.X = 0f;
            }
            Item.velocity.Y = Item.velocity.Y * 0.95f;
            if ((double)Item.velocity.Y < 0.1 && (double)Item.velocity.Y > -0.1)
            {
                Item.velocity.Y = 0f;
            }
            Dust dust = Main.dust[Dust.NewDust(Item.position, Item.width, Item.height, ModContent.DustType<CaeliteDust>())];
            dust.scale = .5f;
            Lighting.AddLight(Item.Center, 1f, 1f, 1f);
        }
    }
}