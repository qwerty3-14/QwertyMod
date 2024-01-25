using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Weapon.Morphs
{
    class ShapeShifterPlayer : ModPlayer
    {
        public int morphCooldownTime = 0;
        public float coolDownDuration = 1f;
        public bool noDraw = false;
        public int ruthlessMorhphing = 0;
        public override void ResetEffects()
        {
            ResetVariables();
        }

        public override void UpdateDead()
        {
            ResetVariables();
        }
        private void ResetVariables()
        {
            coolDownDuration = 1f;
            noDraw = false;
            ruthlessMorhphing--;
        }
        public override void PreUpdate()
        {
            if (noDraw)
            {
                Player.immuneAlpha = 255;
            }
        }
    }
}
