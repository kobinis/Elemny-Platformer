using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.Scenes;

namespace SolarConflict.Session.World.MissionManagment.GlobalObjectives
{
    [Serializable]
    class OpenSceneComponentObjective : MissionObjective
    {
        public SceneComponentType ComponentToOpen;

        public OpenSceneComponentObjective(SceneComponentType componentToOpen)
        {
            ComponentToOpen = componentToOpen;
        }

        public override ObjectiveStatus CheckStatus(Mission mission, Scene scene)
        {
            if (scene.SceneComponentSelector.LastComponentSelected == ComponentToOpen)
                return ObjectiveStatus.Completed;
            return ObjectiveStatus.Ongoing;
        }

        public override string GetObjectiveText()
        {
            return string.Empty; //Maybe null;
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
