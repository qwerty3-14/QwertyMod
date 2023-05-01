using QwertyMod.Content.Items.MiscMaterials;
using QwertyMod.Content.Items.Tool.FishingRod;
using QwertyMod.Content.NPCs.Bosses.Hydra;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.GameContent.ItemDropRules;
using QwertyMod.Content.Items.Weapon.Magic.HydraBeam;
using QwertyMod.Content.Items.Weapon.Magic.HydraMissile;
using QwertyMod.Content.Items.Weapon.Melee.Javelin.Hydra;
using QwertyMod.Content.Items.Weapon.Melee.Spear.Hydrent;
using QwertyMod.Content.Items.Weapon.Minion.HydraHead;
using QwertyMod.Content.Items.Weapon.Morphs.HydraBarrage;
using QwertyMod.Content.Items.Weapon.Ranged.Gun;
using QwertyMod.Content.Items.Tool.Mining;

namespace QwertyMod.Content.Items.Consumable.BossBag
{
    public class HydraBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.width = 76;
            Item.height = 35;
            Item.rare = ItemRarityID.Cyan;
            Item.expert = true;
            //bossBagNPC = mod.NPCType("Hydra");
        }


        public override bool CanRightClick()
        {
            return true;
        }
        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Hydrator>(), 5, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<HydraScale>(), 5, 30, 40));
            itemLoot.Add(ItemDropRule.Coins(100000, true));
            itemLoot.Add(ItemDropRule.FewFromOptions(3, 1, new int[] { ModContent.ItemType<HydraBarrage>(), ModContent.ItemType<HydraBeam>(), ModContent.ItemType<HydraCannon>(), ModContent.ItemType<HydraHeadStaff>(), ModContent.ItemType<HydraJavelin>(), ModContent.ItemType<Hydrent>(), ModContent.ItemType<Hydrill>(), ModContent.ItemType<HydraMissileStaff>()}));
        }
    }
}