using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Equipment.Armor.Vitallum
{
    public class VitallumCoreUncharged : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vitallum Core");
            Tooltip.SetDefault("Uncharged, kill enemies to charge it.\nWhen charged can be used in crafting, removing the charge. \n");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        private int charge = 0;
        private int maxCharge = 50;

        public override void SetDefaults()
        {
            Item.width = 13;
            Item.height = 13;
            Item.value = 10000;
            Item.rare = 8;
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
                player.QuickSpawnItem(ItemType<VitallumCoreCharged>(), 1);
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            String s = charge + "/" + maxCharge + " enemies killed.";
            foreach (TooltipLine line in tooltips) //runs through all tooltip lines
            {
                if (line.mod == "Terraria" && line.Name == "Tooltip2") //this checks if it's the line we're interested in
                {
                    line.text = s;//change tooltip
                }
            }
        }
    }

    public class VitallumCoreCharged : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vitallum Core");
            Tooltip.SetDefault("Charged, ready for crafting.");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(10, 6));
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 13;
            Item.height = 13;
            Item.value = 10000;
            Item.rare = 8;
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

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            if (gainCharges && target.life < 0)
            {
                chargesToAdd++;
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
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
                notExpertRule.OnSuccess(ItemDropRule.Common(ItemType<VitallumCoreUncharged>()));
                //Finally add the leading rule
                npcLoot.Add(notExpertRule);
            }
        }
    }

    public class CoreBagDrop : GlobalItem
    {
        public override void OpenVanillaBag(string context, Player player, int arg)
        {
            if (context == "bossBag" && arg == ItemID.PlanteraBossBag)
            {
                player.QuickSpawnItem(ItemType<VitallumCoreUncharged>());
            }
        }
    }
}