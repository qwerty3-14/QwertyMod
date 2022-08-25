using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ID.ArmorIDs;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;

namespace QwertyMod.Content.Items.Equipment.Armor.Vitallum
{
    [AutoloadEquip(EquipType.Head)]
    public class VitallumHeadress : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vitallum Headress");
            Tooltip.SetDefault("Increases max life by 80 \n7% increased damage \n Drain life from nearby enemies");
            Head.Sets.DrawHatHair[Item.headSlot] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.rare = 8;
            Item.value = Item.sellPrice(gold: 6);
        }

        public override void UpdateEquip(Player player)
        {
            player.statLifeMax2 += 80;
            player.GetDamage(DamageClass.Generic) += .07f;
            player.GetModPlayer<HeadressEffects>().poisonHeal = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.ChlorophyteBar, 12)
                .AddIngredient(ItemID.LifeCrystal, 4)
                .AddIngredient(ItemType<VitallumCoreCharged>(), 1)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemType<VitallumLifeguard>() && legs.type == ItemType<VitallumJeans>();
        }

        public override void UpdateArmorSet(Player player)
        {
            String s = "Please go to conrols and bind the 'Yet another special ability key'";
            foreach (String key in QwertyMod.YetAnotherSpecialAbility.GetAssignedKeys()) //get's the string of the hotkey's name
            {
                s = "Set Bonus: You generate Vitallum hearts over time" + "\nPress " + key + " to consume the hearts for health.";
            }
            player.setBonus = s;
            player.GetModPlayer<HeadressEffects>().setBonus = true;
        }

        public override void OnCraft(Recipe recipe)
        {
            Main.LocalPlayer.QuickSpawnItem(new EntitySource_Misc(""), ItemType<VitallumCoreUncharged>(), 1);
        }
    }

    public class HeadressEffects : ModPlayer
    {
        public bool poisonHeal = false;
        private int counter = 0;
        private List<Dust> leechDusts = new List<Dust>();
        public bool setBonus = false;
        public int heartCount = 0;
        public int maxHearts = 5;
        public int heartReplenishTime = 300;
        public int heartCounter = 0;
        public int heartRadius = 60;
        public float trigCounter = 0;

        public override void ResetEffects()
        {
            poisonHeal = false;
            setBonus = false;
        }

        public override void ProcessTriggers(TriggersSet triggersSet) //runs hotkey effects
        {
            if (QwertyMod.YetAnotherSpecialAbility.JustPressed) //hotkey is pressed
            {
                if (setBonus && heartCount > 0 && heartCounter > 0)
                {
                    heartCounter = -heartRadius;
                }
            }
        }

        public override void PreUpdate()
        {
            if (setBonus)
            {
                trigCounter += (float)Math.PI / 60;
                heartCounter++;
                if (heartCounter < 0)
                {
                    heartRadius = -heartCounter;
                }
                else if (heartCounter == 0)
                {
                    for (int i = 0; i < heartCount; i++)
                    {
                        int amt = 20;
                        Player.statLife += amt;
                        Player.HealEffect(amt, true);
                    }
                    if (Player.statLife > Player.statLifeMax2)
                    {
                        Player.statLife = Player.statLifeMax2;
                    }
                    heartCount = 0;
                    heartRadius = 60;
                }
                else if (heartCount < maxHearts)
                {
                    if (heartCounter > heartReplenishTime + 1)
                    {
                        heartCount++;
                        heartCounter = 1;
                    }
                }
            }
            else
            {
                heartCount = 0;
                heartCounter = 0;
            }
        }

        public override void UpdateEquips()
        {
            if (poisonHeal)
            {
                for (int i = 0; i < Main.npc.Length; i++)
                {
                    if (Main.npc[i].active && !Main.npc[i].immortal && !Main.npc[i].dontTakeDamage && (Main.npc[i].Center - Player.Center).Length() < 400)
                    {
                        Main.npc[i].AddBuff(151, 30);
                        Player.soulDrain++;
                        if (Main.rand.Next(3) != 0)
                        {
                            Vector2 center = Main.npc[i].Center;
                            center.X += (float)Main.rand.Next(-100, 100) * 0.05f;
                            center.Y += (float)Main.rand.Next(-100, 100) * 0.05f;
                            center += Main.npc[i].velocity;
                            int num = Dust.NewDust(center, 1, 1, 235);
                            Main.dust[num].velocity *= 0f;
                            Main.dust[num].scale = (float)Main.rand.Next(70, 85) * 0.01f;
                            Main.dust[num].fadeIn = Player.whoAmI + 1;
                        }
                    }
                }
            }
        }

    }
    public class HeartDraw : PlayerDrawLayer
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return true;
        }
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.HeldItem);
        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            HeadressEffects modPlayer = drawPlayer.GetModPlayer<HeadressEffects>();
            Mod mod = ModLoader.GetMod("QwertyMod");
            int horizontalFrame = 0;
            int horizontalFrames = 4;
            if (horizontalFrames > 1)
            {
                int frameTimer = drawPlayer.GetModPlayer<CommonStats>().genericCounter % ((2 * horizontalFrames - 2) * 10);
                horizontalFrame = frameTimer / 10;

                if (horizontalFrame > horizontalFrames - 1)
                {
                    horizontalFrame = (horizontalFrames - 1) - (horizontalFrame - (horizontalFrames - 1));
                }
            }
            for (int e = 0; e < modPlayer.heartCount; e++)
            {
                Vector2 heartPos = drawPlayer.Center + QwertyMethods.PolarVector(modPlayer.heartRadius, modPlayer.trigCounter + ((float)Math.PI * 2 * e) / (float)modPlayer.heartCount) - Main.screenPosition;
                Texture2D heartTexture = Request<Texture2D>("QwertyMod/Content/Items/Equipment/Armor/Vitallum/VitallumCoreUncharged").Value;
                DrawData data = new DrawData(heartTexture, heartPos, null, Color.White, 0, heartTexture.Size() * .5f, 1f, 0, 0);
                data.shader = drawInfo.cBody;
                drawInfo.DrawDataCache.Add(data);

                Texture2D veinTexture = Request<Texture2D>("QwertyMod/Content/Items/Equipment/Armor/Vitallum/VitallumHeartVein").Value;
                data = new DrawData(veinTexture, heartPos, new Rectangle(0, 22 * horizontalFrame, 26, 22), Color.White, 0, heartTexture.Size() * .5f, 1f, 0, 0);
                data.shader = drawPlayer.dye[3].dye;
                drawInfo.DrawDataCache.Add(data);
            }
        }
    }
}