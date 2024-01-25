using QwertyMod.Content.Items.Tool.Mining.TheDevourer;
using QwertyMod.Content.Items.Weapon.Magic.BlackHole;
using QwertyMod.Content.Items.Weapon.Magic.Plasma;
using QwertyMod.Content.Items.Weapon.Minion.UrQuan;
using QwertyMod.Content.Items.Weapon.Ranged.Bow.B4Bow;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using QwertyMod.Content.Items.Equipment.Vanity.BossMasks;

namespace QwertyMod.Content.Items.Consumable.BossBag
{
    public class B4Bag : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
            ItemID.Sets.BossBag[Type] = true; // This set is one that every boss bag should have, it, for example, lets our boss bag drop dev armor..
			ItemID.Sets.PreHardmodeLikeBossBag[Type] = true; // ..But this set ensures that dev armor will only be dropped on special world seeds, since that's the behavior of pre-hardmode boss bags.
        }

        public override void SetDefaults()
        {
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.width = 60;
            Item.height = 34;
            Item.rare = ItemRarityID.Cyan;
            Item.expert = true;
            //bossBagNPC = mod.NPCType("WeakPoint");
        }

        public override bool CanRightClick()
        {
            return true;
        }
        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            itemLoot.Add(ItemDropRule.Common(ItemID.Penguin, 1, 40, 80));
            
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<TheDevourer>(), 5));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<B4ExpertItem>(), 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<OLORDMask>(), 7, 1, 1));
            itemLoot.Add(ItemDropRule.Coins(1000000, true));
            itemLoot.Add(ItemDropRule.FewFromOptions(1, 1, ModContent.ItemType<BlackHoleStaff>(), ModContent.ItemType<ExplosivePierce>(), ModContent.ItemType<DreadnoughtStaff>(), ModContent.ItemType<B4Bow>()));
        }
    }
}