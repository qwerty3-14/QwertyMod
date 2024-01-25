using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace QwertyMod.Common
{
    //Saving and loading these flags requires TagCompounds, a guide exists on the wiki: https://github.com/tModLoader/tModLoader/wiki/Saving-and-loading-using-TagCompound
    public class DownedBossSystem : ModSystem
    {
        public static bool downedBear = false;
        public static bool downedHydra = false;
        public static bool downedAncient = false;
        public static bool downedBlade = false;
        public static bool downedNoehtnap = false;
        public static bool downedRuneGhost = false;
        public static bool downedDivineLight = false;
        public static bool downedOLORD = false;
        public static bool downedDinos = false;
        public static bool downedBattleship = false;
        //public static bool downedOtherBoss = false;

        public override void OnWorldLoad()
        {
            downedBear = false;
            downedHydra = false;
            downedAncient = false;
            downedBlade = false;
            downedNoehtnap = false;
            downedRuneGhost = false;
            downedDivineLight = false;
            downedOLORD = false;
            downedDinos = false;
            downedBattleship = false;
            //downedOtherBoss = false;
        }

        public override void OnWorldUnload()
        {
            downedBear = false;
            downedHydra = false;
            downedAncient = false;
            downedBlade = false;
            downedNoehtnap = false;
            downedRuneGhost = false;
            downedDivineLight = false;
            downedOLORD = false;
            downedDinos = false;
            downedBattleship = false;
            //downedOtherBoss = false;
        }
        public override void SaveWorldData(TagCompound tag)
        {

            if (downedBear)
            {
                tag["downedBear"] = true;
            }
            if (downedHydra)
            {
                tag["downedHydra"] = true;
            }
            if (downedAncient)
            {
                tag["downedAncient"] = true;
            }
            if (downedBlade)
            {
                tag["downedBlade"] = true;
            }
            if (downedNoehtnap)
            {
                tag["downedNoehtnap"] = true;
            }
            if (downedRuneGhost)
            {
                tag["downedRuneGhost"] = true;
            }
            if (downedDivineLight)
            {
                tag["downedDivineLight"] = true;
            }
            if (downedOLORD)
            {
                tag["downedOLORD"] = true;
            }
            if (downedDinos)
            {
                tag["downedDinos"] = true;
            }
            if (downedBattleship)
            {
                tag["downedBattleship"] = true;
            }
            //if (downedOtherBoss) {
            //	downed.Add("downedOtherBoss");
            //}

        }

        public override void LoadWorldData(TagCompound tag)
        {
            downedBear = tag.ContainsKey("downedBear");
            downedHydra = tag.ContainsKey("downedHydra");
            downedAncient = tag.ContainsKey("downedAncient");
            downedBlade = tag.ContainsKey("downedBlade");
            downedNoehtnap = tag.ContainsKey("downedNoehtnap");
            downedRuneGhost = tag.ContainsKey("downedRuneGhost");
            downedDivineLight = tag.ContainsKey("downedDivineLight");
            downedOLORD = tag.ContainsKey("downedOLORD");
            downedDinos = tag.ContainsKey("downedDinos");
            downedBattleship = tag.ContainsKey("downedBattleship");
            //downedOtherBoss = downed.Contains("downedOtherBoss");
        }

        public override void NetSend(BinaryWriter writer)
        {
            //Order of operations is important and has to match that of NetReceive
            var flags = new BitsByte();
            flags[0] = downedBear;
            flags[1] = downedHydra;
            flags[2] = downedAncient;
            flags[3] = downedBlade;
            flags[4] = downedNoehtnap;
            flags[5] = downedRuneGhost;
            flags[6] = downedDivineLight;
            flags[7] = downedOLORD;
            //flags[1] = downedOtherBoss;
            writer.Write(flags);
            flags = new BitsByte();
            flags[0] = downedDinos;
            flags [1] = downedBattleship;
            writer.Write(flags);
            /*
			Remember that Bytes/BitsByte only have up to 8 entries. If you have more than 8 flags you want to sync, use multiple BitsByte:
				This is wrong:
			flags[8] = downed9thBoss; // an index of 8 is nonsense. 
				This is correct:
			flags[7] = downed8thBoss;
			writer.Write(flags);
			BitsByte flags2 = new BitsByte(); // create another BitsByte
			flags2[0] = downed9thBoss; // start again from 0
			// up to 7 more flags here
			writer.Write(flags2); // write this byte
			*/

            //If you prefer, you can use the BitsByte constructor approach as well.
            //BitsByte flags = new BitsByte(downedBear, downedOtherBoss);
            //writer.Write(flags);

            // This is another way to do the same thing, but with bitmasks and the bitwise OR assignment operator (the |=)
            // Note that 1 and 2 here are bit masks. The next values in the pattern are 4,8,16,32,64,128. If you require more than 8 flags, make another byte.
            //byte flags = 0;
            //if (downedBear)
            //{
            //	flags |= 1;
            //}
            //if (downedOtherBoss)
            //{
            //	flags |= 2;
            //}
            //writer.Write(flags);

            //If you plan on having more than 8 of these flags and don't want to use multiple BitsByte, an alternative is using a System.Collections.BitArray
            /*
			bool[] flags = new bool[] {
				downedBear,
				downedOtherBoss,
			};
			BitArray bitArray = new BitArray(flags);
			byte[] bytes = new byte[(bitArray.Length - 1) / 8 + 1]; //Calculation for correct length of the byte array
			bitArray.CopyTo(bytes, 0);
			writer.Write(bytes.Length);
			writer.Write(bytes);
			*/
        }

        public override void NetReceive(BinaryReader reader)
        {
            //Order of operations is important and has to match that of NetSend
            BitsByte flags = reader.ReadByte();
            downedBear = flags[0];
            downedHydra = flags[1];
            downedAncient = flags[2];
            downedBlade = flags[3];
            downedNoehtnap = flags[4];
            downedRuneGhost = flags[5];
            downedDivineLight = flags[6];
            downedOLORD = flags[7];
            flags = reader.ReadByte();
            downedDinos = flags[0];
            downedBattleship = flags[1];
            //downedOtherBoss = flags[1];

            // As mentioned in NetSend, BitBytes can contain up to 8 values. If you have more, be sure to read the additional data:
            // BitsByte flags2 = reader.ReadByte();
            // downed9thBoss = flags[0];

            //System.Collections.BitArray approach:
            /*
			int length = reader.ReadInt32();
			byte[] bytes = reader.ReadBytes(length);
			BitArray bitArray = new BitArray(bytes);
			downedBear = bitArray[0];
			downedOtherBoss = bitArray[1];
			*/
        }
    }
}
