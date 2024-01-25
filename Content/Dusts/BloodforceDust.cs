using Terraria;
using Terraria.ModLoader;

namespace QwertyMod.Content.Dusts
{
    public class BloodforceDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.noLight = true;
            dust.scale = 1f;
        }
    }
}