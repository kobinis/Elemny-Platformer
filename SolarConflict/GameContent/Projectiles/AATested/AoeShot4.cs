using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Projectiles.AATested
{
    class AoeShot4
    {
        public static ProjectileProfile Make() //TODO: add sound,
        {
            ProjectileProfile profile = AoeShot1.Make();
            //profile.CollisionSpec = new CollisionSpec()
            return profile;
        }
    }
}
