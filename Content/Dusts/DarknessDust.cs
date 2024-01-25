using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace QwertyMod.Content.Dusts
{
    public class DarknessDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.scale = 1f;
        }

        public override bool Update(Dust dust)
        {
            if (dust.alpha > 0)
            {
                dust.alpha -= (int)(255f / 30f);
            }
            return true;
        }

        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            return Color.White;
        }
    }
}