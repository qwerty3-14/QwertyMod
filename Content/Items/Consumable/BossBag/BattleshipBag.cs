using QwertyMod.Content.Items.MiscMaterials;
using QwertyMod.Content.Items.Equipment.Accessories.Expert;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using QwertyMod.Content.Items.Weapon.Ranged.Gun.SuperquantumRifle;
using QwertyMod.Content.Items.Weapon.Minion.DVR;
using QwertyMod.Content.Items.Weapon.Melee.Misc.FightKit;
using QwertyMod.Content.Items.Equipment.Vanity.BossMasks;

namespace QwertyMod.Content.Items.Consumable.BossBag
{
    public class BattleshipBag : ModItem
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
            Item.width = 32;
            Item.height = 32;
            Item.rare = ItemRarityID.Cyan;
            Item.expert = true;
        }

        public override bool CanRightClick()
        {
            return true;
        }
        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            itemLoot.Add(ItemDropRule.Coins(1000000, true));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<InvaderPlating>(), 1, 100, 200));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<InvaderIDCard>(), 1, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<InvaderMask>(), 7, 1, 1));
            itemLoot.Add(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, ModContent.ItemType<SuperquantumRifle>(), ModContent.ItemType<DVRStaff>(), ModContent.ItemType<FightKit>()));
        }
    }
}