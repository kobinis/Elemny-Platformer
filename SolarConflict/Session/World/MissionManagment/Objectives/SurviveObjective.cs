using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SolarConflict.Session.World.MissionManagment.Objectives {
    [Serializable]
    class SurviveObjective : MissionObjective {

        private bool _died;
        private Agent _originalAgent;        

        public override ObjectiveStatus CheckStatus(Mission mission, Scene scene) {
            if (_died)
                return ObjectiveStatus.Failed;

            _originalAgent = _originalAgent ?? scene.PlayerAgent;

            if (!(_originalAgent?.IsActive ?? false)) {
                _died = true;
                return ObjectiveStatus.Failed;
            }

            return ObjectiveStatus.Completed;
        }

        public override string GetObjectiveText() {
            return "Survive";
        }

        public override Vector2? GetPosition() {
            return null;
        }

        public override float GetRadius() {
            return 0f;
        }
    }
}
