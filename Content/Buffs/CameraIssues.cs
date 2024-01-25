using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace QwertyMod.Content.Buffs
{
    public class CameraIssues : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<CrazyScreen>().shake = true;
        }
    }
    

    public class CrazyScreen : ModPlayer
    {
        public bool shake = false;
        float rot = 0;

        public override void ResetEffects()
        {
            shake = false;
        }
        public override void PostUpdate()
        {
            rot += MathF.PI / 120f;
        }

        public override void ModifyScreenPosition()
        {
            if (shake)
            {
                Main.screenPosition += QwertyMethods.PolarVector(50, rot) + Vector2.UnitY * MathF.Sin(rot) * 200;
            }
        }
    }
}