using XnaUtils.Graphics;
using SolarConflict.GameContent.Projectiles;
using Microsoft.Xna.Framework;

namespace SolarConflict.GameContent.Emitters {
    /// <summary>Equivalent of the GunFlashFx muzzle flash for lasers.</summary>
    class LaserFlashFx {
        public static ProjectileProfile Make() {            
            return LightFlash(Lights.ContactLight(Color.Red));
        }

        /// <remarks>TODO: find me a home. Where does stuff like this go? Parametrized factory methods and such.</remarks>        
        public static ProjectileProfile LightFlash(PointLight light) {
            var profile = new ProjectileProfile();

            profile.CollisionType = CollisionType.Effects;
            profile.InitMaxLifetimeID = "1";
           // profile.Lights = new ILight[] { light.Clone() as ILight };


            return profile;
        }
    }
}