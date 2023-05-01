using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common.PlayerLayers;
using QwertyMod.Content.Items.Tool.Mining.Ancient;
using QwertyMod.Content.NPCs.Bosses.AncientMachine;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using QwertyMod.Content.Items.Equipment.Accessories.Expert.AncientGemstone;
using Terraria.ID;
using Terraria.GameContent.ItemDropRules;
using QwertyMod.Content.Items.Weapon.Magic.AncientMissile;
using QwertyMod.Content.Items.Weapon.Magic.AncientWave;
using QwertyMod.Content.Items.Weapon.Melee.Sword.AncientBlade;
using QwertyMod.Content.Items.Weapon.Melee.Yoyo.AncientThrow;
using QwertyMod.Content.Items.Weapon.Minion.AncientMinion;
using QwertyMod.Content.Items.Weapon.Morphs.AncientNuke;
using QwertyMod.Content.Items.Weapon.Ranged.Bow.Ancient;
using QwertyMod.Content.Items.Weapon.Ranged.Gun.Ancient;

namespace QwertyMod.Content.Items.Consumable.BossBag
{
    public class AncientMachineBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }


        public override void SetDefaults()
        {
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.width = 48;
            Item.height = 32;
            Item.rare = ItemRarityID.Cyan;
            Item.expert = true;

            if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("QwertyMod/Content/Items/Consumable/BossBag/AncientMachineBag_Glow").Value;
            }
        }

        public override bool CanRightClick()
        {
            return true;
        }
        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            itemLoot.Add(ItemDropRule.FewFromOptionsNotScalingWithLuck(3, 1, ItemType<AncientBlade>(), ItemType<AncientSniper>(), ItemType<AncientWave>(), ItemType<AncientThrow>(), ItemType<AncientMinionStaff>(), ItemType<AncientMissileStaff>(), ItemType<AncientLongbow>(), ItemType<AncientNuke>()));

            itemLoot.Add(ItemDropRule.Common(ItemType<AncientMiner>(), 5));
            itemLoot.Add(ItemDropRule.Common(ItemType<AncientGemstone>(), 1));
            itemLoot.Add(ItemDropRule.Coins(80000, true));
        }
    }
}