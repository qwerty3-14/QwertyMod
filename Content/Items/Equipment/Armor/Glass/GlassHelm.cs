using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Content.Buffs;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ID.ArmorIDs;
using static Terraria.ModLoader.ModContent;


namespace QwertyMod.Content.Items.Equipment.Armor.Glass
{
    [AutoloadEquip(EquipType.Head)]
    public class GlassHelm : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Glass Helm");
            Tooltip.SetDefault("A glass prism orbits you zapping enemies");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Head.Sets.DrawHatHair[Item.headSlot] = true;
        }

        public override void SetDefaults()
        {
            Item.value = 10000;
            Item.rare = 1;
            Item.width = 22;
            Item.height = 14;
            Item.defense = 3;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<HelmEffects>().helmEffect = true;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemType<GlassAbsorber>() && legs.type == ItemType<GlassLimbguards>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Ranged attacks Inflict 'Arcanely tuned' \nMagic attacks chase enemies inflicted with 'Arcanely tuned'";
            player.GetModPlayer<HelmEffects>().setBonus = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.Glass, 15)
                .AddIngredient(ItemID.SilverBar, 4)
                .AddTile(TileID.Anvils)
                .Register();
            CreateRecipe(1).AddIngredient(ItemID.Glass, 15)
                .AddIngredient(ItemID.TungstenBar, 4)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class HelmEffects : ModPlayer
    {
        public float PrismTrigonometryCounterOfAwsomenessWowThisIsAVeryLongVariableName;
        public bool helmEffect = false;
        public int prismDazzleCounter = 90;
        public bool setBonus = false;

        public override void ResetEffects()
        {
            setBonus = false;
            if (!helmEffect)
            {
                prismDazzleCounter = 90;
            }
            helmEffect = false;
        }

        public override void PreUpdate()
        {
            if (helmEffect)
            {
                PrismTrigonometryCounterOfAwsomenessWowThisIsAVeryLongVariableName += (float)MathHelper.Pi / 30;
                prismDazzleCounter--;
                NPC target = new NPC();
                Vector2 prismCenter = Player.Center + new Vector2((float)Math.Sin(PrismTrigonometryCounterOfAwsomenessWowThisIsAVeryLongVariableName) * 40f, 0);
                if (QwertyMethods.ClosestNPC(ref target, 4000, prismCenter) && prismDazzleCounter <= 0)
                {
                    Projectile.NewProjectile(new EntitySource_Misc(""), prismCenter, QwertyMethods.PolarVector(1, (target.Center - prismCenter).ToRotation()), ProjectileType<PrismDazzle>(), (int)(12f * Player.GetDamage(DamageClass.Magic).Multiplicative), 0f, Player.whoAmI);
                    prismDazzleCounter = 90;
                }
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (proj.CountsAsClass(DamageClass.Ranged) && setBonus)
            {
                target.AddBuff(BuffType<ArcanelyTuned>(), 360);
            }
        }
    }
    public class PrismDraw : PlayerDrawLayer
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return true;
        }
        public override Position GetDefaultPosition() =>  new Multiple()
        {
            { new Between(PlayerDrawLayers.CaptureTheGem, PlayerDrawLayers.BeetleBuff), drawInfo => Math.Cos(drawInfo.drawPlayer.GetModPlayer<HelmEffects>().PrismTrigonometryCounterOfAwsomenessWowThisIsAVeryLongVariableName) <= 0 },
            { new Between(PlayerDrawLayers.JimsCloak, PlayerDrawLayers.MountBack), drawInfo => Math.Cos(drawInfo.drawPlayer.GetModPlayer<HelmEffects>().PrismTrigonometryCounterOfAwsomenessWowThisIsAVeryLongVariableName) > 0 }
        };
        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("QwertyMod");
            Texture2D texture = Request<Texture2D>("QwertyMod/Content/Items/Equipment/Armor/Glass/GlassPrism").Value;
            Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)drawInfo.Position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawInfo.Position.Y + (double)drawPlayer.height * 0.5) / 16, Microsoft.Xna.Framework.Color.White), 0f);
            
            if (drawPlayer.GetModPlayer<HelmEffects>().helmEffect)
            {
                Vector2 Center = drawInfo.Position + new Vector2(drawPlayer.width / 2, drawPlayer.height / 2) - Main.screenPosition;
                Center.X += (float)Math.Sin(drawPlayer.GetModPlayer<HelmEffects>().PrismTrigonometryCounterOfAwsomenessWowThisIsAVeryLongVariableName) * 40;
                DrawData data = new DrawData(texture, Center, texture.Frame(), color12, 0f, texture.Size() * .5f, 1f, drawInfo.playerEffect, 0);
                data.shader = (int)drawPlayer.dye[3].dye;
                drawInfo.DrawDataCache.Add(data);
            }
        }
    }

    public class PrismDazzle : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.extraUpdates = 99;
            Projectile.timeLeft = 1200;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 2;
            Projectile.usesLocalNPCImmunity = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }

        public override void AI()
        {
            if (Main.rand.Next(8) == 0)
            {
                Dust d = Main.dust[Dust.NewDust(Projectile.Center, 0, 0, DustType<GlassSmoke>())];
                d.velocity *= .1f;
                d.noGravity = true;
                d.position = Projectile.Center;
                d.shader = GameShaders.Armor.GetSecondaryShader(Main.player[Projectile.owner].dye[3].dye, Main.player[Projectile.owner]);
            }
            for (int k = 0; k < 200; k++)
            {
                if (!Collision.CheckAABBvAABBCollision(Projectile.position, Projectile.Size, Main.npc[k].position, Main.npc[k].Size))
                {
                    Projectile.localNPCImmunity[k] = 0;
                }
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!target.boss && Main.rand.Next(20) == 0)
            {
                target.AddBuff(BuffType<Stunned>(), 120);
            }
            Projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[Projectile.owner] = 0;
        }
    }

    public class ArcanelyTunedHoming : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        private NPC target;
        private bool foundTarget = false;

        public override void PostAI(Projectile projectile)
        {
            float maxDistance = 10000;
            foundTarget = false;
            if (projectile.CountsAsClass(DamageClass.Magic) && projectile.friendly)
            {
                for (int k = 0; k < 200; k++)
                {
                    NPC possibleTarget = Main.npc[k];
                    float distance = (possibleTarget.Center - projectile.Center).Length();
                    if (distance < maxDistance && possibleTarget.HasBuff(BuffType<ArcanelyTuned>()) && possibleTarget.active && !possibleTarget.dontTakeDamage && !possibleTarget.friendly && possibleTarget.lifeMax > 5 && (Collision.CanHit(projectile.Center, 0, 0, possibleTarget.Center, 0, 0) || !projectile.tileCollide))
                    {
                        target = Main.npc[k];
                        foundTarget = true;

                        maxDistance = (target.Center - projectile.Center).Length();
                    }
                }
                if (foundTarget)
                {
                    float aimToward = (target.Center - projectile.Center).ToRotation();
                    float dir = projectile.velocity.ToRotation();
                    dir = QwertyMethods.SlowRotation(dir, aimToward, 2);
                    projectile.velocity = QwertyMethods.PolarVector(projectile.velocity.Length(), dir);
                }
            }
        }
    }
}