using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Content.NPCs.Bosses.InvaderBattleship;
using QwertyMod.Content.NPCs.Fortress;
using QwertyMod.Content.NPCs.Invader;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Common.Fortress
{
    public class BattleshipSpawnIn : ModSystem
    {
        
        public static int spawnTimer = -1;
        public const int alarmTime = 60;
        public const int scanTime = 60;
        public const int spawnTime = 60;
        public override void PostUpdateProjectiles()
        {
            if(spawnTimer > -1)
            {
                spawnTimer--;
                if(spawnTimer == alarmTime + spawnTime)
                {
                    RemoveEnemies();
                }
                if(spawnTimer == spawnTime)
                {
                    SoundEngine.PlaySound(new SoundStyle("QwertyMod/Assets/Sounds/invbattleship_reveal"));
                }
                if(spawnTimer == -1)
                {
                    if (Main.netMode == NetmodeID.SinglePlayer)
                    {
                        int npcID = NPC.NewNPC(NPC.GetBossSpawnSource(Main.myPlayer), 0, 0, ModContent.NPCType<InvaderBattleship>());
                    }
                    else
                    {
                        ModPacket packet = Mod.GetPacket();
                        packet.Write((byte)ModMessageType.SummonBattleship);
                        packet.WriteVector2(Vector2.Zero);
                        packet.Write(Main.myPlayer);
                        packet.Send();
                    }
                }
            }
        }
        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(spawnTimer);
        }
        public override void NetReceive(BinaryReader reader)
        {
            spawnTimer = reader.ReadInt32();
        }
        void RemoveEnemies()
        {
            for(int i = 0; i < Main.npc.Length; i++)
            {
                if(Main.npc[i].active)
                {
                    if(Main.npc[i].GetGlobalNPC<InvaderNPCGeneral>().invaderNPC)
                    {
                        InvaderNPCGeneral.SpawnOut(Main.npc[i]);
                    }
                    if(Main.netMode != NetmodeID.MultiplayerClient && Main.npc[i].GetGlobalNPC<FortressNPCGeneral>().fortressNPC)
                    {
                        Main.npc[i].StrikeInstantKill();
                    }
                }
            }
        }
    }
    public class ScanPlayer : ModPlayer
    {

    }
    class ScanPlayerLayer : PlayerDrawLayer
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return true;
        }
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.FrontAccFront);

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            if(BattleshipSpawnIn.spawnTimer > BattleshipSpawnIn.spawnTime + BattleshipSpawnIn.alarmTime)
            {
                Player drawPlayer = drawInfo.drawPlayer;
                Texture2D texture = ModContent.Request<Texture2D>("QwertyMod/Common/Fortress/ScanPlayer").Value;
                float amt =(1f - ((float)(BattleshipSpawnIn.spawnTimer - (BattleshipSpawnIn.spawnTime + BattleshipSpawnIn.alarmTime)) / BattleshipSpawnIn.scanTime));
                Vector2 drawAt = drawInfo.Position + drawPlayer.Size * 0.5f + Vector2.UnitY * MathF.Cos(amt * 2f * MathF.PI) * drawPlayer.height / 2;
                DrawData drawData = new DrawData(texture, drawAt - Main.screenPosition, null, Color.White, 0, texture.Size() * 0.5f, 1f, drawInfo.playerEffect, 0);
                drawInfo.DrawDataCache.Add(drawData);
                
                
            }
        }

    }
}