using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.GameInput;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Equipment.Accessories.Expert.Sheath
{
    public class ImperiousSheath : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Imperious's Sheath");

            Tooltip.SetDefault("After dealing 10,000 damage you can summon Imperious to fight for you breifly with the " + "'" + "Special Ability" + "' key" + "\nCan be changed in controls");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void PostUpdate()
        {
        }

        public override void SetDefaults()
        {
            Item.value = 200000;
            Item.rare = 7;
            Item.expert = true;

            Item.width = 34;
            Item.height = 32;
            Item.damage = 400;
            Item.DamageType = DamageClass.Summon;
            Item.knockBack = 8f;
            Item.accessory = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<ImperiousEffect>().effect = true;
        }

        //this changes the tooltip based on what the hotkey is configured to
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            String s = "Please go to conrols and bind the 'Yet another special ability key'";
            foreach (String key in QwertyMod.YetAnotherSpecialAbility.GetAssignedKeys()) //get's the string of the hotkey's name
            {
                s = "After dealing 10,000 damage you can summon Imperious to fight for you breifly with the " + "'" + key + "' key";
            }
            foreach (TooltipLine line in tooltips) //runs through all tooltip lines
            {
                if (line.Mod == "Terraria" && line.Name == "Tooltip0") //this checks if it's the line we're interested in
                {
                    line.Text = s;//change tooltip
                }
            }
        }
    }

    public class ImperiousEffect : ModPlayer
    {
        public bool effect = false; //does the player get this effect
        public int damageTally; //used to count as damage is dealt
        public int damageTallyMax = 10000;

        public override void ResetEffects() //used to reset if the player unequips the accesory
        {
            effect = false;
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit) //runs when an npc is hit by the player's projectile
        {
            if (proj.owner == Player.whoAmI && effect && !target.immortal && proj.type != ProjectileType<ImperiousP>()) //check if vallid npc and effect is active
            {
                damageTally += damage; //count up
            }
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit) //runs when an npc is hit by an item (sword blade)
        {
            if (effect && !target.immortal)  //check if vallid npc  and effect is active
            {
                damageTally += damage; //count up
            }
        }

        public override void PreUpdate() //runs every frame
        {
            if (damageTally > damageTallyMax)
            {
                damageTally = damageTallyMax; //limits the tally
            }
        }

        public override void ProcessTriggers(TriggersSet triggersSet) //runs hotkey effects
        {
            if (QwertyMod.YetAnotherSpecialAbility.JustPressed) //hotkey is pressed
            {
                if (effect && damageTally >= damageTallyMax)
                {
                        Projectile.NewProjectile(new EntitySource_Misc(""), Player.Center, Vector2.Zero, ProjectileType<ImperiousP>(), (int)(500f * Player.GetDamage(DamageClass.Summon).Multiplicative), 8f * Player.GetKnockback(DamageClass.Summon).Multiplicative, Player.whoAmI); //summons Imperious to fight!
                        damageTally = 0; //resets the tally
                    
                }
            }
        }

        
    }

    public class ImperiousP : ModProjectile
    {
        private int rotateDirection = 1;

        public override void SetDefaults()
        {
            Projectile.width = 84;
            Projectile.height = 94;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 180;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.rotation = (float)Math.PI;
            Player player = Main.player[Projectile.owner];
            rotateDirection = player.direction;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.localNPCImmunity[target.whoAmI] = Projectile.localNPCHitCooldown; //local immunity
            target.immune[Projectile.owner] = 0;
        }

        private float bladeWidth = 74;
        private float HiltLength = 94;
        private float HiltWidth = 84;
        private Vector2 BladeStart;
        private Vector2 BladeTip;
        private float BladeLength = 300;

        public override void AI()
        {
            BladeStart = Projectile.Center + QwertyMethods.PolarVector(HiltLength / 2, Projectile.rotation + (float)Math.PI / 2);
            BladeTip = Projectile.Center + QwertyMethods.PolarVector((HiltLength / 2) + BladeLength, Projectile.rotation + (float)Math.PI / 2);
            Player player = Main.player[Projectile.owner];
            Projectile.Center = player.Center;
            Projectile.rotation += (float)Math.PI / 15 * rotateDirection;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) //custom collision
        {
            float col = 0;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), BladeStart, BladeTip, bladeWidth, ref col);
        }
    }
}