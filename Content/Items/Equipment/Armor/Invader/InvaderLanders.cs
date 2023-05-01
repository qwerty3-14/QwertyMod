using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.GameInput;
using Microsoft.Xna.Framework;
using QwertyMod.Content.Items.MiscMaterials;
using Terraria.ID;

namespace QwertyMod.Content.Items.Equipment.Armor.Invader
{
    [AutoloadEquip(EquipType.Legs)]
    public class InvaderLanders : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.value = QwertyMod.InvaderGearValue;
            Item.rare = ItemRarityID.Yellow;
            Item.width = 22;
            Item.height = 18;
            Item.defense = 15;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) += 0.13f;
            player.GetAttackSpeed(DamageClass.Melee) += 0.15f;
            player.moveSpeed += 0.15f;
            player.noFallDmg = true;
        }
        public override void UpdateArmorSet(Player player)
        {
            string s = "Please go to conrols and bind the 'Yet another special ability key'";
            foreach (string key in QwertyMod.YetAnotherSpecialAbility.GetAssignedKeys()) //get's the string of the hotkey's name
            {
                s = "Press the " + key + " and a direction to redirect all your velocity in that direction.";
            }
            player.setBonus = s;
            player.GetModPlayer<InvaderArmor>().setBonus = true;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<InvaderProtector>() && head.type == ModContent.ItemType<InvaderPercepticals>();
        }
        public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
        {
            if (male) equipSlot = QwertyMod.invaderLanderMale;
            if (!male) equipSlot = QwertyMod.invaderLanderFemale;
        }
        
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<InvaderPlating>(), 30)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    public class InvaderArmor : ModPlayer
    {
        public bool setBonus = false;
        public override void ResetEffects()
        {
            setBonus = false;
        }
        
        public override void ProcessTriggers(TriggersSet triggersSet) //runs hotkey effects
        {
            if (QwertyMod.YetAnotherSpecialAbility.JustPressed) //hotkey is pressed
            {
                if (setBonus)
                {
                    if(Player.controlUp)
                    {
                        Player.velocity = Vector2.UnitY * -1 * Player.velocity.Length();
                    }
                    else if(Player.controlLeft)
                    {
                        Player.velocity = Vector2.UnitX * -1 * Player.velocity.Length();
                    }
                    else if(Player.controlRight)
                    {
                        Player.velocity = Vector2.UnitX * 1 * Player.velocity.Length();
                    }
                    else if(Player.controlDown)
                    {
                        Player.velocity = Vector2.UnitY * 1 * Player.velocity.Length();
                    }
                    else
                    {
                        Player.velocity = Vector2.Zero;
                    }
                }
            }
        }
    }

}