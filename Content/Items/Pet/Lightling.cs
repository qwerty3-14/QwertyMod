using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace QwertyMod.Content.Items.Pet
{
    public class Lightling : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lightling");
            Tooltip.SetDefault("Emits light");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 0;
            Item.useStyle = 1;
            Item.shoot = ProjectileType<LightlingP>();
            Item.width = 16;
            Item.height = 30;
            Item.UseSound = SoundID.Item2;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.rare = 8;
            Item.noMelee = true;
            Item.value = Item.sellPrice(0, 5, 50, 0);
            Item.buffType = BuffType<LightlingBuff>();
        }
        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(Item.buffType, 3600, true);
            }
        }
    }

    public class LightlingP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lightling");
            Main.projFrames[Projectile.type] = 2;
            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 20;
            Projectile.penetrate = -1;
            Projectile.netImportant = true;
            Projectile.timeLeft *= 5;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;

            Projectile.tileCollide = false;
            Projectile.light = 1f;
        }

        private int shader = 0;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            shader = player.miscDyes[1].dye;
            LightlingOwnership modPlayer = player.GetModPlayer<LightlingOwnership>();
            if (!player.active)
            {
                Projectile.active = false;
                return;
            }
            if (player.dead)
            {
                modPlayer.Lightling = false;
            }
            if (modPlayer.Lightling)
            {
                Projectile.timeLeft = 2;
            }
            Vector2 flyTo = player.Center - Projectile.Center;
            //float dir = flyTo.ToRotation();
            //flyTo = (player.Center +QwertyMethods.PolarVector(-200, dir)) - Projectile.Center;
            // Main.NewText(flyTo.Length());
            if (flyTo.Length() < 120)
            {
                Projectile.velocity = Vector2.Zero;
            }
            else
            {
                Projectile.velocity = flyTo * .01f;
            }
            Projectile.frameCounter++;
            if (Projectile.frameCounter % 10 == 0)
            {
                Projectile.frame = 1;
            }
            else if (Projectile.frameCounter % 5 == 0)
            {
                Projectile.frame = 0;
            }
        }

        public override void PostDraw(Color drawColor)
        {
            // As mentioned above, be sure not to forget this step.
            Player player = Main.player[Projectile.owner];
            if (shader != 0)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.Transform);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];

            if (shader != 0)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

                GameShaders.Armor.GetSecondaryShader(shader, player).Apply(null);
            }
            return true;
        }
    }

    public class LightlingBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lightling");
            Description.SetDefault("Emits light");
            Main.buffNoTimeDisplay[Type] = true;
            Main.lightPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<LightlingOwnership>().Lightling = true;
            player.buffTime[buffIndex] = 18000;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[ProjectileType<LightlingP>()] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.GetProjectileSource_Buff(buffIndex), player.Center.X, player.Center.Y, 0f, 0f, ProjectileType<LightlingP>(), 0, 0f, player.whoAmI, 0f, 0f);
            }
        }
    }
    public class LightlingOwnership : ModPlayer
    {
        public bool Lightling = false;
        public override void ResetEffects()
        {
            Lightling = false;
        }
    }
}