using Microsoft.Xna.Framework;
using System;

namespace XnaUtils.Graphics {
    public static class LightingGlobals {
        /// <summary>By default, light intensity attenuates by a factor of (distance*c)^DefaultIntensityExponent,
        /// where c is some constant (see DefaultIntensityHalvingDistance)</summary>
        public static int DefaultIntensityExponent = 2;

        /// <summary>By default, light intensity attenuates by 50% at this distance</summary>
        public static float DefaultIntensityHalvingDistance = 200f;

        /// <summary>For saturation prevention, see shader.</summary>
        public static float MaxIntensity = 2f;


        /// <summary>The max number of lights to consider in-depth when illuminating an object.</summary>
        /// <remarks>Note that this is bounded above by the size of the normal map shader's LightDirections array.
        /// Lights are ignored in ascending order of illumination (so the light which illuminates an object least will be ignored first).</remarks>
        public static int MaxLightsPerObject = 32;

        /// <remarks>We work out most of our lighting in 2D, then convert everything to 3D directional lights and give their direction vectors some
        /// arbitrary Z component. This is that component. Note that the direction vectors point towards the lights, so this should generally be positive
        /// (for lights on the player's side of the screen)</remarks>
        public static float Tilt = 0.2f;        
    }


    //public interface IIntensityCalculator : ICloneable {
    //    float IntensityAt(float distance);

    //    float DistanceForIntensity(float targetIntensity);
    //}
    
    [Serializable]
    public class PointLight
    {               
        /// /// <summary>The base color of the light, before being modified by intensity.</summary>
        public Vector3 BaseColor;
        /// <remarks>The light's color at a given point will be BaseColor * intensityAtPoint.
        public float Intensity;
        public float Attenuation;
        public float Hotspot;
   
        //Simplified constructor
        public PointLight(Vector3 baseColor, float attenuation, float intensity)
        {
            BaseColor = baseColor;
            Attenuation = attenuation;
            Intensity = intensity;
            Hotspot = 0;
        }

        public PointLight(Vector3 baseColor, float attenuation, float intensity, float hotspot)
        {
            BaseColor = baseColor;
            Attenuation = attenuation;
            Intensity = intensity;
            Hotspot = hotspot;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public bool InRange(Vector2 lightPosition, Vector2 position)
        {
            Vector2 diff = lightPosition - position;
            float range = Attenuation + Hotspot;
            return Math.Abs( diff.X) < range &  Math.Abs( diff.Y) < range;
        }

        public PointLight GetWorkingCopy()
        {
            return (PointLight)MemberwiseClone();
        }
    }
        
}
