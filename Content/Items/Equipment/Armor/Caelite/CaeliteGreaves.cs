using QwertyMod.Content.Items.Consumable.Tiles.Bars;
using QwertyMod.Content.Items.MiscMaterials;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ID.ArmorIDs;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Equipment.Armor.Caelite
{
    [AutoloadEquip(EquipType.Legs)]
    public class CaeliteGreaves : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Caelite Reinforcmentss");
            Tooltip.SetDefault("Melee and magic attacks hasten the cooldown for healing potions");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Legs.Sets.HidesBottomSkin[Item.legSlot] = true;
        }

        public override void SetDefaults()
        {
            Item.value = 30000;
            Item.rare = 3;

            Item.width = 22;
            Item.height = 18;
            Item.defense = 7;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemType<CaeliteBar>(), 12)
                .AddIngredient(ItemType<CaeliteCore>(), 6)
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

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (hasEffect && (proj.CountsAsClass(DamageClass.Melee) || proj.CountsAsClass(DamageClass.Magic)) && Player.HasBuff(BuffID.PotionSickness))
            {
                int healAmount = damage / 2;
                if (healAmount > healLimiter)
                {
                    healAmount = healLimiter;
                    healLimiter = 0;
                }
                else
                {
                    healAmount -= healAmount;
                }
                if (Player.GetModPlayer<CaeliteSetBonus>().setBonus)
                {
                    healAmount = (int)(healAmount * 1.25f);
                }
                Player.buffTime[Player.FindBuffIndex(BuffID.PotionSickness)] -= healAmount;
            }
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            if (hasEffect && (item.CountsAsClass(DamageClass.Melee)) && Player.HasBuff(BuffID.PotionSickness))
            {
                int healAmount = damage / 2;
                if (healAmount > healLimiter)
                {
                    healAmount = healLimiter;
                    healLimiter = 0;
                }
                else
                {
                    healAmount -= healAmount;
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