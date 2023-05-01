using Microsoft.Xna.Framework;
using QwertyMod.Content.Dusts;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using QwertyMod.Common.PlayerLayers;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Content.Items.MiscMaterials;
using Terraria.GameContent.Creative;

namespace QwertyMod.Content.Items.Weapon.Magic.Catalyst
{
    public class InvaderCatalyst : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Invader Catalyst");
            //Tooltip.SetDefault("Melee attacks will cause magic explosions for a short time");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.value = QwertyMod.InvaderGearValue;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.DamageType = DamageClass.Magic;
            Item.noMelee = true;
            Item.damage = 50;
            Item.mana = 100;
            Item.UseSound = SoundID.Item29;
            Item.useTime = Item.useAnimation = 30;
            Item.rare = ItemRarityID.Yellow;


            if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = ModContent.Request<Texture2D>("QwertyMod/Content/Items/Weapon/Magic/Catalyst/InvaderCatalyst_Glow").Value;
            }

        }
        public override bool? UseItem(Player player)
        {
            player.AddBuff(ModContent.BuffType<CatalystBuff>(), 4 * 60);
            player.GetModPlayer<CatalystEffect>().CatalystDamage = player.GetWeaponDamage(Item);
            return base.UseItem(player);
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<InvaderPlating>(), 30)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    public class CatalystEffect : ModPlayer
    {
        public int CatalystDamage = 0;
        void ProcessCatalystEffect(NPC target, IEntitySource source)
        {
            if(Player.HasBuff(ModContent.BuffType<CatalystBuff>()))
            {

                SoundEngine.PlaySound(SoundID.Item91, target.Center);
                for (int i = 0; i < 100; i++)
                {
                    float rot = MathF.PI * 2f * ((float)i / 30f);
                    Dust.NewDustPerfect(target.Center, ModContent.DustType<InvaderGlow>(), QwertyMethods.PolarVector(5f, rot));
                }
                QwertyMethods.PokeNPC(Player, target, source, Player.GetModPlayer<CatalystEffect>().CatalystDamage, DamageClass.Magic, 0);
                //Projectile.NewProjectile(player.GetSource_Buff(buffIndex), target.Center, Vector2.Zero, ModContent.ProjectileType<CatalystExplosion>(), Player.GetModPlayer<CatalystEffect>().CatalystDamage, 0, Player.whoAmI);
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if(hit.DamageType == DamageClass.Melee)
            {
                ProcessCatalystEffect(target, Player.GetSource_ItemUse(Player.HeldItem));
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if(proj.CountsAsClass(DamageClass.Melee))
            {
                ProcessCatalystEffect(target, Projectile.InheritSource(proj));
            }
        }
    }
    public class CatalystBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Catalyst");
            //Description.SetDefault("Why are you reading this GO KILL");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
        }
    }
    public class CatalystExplosion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("explosion");
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 2;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            SoundEngine.PlaySound(SoundID.Item91, Projectile.Center);
            for (int i = 0; i < 100; i++)
            {
                float rot = MathF.PI * 2f * ((float)i / 30f);
                Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<InvaderGlow>(), QwertyMethods.PolarVector(5f, rot));
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
}