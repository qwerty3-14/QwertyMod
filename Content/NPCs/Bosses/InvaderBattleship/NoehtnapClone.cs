using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.Audio;
using QwertyMod.Content.Buffs;
using QwertyMod.Content.Dusts;
using QwertyMod.Common.Fortress;
using QwertyMod.Content.NPCs.Invader;


namespace QwertyMod.Content.NPCs.Bosses.InvaderBattleship
{
    public class NoehtnapClone : ModNPC
    {

        public override void SetDefaults()
        {
            NPC.lifeMax = 40000;
            NPC.width = 150;
            NPC.height = 100;
            NPC.value = 100000;
            NPC.damage = 200;
            NPC.noGravity = true;
            NPC.boss = true;
            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/BuiltToDestroy");
            }
            NPC.knockBackResist = 0;
            NPC.HitSound = new SoundStyle("QwertyMod/Assets/Sounds/invbattleship_hurt1");
            NPC.DeathSound = new SoundStyle("QwertyMod/Assets/Sounds/invbattleship_hurt1");
            NPC.noTileCollide = true;
            NPC.aiStyle = -1;
        }

        public override bool CheckDead()
        {
            NPC.life = 1;
            NPC.dontTakeDamage = true;
            NPC.immortal = true;
            return false;
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(0.75f * NPC.lifeMax * bossAdjustment);
            NPC.damage = NPC.damage / 2;
        }
        float pupilDirection = 0;
        float pupilStareOutAmount = 0;
        int timer = 640;
        int teleportframe = 20;
        public override void AI()
        {
            if(NPC.ai[1] == 1)
            {
                if(teleportframe < 20)
                {
                    teleportframe++;
                }
                else
                {
                    NPC.active = false;
                }
                return;
            }
            if(teleportframe > -1)
            {
                teleportframe--;
                NPC.dontTakeDamage = true;
                return;
            }
            timer--;
            NPC.dontTakeDamage = false;
            if(timer < 600 && timer > 0)
            {
                NoehtnapSpells.UpdateSpell(NPC.GetSource_FromAI(), Spell.AimedShot, NPC.Center, timer, out pupilDirection, out pupilStareOutAmount);
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if(teleportframe != -1)
            {
                if(teleportframe < 20)
                {
                    spriteBatch.Draw(NoehtnapAnimations.teleportAnimaion[19 - teleportframe], NPC.Center - screenPos,
                        null, drawColor, 0,
                        NoehtnapAnimations.teleportAnimaion[19 - teleportframe].Size() * 0.5f, 1f, SpriteEffects.None, 0f);
                }
                return false;
            }
            NoehtnapSpells.DrawBody(spriteBatch, NPC.Center - screenPos, drawColor, pupilDirection, pupilStareOutAmount, false);
            return false;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(NPC.active);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NPC.active = reader.ReadBoolean();
        }
    }
}