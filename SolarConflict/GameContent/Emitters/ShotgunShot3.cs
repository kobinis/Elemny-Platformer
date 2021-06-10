using SolarConflict.GameContent;
using SolarConflict.NewContent.Projectiles;

namespace SolarConflict.NewContent.Emitters
{
    class ShotgunShot3
    {
        public static ParamEmitter Make()
        {
            return MakeWithTweaks(typeof(ShotgunShot3).Name);
        }

        public static ParamEmitter MakeWithTweaks(string outputID) {            
            ParamEmitter emitter = new ParamEmitter();
            emitter.EmitterID = typeof(KineticShot1).Name;
            emitter.MinNumberOfGameObjects = 25;
            emitter.RangeNumberOfGameObject = 5;
            emitter.VelocityAngleRange = 70;
            emitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.Random;
            emitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Random;
            emitter.VelocityMagMin = 20;
            emitter.VelocityMagRange = 5;
            emitter.RotationType = ParamEmitter.EmitterRotation.VelocityAngle;

            // Projectiles randomly fade out over time. That's an alternative to scaling their damage or decelerating them
            // Looks cool, but the latter might make more sense in terms of the interaction with armor (so shotguns would still be
            // moderately effective against lightly-armored targets at medium range, worthless against heavy armor)
            emitter.LifetimeType = ParamEmitter.EmitterLifetime.Random;
            emitter.LifetimeMin = (int)(60f);
            emitter.LifetimeRange = (int)(20f);

            return emitter;
        }
    }
}
