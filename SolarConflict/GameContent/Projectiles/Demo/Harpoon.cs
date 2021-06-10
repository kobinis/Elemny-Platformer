using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using SolarConflict.NewContent.Emitters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles
{
    public class Harpoon
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.Alpha;
            profile.ColorLogic = ColorUpdater.FadeOutSlow;
            profile.TextureID = "HarpoonShot";
            profile.CollisionWidth = profile.Sprite.Width;
            profile.InitSizeID = "15";
            profile.UpdateSize = null;
            //profile.Draw = new ProjectileDrawRotateWithTime(0.1f, -0.12f, null, 1);
            profile.InitMaxLifetime = new InitFloatConst(100);
            profile.Mass = 0.1f;

         //   ParamEmitter emitter = new ParamEmitter();
          //  emitter.EmitterId = typeof(HeavyShotTrail).Name; ;
         //   emitter.RotationType = ParamEmitter.EmitterRotation.Random;
        //    emitter.RotationRange = 360;
            //profile.UpdateEmitter = emitter;
            //profile.UpdateEmitterCooldownTime = 1;
            profile.ImpactEmitterID = typeof(FxEmitterRockExp).Name; 
            profile.CollisionSpec = new CollisionSpec(20,0); //Mass 1000
            profile.CollisionSpec.Force = -20;
            profile.CollisionSpec.ForceType = ForceType.Rotation; //Or Rotation
            profile.IsDestroyedOnCollision = true;
            profile.IsEffectedByForce = false;
            return profile;
        }
    }
}
