using Microsoft.Xna.Framework;
using QwertyMod.Common;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Equipment.Armor.Shaman
{
    [AutoloadEquip(EquipType.Head)]
    public class ShamanHead : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shaman Skull");
            Tooltip.SetDefault("6% increased minion damage and melee critical strike chance");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = 1;

            Item.width = 22;
            Item.height = 14;
            Item.defense = 5;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Summon) += .06f;
            player.GetCritChance(DamageClass.Melee) += 6;
        }

        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawAltHair = true;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemType<ShamanBody>() && legs.type == ItemType<ShamanLegs>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.JungleSpores, 4)
                .AddIngredient(ItemID.Bone, 25)
                .AddTile(TileID.Anvils)
                .Register();
        }
        public override void UpdateArmorSet(Player player)
        {
            string s = "Please go to conrols and bind the 'Yet another special ability key'";
            foreach (string key in QwertyMod.YetAnotherSpecialAbility.GetAssignedKeys()) //get's the string of the hotkey's name
            {
                s = "Press the "+ key + " key to to call war spirits which temporarily make minions attack much faster and you gain 40% melee speed! \n 60 second cooldown";
            }
            player.setBonus = s;
            player.GetModPlayer<ShamanHeadEffects>().setBonus = true;
        }
    }

    public class ShamanHeadEffects : ModPlayer
    {
        public bool setBonus = false;
        public int hasteTime = 0;

        public override void ResetEffects()
        {
            setBonus = false;
            //hasteTime = 0;
        }

        public override void UpdateEquips()
        {
            if (hasteTime > 0)
            {
                Player.meleeSpeed += .4f;
                hasteTime--;
                Player.GetModPlayer<MinionSpeedStats>().minionSpeed += 1f;
            }
        }

        public override void ProcessTriggers(TriggersSet triggersSet) //runs hotkey effects
        {
            if (QwertyMod.YetAnotherSpecialAbility.JustPressed) //hotkey is pressed
            {
                if (setBonus && !Player.HasBuff(BuffType<SpiritCallCooldown>()))
                {
                    hasteTime = 600;
                    Player.AddBuff(BuffType<SpiritCallCooldown>(), 60 * 60);
                }
            }
        }
    }
}