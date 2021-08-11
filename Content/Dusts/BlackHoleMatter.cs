using Terraria;
using Terraria.ModLoader;

namespace QwertyMod.Content.Dusts
{
    public class BlackHoleMatter : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.noLight = false;
            dust.scale = 1f;
        }
    }
}