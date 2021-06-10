using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Projectiles.AATested
{
    class AoeShot1
    {

        public static ProjectileProfile Make()
        {
            ProjectileProfile projectileProfile = new ProjectileProfile();
            projectileProfile.CollisionType = CollisionType.Collide1;
            projectileProfile.DrawType = DrawType.Additive;
            projectileProfile.ColorLogic = ColorUpdater.FadeInOut;
            projectileProfile.TextureID = "bigshockwave";
            projectileProfile.CollisionWidth = projectileProfile.Sprite.Width - 20;
            projectileProfile.InitSize = new InitFloatParentSize(1.1f, 2);
            projectileProfile.InitMaxLifetime = new InitFloatConst(25);
            projectileProfile.IsDestroyedOnCollision = false;
            projectileProfile.InitColor = new InitColorConst(new Color(255, 200, 150));
            projectileProfile.CollisionSpec = new CollisionSpec(2, 0.001f);
            projectileProfile.IsDestroyedOnCollision = true;
            projectileProfile.ImpactEmitter = AoeExp();
            projectileProfile.TimeOutEmitter = projectileProfile.ImpactEmitter;
            projectileProfile.UpdateEmitter = Trail();
            return projectileProfile;
        }

        public static ProjectileProfile AoeExp()
        {
            ProjectileProfile projectileProfile = new ProjectileProfile();
            projectileProfile.CollisionType = CollisionType.Collide1;
            projectileProfile.DrawType = DrawType.Additive;
            //projectileProfile.ColorLogic = ColorUpdater.FadeInOut;
            projectileProfile.TextureID = "bigshockwave";
            projectileProfile.CollisionWidth = projectileProfile.Sprite.Width - 20;
            projectileProfile.InitSize = new InitFloatParentSize(1.1f, 0);
            projectileProfile.UpdateSize = new UpdateSizeGrow(15);
            projectileProfile.InitMaxLifetime = new InitFloatConst(25);
            projectileProfile.IsDestroyedOnCollision = false;
            projectileProfile.InitColor = new InitColorConst(new Color(255, 200, 150));
            projectileProfile.CollisionSpec = new CollisionSpec(2, 0.001f);
            projectileProfile.VelocityInertia = 0.8f; //??
            projectileProfile.MovementLogic = new MoveToTarget(ProjectileTargetType.Parent, 0, 0); //??
            return projectileProfile;
        }

        public static IEmitter Trail()
        {
            ParamEmitter emitter = new ParamEmitter();
            emitter.RotationSpeedType = ParamEmitter.EmitterRotationSpeed.Random;
            emitter.RotationSpeedRange = 80;
            emitter.RotationType = ParamEmitter.EmitterRotation.Random;
            emitter.RotationRange = 360;

            ProjectileProfile projectileProfile = new ProjectileProfile();
            projectileProfile.CollisionType = CollisionType.Effects;
            projectileProfile.DrawType = DrawType.Additive;
            projectileProfile.ColorLogic = ColorUpdater.FadeInOut;
            projectileProfile.TextureID = "bigshockwave";
            projectileProfile.CollisionWidth = projectileProfile.Sprite.Width - 20;
            projectileProfile.InitSize = new InitFloatParentSize(1.1f, 2);
            projectileProfile.InitMaxLifetime = new InitFloatConst(10);
            projectileProfile.IsDestroyedOnCollision = false;
            projectileProfile.InitColor = new InitColorConst(new Color(255, 200, 150));
            projectileProfile.CollisionSpec = new CollisionSpec(2, 0.001f);
            projectileProfile.VelocityInertia = 0.95f; //??  
            projectileProfile.IsDestroyedWhenParentDestroyed = true;
            emitter.Emitter = projectileProfile;
            return emitter;
        }
    }
}
