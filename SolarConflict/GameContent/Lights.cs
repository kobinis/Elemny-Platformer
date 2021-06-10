using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent {
    static class Lights
    {        
        
        
        /// <summary>Light so weak it'd need to more or less be in contact with a GameObject to noticeably illuminate it.</summary>
        public static PointLight ContactLight(Vector3 baseColor)
        {
            return new PointLight(baseColor, 300, 0.1f);
        }

        public static PointLight ContactLight(Color baseColor)
        {
            return ContactLight(baseColor.ToVector3());
        }

        public static PointLight SmallLight(Color baseColor)
        {
            return new PointLight(baseColor.ToVector3(), 150, 0.1f);
        }
        

        public static PointLight MediumLight(Color baseColor)
        {
            return new PointLight(baseColor.ToVector3(), 1000, 0.2f);
        }
        
        public static PointLight LargeLight(Color baseColor)
        {
            return new PointLight(baseColor.ToVector3(), 6000, 2);
        }

        public static PointLight HugeLight(Color baseColor)
        {
            return new PointLight(baseColor.ToVector3(), 150000, 2);
        }                
    }
}
