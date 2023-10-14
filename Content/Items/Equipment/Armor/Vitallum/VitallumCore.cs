using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertyMod.Content.Items.Equipment.Armor.Vitallum
{
    public class VitallumCoreUncharged : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        private int charge = 0;
        private int maxCharge = 50;

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 22;
            Item.value = 10000;
            Item.rare = ItemRarityID.Yellow;
            Item.maxStack = 1;
        }
        public override ModItem Clone(Item item)
        {
            return base.Clone(item);
        }

        public override void UpdateInventory(Player player)
        {
            player.GetModPlayer<CoreManager>().gainCharges = true;
            if (player.GetModPlayer<CoreManager>().chargesToAdd > 0)
            {
                charge += player.GetModPlayer<CoreManager>().chargesToAdd;
                player.GetModPlayer<CoreManager>().chargesToAdd = 0;
            }
            if (charge > maxCharge)
            {
                Item.TurnToAir();
                player.QuickSpawnItem(new EntitySource_Misc("Recipe"), ModContent.ItemType<VitallumCoreCharged>(), 1);
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            String s = charge + "/" + maxCharge + " enemies killed.";
            foreach (TooltipLine line in tooltips) //runs through all tooltip lines
            {
                if (line.Mod == "Terraria" && line.Name == "Tooltip2") //this checks if it's the line we're interested in
                {
                    line.Text = s;//change tooltip
                }
            }
        }
    }

    public class VitallumCoreCharged : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(10, 6));
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 13;
            Item.height = 13;
            Item.value = 10000;
            Item.rare = ItemRarityID.Yellow;
            Item.maxStack = 1;
        }
    }

    public class CoreManager : ModPlayer
    {
        public int chargesToAdd = 0;
        public bool gainCharges = false;

        public override void ResetEffects()
        {
            //chargesToAdd = 0;
            gainCharges = false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (gainCharges && target.life < 0)
            {
                chargesToAdd++;
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (gainCharges && target.life < 0)
            {
                chargesToAdd++;
            }
        }
    }

    public class CoreDrop : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.Plantera)
            {
                LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());

                //Notice we use notExpertRule.OnSuccess instead of npcLoot.Add so it only applies in normal mode
                notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<VitallumCoreUncharged>()));
                //Finally add the leading rule
                npcLoot.Add(notExpertRule);
            }
        }
    }

    public class CoreBagDrop : GlobalItem
    {
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            if(item.type == ItemID.FishronBossBag)
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<VitallumCoreUncharged>()));
            }
        }
    }
}