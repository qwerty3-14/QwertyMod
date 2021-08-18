using Terraria;
using Terraria.ModLoader;

namespace QwertyMod.Content.Dusts
{
    public class DinoSkin2 : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = false;
            dust.noLight = true;
            dust.scale = 1f;
        }
    }
}