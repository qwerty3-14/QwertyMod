using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common.PlayerLayers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using QwertyMod.Common.Fortress;
using Terraria.Localization;

namespace QwertyMod.Content.Items.Tool
{
    public class InvasionCaller : ModItem
    {
        public override void SetDefaults()
        {

            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.value = GearStats.InvaderGearValue;
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item6;
            Item.width = 32;
            Item.height = 32;
            Item.autoReuse = true;
            Item.useTurn = true;
            if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = ModContent.Request<Texture2D>("QwertyMod/Content/Items/Tool/InvasionCaller_Glow").Value;
            }
        }
        

        public override bool? UseItem(Player player)
        {
            if(!SkyFortress.beingInvaded)
            {
                SkyFortress.beingInvaded = true;
                string key = Language.GetTextValue(Mod.GetLocalizationKey("FortressInvasion"));
                Color messageColor = Color.Green;
                if (Main.netMode == NetmodeID.Server) // Server
                {
                    Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey(key), messageColor);
                }
                else if (Main.netMode == NetmodeID.SinglePlayer) // Single Player
                {
                    Main.NewText(Language.GetTextValue(key), messageColor);
                }
                if(Main.netMode != NetmodeID.SinglePlayer)
                {
                    ModPacket packet = Mod.GetPacket();
                    packet.Write((byte)ModMessageType.SetFortressInvasionStatus);
                    packet.Write(SkyFortress.beingInvaded);
                    packet.Write(SkyFortress.initalInvasion);
                    packet.Send();
                }
            }

            return base.UseItem(player);
        }
    }
        

}