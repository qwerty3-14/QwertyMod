using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using QwertyMod.Content.Dusts;
using Terraria.GameContent.Creative;

namespace QwertyMod.Content.Items.Equipment.Accessories.RuneScrolls
{
    public class LeechScroll : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Leech Scroll");
            Tooltip.SetDefault("Ranged attacks may summon leech runes that can heal you");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.value = 500000;
            Item.rare = 9;
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 50;

            Item.width = 54;
            Item.height = 56;

            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ScrollEffects>().leech = true;
        }
    }

    internal class LeechRuneFreindly : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;

            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;

            Projectile.tileCollide = true;
            Projectile.timeLeft = 180;
            Projectile.DamageType = DamageClass.Ranged;
        }

        public int runeTimer;
        public float startDistance = 200f;
        public float direction;
        public float runeSpeed = 10;
        public bool runOnce = true;
        public float aim;

        public override void AI()
        {
            Projectile.rotation += MathHelper.ToRadians(3);
        }

        public override void Kill(int timeLeft)
        {
            for (int d = 0; d <= 100; d++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<LeechRuneDeath>());
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!target.immortal && !target.SpawnedFromStatue)
            {
                Player player = Main.player[Projectile.owner];
                if(Main.rand.Next(2) == 0)
                {
                    player.statLife++;
                    player.HealEffect(1, true);
                }
                
            }
        }
    }
}