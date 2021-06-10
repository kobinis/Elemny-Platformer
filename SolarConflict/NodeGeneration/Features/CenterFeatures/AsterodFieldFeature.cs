using SolarConflict.Framework.World.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolarConflict.GameWorld;

namespace SolarConflict.NodeGeneration.Features.CenterFeatures
{
    class AsterodFieldFeature:GenerationFeature
    {
        //TODO: add position
        public override GameObject GenerationLogic(Scene scene, SceneGenerator generator)
        {
            scene.AddObjectRandomlyInCircle("Asteroid1", 500, 20000);
            scene.AddObjectRandomlyInCircle("SmallAsteroid1", 800, 21000);

           // scene.AddObjectRandomlyInCircle("Asteroid1", 1000, 80000, 71000);
            return null;
            
        }
    }
}
