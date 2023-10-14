using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using QwertyMod.Content.Items.Consumable.BossSummon;
using QwertyMod.Content.Items.Consumable.Tiles.Trophy;
using QwertyMod.Content.Items.Consumable.Tiles.Relics;
using QwertyMod.Content.Items.Consumable.Tiles.Trophy.Polar;
using QwertyMod.Content.Items.Equipment.Vanity.BossMasks;
using QwertyMod.Content.NPCs.Bosses.AncientMachine;
using QwertyMod.Content.Items.Consumable.Tiles.Trophy.Ancient;
using QwertyMod.Content.NPCs.Bosses.FortressBoss;
using QwertyMod.Content.Items.Consumable.Tiles.Trophy.FortressBoss;
using QwertyMod.Content.NPCs.Bosses.CloakedDarkBoss;
using QwertyMod.Content.NPCs.Bosses.Hydra;
using QwertyMod.Content.Items.Consumable.Tiles.Trophy.Hydra;
using QwertyMod.Content.NPCs.Bosses.BladeBoss;
using QwertyMod.Content.Items.Consumable.Tiles.Trophy.Blade;
using QwertyMod.Content.NPCs.Bosses.RuneGhost;
using QwertyMod.Content.NPCs.Bosses.OLORD;
using QwertyMod.Content.NPCs.Bosses.InvaderBattleship;
using QwertyMod.Content.Items.Consumable.Tiles.Trophy.RuneGhost;
using QwertyMod.Content.Items.Consumable.Tiles.Trophy.OLORD;
using QwertyMod.Content.Items.Consumable.Tiles.Trophy.Invader;
using QwertyMod.Content.Items.Consumable.Tiles.Trophy.Noehtnap;

namespace QwertyMod.Common
{
    public class BossChecklist : ModSystem
    {
        public override void PostSetupContent() {
			// Most often, mods require you to use the PostSetupContent hook to call their methods. This guarantees various data is initialized and set up properly

			// Boss Checklist shows comprehensive information about bosses in its own UI. We can customize it:
			// https://forums.terraria.org/index.php?threads/.50668/
			DoBossChecklistIntegration();

			// We can integrate with other mods here by following the same pattern. Some modders may prefer a ModSystem for each mod they integrate with, or some other design.
		}

