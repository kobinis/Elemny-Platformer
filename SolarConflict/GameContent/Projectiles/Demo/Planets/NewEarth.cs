using Microsoft.Xna.Framework;
using SolarConflict.NodeGeneration.PlanetGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarConflict.GameContent.Projectiles.Demo.Planets
{
    class NewEarth
    {
        public static IEmitter Make()
        {
            //PlanetData data = new PlanetData("New Earth", "EarthV3", "earthV2cities", 1500);
            //PlanetData data = new PlanetData("New Earth", "earthV4", "earthV4cities", 1500, new Vector3(0.15f, 0.75f, 1.0f), new Vector3(0.141f, 1, 0.956f) * 0.8f);
            //PlanetData data = new PlanetData("Mars", "marsV1", "marsV1cities", 1500, new Vector3(0.870f, 0.533f, 0.247f), new Vector3(1.2141f, 0.4f, 0.156f) * 0.8f);

            PlanetData data = new PlanetData("New Earth", "earthV4", "earthV4cities", 1500, new Vector3(0.15f, 0.75f, 1.0f), new Vector3(0.141f, 1, 0.956f) * 0.8f);
            //PlanetData data = new PlanetData("Mars", "marsV1", "earthV2cities", 1500, new Vector3(0.870f, 0.533f, 0.247f), Vector3.Zero);

            return PlanetQuickStart.Make(data);
        }
    }
}
