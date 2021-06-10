using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolarConflict.GameContent.Projectiles;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;

namespace SolarConflict.NewContent.Projectiles
{
    class HomingFusionShot
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = FusionShot.Make();
            profile.RotationLogic = new UpdateRotationHoming();
            profile.MovementLogic = new MoveForward(0.6f, 10);
            return profile;
        }
    }
}
