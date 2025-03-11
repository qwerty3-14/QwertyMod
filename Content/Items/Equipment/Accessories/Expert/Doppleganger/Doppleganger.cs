using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertyMod.Content.Items.Equipment.Accessories.Expert.Doppleganger
{
    public class Doppleganger : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Orange;
            Item.expert = true;
            Item.value = 500000;
            Item.width = 32;
            Item.height = 22;
            Item.GetGlobalItem<DoppleItem>().isDoppleganger = true;
        }
		public override bool OnPickup(Player player)
		{
            Item.prefix = 0;
            Item.ResetPrefix();
			return base.OnPickup(player);
		}
		public override void UpdateInventory(Player player)
		{
		}
        public override void PreReforge()
        {
            Player player = Main.LocalPlayer;
            if (player.difficulty != 2)
            {
                player.KillMe(PlayerDeathReason.ByCustomReason(player.name + " tampered with forces beyond " + (player.Male ? "his" : "her") + " control!"), 1000, 0);
            }
            else
            {
                Main.NewText("You shouldn't tamper with things beyond your control!", Color.Red);
            }
            for (int n = 0; n < 200; n++)
            {
                if (Main.npc[n].active && Main.npc[n].type == NPCID.GoblinTinkerer)
                {
                    NPC.HitInfo hit = new NPC.HitInfo();
                    hit.InstantKill = true;
                    Main.npc[n].StrikeNPC(hit);
                }
            }
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            int mimicId = -1;
            Player player = Main.LocalPlayer;
            for (int a = 4; a < 10; a++)
            {
                if (!player.armor[a].IsAir && player.armor[a] == Item && !player.armor[a - 1].IsAir)
                {
                    mimicId = player.armor[a - 1].type;
                }
            }
            Texture2D texture = TextureAssets.Item[Item.type].Value;
            if (mimicId != -1)
            {
                //spriteBatch.Draw(texture, position, null, Color.White, 0, origin, scale, SpriteEffects.None, 0f);;

                Texture2D mimicTexture = TextureAssets.Item[mimicId].Value;
                int frameCount = 1;
                if (Main.itemAnimations[mimicId] != null)
                {
                    frameCount = Main.itemAnimations[mimicId].FrameCount;
                }
                float greaterLength = Math.Max(mimicTexture.Width, mimicTexture.Height / frameCount);
                //spriteBatch.Draw(mimicTexture, position + Vector2.UnitY * -3, new Rectangle(0, 0, mimicTexture.Width, mimicTexture.Height / frameCount), new Color(180, 100, 100, 255), 0, origin, (44f / greaterLength) * scale, SpriteEffects.None, 0f);
                spriteBatch.Draw(mimicTexture, position, new Rectangle(0, 0, mimicTexture.Width, mimicTexture.Height / frameCount), new Color(180, 100, 100, 255), 0, new Vector2(mimicTexture.Width, mimicTexture.Height / frameCount) * 0.5f, (44f / greaterLength) * scale, SpriteEffects.None, 0f);
                

                return false;
            }

            return true;
        }
    }

    public class DoppleItem : GlobalItem
    {
        public bool isDoppleganger = false;
        public override bool InstancePerEntity => true;
        public override GlobalItem Clone(Item item, Item itemClone)
        {
            return base.Clone(item, itemClone);
        }
    }

    public class DopplePlayer : ModPlayer
    {
        public override void PreUpdate()
        {
            for (int a = 4; a < 10; a++)
            {
                if (!Player.armor[a].IsAir && Player.armor[a].type == ModContent.ItemType<Doppleganger>() && !Player.armor[a - 1].IsAir)
                {
                    Player.armor[a].type = Player.armor[a - 1].type;
                }
            }
        }
        public override void UpdateEquips()
        {
            for (int a = 4; a < 10; a++)
            {
                if (!Player.armor[a].IsAir && Player.armor[a].GetGlobalItem<DoppleItem>().isDoppleganger)
                {
                    if(Player.armor[a].prefix != 0)
                    {
                        Player.armor[a].prefix = 0;
                        Player.armor[a].ResetPrefix();

                    }
                    if (!Player.armor[a - 1].IsAir && Player.armor[a - 1].type > ItemID.Count)
                    {
                        ItemLoader.UpdateAccessory(Player.armor[a - 1], Player, Player.hideVisibleAccessory[a]);
                        ItemLoader.UpdateEquip(Player.armor[a - 1], Player);
                        Player.statDefense += Player.armor[a - 1].defense;
                    }
                }
            }
        }

        public override void PostUpdateEquips()
        {
            for (int a = 4; a < 10; a++)
            {
                if (!Player.armor[a].IsAir && Player.armor[a].GetGlobalItem<DoppleItem>().isDoppleganger)
                {
                    Player.armor[a].type = ModContent.ItemType<Doppleganger>();
                }
            }
        }
    }
}