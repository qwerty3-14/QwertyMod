using Microsoft.Xna.Framework;
using QwertyMod.Content.NPCs.Bosses.BladeBoss;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items.Equipment.Accessories.Expert.Sheath
{
    public class ImperiousSheath : ModItem
    {

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void PostUpdate()
        {
        }

        public override void SetDefaults()
        {
            Item.value = 200000;
            Item.rare = ItemRarityID.Lime;
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
            player.GetModPlayer<ImperiousEffect>().effect++;
        }

        //this changes the tooltip based on what the hotkey is configured to
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            String s = Language.GetTextValue(Mod.GetLocalizationKey("CustomTooltipBindKey"));
            foreach (String key in QwertyMod.YetAnotherSpecialAbility.GetAssignedKeys()) //get's the string of the hotkey's name
            {
                s = Language.GetTextValue(Mod.GetLocalizationKey("CustomTooltipSheathStart")) + key + Language.GetTextValue(Mod.GetLocalizationKey("CustomTooltipSheathEnd"));
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
        public int effect = 0; //does the player get this effect
        public int damageTally; //used to count as damage is dealt
        public int damageTallyMax = 10000;

        public override void ResetEffects() //used to reset if the player unequips the accesory
        {
            effect = 0;
        }
        bool ImperiousActive()
        {
            for(int p = 0; p < 1000; p++)
            {
                if(Main.projectile[p].active && Main.projectile[p].type == ModContent.ProjectileType<ImperiousP>())
                {
                    return true;
                }
            }
            if(NPC.AnyNPCs(ModContent.NPCType<Imperious>()))
            {
                return true;
            }
            return false;
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) //runs when an npc is hit by the player's projectile
        {
            if(ImperiousActive()) { return; }
            if (proj.owner == Player.whoAmI && effect > 0 && !target.immortal && proj.type != ModContent.ProjectileType<ImperiousP>()) //check if vallid npc and effect is active
            {
                damageTally += damageDone * effect; //count up
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) //runs when an npc is hit by an item (sword blade)
        {
            if(ImperiousActive()) { return; }
            if (effect > 0 && !target.immortal)  //check if vallid npc  and effect is active
            {
                damageTally += damageDone * effect; //count up
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
            if(ImperiousActive()) { return; }
            if (QwertyMod.YetAnotherSpecialAbility.JustPressed) //hotkey is pressed
            {
                if (effect > 0 && damageTally >= damageTallyMax)
                {
                    Projectile.NewProjectile(new EntitySource_Misc("Accesory_ImperiousSheath"), Player.Center, Vector2.Zero, ModContent.ProjectileType<ImperiousP>(), (int)(500f * Player.GetDamage(DamageClass.Summon).Multiplicative), 8f * Player.GetKnockback(DamageClass.Summon).Multiplicative, Player.whoAmI); //summons Imperious to fight!
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
            Projectile.rotation = MathF.PI;
            Player player = Main.player[Projectile.owner];
            rotateDirection = player.direction;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.localNPCImmunity[target.whoAmI] = Projectile.localNPCHitCooldown; //local immunity
            target.immune[Projectile.owner] = 0;
        }

        private float bladeWidth = 74;
        private float HiltLength = 94;
        private Vector2 BladeStart;
        private Vector2 BladeTip;
        private float BladeLength = 300;

        public override void AI()
        {
            BladeStart = Projectile.Center + QwertyMethods.PolarVector(HiltLength / 2, Projectile.rotation + MathF.PI / 2);
            BladeTip = Projectile.Center + QwertyMethods.PolarVector((HiltLength / 2) + BladeLength, Projectile.rotation + MathF.PI / 2);
            Player player = Main.player[Projectile.owner];
            Projectile.Center = player.Center;
            Projectile.rotation += MathF.PI / 15 * rotateDirection;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) //custom collision
        {
            float col = 0;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), BladeStart, BladeTip, bladeWidth, ref col);
        }
    }
}