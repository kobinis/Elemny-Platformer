


namespace SolarConflict.NewContent.Emitters
{
    class EmitterFxSmoke
    {
        public static ParamEmitter Make()
        {
            ParamEmitter emitter = new ParamEmitter();            
            emitter.EmitterID = "ProjFxSmoke1";
            emitter.RefVelocityMult = 0.1f;
            emitter.MinNumberOfGameObjects = 3;
            emitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.Random;
            emitter.VelocityAngleRange = 360;
            emitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Random;
            emitter.VelocityMagMin = 0;
            emitter.VelocityMagRange = 1;

            emitter.RotationType = ParamEmitter.EmitterRotation.Random;
            emitter.RotationRange = 360;

            emitter.RotationSpeedType = ParamEmitter.EmitterRotationSpeed.Random;
            emitter.RotationSpeedRange = 10;

            emitter.SizeType = ParamEmitter.InitSizeType.Const;
            emitter.SizeBase = 1;

            emitter.LifetimeType = ParamEmitter.EmitterLifetime.Const;
            emitter.LifetimeMin = 25;
            return emitter;
        }
    }
}
