using Microsoft.Xna.Framework;
using SolarConflict.NodeGeneration.PlanetGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles
{
    class Mars
    {
        public static IEmitter Make()
        {
            //PlanetData data = new PlanetData("New Earth", "EarthV3", "earthV2cities", 1500);
            //PlanetData data = new PlanetData("New Earth", "earthV4", "earthV4cities", 1500, new Vector3(0.15f, 0.75f, 1.0f), new Vector3(0.141f, 1, 0.956f) * 0.8f);
            PlanetData data = new PlanetData("Mars", "marsV1", "earthV2cities", 1500, new Vector3(0.870f, 0.533f, 0.247f), new Vector3(0.141f, 1, 0.956f) * 0.8f);
            return PlanetQuickStart.Make(data);
        }
    }
}
