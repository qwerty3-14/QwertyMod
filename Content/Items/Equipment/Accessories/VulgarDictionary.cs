using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace QwertyMod.Content.Items.Equipment.Accessories
{
    public class VulgarDictionary : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 26;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.aggro += 800;
            if(!hideVisual)
            {
                player.GetModPlayer<ColorfulLanguage>().hasBook = true;
            }
        }
		public override void UpdateVanity(Player player)
		{
            player.GetModPlayer<ColorfulLanguage>().hasBook = true;
		}
    }
    public class ColorfulLanguage : ModPlayer
    {
        public bool hasBook = false;
        int boredomTimer = 0;
        public override void ResetEffects()
        {
            hasBook = false;

        }
        string[] attackedComplaints = {"You little!", "You'll regret that!", "I'll return the favor!", "Now I'm mad!"};
        string[] attackDismissle = {"Was that an attack?", "My mother hits harder than that!", "...", "Are you even trying?", "Skill issue"};
        string[] seriousAttack = {"F#%&@", "You B*+#@", "@#%7@#^&% and $&*@%$^!", "That attack is a war crime!", "I'm calling B*#$S^&@!"};
        string[] impatient = {"Cmon do something!", "I should watch paint dry", "Just logout if you aren't going to do anything!", "I should AFK you"};
        void SayIt(string text)
        {
            CombatText.NewText(Player.getRect(), Color.Red, text, true, false);
        }
		public override void OnHurt(Player.HurtInfo info)
		{
            if(hasBook)
            {
                if(info.DamageSource.SourceNPCIndex != -1 || info.DamageSource.SourceProjectileType > 0)
                {
                    if(info.Damage < 10 && Main.rand.NextBool(10))
                    {
                        SayIt(attackDismissle[Main.rand.Next(attackDismissle.Length)]);
                    }
                    else if(Main.rand.Next(100) < info.Damage)
                    {
                        if(info.Damage > Player.statLifeMax2 / 5f)
                        {
                            SayIt(seriousAttack[Main.rand.Next(seriousAttack.Length)]);
                        }
                        else
                        {
                            SayIt(attackedComplaints[Main.rand.Next(attackedComplaints.Length)]);
                        }
                    }
                }
            }
		}
		public override void PreUpdate()
		{
            if(hasBook)
            {
                if(Player.velocity != Vector2.Zero || Player.controlLeft || Player.controlRight || Player.controlJump || Player.controlUseItem || Player.sleeping.isSleeping)
                {
                    boredomTimer = 0;
                }
                else
                {
                    boredomTimer++;
                    if(boredomTimer > 60 * 60)
                    {
                        boredomTimer = 60 * 30;
                        SayIt(impatient[Main.rand.Next(impatient.Length)]);
                    }
                }
            }
            else
            {
                boredomTimer = 0;
            }
		}

    }
    public class VulgarDictionaryT : ModTile
    {
        
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);
        }
		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 48, ModContent.ItemType<VulgarDictionary>());
			base.KillTile(i, j, ref fail, ref effectOnly, ref noItem);
		}
    }
    public class PlaceInWorld : ModSystem
    {
		public override void PostWorldGen()
		{
            for(int i = 0; i < Main.maxTilesX; i++)
            {
                for(int j = 0; j < Main.maxTilesY; j++)
                {
                    if(Main.tile[i, j].TileType == TileID.Books)
                    {
                        if(WorldGen.genRand.NextBool(100))
                        {
                            WorldGen.KillTile(i, j);
                            WorldGen.PlaceTile(i, j, ModContent.TileType<VulgarDictionaryT>());
                        }
                    }
                }
            }
			base.PostWorldGen();
		}
    }
    
}

