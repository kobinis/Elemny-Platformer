
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.Scenes;

namespace SolarConflict.Session.World.MissionManagment.GlobalObjectives
{
    [Serializable]
    class PlayerCommandObjective : MissionObjective
    {
        private PlayerCommand _playerCommands;
        private ObjectiveStatus _state;
        public PlayerCommandObjective(PlayerCommand playerCommands)
        {
            _playerCommands = playerCommands;
            _state = ObjectiveStatus.Ongoing;
        }

        public override ObjectiveStatus CheckStatus(Mission mission, Scene scene)
        {
            if (scene.PlayerAgent != null)
            {
                if (scene.PlayersManager.players[0].IsCommandClicked(_playerCommands))
                {
                    _state = ObjectiveStatus.Completed;                    
                }
            }

            return _state;
        }

        public override string GetObjectiveText()
        {
            return null;
        }

        public override Vector2? GetPosition()
        {
            return null;
        }

        public override float GetRadius()
        {
            return 0;
        }
    }
}


//PlayersManager.players[i]