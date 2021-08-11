using Terraria;
using Terraria.ModLoader;

namespace QwertyMod.Content.Dusts
{
    public class HydraBeamGlow : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.noLight = false;
            dust.scale = 1f;
        }

        public override bool Update(Dust dust)
        {
            Lighting.AddLight(dust.position, 1f, 1f, 1f);
            return true;
        }
    }
}