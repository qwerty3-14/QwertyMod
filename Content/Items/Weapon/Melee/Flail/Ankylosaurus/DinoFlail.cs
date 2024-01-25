using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Content.Buffs;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertyMod.Content.Items.Weapon.Melee.Flail.Ankylosaurus
{
    public class DinoFlail : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 60;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.noMelee = true;
            Item.scale = 1f;
            Item.noUseGraphic = true;
            Item.width = 30;
            Item.height = 32;
            Item.useTime = 45;
            Item.useAnimation = 45;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 3;
            Item.rare = ItemRarityID.LightPurple;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = false;
            Item.channel = true;
            Item.shoot = ModContent.ProjectileType<AnkylosaurusTail>();
            Item.shootSpeed = 15f;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (line.Mod == "Terraria" && line.Name == "Damage") //this checks if it's the line we're interested in
                {
                    string[] strings = line.Text.Split(' ');
                    int dmg = int.Parse(strings[0]);
                    dmg *= 2;
                    line.Text = dmg + "";//change tooltip
                    for (int i = 1; i < strings.Length; i++)
                    {
                        line.Text += " " + strings[i];
                    }
                }
            }
        }
    }

    public class AnkylosaurusTail : Flail
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 34;
            //Projectile.aiStyle = 15;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            //Projectile.extraUpdates = 1;
            //throwTime = 13;
            //throwSpeed = 22f;
        }

        public override void SetStats(ref int throwTime, ref float throwSpeed, ref float recoverDistance, ref float recoverDistance2, ref int attackCooldown)
        {
            throwTime = 13;
            throwSpeed = 22f;
            recoverDistance = 22f;
            recoverDistance2 = 26f;
            attackCooldown = 15;
        }
        public override void ExtraAI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.rotation = (player.Center - Projectile.Center).ToRotation() + MathF.PI / 2;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!target.boss && hit.Crit)
            {
                target.AddBuff(ModContent.BuffType<Stunned>(), 240);
            }
        }



        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 playerCenter = Main.player[Projectile.owner].MountedCenter;
            Vector2 center = Projectile.Center;
            Vector2 distToProj = playerCenter - Projectile.Center;
            float projRotation = distToProj.ToRotation() - 1.57f;
            float distance = distToProj.Length();
            for (int i = 0; i < 1000; i++)
            {
                if (distance > 4f && !float.IsNaN(distance))
                {
                    distToProj.Normalize();
                    distToProj *= 8f;
                    center += distToProj;
                    distToProj = playerCenter - center;
                    distance = distToProj.Length();
                    Color drawColor = lightColor;

                    //Draw chain
                    Main.EntitySpriteDraw(ModContent.Request<Texture2D>("QwertyMod/Content/Items/Weapon/Melee/Flail/Ankylosaurus/DinoFlailChain").Value, new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
                        new Rectangle(0, 0, 18, 12), drawColor, projRotation,
                        new Vector2(18 * 0.5f, 12 * 0.5f), 1f, SpriteEffects.None, 0);
                }
            }

            return true;
        }
    }
}