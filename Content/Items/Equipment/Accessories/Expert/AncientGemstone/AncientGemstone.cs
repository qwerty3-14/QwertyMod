using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using QwertyMod.Content.Items.Weapon.Morphs;

namespace QwertyMod.Content.Items.Equipment.Accessories.Expert.AncientGemstone
{
    public class AncientGemstone : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Gemstone");
            Tooltip.SetDefault("Allows you use quick morphs while still on cooldown... at the cost of your life!");
        }


        public override void SetDefaults()
        {
            Item.value = 10000;
            Item.rare = 1;

            Item.expert = true;
            Item.width = 32;
            Item.height = 32;
            Item.value = 150000;
            Item.rare = 3;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ShapeShifterPlayer>().ruthlessMorhphing = 2;
            base.UpdateAccessory(player, hideVisual);
        }
    }
}