using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Projectiles
{
    class AoeStun2
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile projectileProfile = AoeStun1.Make();          
            projectileProfile.InitMaxLifetime = new InitFloatConst(70);                        
            projectileProfile.CollisionSpec = new CollisionSpec(0, 0f, 60 * 8);            
            return projectileProfile;
        }
    }
}
