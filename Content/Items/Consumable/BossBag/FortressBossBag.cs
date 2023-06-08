using QwertyMod.Content.Items.Consumable.Tiles.Bars;
using QwertyMod.Content.Items.Equipment.Accessories.Sword;
using QwertyMod.Content.Items.Equipment.Vanity.BossMasks;
using QwertyMod.Content.Items.MiscMaterials;
using QwertyMod.Content.Items.Pet;
using QwertyMod.Content.Items.Weapon.Magic.RestlessSun;
using QwertyMod.Content.Items.Weapon.Melee.Misc;
using QwertyMod.Content.Items.Weapon.Minion.Priest;
using QwertyMod.Content.Items.Weapon.Ranged.Bow.HolyExiler;
using QwertyMod.Content.NPCs.Bosses.FortressBoss;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.ItemDropRules;

namespace QwertyMod.Content.Items.Consumable.BossBag
{
    public class FortressBossBag : ModItem
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
            //bossBagNPC = mod.NPCType("FortressBoss");
        }

        public override bool CanRightClick()
        {
            return true;
        }
        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<CaeliteBar>(), 1, 18, 30));            
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<CaeliteCore>(), 1, 9, 15));      
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<ExpertChalice>(), 1, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<DivineLightMask>(), 7, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Lightling>(), 5, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<SkywardHilt>(), 5, 1, 1));
            
            itemLoot.Add(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, new int[] { ModContent.ItemType<CaeliteMagicWeapon>(), ModContent.ItemType<HolyExiler>(), ModContent.ItemType<CaeliteRainKnife>(), ModContent.ItemType<PriestStaff>()}));
        }

        
    }
}