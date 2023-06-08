using QwertyMod.Content.Items.Equipment.Accessories.Expert.Sheath;
using QwertyMod.Content.Items.Equipment.Accessories.Sword;
using QwertyMod.Content.NPCs.Bosses.BladeBoss;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.GameContent.ItemDropRules;
using QwertyMod.Content.Items.MiscMaterials;
using QwertyMod.Content.Items.Weapon.Magic.Swordpocalypse;
using QwertyMod.Content.Items.Weapon.Melee.Javelin.Imperium;
using QwertyMod.Content.Items.Weapon.Melee.Sword.ImperiousTheIV;
using QwertyMod.Content.Items.Weapon.Melee.Yoyo.Arsenal;
using QwertyMod.Content.Items.Weapon.Minion.Longsword;
using QwertyMod.Content.Items.Weapon.Morphs.Swordquake;
using QwertyMod.Content.Items.Weapon.Whip.Discipline;

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
            itemLoot.Add(ItemDropRule.Coins(150000, true));
            itemLoot.Add(ItemDropRule.FewFromOptions(3, 1, ItemType<BladedArrowShaft>(), ItemType<ImperiousTheIV>(), ItemType<Imperium>(), ItemType<SwordStormStaff>(), ItemType<Arsenal>(), ItemType<Discipline>(), ItemType<SwordMinionStaff>(), ItemType<Swordquake>()));
        }
    }
}