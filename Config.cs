
using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace QwertyMod
{
    public class QwertyConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [DefaultValue(true)]
        public bool ImperiousScreenShake { get; set; }

        [DefaultValue(true)]
        public bool OLORDScreenShake { get; set; }
    }
     
}