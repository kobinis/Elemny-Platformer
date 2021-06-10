using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Projectiles
{
    class ItemPullingSmallAoe
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = ItemPullingAoe.Make();
            profile.InitSizeID = "300";
            return profile;
        }
    }
}
