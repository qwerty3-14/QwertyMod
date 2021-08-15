using Terraria;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Equipment.Armor.Glass
{
    public class DazzleSparkle : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = false;
            dust.noLight = true;
            dust.scale = 2f;
            dust.noGravity = true;
        }
    }
}