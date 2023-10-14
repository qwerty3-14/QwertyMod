using QwertyMod.Content.NPCs.Bosses.RuneGhost;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertyMod.Content.Items.Consumable.BossSummon
{
    public class SummoningRune : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            ItemID.Sets.SortingPriorityBossSpawns[Item.type] = 13; // This helps sort inventory know this is a boss summoning Item.
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }

        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 54;
            Item.maxStack = 20;
            Item.rare = ItemRarityID.Orange;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item44;
            Item.consumable = true;
            Item.noUseGraphic = true;
        }

        public override bool? UseItem(Player player)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<RuneGhost>());
            }

            if (Main.netMode != NetmodeID.Server)
            {
                SoundEngine.PlaySound(SoundID.Roar, player.Center);
            }

            return true;
        }
    }
    public class DropFromWisps : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.DungeonSpirit)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SummoningRune>(), 4));
            }
        }
    }
}