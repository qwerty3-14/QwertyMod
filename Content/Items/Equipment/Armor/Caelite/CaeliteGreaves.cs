using QwertyMod.Content.Items.Consumable.Tiles.Bars;
using QwertyMod.Content.Items.MiscMaterials;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertyMod.Content.Items.Equipment.Armor.Caelite
{
    [AutoloadEquip(EquipType.Legs)]
    public class CaeliteGreaves : ModItem
    {

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ArmorIDs.Legs.Sets.HidesBottomSkin[Item.legSlot] = true;
        }

        public override void SetDefaults()
        {
            Item.value = 30000;
            Item.rare = ItemRarityID.Orange;

            Item.width = 22;
            Item.height = 18;
            Item.defense = 7;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<CaeliteBar>(), 12)
                .AddIngredient(ModContent.ItemType<CaeliteCore>(), 6)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<CaeliteGreavesEffect>().hasEffect = true;
        }

        public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
        {
            if (male) equipSlot = QwertyMod.CaeliteLegMale;
            if (!male) equipSlot = QwertyMod.CaeliteLegFemale;
        }

    }

    public class CaeliteGreavesEffect : ModPlayer
    {
        public bool hasEffect;
        private int healLimiter = 0;

        public override void PreUpdate()
        {
            if (healLimiter < 60)
            {
                healLimiter++;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hasEffect && (hit.DamageType == DamageClass.Melee && Player.HasBuff(BuffID.PotionSickness)))
            {
                int healAmount = damageDone / 2;
                if (healAmount > healLimiter)
                {
                    healAmount = healLimiter;
                    healLimiter = 0;
                }
                else
                {
                    healLimiter -= healAmount;
                }
                if (Player.GetModPlayer<CaeliteSetBonus>().setBonus)
                {
                    healAmount = (int)(healAmount * 1.25f);
                }
                Player.buffTime[Player.FindBuffIndex(BuffID.PotionSickness)] -= healAmount;
            }
        }
    }
}