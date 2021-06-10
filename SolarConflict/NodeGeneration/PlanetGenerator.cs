using Microsoft.Xna.Framework;
using SolarConflict.Framework.MetaGame.World;
using SolarConflict.GameWorld;
using SolarConflict.NodeGeneration.PlanetGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XnaUtils;

namespace SolarConflict.NodeGeneration
{
    class PlanetGenerator
    {
        
        public static void GeneratePlanets(SceneGenerator generator, Scene scene )
        {
           
            NodeInfo info = generator.NodeInfo;
            if (info.Type == Framework.World.MetaGame.NodeType.RedSun || info.Type == Framework.World.MetaGame.NodeType.Vile)
            {
                int planetNum = generator.Rand.Next(5) + 1;
                for (int i = 0; i < planetNum; i++)
                {
                    //PlanetData data = new PlanetData(info.Name + (i + 1).ToString(), "earthV4", "earthV4cities", 1500, new Vector3(0.15f, 0.75f, 1.0f), new Vector3(0.141f, 1, 0.956f) * 0.8f);
                    PlanetData data = new PlanetData(FMath.Rand.Next());
                    var planet = PlanetQuickStart.Make(data).Emit(scene.GameEngine, generator.Centerpice, Framework.FactionType.Neutral, FMath.ToCartesian (generator.SunRadius * 2f + i / (float)planetNum * generator.SceneRadius * 1.3f, FMath.Rand.NextAngle()), Vector2.Zero, 0);
                    //info.Name + (i + 1).ToString( = new PlanetData(info.Name + (i+1).ToString(), "Earth", )
                }

            }
        }
    }
}
