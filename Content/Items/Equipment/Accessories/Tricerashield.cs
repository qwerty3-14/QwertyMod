using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertyMod.Common;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace QwertyMod.Content.Items.Equipment.Accessories
{
    [AutoloadEquip(EquipType.Shield)]
    public class Tricerashield : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName,SetDefault("Tricerashield");
            //Tooltip.SetDefault("Allows you to dash into an enemy" + "\nProvides immunity to knockback");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.value = 10000;
            Item.rare = ItemRarityID.LightPurple;
            Item.damage = 180;
            Item.DamageType = DamageClass.Melee;
            Item.width = 20;
            Item.height = 26;
            Item.defense = 3;
            Item.accessory = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<Dash>().SetDash(5.6f);
            player.GetModPlayer<Dash>().customDashRam = Item.damage;
            player.thorns = .2f;
            player.noKnockback = true;
        }
    }
    public class TricerashieldDraw : PlayerDrawLayer
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return true;
        }
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Shield);

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("QwertyMod");
            if (drawPlayer.shield == EquipLoader.GetEquipSlot(mod, "Tricerashield", EquipType.Shield))
            {
                //Main.NewText("Hi");
                Vector2 Position = drawInfo.Position;
                DrawData value = default(DrawData);
                Color color12 = drawInfo.colorArmorBody;
                Vector2 zero = Vector2.Zero;
                Vector2 origin = new Vector2((float)drawPlayer.legFrame.Width * 0.5f, (float)drawPlayer.legFrame.Height * 0.5f);

                Rectangle BigShieldFrame = new Rectangle(drawPlayer.bodyFrame.X, drawPlayer.bodyFrame.Y, drawPlayer.bodyFrame.Width * 2, drawPlayer.bodyFrame.Height);
                if (drawPlayer.direction == 1)
                {
                }
                else
                {
                    Position.X -= drawPlayer.bodyFrame.Width;
                }
                int shader8 = 0;
                for (int i = 0; i < 20; i++)
                {
                    if (drawPlayer.armor[i].type == ItemType<Tricerashield>())
                    {
                        shader8 = (int)drawPlayer.dye[i % 10].dye;
                    }
                    //int num8 = i % 10;
                    //shader8 = (int)drawPlayer.dye[num8].dye; ;
                }
                if (drawPlayer.shieldRaised)
                {
                    zero.Y -= 4f;
                }
                if (drawPlayer.shieldRaised)
                {
                    float num92 = MathF.Sin(((float)Main.time * 6.28318548f));
                    float x2 = 2.5f + 1.5f * num92;
                    Microsoft.Xna.Framework.Color color33 = color12;
                    color33.A = 0;
                    color33 *= 0.45f - num92 * 0.15f;
                    for (float num93 = 0f; num93 < 4f; num93 += 1f)
                    {
                        value = new DrawData(Request<Texture2D>("QwertyMod/Content/Items/Equipment/Accessories/Tricerashield_Shield").Value, new Vector2((float)((int)(Position.X - Main.screenPosition.X - (float)(BigShieldFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(Position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)BigShieldFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(BigShieldFrame.Width / 2), (float)(BigShieldFrame.Height / 2)) + zero + new Vector2(x2, 0f).RotatedBy((double)(num93 / 4f * 6.28318548f), default(Vector2)), BigShieldFrame, color33, drawPlayer.bodyRotation, origin, 1f, drawInfo.playerEffect, 0);
                        value.shader = shader8;
                        drawInfo.DrawDataCache.Add(value);
                    }
                }
                value = new DrawData(Request<Texture2D>("QwertyMod/Content/Items/Equipment/Accessories/Tricerashield_Shield").Value, new Vector2((float)((int)(Position.X - Main.screenPosition.X - (float)(BigShieldFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(Position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)BigShieldFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(BigShieldFrame.Width / 2), (float)(BigShieldFrame.Height / 2)) + zero, BigShieldFrame, color12, drawPlayer.bodyRotation, origin, 1f, drawInfo.playerEffect, 0);
                value.shader = shader8;
                drawInfo.DrawDataCache.Add(value);
                if (drawPlayer.shieldRaised)
                {
                    Microsoft.Xna.Framework.Color color34 = color12;
                    float num94 = MathF.Sin(((float)Main.time * 3.14159274f));
                    color34.A = (byte)((float)color34.A * (0.5f + 0.5f * num94));
                    color34 *= 0.5f + 0.5f * num94;
                    value = new DrawData(Request<Texture2D>("QwertyMod/Content/Items/Equipment/Accessories/Tricerashield_Shield").Value, new Vector2((float)((int)(Position.X - Main.screenPosition.X - (float)(BigShieldFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(Position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)BigShieldFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(BigShieldFrame.Width / 2), (float)(BigShieldFrame.Height / 2)) + zero, BigShieldFrame, color34, drawPlayer.bodyRotation, origin, 1f, drawInfo.playerEffect, 0);
                    value.shader = shader8;
                }
                if (drawPlayer.shieldRaised && drawPlayer.shieldParryTimeLeft > 0)
                {
                    float num95 = (float)drawPlayer.shieldParryTimeLeft / 20f;
                    float num96 = 1.5f * num95;
                    Vector2 vector9 = new Vector2((float)((int)(Position.X - Main.screenPosition.X - (float)(BigShieldFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(Position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)BigShieldFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(BigShieldFrame.Width / 2), (float)(BigShieldFrame.Height / 2)) + zero;
                    Microsoft.Xna.Framework.Color color35 = color12;
                    float num97 = 1f;
                    Vector2 value14 = Position + drawPlayer.Size / 2f - Main.screenPosition;
                    Vector2 value15 = vector9 - value14;
                    vector9 += value15 * num96;
                    num97 += num96;
                    color35.A = (byte)((float)color35.A * (1f - num95));
                    color35 *= 1f - num95;
                    value = new DrawData(Request<Texture2D>("QwertyMod/Content/Items/Equipment/Accessories/Tricerashield_Shield").Value, vector9, BigShieldFrame, color35, drawPlayer.bodyRotation, origin, num97, drawInfo.playerEffect, 0);
                    value.shader = shader8;
                    drawInfo.DrawDataCache.Add(value);
                }
                if (drawPlayer.mount.Cart)
                {
                    drawInfo.DrawDataCache.Reverse(drawInfo.DrawDataCache.Count - 2, 2);
                }
            }
        }
    }

}