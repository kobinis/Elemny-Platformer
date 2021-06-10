using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using SolarConflict.Session.World.MissionManagment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.NodeGeneration.NodeProcesess
{
    [Serializable]
    class TeslaMission : GameProcess
    {
        private Vector2 _position;

        public TeslaMission(Vector2 position)
        {
            _position = position;
        }

        public override void InitProcess(GameEngine gameEngine)
        {

            var tesla = gameEngine.AddGameObject("Roadster", FactionType.Neutral, _position + Vector2.One * 100);            
            var theCollector = gameEngine.AddGameObject("Roadster", FactionType.Neutral, _position);
        }

        public override void Update(GameEngine gameEngine)
        {
            
        }

        public override GameProcess GetWorkingCopy()
        {
            return this;
        }

        private Mission GenerateMission()
        {
            return null;
        }
    }
}
