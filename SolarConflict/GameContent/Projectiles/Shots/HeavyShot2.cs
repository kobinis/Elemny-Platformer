using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Projectiles.Shots
{
    class HeavyShot2
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = HeavyShot1.Make();
            profile.CollisionSpec = new CollisionSpec(400, 12);
            return profile;
        }
    }
}
