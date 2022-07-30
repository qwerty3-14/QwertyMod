using System;
using Terraria;
using Terraria.ModLoader;

namespace QwertyMod.Common
{
    public class MinionManager : ModPlayer
    {
        public bool HydraHeadMinion = false;
        public bool LuneArcher = false;
        public bool Dreadnought = false;
        public bool AncientMinion = false;
        public bool GoldDagger = false;
        public bool PlatinumDagger = false;
        public bool mythrilPrism = false;
        public bool OrichalcumDrifter = false;
        public bool chlorophyteSniper = false;
        public bool miniTank = false;
        public bool GlassSpike = false;
        public bool SpaceFighter = false;
        public bool ShieldMinion = false;
        public bool SwordMinion = false;
        public bool TileMinion = false;
        public bool PriestMinion = false;
        public bool RuneMinion = false;
        public bool MechCrossbow = false;
        public bool CasterMinion = false;
        public int PriestSynchroniser = 0;
        public float PriestAngle = 0f;

        public float mythrilPrismRotation = 0;

        public override void ResetEffects()
        {
            HydraHeadMinion = false;
            LuneArcher = false;
            Dreadnought = false;
            AncientMinion = false;
            GoldDagger = false;
            PlatinumDagger = false;
            mythrilPrism = false;
            OrichalcumDrifter = false;
            chlorophyteSniper = false;
            miniTank = false;
            GlassSpike = false;
            SpaceFighter = false;
            ShieldMinion = false;
            SwordMinion = false;
            TileMinion = false;
            PriestMinion = false;
            RuneMinion = false;
            MechCrossbow = false;
            CasterMinion = false;
        }

        public override void PreUpdate()
        {
            mythrilPrismRotation += (float)Math.PI / 90f;
            PriestSynchroniser++;
            PriestAngle = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
        }
    }
}
