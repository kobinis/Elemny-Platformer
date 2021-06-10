using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.NewContent.Projectiles
{
    class PushForceField
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.CollisionType = CollisionType.CollideAll;
           // profile.CollideWithMask = GameObjectType.Asteroid | GameObjectType.Ship;
            profile.DrawType = DrawType.Alpha;
            profile.ColorLogic = ColorUpdater.FadeInOut;
            profile.TextureID = "shockwave2";
            profile.CollisionWidth = profile.Sprite.Width - 10;

            /*profile.InitSizeId = "40";
            profile.UpdateSize = new UpdateSizeGrow(5, 1.1f);*/

            profile.InitSizeID = "1600";
            profile.UpdateSize = new UpdateSizeGrow(-5, 1f);

            profile.MovementLogic = new MoveToTarget(ProjectileTargetType.Parent, 0, 0);

            profile.InitMaxLifetime = new InitFloatConst(30);
            profile.Mass = 0.1f;
            //profile.ImpactEmitterId = "EmitterImpactFx1";
            profile.CollisionSpec = new CollisionSpec(0, 0.5f);
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = false;
            return profile;
        }
    }
}
