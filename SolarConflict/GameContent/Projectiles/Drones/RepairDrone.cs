using SolarConflict.NewContent.Projectiles;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles
{
    class RepairDrone
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.Alpha;
            profile.TextureID = "item4";
            profile.CollisionWidth = profile.Sprite.Width;
            profile.InitSizeID = "25";            
            profile.InitMaxLifetimeID = "10000";
            profile.Mass = 1f;
            profile.InitHitPointsID = "1000";

            profile.UpdateEmitterID = typeof(RepairShot).Name;
            profile.UpdateEmitterCooldownTime = 30;            
            profile.CollisionSpec = new CollisionSpec(0, 0.5f);
            profile.MovementLogic = new MoveToTarget(ProjectileTargetType.Parent, 0.07f, 7f, true);
            profile.VelocityInertia = 0.97f;
            profile.RotationLogic = new UpdateRotationForward();
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = true;
            profile.CollisionType = CollisionType.CollideAll;

            return profile;
        }
    }
}
