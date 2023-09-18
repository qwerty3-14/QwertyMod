using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common.PlayerLayers;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using QwertyMod.Common.Fortress;

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
            Item.UseSound = SoundID.Item1;
            Item.width = 32;
            Item.height = 32;
            Item.autoReuse = true;
            Item.useTurn = true;
            if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Request<Texture2D>("QwertyMod/Content/Items/Tool/InvasionCaller_Glow").Value;
            }
        }
        

        public override bool? UseItem(Player player)
        {
            SkyFortress.beingInvaded = true;

            return base.UseItem(player);
        }
    }
        

}