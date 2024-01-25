using Microsoft.Xna.Framework;
using QwertyMod.Content.Dusts;
using QwertyMod.Content.NPCs.Bosses.OLORD;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;



namespace QwertyMod.Content.Items.Weapon.Magic.BlackHole
{
    public class BlackHoleStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Item.staff[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.damage = 80;
            Item.mana = ModLoader.HasMod("TRAEProject") ? 50 : 14;
            Item.width = 100;
            Item.height = 114;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 0f;
            Item.value = 750000;
            Item.rare = ItemRarityID.Red;
            Item.UseSound = SoundID.Item44;
            Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<BlackHolePlayer>();
            Item.DamageType = DamageClass.Magic;
            Item.channel = true;
        }

        public Projectile BlackHole;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 SPos = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);
            position = SPos;
            velocity = Vector2.Zero;
            for (int l = 0; l < Main.projectile.Length; l++)
            {                                                                  //this make so you can only spawn one of this projectile at the time,
                Projectile proj = Main.projectile[l];
                if (proj.active && proj.type == Item.shoot && proj.owner == player.whoAmI)
                {
                    return false;
                }
            }
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }
    }

    public class BlackHolePlayer : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 200;
            Projectile.height = 200;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 3;
        }

        public float horiSpeed;
        public float vertSpeed;
        public float direction;
        public float pullSpeed = .5f;
        public float dustSpeed = 20f;
        public NPC mass;
        public Projectile proj;
        public int frameTimer;
        public Dust dust;
        public Item item;
        public int manaTimer;

        public override void AI()
        {
            pullSpeed = Projectile.damage / 100f;
            Projectile.velocity = new Vector2(0, 0);
            Projectile.scale = Projectile.damage / 40f;

            Player player = Main.player[Projectile.owner];
            player.itemAnimation = 2;
            if (!player.channel)
            {
                Projectile.Kill();
            }
            else
            {
                manaTimer++;
                if (manaTimer % 15 == 0)
                {
                    if (player.statMana > player.inventory[player.selectedItem].mana)
                    {
                        player.statMana -= player.inventory[player.selectedItem].mana;
                        Projectile.timeLeft = 60;
                    }
                    else
                    {
                        Projectile.Kill();
                    }
                }
                else
                {
                }
            }

            direction = (Projectile.Center - player.Center).ToRotation();
            horiSpeed = MathF.Cos(direction) * pullSpeed / 2;
            vertSpeed = MathF.Sin(direction) * pullSpeed / 2;
            player.velocity += new Vector2(horiSpeed, vertSpeed);
            for (int d = 0; d < (int)(80 * Projectile.scale); d++)
            {
                float theta = Main.rand.NextFloat(-MathF.PI, MathF.PI);
                Dust dust = Dust.NewDustPerfect(Projectile.Center + QwertyMethods.PolarVector(Main.rand.NextFloat(10 * Projectile.scale, 200 * Projectile.scale), theta), ModContent.DustType<BlackHoleMatter>(), QwertyMethods.PolarVector(6 * Projectile.scale, theta + MathF.PI / 2));
                dust.scale = 1f;
            }

            for (int i = 0; i < Main.dust.Length; i++)
            {
                dust = Main.dust[i];
                if (!dust.noGravity)
                {
                    direction = (Projectile.Center - dust.position).ToRotation();
                    horiSpeed = MathF.Cos(direction) * pullSpeed * 5;
                    vertSpeed = MathF.Sin(direction) * pullSpeed * 5;
                    dust.velocity += new Vector2(horiSpeed, vertSpeed);
                }
                if (dust.type == ModContent.DustType<BlackHoleMatter>())
                {
                    direction = (Projectile.Center - dust.position).ToRotation();
                    dust.velocity += QwertyMethods.PolarVector(.8f, direction);
                    if ((dust.position - Projectile.Center).Length() < 10 * Projectile.scale)
                    {
                        dust.scale = 0f;
                    }
                    else
                    {
                        dust.scale = .35f;
                    }
                }
            }
            for (int i = 0; i < 200; i++)
            {
                mass = Main.npc[i];
                if (!mass.boss && mass.active && mass.knockBackResist != 0f)
                {
                    direction = (Projectile.Center - mass.Center).ToRotation();
                    horiSpeed = MathF.Cos(direction) * pullSpeed;
                    vertSpeed = MathF.Sin(direction) * pullSpeed;
                    mass.velocity += new Vector2(horiSpeed, vertSpeed);
                    for (int g = 0; g < 1; g++)
                    {
                        int dust = Dust.NewDust(mass.position, mass.width, mass.height, ModContent.DustType<B4PDust>(), horiSpeed * dustSpeed, vertSpeed * dustSpeed);
                    }
                }
            }
            for (int i = 0; i < 200; i++)
            {
                item = Main.item[i];
                if (item.position != new Vector2(0, 0))
                {
                    //This part of the code puts the items the black hole grabs in the player inventory. It's partialy based on the lugage from The Luggage mod
                    if (item.active && item.noGrabDelay == 0 && ItemLoader.CanPickup(item, player))
                    {
                        int num = Player.defaultItemGrabRange;
                        if (new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height).Intersects(new Rectangle((int)item.position.X, (int)item.position.Y, item.width, item.height)))
                        {
                            if (Projectile.owner == Main.myPlayer && (player.inventory[player.selectedItem].type != ItemID.None || player.itemAnimation <= 0))
                            {
                                if (ItemID.Sets.NebulaPickup[item.type])
                                {
                                    item.velocity = new Vector2(0, 0);
                                    item.position = player.Center;
                                }
                                if (item.type == ItemID.Heart || item.type == ItemID.CandyApple || item.type == ItemID.CandyCane)
                                {
                                    item.velocity = new Vector2(0, 0);
                                    item.position = player.Center;
                                }
                                else if (item.type == ItemID.Star || item.type == ItemID.SoulCake || item.type == ItemID.SugarPlum)
                                {
                                    item.velocity = new Vector2(0, 0);
                                    item.position = player.Center;
                                }
                                else
                                {
                                    for (int g = 0; g < 58; g++)
                                    {
                                        if (!player.inventory[g].active)
                                        {
                                            item.velocity = new Vector2(0, 0);
                                            item.position = player.Center;
                                        }
                                    }

                                    //item = player.GetItem(Projectile.owner, item, false, false);
                                }
                            }
                        }
                    }
                    /////////////////////
                    direction = (Projectile.Center - item.Center).ToRotation();
                    horiSpeed = MathF.Cos(direction) * pullSpeed;
                    vertSpeed = MathF.Sin(direction) * pullSpeed;
                    item.velocity += new Vector2(horiSpeed, vertSpeed);
                    for (int g = 0; g < 1; g++)
                    {
                        int dust = Dust.NewDust(item.position, item.width, item.height, ModContent.DustType<B4PDust>(), horiSpeed * dustSpeed, vertSpeed * dustSpeed);
                    }
                }
            }
            for (int i = 0; i < 1000; i++)
            {
                proj = Main.projectile[i];
                if (proj.active && proj.type != ModContent.ProjectileType<BlackHolePlayer>() && proj.type != ModContent.ProjectileType<SideLaser>())
                {
                    direction = (Projectile.Center - proj.Center).ToRotation();
                    horiSpeed = MathF.Cos(direction) * pullSpeed;
                    vertSpeed = MathF.Sin(direction) * pullSpeed;
                    proj.velocity += new Vector2(horiSpeed, vertSpeed);
                    for (int g = 0; g < 1; g++)
                    {
                        int dust = Dust.NewDust(proj.position, proj.width, proj.height, ModContent.DustType<B4PDust>(), horiSpeed * dustSpeed, vertSpeed * dustSpeed);
                    }
                }
            }
        }
    }
}