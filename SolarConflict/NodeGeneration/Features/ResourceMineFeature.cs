using SolarConflict.Framework.World.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolarConflict.GameWorld;
using SolarConflict.NodeGeneration.Features;
using SolarConflict.GameContent.Events;
using XnaUtils;

namespace SolarConflict.Framework.World.Generation.Profiles
{
    class ResourceMineFeature : AgentFeature
    {

       // public
            //MiningStation

        public override GameObject GenerationLogic(Scene scene, SceneGenerator generator)
        {
            AgenEmitterID = $"ResourceMine{Level}";

            var result = base.GenerationLogic(scene, generator);

            scene.AddGameObject("MiningStation", Position + FMath.ToCartesian(1000, FMath.Rand.NextAngle()));

            var guardianSpawner = GuardianSpawner.MakeEvent(result);
            scene.GameEngine.AddGameProcces(guardianSpawner);

            return result;
        }
    }
}
