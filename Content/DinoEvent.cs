using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common;
using QwertyMod.Content.NPCs.DinoMilitia;
using System.Collections.Generic;
using System.IO;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content
{

    class DinoEvent : ModSystem
    {
        public static bool EventActive = false;
        public static int DinoKillCount = 0;
        public override void OnWorldLoad()
        {
            DinoKillCount = 0;
            EventActive = false;
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(DinoKillCount);
        }

        public override void NetReceive(BinaryReader reader)
        {
            DinoKillCount = reader.ReadInt32();
        }

        public override void PreUpdateWorld()
        {
            if (DinoKillCount >= 150)
            {
                EventActive = false;
                DownedBossSystem.downedDinos = true;
                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
                string key = "You drove the dinosaurs to extinction!";
                Color messageColor = Color.Orange;
                if (Main.netMode == 2) // Server
                {
                    Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey(key), messageColor);
                }
                else if (Main.netMode == 0) // Single Player
                {
                    Main.NewText(Language.GetTextValue(key), messageColor);
                }
                DinoKillCount = 0;
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            if (EventActive)
            {
                int index = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
                LegacyGameInterfaceLayer orionProgress = new LegacyGameInterfaceLayer("Dino Militia",
                    delegate
                    {
                        DrawDinoEvent(Main.spriteBatch);
                        return true;
                    },
                    InterfaceScaleType.UI);
                layers.Insert(index, orionProgress);
            }
        }

        public void DrawDinoEvent(SpriteBatch spriteBatch)
        {
            if (EventActive && !Main.gameMenu)
            {
                float scaleMultiplier = 0.5f + 1 * 0.5f;
                float alpha = 0.5f;
                Texture2D progressBg = TextureAssets.ColorBar.Value;
                Texture2D progressColor = TextureAssets.ColorBar.Value;
                Texture2D orionIcon = Request<Texture2D>("QwertyMod/Content/Items/Consumable/BossSummon/DinoEgg").Value;
                Color descColor = new Color(39, 86, 134);

                Color waveColor = new Color(255, 241, 51);
                Color barrierColor = new Color(255, 241, 51);

                try
                {
                    //draw the background for the waves counter
                    const int offsetX = 20;
                    const int offsetY = 20;
                    int width = (int)(200f * scaleMultiplier);
                    int height = (int)(46f * scaleMultiplier);
                    Rectangle waveBackground = Utils.CenteredRectangle(new Vector2(Main.screenWidth - offsetX - 100f, Main.screenHeight - offsetY - 23f), new Vector2(width, height));
                    Utils.DrawInvBG(spriteBatch, waveBackground, new Color(63, 65, 151, 255) * 0.785f);

                    //draw wave text

                    string waveText = "Cleared " + (int)(((float)DinoKillCount / 150f) * 100) + "%";
                    Utils.DrawBorderString(spriteBatch, waveText, new Vector2(waveBackground.X + waveBackground.Width / 2, waveBackground.Y), Color.White, scaleMultiplier, 0.5f, -0.1f);

                    // Main.NewText(MathHelper.Clamp((modWorld.DinoKillCount/modWorld.MaxDinoKillCount), 0f, 1f));
                    Rectangle waveProgressBar = Utils.CenteredRectangle(new Vector2(waveBackground.X + waveBackground.Width * 0.5f, waveBackground.Y + waveBackground.Height * 0.75f), new Vector2(progressColor.Width, progressColor.Height));
                    Rectangle waveProgressAmount = new Rectangle(0, 0, (int)(progressColor.Width * MathHelper.Clamp(((float)DinoKillCount / 150f), 0f, 1f)), progressColor.Height);
                    Vector2 offset = new Vector2((waveProgressBar.Width - (int)(waveProgressBar.Width * scaleMultiplier)) * 0.5f, (waveProgressBar.Height - (int)(waveProgressBar.Height * scaleMultiplier)) * 0.5f);

                    spriteBatch.Draw(progressBg, waveProgressBar.Location.ToVector2() + offset, null, Color.White * alpha, 0f, new Vector2(0f), scaleMultiplier, SpriteEffects.None, 0f);
                    spriteBatch.Draw(progressBg, waveProgressBar.Location.ToVector2() + offset, waveProgressAmount, waveColor, 0f, new Vector2(0f), scaleMultiplier, SpriteEffects.None, 0f);

                    //draw the icon with the event description

                    //draw the background
                    const int internalOffset = 6;
                    Vector2 descSize = new Vector2(154, 40) * scaleMultiplier;
                    Rectangle barrierBackground = Utils.CenteredRectangle(new Vector2(Main.screenWidth - offsetX - 100f, Main.screenHeight - offsetY - 19f), new Vector2(width, height));
                    Rectangle descBackground = Utils.CenteredRectangle(new Vector2(barrierBackground.X + barrierBackground.Width * 0.5f, barrierBackground.Y - internalOffset - descSize.Y * 0.5f), descSize);
                    Utils.DrawInvBG(spriteBatch, descBackground, descColor * alpha);

                    //draw the icon
                    int descOffset = (descBackground.Height - (int)(32f * scaleMultiplier)) / 2;
                    Rectangle icon = new Rectangle(descBackground.X + descOffset, descBackground.Y + descOffset, (int)(32 * scaleMultiplier), (int)(32 * scaleMultiplier));
                    spriteBatch.Draw(orionIcon, icon, Color.White);

                    //draw text

                    Utils.DrawBorderString(spriteBatch, "Dino Militia", new Vector2(barrierBackground.X + barrierBackground.Width * 0.5f, barrierBackground.Y - internalOffset - descSize.Y * 0.5f), Color.White, 0.80f, 0.3f, 0.4f);
                }
                catch
                {
                    //ErrorLogger.Log(e.ToString());
                }
            }
        }

    }
    public class DinoSpawnRates : GlobalNPC
    {
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            int defaultSpawnRate = 600;
            int defaultmaxSpawn = 5;
            if (DinoEvent.EventActive)
            {
                if (NPC.AnyNPCs(NPCType<TheGreatTyrannosaurus>()))
                {
                    spawnRate = 0;
                    maxSpawns = 0;
                }
                else
                {
                    spawnRate = (int)((spawnRate * 10f) / defaultSpawnRate);
                    maxSpawns = (int)((maxSpawns * 10f) / defaultmaxSpawn);
                }
            }
        }
    }
    public class FunnyDinoDeathQuotes : ModPlayer
    {
        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter)
        {

            if (damageSource.SourceProjectileType == ProjectileType<SnowFlake>())
            {
                damageSource = PlayerDeathReason.ByCustomReason(Player.name + " was driven to extintion by climate change!"); // change death message
            }
            if (damageSource.SourceProjectileType == ProjectileType<DinoBomb>() || damageSource.SourceProjectileType == ProjectileType<DinoBombExplosion>())
            {
                damageSource = PlayerDeathReason.ByCustomReason(Player.name + " was driven to extintion by dino bomb"); // change death message
            }
            if (damageSource.SourceProjectileType == ProjectileType<TankCannonBall>() || damageSource.SourceProjectileType == ProjectileType<TankCannonBallExplosion>())
            {
                damageSource = PlayerDeathReason.ByCustomReason(Player.name + " was driven to extintion by triceratank's cannon"); // change death message
            }
            if (damageSource.SourceProjectileType == ProjectileType<MeteorFall>())
            {
                damageSource = PlayerDeathReason.ByCustomReason(Player.name + " was driven to extintion by meteor!"); // change death message
            }
            if (damageSource.SourceProjectileType == ProjectileType<MeteorLaunch>())
            {
                damageSource = PlayerDeathReason.ByCustomReason(Player.name + " was driven to extintion by meteor!"); // change death message
            }
            if (damageSource.SourceNPCIndex >= 0 && Main.npc[damageSource.SourceNPCIndex].type == NPCType<TheGreatTyrannosaurus>())
            {
                damageSource = PlayerDeathReason.ByCustomReason(Player.name + " was driven to extintion by The Great Tyrannosaurus!");
            }
            if (damageSource.SourceNPCIndex >= 0 && Main.npc[damageSource.SourceNPCIndex].type == NPCType<Triceratank>())
            {
                damageSource = PlayerDeathReason.ByCustomReason(Player.name + " was driven to extintion by Triceratank");
            }
            if (damageSource.SourceNPCIndex >= 0 && Main.npc[damageSource.SourceNPCIndex].type == NPCType<Utah>())
            {
                damageSource = PlayerDeathReason.ByCustomReason(Player.name + " was driven to extintion by Utah");
            }
            if (damageSource.SourceNPCIndex >= 0 && Main.npc[damageSource.SourceNPCIndex].type == NPCType<Velocichopper>())
            {
                damageSource = PlayerDeathReason.ByCustomReason(Player.name + " was driven to extintion by Velocichopper");
            }
            return true;
        }
    }
}
