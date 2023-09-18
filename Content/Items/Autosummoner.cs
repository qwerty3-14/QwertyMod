using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertyMod.Content.Items
{
    public class Autosummoner : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 1;
            Item.width = 40;
            Item.height = 40;
            Item.rare = ItemRarityID.Orange;
            Item.value = 20000;
            Item.expert = true;
        }
		public override void UpdateInventory(Player player)
		{
            int i = 0;
            for(; i < 50; i++)
            {
                if(player.inventory[i] == Item)
                {
                    player.GetModPlayer<AutoSummoning>().hasAuto = true;
                    break;
                }
            }
            if(player.GetModPlayer<AutoSummoning>().hasAuto)
            {
                if(i >= 10)
                {
                    if(player.inventory[i - 10].IsAir)
                    {

                    }
                    else
                    {
                        int projID = player.inventory[i - 10].shoot;
                        Projectile dummyProjectile = new Projectile();
                        dummyProjectile.SetDefaults(projID);
                        if(dummyProjectile.minionSlots > 0)
                        {
                            player.GetModPlayer<AutoSummoning>().defaultSummon = i - 10;
                            //Main.NewText(i - 10);
                        }
                    }
                }
                if(i < 49)
                {
                    if(player.inventory[i + 1].IsAir)
                    {

                    }
                    else
                    {
                        int projID = player.inventory[i + 1].shoot;
                        Projectile dummyProjectile = new Projectile();
                        dummyProjectile.SetDefaults(projID);
                        if(dummyProjectile.minionSlots > 0)
                        {
                            player.GetModPlayer<AutoSummoning>().loadoutSummon[0] = i + 1;
                            //Main.NewText(i - 10);
                        }
                    }
                }
                if(i < 40)
                {
                    if(player.inventory[i + 10].IsAir)
                    {

                    }
                    else
                    {
                        int projID = player.inventory[i + 10].shoot;
                        Projectile dummyProjectile = new Projectile();
                        dummyProjectile.SetDefaults(projID);
                        if(dummyProjectile.minionSlots > 0)
                        {
                            player.GetModPlayer<AutoSummoning>().loadoutSummon[1] = i + 10;
                            //Main.NewText(i - 10);
                        }
                    }
                }
                if(i > 0)
                {
                    if(player.inventory[i - 1].IsAir)
                    {

                    }
                    else
                    {
                        int projID = player.inventory[i - 1].shoot;
                        Projectile dummyProjectile = new Projectile();
                        dummyProjectile.SetDefaults(projID);
                        if(dummyProjectile.minionSlots > 0)
                        {
                            player.GetModPlayer<AutoSummoning>().loadoutSummon[2] = i - 1;
                            //Main.NewText(i - 10);
                        }
                    }
                }
            }
            //Main.NewText(i);
		}
    }
    public class AutoSummoning : ModPlayer
    {
        public bool hasAuto = false;
        public int defaultSummon = -1;
        public int[] loadoutSummon = new int[3] {-1, -1, -1};
        int oldSlot = -1;
        int oldLoadout = -1;
		public override void ResetEffects()
		{
            hasAuto = false;
			defaultSummon = -1;
            loadoutSummon[0] = -1;
            loadoutSummon[1] = -1;
            loadoutSummon[2] = -1;
		}
		public override bool PreItemCheck()
		{
            if(hasAuto)
            {
                if(oldLoadout != -1 && oldLoadout != Player.CurrentLoadoutIndex && (loadoutSummon[Player.CurrentLoadoutIndex] != -1 || loadoutSummon[oldLoadout] != -1))
                {
                    if(Main.netMode == NetmodeID.SinglePlayer)
                    {
                        for(int i = 0; i < 1000; i++)
                        {
                            if(Main.projectile[i].active && Main.projectile[i].owner == Player.whoAmI && Main.projectile[i].minionSlots > 0)
                            {
                                Main.projectile[i].Kill();
                            }
                        }
                    }
                    else
                    {
                        ModPacket packet = Mod.GetPacket();
                        packet.Write((byte)ModMessageType.AutoDesummon);
                        packet.Write(Player.whoAmI);
                        packet.Send();
                    }
                    
                
                }
                oldLoadout = Player.CurrentLoadoutIndex;
                float minionCount = 0;
                for(int i = 0; i < 1000; i++)
                {
                    if(Main.projectile[i].active && Main.projectile[i].owner == Player.whoAmI && Main.projectile[i].minionSlots > 0)
                    {
                        minionCount += Main.projectile[i].minionSlots;
                    }
                }
                float currentMinionSlotCount = 1;
                if(loadoutSummon[Player.CurrentLoadoutIndex] != -1)
                {
                    Projectile dummy = new Projectile();
                    dummy.SetDefaults(Player.inventory[loadoutSummon[Player.CurrentLoadoutIndex]].shoot);
                    currentMinionSlotCount = dummy.minionSlots;
                }
                if(defaultSummon != -1)
                {
                    Projectile dummy = new Projectile();
                    dummy.SetDefaults(Player.inventory[defaultSummon].shoot);
                    currentMinionSlotCount = dummy.minionSlots;
                }

                if (oldSlot != -1 && Player.itemAnimation == 0)
                {
                    Player.selectedItem = oldSlot;
                    oldSlot = -1;
                }
                else if(Player.itemAnimation == 0 && loadoutSummon[Player.CurrentLoadoutIndex] != -1 && Player.maxMinions - minionCount >= currentMinionSlotCount && currentMinionSlotCount != 0)
                {
                    oldSlot = Player.selectedItem;
                    Player.selectedItem = loadoutSummon[Player.CurrentLoadoutIndex];
                    Player.controlUseItem = true;
                }
                else if(Player.itemAnimation == 0 && defaultSummon != -1 && Player.maxMinions - minionCount >= currentMinionSlotCount && currentMinionSlotCount != 0)
                {
                    oldSlot = Player.selectedItem;
                    Player.selectedItem = defaultSummon;
                    Player.controlUseItem = true;
                }
            }
			return base.PreItemCheck();
		}
    }
}