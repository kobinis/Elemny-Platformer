using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode.Draw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Projectiles.Effects
{
    class DefeatImage
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.TextureID = "defeat";
            profile.InitSizeID = "100000";
            profile.ScaleMult = 1 / 100000f;
            profile.CollisionType = CollisionType.Collide1;
            //profile.ColorLogicID = "FadeIn";
            profile.DrawType = DrawType.Alpha;
            profile.Draw = new DrawInScreenCord();
            profile.IsEffectedByForce = false;
            profile.IsDestroyedOnCollision = false;
            profile.InitMaxLifetimeID = "10000";
            profile.Flags = GameObjectFlags.AddOnlyOnScreen;
            return profile;
        }
    }
}
