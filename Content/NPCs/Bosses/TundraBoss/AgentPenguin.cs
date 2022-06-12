using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.NPCs.Bosses.TundraBoss
{
    public class AgentPenguin : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Agent Penguin");
            Main.npcFrameCount[NPC.type] = 2;
        }

        public override void SetDefaults()
        {
            NPC.width = 22;
            NPC.height = 38;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = 0;
            NPC.lifeMax = 10;
            NPC.defense = 4;
            NPC.damage = 0;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.behindTiles = true;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            database.Entries.Remove(bestiaryEntry);
            // Sets the description of this NPC that is listed in the bestiary
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundSnow,
                new FlavorTextBestiaryInfoElement("No! Just No! Immersion Ruined!")
            }); 
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0f;
        }
        int preJump = 180;
        float maxRopeLength = 300;
        Vector2 start = Vector2.Zero;
        
        public override void AI()
        {
            preJump--;
            if (preJump > 0)
            {
                
                NPC.velocity = Vector2.Zero;
                Dust.NewDust(NPC.BottomLeft + Vector2.UnitY * NPC.width, NPC.width, NPC.width, DustID.Ice);
                start = NPC.Center;
            }
            else
            {
                NPC.noGravity = false;
                if(preJump == -120 || preJump == -180 && Main.netMode != 1)
                {
                    NPC.TargetClosest(true);
                    Vector2 pos = NPC.Center + new Vector2(11 * NPC.spriteDirection * -1, 0);
                    Projectile p = Main.projectile[Projectile.NewProjectile(new EntitySource_Misc(""),  pos, QwertyMethods.PolarVector(11, (Main.player[NPC.target].Center - pos).ToRotation()), ProjectileID.SnowBallFriendly, 10, 0, 255)];
                    p.hostile = true;
                    p.friendly = false;
                }
                if(preJump < -240 && maxRopeLength > 0)
                {
                    maxRopeLength -= 2;
                }
                if(maxRopeLength <= 0)
                {
                    NPC.active = false;
                }
                else
                {
                    Vector2 diff = NPC.Center - start;
                    NPC.position += (-1 * NPC.velocity) * (diff.Length() / maxRopeLength);
                }
            }
            
        }
        int frame = 0;
        public override void FindFrame(int frameHeight)
        {
            frame = ((preJump < -120 && preJump > -150) || (preJump < -180 && preJump > -210)) ? 1 : 0;
            NPC.frame.Y = frame * frameHeight;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (preJump <= 0)
            {
                Vector2 diff = start - NPC.Center;
                for (int i = 0; i < diff.Length(); i += 8)
                {
                    Texture2D rope = Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/TundraBoss/Rope").Value;
                    spriteBatch.Draw(rope, NPC.Center + QwertyMethods.PolarVector(i, diff.ToRotation()) - screenPos, null, drawColor, diff.ToRotation() + (float)Math.PI / 2, new Vector2(3, 0), 1f, 0, 0);
                }
            }
            return preJump <= 0;
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                Gore.NewGore(new EntitySource_Misc(""), NPC.position, NPC.velocity, 160);
                Gore.NewGore(new EntitySource_Misc(""), new Vector2(NPC.position.X, NPC.position.Y), NPC.velocity, 161);
            }
        }
    }
    
}
