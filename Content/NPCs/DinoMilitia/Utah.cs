using QwertyMod.Content.Dusts;
using QwertyMod.Content.Items.Consumable.Tiles.Banners;
using QwertyMod.Content.Items.Equipment.Accessories;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertyMod.Content.NPCs.DinoMilitia
{
    public class Utah : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4;
        }

        public override void SetDefaults()
        {
            NPC.width = 80;
            NPC.height = 48;
            NPC.damage = 70;
            NPC.defense = 18;
            NPC.lifeMax = 340;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 60f;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = 26;
            //aiType = 86;
            //animationType = 3;
            NPC.buffImmune[BuffID.Confused] = false;
            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/OldDinosNewGuns");
            }
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<UtahBanner>();
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Sets the description of this NPC that is listed in the bestiary

            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
            {
                new MoonLordPortraitBackgroundProviderBestiaryInfoElement(), //Plain black background
                new FlavorTextBestiaryInfoElement("Utahraptors make up the majority of the dino militia's infantry, while thier smaller cousins the velociraptors operate the technology.")
            });
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (DinoEvent.EventActive)
            {
                return 45;
            }
            else
            {
                return 0f;
            }
        }
        public override void OnKill()
        {
            if (DinoEvent.EventActive)
            {
                DinoEvent.DinoKillCount += 1;
                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
            }
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int i = 0; i < 10; i++)
            {
                int dustType = ModContent.DustType<DinoSkin>();
                int dustIndex = Dust.NewDust(NPC.position, NPC.width, NPC.height, dustType);
                Dust dust = Main.dust[dustIndex];
                dust.velocity.X = dust.velocity.X + Main.rand.Next(-50, 51) * 0.01f;
                dust.velocity.Y = dust.velocity.Y + Main.rand.Next(-50, 51) * 0.01f;
                dust.scale *= 1f + Main.rand.Next(-30, 31) * 0.01f;
            }
        }

        public override void AI()
        {
            Player player = Main.player[NPC.target];
            NPC.TargetClosest(true);
        }

        public override void FindFrame(int frameHeight)
        {
            // This makes the sprite flip horizontally in conjunction with the NPC.direction.
            NPC.spriteDirection = NPC.direction;
            NPC.frameCounter++;
            if (NPC.frameCounter < 10)
            {
                NPC.frame.Y = 0 * frameHeight;
            }
            else if (NPC.frameCounter < 20)
            {
                NPC.frame.Y = 1 * frameHeight;
            }
            else if (NPC.frameCounter < 30)
            {
                NPC.frame.Y = 2 * frameHeight;
            }
            else if (NPC.frameCounter < 40)
            {
                NPC.frame.Y = 3 * frameHeight;
            }
            else
            {
                NPC.frameCounter = 0;
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<DinoTooth>(), 100, 1, 1));
        }
    }
}