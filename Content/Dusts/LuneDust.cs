using Terraria;
using Terraria.ModLoader;

namespace QwertyMod.Content.Dusts
{
    public class LuneDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.noLight = false;
            dust.scale = 1f;
        }

        public override bool Update(Dust dust)
        {
            Lighting.AddLight(dust.position, .5f, .5f, .5f);
            return true;
        }
    }
}