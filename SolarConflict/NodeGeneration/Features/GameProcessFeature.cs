using SolarConflict.Framework.World.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolarConflict.GameWorld;
using SolarConflict.Framework;

namespace SolarConflict.NodeGeneration.Features
{
    class GameProcessFeature:GenerationFeature
    {
        private GameProcess gameProcess;

        public GameProcessFeature(GameProcess gameProcess)
        {
            this.gameProcess = gameProcess;
        }

        public override GameObject GenerationLogic(Scene scene, SceneGenerator generator)
        {
            scene.GameEngine.AddGameProcces(gameProcess.GetWorkingCopy());
            return base.GenerationLogic(scene, generator);
        }
    }
}
