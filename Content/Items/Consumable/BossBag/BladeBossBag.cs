using QwertyMod.Content.Items.Equipment.Accessories.Expert.Sheath;
using QwertyMod.Content.Items.Equipment.Accessories.Sword;
using QwertyMod.Content.Items.Weapon.Magic.Swordpocalypse;
using QwertyMod.Content.Items.Weapon.Melee.Javelin.Imperium;
using QwertyMod.Content.Items.Weapon.Melee.Sword.ImperiousTheIV;
using QwertyMod.Content.Items.Weapon.Melee.Yoyo.Arsenal;
using QwertyMod.Content.Items.Weapon.Minion.Longsword;
using QwertyMod.Content.Items.Weapon.Morphs.Swordquake;
using QwertyMod.Content.Items.Weapon.Whip.Discipline;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using Terraria.ID;
using QwertyMod.Content.Items.Equipment.Vanity.BossMasks;
using QwertyMod.Content.Items.Equipment.Accessories.SuperArrow.BladedArrow;

namespace QwertyMod.Content.Items.Consumable.BossBag
{
    public class BladeBossBag : ModItem
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
            Item.width = 36;
            Item.height = 34;
            Item.rare = ItemRarityID.Cyan;
            Item.expert = true;
            //bossBagNPC = mod.NPCType("BladeBoss");
        }


        public override bool CanRightClick()
        {
            return true;
        }
        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<SwordsmanBadge>(), 4));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<ImperiousSheath>(), 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<ImperiousMask>(), 7, 1, 1));
            itemLoot.Add(ItemDropRule.Coins(150000, true));
            itemLoot.Add(ItemDropRule.FewFromOptions(3, 1, ModContent.ItemType<BladedArrow>(), ModContent.ItemType<ImperiousTheIV>(), ModContent.ItemType<Imperium>(), ModContent.ItemType<SwordStormStaff>(), ModContent.ItemType<Arsenal>(), ModContent.ItemType<Discipline>(), ModContent.ItemType<SwordMinionStaff>(), ModContent.ItemType<Swordquake>()));
        }
    }
}