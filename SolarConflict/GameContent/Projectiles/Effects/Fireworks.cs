using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode.Init.InitColor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Projectiles.Effects
{
    class Fireworks
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.Mass = 1;
            profile.MovementLogic = new MoveForward(0.3f, 20);
            profile.TextureID = "add8Gray";
            profile.InitMaxLifetimeID = "30";
            profile.InitSizeID = "35";
            //profile.ColorLogicID = "FadeInOut"; //maybe FadeinOut
            profile.CollisionType = CollisionType.Effects;
            profile.DrawType = DrawType.None;
            profile.InitColor = new InitColorRandomHue();
            profile.UpdateEmitterID = "GenericTrail";
            profile.TimeOutEmitter = FireworksExp();
            profile.Flags = GameObjectFlags.AddOnlyOnScreen;
            return profile;
        }


        public static ParamEmitter FireworksExp()
        {
            var param = ParamEmitter.MakeSpreadParam(11, 3);
            param.Emitter = MakeTrail();
            param.RefVelocityMult = 0.5f;
           // param.
            return param;
        }

        public static ProjectileProfile MakeTrail()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.Mass = 1;
            profile.MovementLogic = new MoveForward(0.3f, 15);
            profile.RotationLogic = new UpdateRotationHoming(0.07f, 0, ProjectileTargetType.PrimeAncestor);
            profile.TextureID = "add8Gray";            
            profile.InitMaxLifetimeID = "30";
            profile.InitSizeID = "25";
            profile.ColorLogicID = "FadeInOut"; //maybe FadeinOut
            profile.CollisionType = CollisionType.Effects;
            profile.DrawType = DrawType.None;
            profile.InitColor = new InitColorParent(); // new InitColorRandomHue();
            profile.UpdateEmitterID = "GenericTrail";                       
            return profile;
        }
    }
}
