using QwertyMod.Common;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertyMod.Content.Items.Equipment.Armor.Shaman
{
    [AutoloadEquip(EquipType.Head)]
    public class ShamanHead : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
        }

        public override void SetDefaults()
        {
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Blue;

            Item.width = 22;
            Item.height = 16;
            Item.defense = 5;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Summon) += .06f;
            player.GetCritChance(DamageClass.Melee) += 6;
        }


        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<ShamanBody>() && legs.type == ModContent.ItemType<ShamanLegs>();
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
                s = "Press the " + key + " key to to call war spirits which temporarily make minions attack much faster and you gain 40% melee speed! \n 60 second cooldown";
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
                Player.GetAttackSpeed(DamageClass.Melee) += .4f;
                hasteTime--;
                Player.GetModPlayer<MinionSpeedStats>().minionSpeed += 1f;
            }
        }

        public override void ProcessTriggers(TriggersSet triggersSet) //runs hotkey effects
        {
            if (QwertyMod.YetAnotherSpecialAbility.JustPressed) //hotkey is pressed
            {
                if (setBonus && !Player.HasBuff(ModContent.BuffType<SpiritCallCooldown>()))
                {
                    hasteTime = 600;
                    Player.AddBuff(ModContent.BuffType<SpiritCallCooldown>(), 60 * 60);
                }
            }
        }
    }
}