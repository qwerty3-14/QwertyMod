using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Consumable.BossSummon
{
    public class DinoEgg : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Summons the Dino Militia" + "\nThey never died out!");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 52;
            Item.maxStack = 20;
            Item.rare = 6;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = 4;
            Item.UseSound = SoundID.Item44;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            return true;
        }

        public override bool? UseItem(Player player)
        {
            string key = "The Dino Militia is coming!";
            Color messageColor = Color.Orange;
            if (Main.netMode == 2) // Server
            {
                Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey(key), messageColor);
            }
            else if (Main.netMode == 0) // Single Player
            {
                Main.NewText(Language.GetTextValue(key), messageColor);
            }

            if (Main.netMode == 0)
            {
                SoundEngine.PlaySound(SoundID.Roar, player.position);
                DinoEvent.EventActive = true;
                DinoEvent.DinoKillCount = 0;
            }
            if (Main.netMode == 1 && player.whoAmI == Main.myPlayer)
            {
                ModPacket packet = Mod.GetPacket();
                packet.Write((byte)ModMessageType.StartDinoEvent);
                packet.Send();
            }

            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.ChlorophyteBar, 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}