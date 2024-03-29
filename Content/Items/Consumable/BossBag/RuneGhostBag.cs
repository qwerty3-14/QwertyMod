using QwertyMod.Content.Items.Equipment.Accessories.Expert.HyperRunestone;
using QwertyMod.Content.Items.Equipment.Accessories.RuneScrolls;
using QwertyMod.Content.Items.Equipment.Vanity.BossMasks;
using QwertyMod.Content.Items.Equipment.Vanity.RunicRobe;
using QwertyMod.Content.Items.MiscMaterials;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

using Terraria.ID;
using Terraria.GameContent.ItemDropRules;

namespace QwertyMod.Content.Items.Consumable.BossBag
{
    public class RuneGhostBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            //Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(10, 36));
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
            //bossBagNPC = mod.NPCType("RuneSpector");
        }


        public override bool CanRightClick()
        {
            return true;
        }
        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            itemLoot.Add(ItemDropRule.Common(ItemID.Penguin, 1, 40, 80));
            
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<RuneGhostMask>(), 7));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<RunicRobe>(), 7));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<HyperRunestone>(), 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<CraftingRune>(), 1, 30, 40));
            itemLoot.Add(ItemDropRule.Coins(350000, true));
            itemLoot.Add(ItemDropRule.FewFromOptions(1, 1, ModContent.ItemType<IceScroll>(), ModContent.ItemType<PursuitScroll>(), ModContent.ItemType<LeechScroll>(), ModContent.ItemType<AggroScroll>()));
        }
    }
}