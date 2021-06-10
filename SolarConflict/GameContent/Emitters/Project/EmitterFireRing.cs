using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.GameContent.Projectiles.Shots;
using SolarConflict.GameContent;

namespace SolarConflict.NewContent.Emitters
{
    class EmitterFireRing
    {
        public static ParamEmitter Make()
        {
            return MakeWithTweaks(typeof(EmitterFireRing).Name);
        }

        public static ParamEmitter MakeWithTweaks(string outputID)
        {
            var innerEmitterID = typeof(FireShot1).Name;
            ParamEmitter emitter = new ParamEmitter(ContentBank.Inst.GetEmitter(innerEmitterID));
            emitter.MinNumberOfGameObjects = 20;
            emitter.RangeNumberOfGameObject = 10;
            emitter.VelocityAngleRange = MathHelper.ToDegrees(360);
            emitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.Random;
            emitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Random;
            emitter.VelocityMagMin = 10f;
            emitter.VelocityMagRange = 5f;
            emitter.RotationType = ParamEmitter.EmitterRotation.Random;
            emitter.RotationRange = 360;
            return emitter;
        }
    }
}