		private void DoBossChecklistIntegration() 
        {
			// The mods homepage links to its own wiki where the calls are explained: https://github.com/JavidPack/BossChecklist/wiki/%5B1.4.4%5D-Boss-Log-Entry-Mod-Call
			// If we navigate the wiki, we can find the "LogBoss" method, which we want in this case
			// A feature of the call is that it will create an entry in the localization file of the specified NPC type for its spawn info, so make sure to visit the localization file after your mod runs once to edit it

			if (!ModLoader.TryGetMod("BossChecklist", out Mod bossChecklistMod)) 
            {
				return;
			}

			// For some messages, mods might not have them at release, so we need to verify when the last iteration of the method variation was first added to the mod, in this case 1.6
			// Usually mods either provide that information themselves in some way, or it's found on the GitHub through commit history/blame
			if (bossChecklistMod.Version < new Version(1, 6)) 
            {
				return;
			}

			// The "LogBoss" method requires many parameters, defined separately below:

			// Your entry key can be used by other developers to submit mod-collaborative data to your entry. It should not be changed once defined
			string internalName = "PolarExterminator";

			// Value inferred from boss progression, see the wiki for details
			float weight = 0.7f;
			// Used for tracking checklist progress
			Func<bool> downed = () => DownedBossSystem.downedBear;
			// The NPC type of the boss
			int bossType = ModContent.NPCType<Content.NPCs.Bosses.TundraBoss.PolarBear>();
			// "collectibles" like relic, trophy, mask, pet
			List<int> collectibles = new List<int>()
			{
				ModContent.ItemType<PolarTrophy>(),
				ModContent.ItemType<PolarBearRelic>(),
				ModContent.ItemType<PolarMask>(),
			};

			bossChecklistMod.Call(
				"LogBoss",
				Mod,
				internalName,
				weight,
				downed,
				bossType,
				new Dictionary<string, object>() 
                {
					["collectibles"] = collectibles,
				}
			);

			internalName = "AncientMachine";
			weight = 4.2f;
            downed = () => DownedBossSystem.downedAncient;
            bossType = ModContent.NPCType<AncientMachine>();
			collectibles = new List<int>()
			{
				ModContent.ItemType<AncientMachineTrophy>(),
				ModContent.ItemType<AncientMachineMask>(),
				ModContent.ItemType<AncientMachineRelic>(),
			};
			bossChecklistMod.Call(
				"LogBoss",
				Mod,
				internalName,
				weight,
				downed,
				bossType,
				new Dictionary<string, object>() 
                {
					["collectibles"] = collectibles,
                    ["spawnItems"] = ModContent.ItemType<AncientEmblem>(),
                    ["customPortrait"] = (SpriteBatch spriteBatch, Rectangle rect, Color color) => 
                    {
                        Texture2D texture = ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/AncientMachine/AncientMachine_Checklist").Value;
                        Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                        spriteBatch.Draw(texture, centered, color);
                    }
				}
			);

            internalName = "DivineLight";
			weight = 5.5f;
            downed = () => DownedBossSystem.downedDivineLight;
            bossType = ModContent.NPCType<FortressBoss>();
			collectibles = new List<int>()
			{
				ModContent.ItemType<DivineLightMask>(),
				ModContent.ItemType<DivineLightRelic>(),
				ModContent.ItemType<FortressBossTrophy>(),
			};
			bossChecklistMod.Call(
				"LogBoss",
				Mod,
				internalName,
				weight,
				downed,
				bossType,
				new Dictionary<string, object>() 
                {
					["collectibles"] = collectibles,
                    ["spawnItems"] = ModContent.ItemType<FortressBossSummon>(),
                    ["customPortrait"] = (SpriteBatch spriteBatch, Rectangle rect, Color color) => 
                    {
                        Texture2D texture = ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/FortressBoss/FortressBoss_Bestiary").Value;
                        Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                        spriteBatch.Draw(texture, centered, color);
                    }
				}
			);

            internalName = "Noehtnap";
			weight = 5.7f;
            downed = () => DownedBossSystem.downedNoehtnap;
            bossType = ModContent.NPCType<CloakedDarkBoss>();
			collectibles = new List<int>()
			{
				ModContent.ItemType<NoehtnapRelic>(),
				ModContent.ItemType<NoehtnapMask>(),
				ModContent.ItemType<NoehtnapTrophy>(),
			};
			bossChecklistMod.Call(
				"LogBoss",
				Mod,
				internalName,
				weight,
				downed,
				bossType,
				new Dictionary<string, object>() 
                {
					["collectibles"] = collectibles,
                    ["spawnItems"] = ModContent.ItemType<RitualInterupter>(),
                    ["customPortrait"] = (SpriteBatch spriteBatch, Rectangle rect, Color color) => 
                    {
                        Texture2D texture = ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/CloakedDarkBoss/CloakedDarkBoss_Checklist").Value;
                        Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                        spriteBatch.Draw(texture, centered, color);
                    }
				}
			);


            internalName = "Hydra";
			weight = 7.000001f;
            downed = () => DownedBossSystem.downedHydra;
            bossType = ModContent.NPCType<Hydra>();
			collectibles = new List<int>()
			{
				ModContent.ItemType<HydraRelic>(),
				ModContent.ItemType<HydraTrophy>(),
				ModContent.ItemType<HydraMask>(),
			};
			bossChecklistMod.Call(
				"LogBoss",
				Mod,
				internalName,
				weight,
				downed,
				bossType,
				new Dictionary<string, object>() 
                {
					["collectibles"] = collectibles,
                    ["spawnItems"] = ModContent.ItemType<HydraSummon>(),
                    ["customPortrait"] = (SpriteBatch spriteBatch, Rectangle rect, Color color) => 
                    {
                        Texture2D texture = ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/Hydra/Hydra_Checklist").Value;
                        Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                        spriteBatch.Draw(texture, centered, color);
                    }
				}
			);

            internalName = "Imperious";
			weight = 11.4f;
            downed = () => DownedBossSystem.downedBlade;
            bossType = ModContent.NPCType<Imperious>();
			collectibles = new List<int>()
			{
				ModContent.ItemType<ImperiousRelic>(),
				ModContent.ItemType<BladeBossTrophy>(),
				ModContent.ItemType<ImperiousMask>(),
			};
			bossChecklistMod.Call(
				"LogBoss",
				Mod,
				internalName,
				weight,
				downed,
				bossType,
				new Dictionary<string, object>() 
                {
					["collectibles"] = collectibles,
                    ["spawnItems"] = ModContent.ItemType<BladeBossSummon>(),
                    ["customPortrait"] = (SpriteBatch spriteBatch, Rectangle rect, Color color) => 
                    {
                        Texture2D texture = ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/BladeBoss/Imperious_Bestiary").Value;
                        Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                        spriteBatch.Draw(texture, centered, color);
                    }
				}
			);

            internalName = "RuneGhost";
			weight = 14.2f;
            downed = () => DownedBossSystem.downedRuneGhost;
            bossType = ModContent.NPCType<RuneGhost>();
			collectibles = new List<int>()
			{
				ModContent.ItemType<RuneGhostRelic>(),
				ModContent.ItemType<RuneGhostMask>(),
				ModContent.ItemType<RuneGhostTrophy>(),
			};
			bossChecklistMod.Call(
				"LogBoss",
				Mod,
				internalName,
				weight,
				downed,
				bossType,
				new Dictionary<string, object>() 
                {
					["collectibles"] = collectibles,
                    ["spawnItems"] = ModContent.ItemType<SummoningRune>(),
				}
			);

            internalName = "InvaderBattleship";
			weight = 16.8f;
            downed = () => DownedBossSystem.downedBattleship;
            bossType = ModContent.NPCType<InvaderBattleship>();
			collectibles = new List<int>()
			{
				
				ModContent.ItemType<InvaderBossTrophy>(),
				ModContent.ItemType<InvaderMask>(),	
			};
			bossChecklistMod.Call(
				"LogBoss",
				Mod,
				internalName,
				weight,
				downed,
				bossType,
				new Dictionary<string, object>() 
                {
					["collectibles"] = collectibles,
                    ["spawnItems"] = ModContent.ItemType<GodSealKeycard>(),
                    ["customPortrait"] = (SpriteBatch spriteBatch, Rectangle rect, Color color) => 
                    {
                        Texture2D texture = ModContent.Request<Texture2D>("QwertyMod/Content/NPCs/Bosses/InvaderBattleship/InvaderBattleship_Bestiary").Value;
                        Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                        spriteBatch.Draw(texture, centered, color);
                    }
				}
			);

            internalName = "OLORD";
			weight = 17.8f;
            downed = () => DownedBossSystem.downedOLORD;
            bossType = ModContent.NPCType<OLORDv2>();
			collectibles = new List<int>()
			{
				ModContent.ItemType<OLORDRelic>(),
				ModContent.ItemType<OLORDTrophy>(),
				ModContent.ItemType<OLORDMask>(),
			};
			bossChecklistMod.Call(
				"LogBoss",
				Mod,
				internalName,
				weight,
				downed,
				bossType,
				new Dictionary<string, object>() 
                {
					["collectibles"] = collectibles,
                    ["spawnItems"] = ModContent.ItemType<B4Summon>(),
				}
			);
		}
    }
}