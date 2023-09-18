using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Content.Items.Equipment.Accessories;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace QwertyMod.Content.Items.Weapon.Magic.HydraBeam
{
    public class HydraBeam : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Hydra Beam");
            //Tooltip.SetDefault("Creates a beam of destructive energy from the sky");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 50;
            Item.DamageType = DamageClass.Magic;

            Item.useTime = 28;
            Item.useAnimation = 28;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 1;
            Item.value = 250000;
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item1;
            Item.width = 28;
            Item.height = 30;
            Item.channel = true;
            Item.mana = ModLoader.HasMod("TRAEProject") ? 60 : 10;
            Item.shoot = ProjectileType<BeamHead>();
            Item.shootSpeed = 9;
            Item.noMelee = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int l = 0; l < Main.projectile.Length; l++)
            {
                Projectile proj = Main.projectile[l];
                if (proj.active && proj.type == Item.shoot && proj.owner == player.whoAmI)
                {
                    proj.active = false;
                }
            }
            return true;
        }
    }

    public class BeamHead : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = 5000;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 300;
            Projectile.width = 72;
            Projectile.height = 94;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
        }

        public bool runOnce = true;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (runOnce)
            {
                Projectile.position = new Vector2(player.Center.X, player.Center.Y - 900); ;
                runOnce = false;
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ProjectileType<HeadBeam>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Projectile.whoAmI, 0f);
            }

            if (Main.LocalPlayer == player)
            {
                if (Math.Abs(Main.MouseWorld.X - Projectile.Center.X) < 6f)
                {
                    Projectile.velocity.X = Main.MouseWorld.X - Projectile.Center.X;
                }
                else
                {
                    Projectile.velocity.X = 6f * Math.Sign(Main.MouseWorld.X - Projectile.Center.X);
                }
                Projectile.position.Y = player.Center.Y - 900;

            }
            Projectile.frameCounter++;
            if (player.channel && player.CheckMana((int)((float)player.inventory[player.selectedItem].mana), !player.GetModPlayer<BloodMedalionEffect>().effect && Projectile.frameCounter % 20 == 0))
            {
                if (player.GetModPlayer<BloodMedalionEffect>().effect)
                {
                    player.statLife -= (int)(player.inventory[player.selectedItem].mana * player.manaCost);
                    if (player.statLife <= 0)
                    {
                        player.KillMe(PlayerDeathReason.ByCustomReason(player.name + " madly drained " + (player.Male ? "his " : "her") + " lifeforce!"), (int)(player.inventory[player.selectedItem].mana * player.manaCost), 0);
                    }
                }
                player.itemTime = player.itemAnimation = 10;
                player.direction = Math.Sign(Projectile.Center.X - player.Center.X);
                Projectile.timeLeft = 2;
            }
            else
            {
                Projectile.Kill();
            }
        }
    }

    public class HeadBeam : ModProjectile
    {
        //The distance charge particle from the player center
        private const float MoveDistance = 70f;

        // The actual distance is stored in the ai0 field
        // By making a property to handle this it makes our life easier, and the accessibility more readable
        public float Distance;

        public Projectile shooter;

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            shooter = Main.projectile[(int)Projectile.ai[0]];
            modifiers.HitDirectionOverride = shooter.velocity.X > 0 ? 1 : -1;
        }

        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Hydra Beam");
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.hostile = false;
            Projectile.hide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.usesLocalNPCImmunity = true;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.localNPCImmunity[target.whoAmI] = 12;
            target.immune[Projectile.owner] = 0;
        }
        // The AI of the projectile
        public bool runOnce = true;

        public override void AI()
        {
            float rOffset = MathF.PI / 2;
            shooter = Main.projectile[(int)Projectile.ai[0]];

            Vector2 mousePos = Main.MouseWorld;
            Player player = Main.player[Projectile.owner];
            if (!shooter.active || shooter.type != ProjectileType<BeamHead>())
            {
                Projectile.Kill();
            }


            #region Set projectile position

            Vector2 diff = new Vector2(MathF.Cos(shooter.rotation + rOffset) * 14f, MathF.Sin(shooter.rotation + rOffset) * 14f);
            diff.Normalize();
            Projectile.velocity = diff;
            Projectile.direction = Projectile.Center.X > shooter.Center.X ? 1 : -1;
            Projectile.netUpdate = true;

            Projectile.position = new Vector2(shooter.Center.X, shooter.Center.Y) + Projectile.velocity * MoveDistance;
            Projectile.timeLeft = 2;
            int dir = Projectile.direction;
            /*
            player.ChangeDir(dir);
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = MathF.Atan2(Projectile.velocity.Y * dir, Projectile.velocity.X * dir);
            */

            #endregion Set projectile position

            #region Charging process

            // Kill the projectile if the player stops channeling

            // Do we still have enough mana? If not, we kill the projectile because we cannot use it anymore

            Vector2 offset = Projectile.velocity;
            offset *= MoveDistance - 20;
            Vector2 pos = new Vector2(shooter.Center.X, shooter.Center.Y) + offset - new Vector2(10, 10);

            #endregion Charging process

            Vector2 start = new Vector2(shooter.Center.X, shooter.Center.Y);
            Vector2 unit = Projectile.velocity;
            unit *= -1;
            for (Distance = MoveDistance; Distance <= 2200f; Distance += 5f)
            {
                start = shooter.Center + Projectile.velocity * Distance;
                if (!Collision.CanHit(new Vector2(shooter.Center.X, player.Center.Y), 1, 1, start, 1, 1) && start.Y > player.Center.Y)
                {
                    Distance -= 5f;
                    break;
                }
            }
        }

        public int colorCounter;
        public Color lineColor;

        public override bool PreDraw(ref Color lightColor)
        {
            DrawLaser(TextureAssets.Projectile[Projectile.type].Value, shooter.Center,
                Projectile.velocity, 10, Projectile.damage, -1.57f, 1f, 4000f, Color.White, (int)MoveDistance);

            return false;
        }

        // The core function of drawing a laser
        public void DrawLaser(Texture2D texture, Vector2 start, Vector2 unit, float step, int damage, float rotation = 0f, float scale = 1f, float maxDist = 4000f, Color color = default(Color), int transDist = 50)
        {
            Vector2 origin = start;
            float r = unit.ToRotation() + rotation;

            #region Draw laser body

            for (float i = transDist; i <= Distance; i += step)
            {
                Color c = Color.White;
                origin = start + i * unit;
                Main.EntitySpriteDraw(texture, origin - Main.screenPosition,
                    new Rectangle(0, 26, 28, 26), i < transDist ? Color.Transparent : c, r,
                    new Vector2(28 * .5f, 26 * .5f), scale, 0, 0);
            }

            #endregion Draw laser body

            #region Draw laser tail

            Main.EntitySpriteDraw(texture, start + unit * (transDist - step) - Main.screenPosition,
                new Rectangle(0, 0, 28, 26), Color.White, r, new Vector2(28 * .5f, 26 * .5f), scale, 0, 0);

            #endregion Draw laser tail

            #region Draw laser head

            Main.EntitySpriteDraw(texture, start + (Distance + step) * unit - Main.screenPosition,
                new Rectangle(0, 52, 28, 26), Color.White, r, new Vector2(28 * .5f, 26 * .5f), scale, 0, 0);

            #endregion Draw laser head
        }

        // Change the way of collision check of the projectile
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            // We can only collide if we are at max charge, which is when the laser is actually fired

            Player player = Main.player[Projectile.owner];
            Vector2 unit = Projectile.velocity;
            float point = 0f;
            // Run an AABB versus Line check to look for collisions, look up AABB collision first to see how it works
            // It will look for collisions on the given line using AABB
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), new Vector2(shooter.Center.X, shooter.Center.Y),
                new Vector2(shooter.Center.X, shooter.Center.Y) + unit * Distance, 22, ref point);
        }


        public override void CutTiles()
        {
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Vector2 unit = Projectile.velocity;
            Utils.PlotTileLine(Projectile.Center, Projectile.Center + unit * Distance, 22, DelegateMethods.CutTiles);
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }
}