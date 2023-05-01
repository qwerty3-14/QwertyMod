using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Tool.Mining.BurstMiner
{
    public class BurstMiner : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Stream Miner");
            //Tooltip.SetDefault("");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 11;
            Item.DamageType = DamageClass.Melee;

            Item.useTime = 9;
            Item.useAnimation = 9;
            Item.tileBoost = -1;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 3;
            Item.value = Item.sellPrice(silver: 54);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;

            Item.width = 30;
            Item.height = 30;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.pick = 95;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            int num292 = Dust.NewDust(new Vector2((float)hitbox.X, (float)hitbox.Y), hitbox.Width, hitbox.Height, DustID.DungeonWater, player.velocity.X * 0.2f + (float)(player.direction * 3), player.velocity.Y * 0.2f, 100, default(Color), 0.9f);
            Main.dust[num292].noGravity = true;
            Main.dust[num292].velocity *= 0.1f;
        }
    }
}