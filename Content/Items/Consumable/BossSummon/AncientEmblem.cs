using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common.PlayerLayers;
using QwertyMod.Content.Items.Consumable.Tiles.Bars;
using QwertyMod.Content.NPCs.Bosses.AncientMachine;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace QwertyMod.Content.Items.Consumable.BossSummon
{
    public class AncientEmblem : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Summons The Ancient Machine");
            ItemID.Sets.SortingPriorityBossSpawns[Item.type] = 13; // This helps sort inventory know this is a boss summoning Item.
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }


        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 28;
            Item.maxStack = 20;
            Item.rare = 3;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = 4;
            Item.UseSound = SoundID.Item44;
            Item.consumable = true;
            if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("QwertyMod/Content/Items/Consumable/BossSummon/AncientEmblem_Glow").Value;
            }
        }

        
        public override bool CanUseItem(Player player)
        {
            if (!NPC.AnyNPCs(NPCType<AncientMachine>()))
            {
                NPC.SpawnOnPlayer(player.whoAmI, NPCType<AncientMachine>());
                SoundEngine.PlaySound(SoundID.Roar, player.position, 0);
                Item.stack--;
                return true;
            }
            return false;
        }
        
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemType<RhuthiniumBar>(), 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}