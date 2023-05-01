using QwertyMod.Content.Items.Equipment.Accessories.Expert.HyperRunestone;
using QwertyMod.Content.Items.Equipment.Accessories.RuneScrolls;
using QwertyMod.Content.Items.Equipment.Vanity.BossMasks;
using QwertyMod.Content.Items.Equipment.Vanity.RunicRobe;
using QwertyMod.Content.Items.MiscMaterials;
using QwertyMod.Content.NPCs.Bosses.RuneGhost;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using Terraria.GameContent.ItemDropRules;

namespace QwertyMod.Content.Items.Consumable.BossBag
{
    public class RuneGhostBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(10, 36));
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
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
            itemLoot.Add(ItemDropRule.FewFromOptions(1, 1, ItemType<IceScroll>(), ItemType<PursuitScroll>(), ItemType<LeechScroll>(), ItemType<AggroScroll>()));
        }
    }
}