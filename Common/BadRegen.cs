using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace QwertyMod.Common
{
    class BadRegen : ModPlayer
    {
        public bool DinoPox = false;
        public bool noRegen = false;
        public override void ResetEffects()
        {
            DinoPox = false;
            noRegen = false;
        }
        public override void UpdateBadLifeRegen()
        {
            if (DinoPox)//Dino Pox
            {
                if (Player.lifeRegen > 0)
                {
                    Player.lifeRegen = 0;
                }
                Player.lifeRegenTime = 0;
                Player.lifeRegen -= 20;
            }
            if (noRegen)
            {
                if (Player.lifeRegen > 0)
                {
                    Player.lifeRegen = 0;
                }
                Player.lifeRegenTime = 0;
            }
        }
    }
}
