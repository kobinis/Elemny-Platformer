using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Projectiles
{
    class AoeDamage2
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile projectileProfile = AoeDamage1.Make();           
            projectileProfile.InitMaxLifetime = new InitFloatConst(60);            
            projectileProfile.CollisionSpec = new CollisionSpec(4, 0.001f);          
            return projectileProfile;
        }
    }
}
