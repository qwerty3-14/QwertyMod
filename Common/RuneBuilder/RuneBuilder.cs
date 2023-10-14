﻿using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;


namespace QwertyMod.Common.RuneBuilder
{
    public static class RuneSprites
    {
        public static Texture2D[] runeCycle;
        public static Texture2D[] runes = new Texture2D[4];
        public static Texture2D[][] runeTransition = new Texture2D[4][];

        public static Texture2D[] bigRunes = new Texture2D[4];
        public static Texture2D[][] bigRuneTransition = new Texture2D[4][];

        public static Texture2D runeGhostMoving;
        public static Texture2D[] runeGhostPhaseIn;

        public static Texture2D[] aggroStrike;

        static Texture2D[] runesToCycle = new Texture2D[4];
        static Texture2D[] runeBases = new Texture2D[4];

        public static void BuildRunes()
        {
            var immediate = AssetRequestMode.ImmediateLoad;

            runeCycle = new Texture2D[80];
            runesToCycle[0] = ModContent.Request<Texture2D>("QwertyMod/Common/RuneBuilder/AggroRune", immediate).Value;
            runesToCycle[1] = ModContent.Request<Texture2D>("QwertyMod/Common/RuneBuilder/LeechRune_WhiteSpaced", immediate).Value;
            runesToCycle[2] = ModContent.Request<Texture2D>("QwertyMod/Common/RuneBuilder/IceRune_WhiteSpaced", immediate).Value;
            runesToCycle[3] = ModContent.Request<Texture2D>("QwertyMod/Common/RuneBuilder/PursuitRune_WhiteSpaced", immediate).Value;
            Texture2D[] runesToAdd = TextureBuilder.TransitionFrames(runesToCycle[0], runesToCycle[1], 20);
            for (int k = 0; k < 20; k++)
            {
                runeCycle[k] = runesToAdd[k];
            }
            runesToAdd = TextureBuilder.TransitionFrames(runesToCycle[1], runesToCycle[2], 20);
            for (int k = 0; k < 20; k++)
            {
                runeCycle[k + 20] = runesToAdd[k];
            }
            runesToAdd = TextureBuilder.TransitionFrames(runesToCycle[2], runesToCycle[3], 20);
            for (int k = 0; k < 20; k++)
            {
                runeCycle[k + 40] = runesToAdd[k];
            }
            runesToAdd = TextureBuilder.TransitionFrames(runesToCycle[3], runesToCycle[0], 20);
            for (int k = 0; k < 20; k++)
            {
                runeCycle[k + 60] = runesToAdd[k];
            }

            runes[0] = ModContent.Request<Texture2D>("QwertyMod/Common/RuneBuilder/AggroRune", immediate).Value;
            runes[1] = ModContent.Request<Texture2D>("QwertyMod/Common/RuneBuilder/LeechRune", immediate).Value;
            runes[2] = ModContent.Request<Texture2D>("QwertyMod/Common/RuneBuilder/IceRune", immediate).Value;
            runes[3] = ModContent.Request<Texture2D>("QwertyMod/Common/RuneBuilder/PursuitRune", immediate).Value;
            runeBases[0] = ModContent.Request<Texture2D>("QwertyMod/Common/RuneBuilder/AggroRune_Base", immediate).Value;
            runeBases[1] = ModContent.Request<Texture2D>("QwertyMod/Common/RuneBuilder/LeechRune_Base", immediate).Value;
            runeBases[2] = ModContent.Request<Texture2D>("QwertyMod/Common/RuneBuilder/IceRune_Base", immediate).Value;
            runeBases[3] = ModContent.Request<Texture2D>("QwertyMod/Common/RuneBuilder/PursuitRune_Base", immediate).Value;
            for (int i = 0; i < 4; i++)
            {
                runeTransition[i] = TextureBuilder.TransitionFrames(runeBases[i], runes[i], 20);
            }

            bigRunes[0] = ModContent.Request<Texture2D>("QwertyMod/Common/RuneBuilder/RedRune", immediate).Value;
            bigRunes[1] = ModContent.Request<Texture2D>("QwertyMod/Common/RuneBuilder/GreenRune", immediate).Value;
            bigRunes[2] = ModContent.Request<Texture2D>("QwertyMod/Common/RuneBuilder/CyanRune", immediate).Value;
            bigRunes[3] = ModContent.Request<Texture2D>("QwertyMod/Common/RuneBuilder/PurpleRune", immediate).Value;
            Texture2D bigBaseRune = ModContent.Request<Texture2D>("QwertyMod/Common/RuneBuilder/BigRune_Base", immediate).Value;
            for (int i = 0; i < 4; i++)
            {
                bigRuneTransition[i] = TextureBuilder.TransitionFrames(bigBaseRune, bigRunes[i], 20);
            }

            runeGhostMoving = ModContent.Request<Texture2D>("QwertyMod/Common/RuneBuilder/RuneGhost", immediate).Value;
            Texture2D runeGhostBase = ModContent.Request<Texture2D>("QwertyMod/Common/RuneBuilder/RuneGhost_Base", immediate).Value;
            runeGhostPhaseIn = TextureBuilder.TransitionFrames(runeGhostBase, runeGhostMoving, 20);

            aggroStrike = TextureBuilder.TransitionFrames(ModContent.Request<Texture2D>("QwertyMod/Common/RuneBuilder/AggroStrike_Base", immediate).Value, ModContent.Request<Texture2D>("QwertyMod/Common/RuneBuilder/AggroStrike", immediate).Value, 4);
        }
    }
    public enum Runes : byte
    {
        Aggro = 0,
        Leech = 1,
        IceRune = 2,
        Pursuit = 3
    }
}
