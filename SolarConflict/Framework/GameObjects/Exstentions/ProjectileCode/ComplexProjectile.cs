//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;

///// <summary>A slightly more versatile Projectile which might or might not be necessary.</summary>
//namespace SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode {
//    [Serializable]
//    class ComplexProjectile : Projectile {
//        /// <summary>Overrides profile.ImpactSpec if present.</summary>
//        CollisionInfo _impactSpec;

//        public Process.IProcess Process;
                
//        public override void Update(GameTime gameTime, GameEngine gameEngine) {
//            Process?.Update(new Process.ProcessContext(this, gameEngine, Position, Rotation)); // note that we use this object's position and rotation for initPosition,
//                // and initRotation, which might or might not be a kludge
//            base.Update(gameTime, gameEngine);            
//        }

//        public override CollisionInfo CollisionInfo {
//            get { return _impactSpec ?? profile.CollisionSpec; } // default to profile
//            set { _impactSpec = value; }
//        }
//    }
//}
