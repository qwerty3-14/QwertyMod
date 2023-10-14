using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.ID;

namespace QwertyMod.Content.Items.Equipment.Accessories.Sword
{
    public class SwordsmanBadge : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Lime;
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
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (critOnHit)
            {
                Player.AddBuff(ModContent.BuffType<ImperialCourage>(), 240);
            }
        }
    }
}