using Microsoft.Xna.Framework;
using QwertyMod.Content.Dusts;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Weapon.Ranged.Bow.HolyExiler
{
    public class HolyExiler : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 19;
            Item.DamageType = DamageClass.Ranged;

            Item.useTime = 34;
            Item.useAnimation = 34;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 2f;
            Item.value = 50000;
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item5;

            Item.width = 18;
            Item.height = 46;

            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.useAmmo = AmmoID.Arrow;
            Item.shootSpeed = 12f;
            Item.noMelee = true;
            Item.autoReuse = true;
        }

        public Projectile arrow;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            arrow = Main.projectile[Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI)];
            arrow.GetGlobalProjectile<ArrowWarping>().warpedArrow = true;

            if(Main.netMode != NetmodeID.SinglePlayer)
            {
                ModPacket packet = Mod.GetPacket();
                packet.Write((byte)ModMessageType.AmmoEnchantArrowWarping);
                packet.Write(arrow.identity);
                packet.Send();
            }
            return false;
        }
    }

    public class ArrowWarping : GlobalProjectile
    {
        public bool warpedArrow;

        public override bool InstancePerEntity => true;

        private NPC target;
        private NPC possibleTarget;
        private List<int> targets = new List<int>();
        private float maxDistance = 300;
        private Projectile portal1;
        private float teleportDistance = 80;
        private int teleportTries = 100;

        public override void OnKill(Projectile projectile, int timeLeft)
        {
            if (warpedArrow)
            {
                for (int n = 0; n < 200; n++)
                {
                    possibleTarget = Main.npc[n];
                    float distance = (possibleTarget.Center - projectile.Center).Length();
                    if (distance < maxDistance && possibleTarget.active && !possibleTarget.dontTakeDamage && !possibleTarget.friendly && possibleTarget.lifeMax > 5 && !possibleTarget.immortal)
                    {
                        targets.Add(n); //save valid possibletarget's id to the targets list
                    }
                }
                if (targets.Count > 0) //only run if a vallid target has been found
                {
                    for (int i = 0; i < 2; i++)
                    {
                        target = Main.npc[targets[Main.rand.Next(targets.Count)]]; // pick a random value in the targets list and use that to pick a target
                        for (int c = 0; c < teleportTries; c++)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient) // don't run on client
                            {
                                //Use the NPC.ai[0] variable as it's easy to sync in multiplayer
                                //server sync whenever randomizing so the client and server won't disagree
                                projectile.ai[0] = Main.rand.NextFloat(-MathF.PI, MathF.PI); // sets the NPC.ai[0] variable to a random radian angle
                                projectile.netUpdate = true; // update the client's NPC.ai[0] variable  to be equal to the server's
                            }
                            Vector2 teleTo = new Vector2(target.Center.X + MathF.Cos(projectile.ai[0]) * teleportDistance, target.Center.Y + MathF.Sin(projectile.ai[0]) * teleportDistance);
                            if (Collision.CanHit(new Vector2(teleTo.X - projectile.width / 2, teleTo.Y - projectile.height / 2), projectile.width, projectile.height, target.position, target.width, target.height))// checks if there are no tiles between player and potential teleport spot
                            {
                                portal1 = Main.projectile[Projectile.NewProjectile(projectile.GetSource_FromThis(), teleTo, Vector2.Zero, ProjectileType<ArrowPortal>(), projectile.damage, projectile.knockBack, projectile.owner, projectile.type, 12f)];
                                portal1.rotation = (target.Center - teleTo).ToRotation();
                                portal1.timeLeft = (i + 1) * 15;
                                break; //end for loop
                            }
                        }
                    }
                }
            }
        }
    }

    public class ArrowPortal : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 36;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.alpha = 255;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.light = .5f;
        }

        private int activeTime = 15;

        public override void AI()
        {
            if (Projectile.timeLeft < activeTime)
            {
                Dust dust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<CaeliteDust>())];
                dust.scale = .5f;
                if (Projectile.timeLeft < activeTime / 5)
                {
                    Projectile.frame = 0;
                }
                else if (Projectile.timeLeft < 2 * (activeTime / 5))
                {
                    Projectile.frame = 1;
                }
                else if (Projectile.timeLeft < 3 * (activeTime / 5))
                {
                    Projectile.frame = 2;
                }
                else if (Projectile.timeLeft < 4 * (activeTime / 5))
                {
                    Projectile.frame = 1;
                }
                else
                {
                    Projectile.frame = 0;
                }
                if (Projectile.timeLeft == activeTime / 2)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, QwertyMethods.PolarVector(Projectile.ai[1], Projectile.rotation), (int)Projectile.ai[0], Projectile.damage, Projectile.knockBack, Projectile.owner);
                }
                Projectile.alpha = 0;
            }
        }

    }
}