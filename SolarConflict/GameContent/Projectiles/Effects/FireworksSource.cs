using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Projectiles.Effects
{
    class FireworksSource
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.Mass = 1;
            profile.TextureID = "add8";            
            profile.InitSizeID = "35";            
            profile.CollisionType = CollisionType.Collide1;
            profile.DrawType = DrawType.None;            
            profile.UpdateEmitter = FireworksSpread();
            profile.UpdateEmitterCooldownTime = 3;
            profile.InitMaxLifetimeID = "10";
            profile.Flags = GameObjectFlags.AddOnlyOnScreen;
            return profile;
        }

        public static IEmitter FireworksSpread()
        {
            ParamEmitter emitter = new ParamEmitter();
            emitter.EmitterID = "Fireworks";
            emitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.Random;
            emitter.VelocityAngleRange = 100;
            emitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Random;
            emitter.VelocityMagMin = 20;
            emitter.VelocityMagRange = 10;
            emitter.RotationType = ParamEmitter.EmitterRotation.VelocityAngle;
            return emitter;
        }
    }
}
