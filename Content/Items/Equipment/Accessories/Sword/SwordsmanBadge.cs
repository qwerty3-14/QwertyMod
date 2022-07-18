using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Equipment.Accessories.Sword
{
    public class SwordsmanBadge : ModItem
    {


        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Swordsman Badge");
            Tooltip.SetDefault("Striking with a sword increases critical rate");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.rare = 7;
            Item.value = Item.sellPrice(0, 10, 0, 0);

            Item.width = Item.height = 20;

            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<BadgeEffect>().critOnHit = true;
        }
    }
    public class BadgeEffect : ModPlayer
    {
        public bool critOnHit;
        public override void ResetEffects()
        {
            critOnHit = false;
        }
        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            if (critOnHit)
            {
                Player.AddBuff(BuffType<ImperialCourage>(), 240);
            }
        }
    }
}