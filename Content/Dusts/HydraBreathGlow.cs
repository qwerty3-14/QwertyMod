using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace QwertyMod.Content.Dusts
{
    public class HydraBreathGlow : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.scale = 1f;
        }


        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            return Color.White;
        }
    }
}