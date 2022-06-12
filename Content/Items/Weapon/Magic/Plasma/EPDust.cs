using Terraria;
using Terraria.ModLoader;



namespace QwertyMod.Content.Items.Weapon.Magic.Plasma
{
    public class EPDust : ModDust
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