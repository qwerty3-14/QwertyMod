using QwertyMod.Content.NPCs.Bosses.Hydra;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertyMod.Content.Items.Consumable.BossSummon
{
    public class HydraSummon : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.SortingPriorityBossSpawns[Item.type] = 13; // This helps sort inventory know this is a boss summoning Item.
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }

        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 46;
            Item.maxStack = 20;
            Item.rare = ItemRarityID.Orange;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item44;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(ModContent.NPCType<Hydra>());
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                SoundEngine.PlaySound(SoundID.Roar, player.Center);
                QwertyMethods.NPCSpawnOnPlayer(player, ModContent.NPCType<Hydra>());
                return true;
            }

            return base.UseItem(player);
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.SoulofLight, 6)
                .AddIngredient(ItemID.SoulofNight, 6)
                .Register();
        }
    }
}