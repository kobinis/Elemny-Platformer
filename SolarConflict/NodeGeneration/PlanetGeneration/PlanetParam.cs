using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SolarConflict.NodeGeneration.PlanetGeneration
{
    public enum PlanetType
    {
        Barren,
        Magma,
        Terra,
        Toxic
    }


    public static class PlanetParam
    {
        public static Vector2 sizeLimit = new Vector2(1200, 2000);


        //Surface colors
        public static Vector3 redMin = new Vector3(0.43f, 0.05f, 0.00f);
        public static Vector3 redMax = new Vector3(0.88f, 0.37f, 0.15f);
        public static Vector3 blueMin = new Vector3(0.14f, 0.20f, 0.23f);
        public static Vector3 blueMax = new Vector3(0.39f, 0.50f, 0.55f);
        public static Vector3 brownMin = new Vector3(0.27f, 0.18f, 0.06f);
        public static Vector3 brownMax = new Vector3(0.64f, 0.53f, 0.37f);
        public static Vector3 greenMin = new Vector3(0.17f, 0.26f, 0.02f);
        public static Vector3 greenMax = new Vector3(0.60f, 0.76f, 0.40f);
        public static Vector3 snowMin = new Vector3(0.87f, 0.88f, 0.89f);
        public static Vector3 snowMax = new Vector3(0.93f, 0.96f, 0.98f);
        public static Vector3 sandMin = new Vector3(0.62f, 0.59f, 0.42f);
        public static Vector3 sandMax = new Vector3(0.88f, 0.86f, 0.62f);
        public static Vector3 grayMin = new Vector3(0.17f, 0.18f, 0.18f);
        public static Vector3 grayMax = new Vector3(0.78f, 0.78f, 0.78f);
        public static Vector3 greenLightMin = new Vector3(0.51f, 0.71f, 0.32f);
        public static Vector3 greenLightMax = new Vector3(0.86f, 0.95f, 0.60f);
        public static Vector3 blackMin = new Vector3(0, 0, 0);
        public static Vector4 black = new Vector4(0, 0, 0, 1);
        public static Vector3 iceMin = new Vector3(0.26f, 0.43f, 0.61f);
        public static Vector3 iceMax = new Vector3(0.59f, 0.66f, 0.74f);
        public static Vector3 whiteMin = new Vector3(0.78f, 0.78f, 0.78f);
        public static Vector3 whiteMax = new Vector3(0.98f, 0.98f, 0.98f);

        //Height phases
        public static Vector2 phaseLimit1 = new Vector2(0.0f, 0.25f); //Min, max
        public static Vector2 phaseLimit2 = new Vector2(0.3f, 0.55f); //Min, max
        public static Vector2 phaseLimit3 = new Vector2(0.6f, 0.88f); //Min, max




        public static Vector2 toxicLimits = new Vector2(0.05f, 0.3f);
        public static Vector2 waterLimits = new Vector2(0, 0.4f);
        public static Vector2 magmaLimits = new Vector2(0.05f, 0.15f);

        public static Vector2 magmaEmittivity = new Vector2(1.2f, 2.2f) * 8;
        public static Vector2 toxicEmittivity = new Vector2(1.2f, 2.2f) * 3;

        //Atmosphere values
        public static Vector2 atmoIntLimits = new Vector2(0.4f, 1.1f);

        public static Vector3 atmoMagmaMin = new Vector3(1f, 0.270f, 0.160f);
        public static Vector3 atmoMagmaMax = new Vector3(1f, 0.650f, 0.160f);
        public static Vector3 atmoBarrensMin = new Vector3(1f, 0.650f, 0.160f);
        public static Vector3 atmoBarrensMax = new Vector3(0.984f, 0.945f, 0.796f);

        public static Vector3 atmoTerraMin = new Vector3(0.721f, 0.898f, 1f);
        public static Vector3 atmoTerraMax = new Vector3(0.219f, 0.431f, 1f);

        public static Vector3 atmoToxicMin = new Vector3(0.219f, 1f, 0.654f);
        public static Vector3 atmoToxicMax = new Vector3(0.180f, 1f, 0.403f);

        //Fluid color
        public static Vector3 magmaColorMin = new Vector3(0.74f, 0.1f, 0.00f);
        public static Vector3 magmaColorMax = new Vector3(1.0f, 0.3f, 0.02f);

        public static Vector3 waterColorMin = new Vector3(0.18f, 0.59f, 1.00f);
        public static Vector3 waterColorMax = new Vector3(0.19f, 0.96f, 1.00f);


    }
}
