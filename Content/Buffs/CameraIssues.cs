using Terraria;
using Terraria.ModLoader;
using QwertyMod.Common;
using System;

namespace QwertyMod.Content.Buffs
{
    public class CameraIssues : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Camera Issues");
            Description.SetDefault("I'm not sorry...");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<CrazyScreen>().shake = true;
        }
    }
    

    public class CrazyScreen : ModPlayer
    {
        public bool shake = false;
        float rot = 0;

        public override void ResetEffects()
        {
            shake = false;
        }
        public override void PostUpdate()
        {
            rot += (float)Math.PI / 120f;
        }

        public override void ModifyScreenPosition()
        {
            if (shake)
            {
                Main.screenPosition += QwertyMethods.PolarVector(50, rot);
            }
        }
    }
}