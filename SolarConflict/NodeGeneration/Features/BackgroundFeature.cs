using SolarConflict.Framework.World.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.GameWorld;
using SolarConflict.Framework.MetaGame.World;

namespace SolarConflict.Session.World.Generation.Features
{
    class BackgroundFeature : GenerationFeature
    {
        Vector3 lightColor = Vector3.One;
        public override GameObject GenerationLogic(Scene scene, SceneGenerator generator)
        {
            scene.SetBackground((int)generator.NodeInfo.Type);
            scene.GameEngine.AmbientColor = lightColor;
            return null;
        }
    }
}
