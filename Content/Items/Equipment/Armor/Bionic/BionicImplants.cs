using QwertyMod.Content.Buffs;
using QwertyMod.Content.Items.Weapon.Morphs;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Equipment.Armor.Bionic
{
    [AutoloadEquip(EquipType.Body)]
    public class BionicImplants : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bionic Implants");
            Tooltip.SetDefault("30% reduced cooldown on morphs\nYour offhand is replaced with a laser cannon!");
        }

        public override void SetDefaults()
        {
            Item.rare = 5;
            Item.value = Item.sellPrice(gold: 5);
            Item.defense = 7;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<ShapeShifterPlayer>().coolDownDuration *= 0.7f;
            player.GetModPlayer<BionicEffects>().ArmCannon = true;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == ModContent.ItemType<BionicEye>() && legs.type == ModContent.ItemType<BionicLimbs>();
        }

        public override void UpdateArmorSet(Player player)
        {
            String s = "Please go to conrols and bind the 'Yet another special ability key'";
            foreach (string key in QwertyMod.YetAnotherSpecialAbility.GetAssignedKeys()) //get's the string of the hotkey's name
            {
                s = "Press the " + key + " key to activate 'overdrive' significantly increaasing your arm cannons fire rate and movement speed.";
            }
            player.setBonus = s;
            player.GetModPlayer<BionicEffects>().setBonus = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.SoulofMight, 8)
                .AddIngredient(ItemID.HallowedBar, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    public class BionicEffects : ModPlayer
    {
        public bool ArmCannon = false;
        public bool setBonus = false;
        int overdriveTime = 0;
        int armCannonCountdown = 0;
        float? aimDirection = null;
        int setDirection = 0;
        public override void ResetEffects()
        {
            ArmCannon = false;
            setBonus = false;
        }
        public override void PostUpdate()
        {
            if (armCannonCountdown <= 0 && ArmCannon)
            {
                bool allowFlip = true;
                NPC target = null;
                if (Player.itemAnimation > 0)
                {
                    allowFlip = false;
                }
                if (QwertyMethods.ClosestNPC(ref target, 1000, Player.MountedCenter, false, -1, delegate (NPC possibleTarget)
                 {
                     return allowFlip || Math.Sign(possibleTarget.Center.X - Player.Center.X) == Player.direction;
                 }))
                {
                    Player.direction = Math.Sign(target.Center.X - Player.Center.X);
                    float shotSpeed = 12;
                    float offest = Player.GetBackHandPosition(Player.CompositeArmStretchAmount.Full, 0).Length();
                    aimDirection = QwertyMethods.PredictiveAim(Player.MountedCenter, shotSpeed * 2, target.Center, target.velocity);

                    if (!float.IsNaN((float)aimDirection))
                    {
                        armCannonCountdown = 30;
                        if (overdriveTime > 60 * 50)
                        {
                            armCannonCountdown = 5;
                        }
                        setDirection = Player.direction;
                        Player.SetCompositeArmBack(enabled: true, Player.CompositeArmStretchAmount.Full, (float)aimDirection - (float)Math.PI / 2);
                        Projectile.NewProjectile(new EntitySource_Misc(""), Player.MountedCenter, QwertyMethods.PolarVector(shotSpeed, (float)aimDirection), ModContent.ProjectileType<ArmCannonLaser>(), (int)(30f * Player.GetDamage(DamageClass.Generic).Multiplicative), 0, Player.whoAmI);
                        SoundEngine.PlaySound(SoundID.Item157, Player.MountedCenter);
                    }

                }
            }
            else if (ArmCannon)
            {
                armCannonCountdown--;
                if (setDirection != 0)
                {
                    if (Player.itemAnimation == 0)
                    {
                        Player.direction = setDirection;
                    }
                    else if (Player.direction != setDirection)
                    {
                        aimDirection = null;
                    }
                }
                if (aimDirection != null)
                {
                    Player.SetCompositeArmBack(enabled: true, Player.CompositeArmStretchAmount.Full, (float)aimDirection - (float)Math.PI / 2);
                }
            }
            overdriveTime--;
            if (overdriveTime > 60 * 50)
            {
                Player.AddBuff(ModContent.BuffType<Overrdrive>(), overdriveTime - 60 * 50);
            }
            else if (overdriveTime > 0)
            {
                Player.AddBuff(ModContent.BuffType<OverrdriveCooldown>(), overdriveTime);
            }
        }
        public override void PostUpdateEquips()
        {
            if (overdriveTime > 60 * 50)
            {
                Player.moveSpeed *= 1.5f;
                Player.accRunSpeed *= 1.5f;
                Player.runAcceleration += 1.5f;
            }
        }


        public override void ProcessTriggers(TriggersSet triggersSet) //runs hotkey effects
        {
            if (QwertyMod.YetAnotherSpecialAbility.JustPressed) //hotkey is pressed
            {
                if (setBonus && overdriveTime <= 0)
                {
                    overdriveTime = 60 * 60;
                }
            }
        }
    }
    public class ArmCannonLaser : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.GreenLaser);
            Projectile.DamageType = DamageClass.Generic;
            Projectile.penetrate = 1;
            AIType = ProjectileID.GreenLaser;
        }
    }
}
