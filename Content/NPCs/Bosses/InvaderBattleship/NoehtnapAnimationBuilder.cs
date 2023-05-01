using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.NPCs.Bosses.InvaderBattleship
{
    public static class NoehtnapAnimations
    {
        public static Texture2D[] teleportAnimaion;
        public static Texture2D[] leftMorphAnimation;
        public static Texture2D[] rightMorphAnimaion;

        public static void BuildAnims()
        {
            var immediate = AssetRequestMode.ImmediateLoad;
            Texture2D noehtnap = Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/InvaderNoehtnap_Checklist", immediate).Value;
            Texture2D warpInto = Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/InvaderNoehtnap_WarpSpot", immediate).Value;
            teleportAnimaion = TextureBuilder.TransitionFrames(warpInto, noehtnap, 20);

            Texture2D noehtnapLeft = Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/InvaderNoehtnap_LeftHalf", immediate).Value;
            Texture2D leftMorphTo = Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/LeftMorphTo", immediate).Value;
            leftMorphAnimation = TextureBuilder.TransitionFrames(leftMorphTo, noehtnapLeft, 30);

            Texture2D noehtnapRight = Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/InvaderNoehtnap_RightHalf", immediate).Value;
            Texture2D rightMorphTo = Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/RightMorphTo", immediate).Value;
            rightMorphAnimaion = TextureBuilder.TransitionFrames(rightMorphTo, noehtnapRight, 30);
        }
    }
}
