using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Serialization;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using XnaUtils;
using XnaUtils.Graphics;

namespace SolarConflict
{
    /// <summary>
    /// Draws a projectile according to the size
    /// </summary>
    [Serializable]
    public class DrawPlanet : BaseDraw
    {
        //public Sprite Sprite;
        public Sprite CityMap;
        public Vector3 AtmosphereColor;
        public float AtmosphereIntensity;
        public Vector3 CityColor;
        public float RotationSpeed;



        public DrawPlanet(string cityMapID, Vector3 atmosphereColor, float atmosphereIntensity, Vector3 cityColor, float rotationSpeed)
        {
            //Sprite = sprite;
            CityMap = Sprite.Get(cityMapID);
            AtmosphereColor = atmosphereColor;
            AtmosphereIntensity = atmosphereIntensity;
            CityColor = cityColor;
            RotationSpeed = rotationSpeed;
        }

        public override void Draw(Camera camera, Projectile projectile)
        {
            Sprite sprite = projectile.profile.Sprite;        
            Camera.NormalMapEffect.Parameters["Rotation"].SetValue(projectile.Lifetime * RotationSpeed);
            Camera.NormalMapEffect.Parameters["RoughnessMap"].SetValue(CityMap.Texture); //Hijacked for city mask
            Camera.NormalMapEffect.Parameters["AtmosphereColor"].SetValue(AtmosphereColor);
            Camera.NormalMapEffect.Parameters["CityColor"].SetValue(CityColor);
            Camera.NormalMapEffect.Parameters["AtmosphereIntensity"].SetValue(AtmosphereIntensity);
            //Camera.NormalMapEffect.Parameters["DominantLightColor"].SetValue(lightColor); //To Do: Uncomment and hook up Vector 3 light color here
            Vector2 lightPos = new Vector2(0, 0) - camera.GetWorldPos(ActivityManager.ScreenCenter);
            Camera.NormalMapEffect.Parameters["DominantLightPos"].SetValue(new Vector3(lightPos.X, lightPos.Y, 0));
            camera.CameraDraw(projectile.profile.Sprite.Texture, projectile.Position, projectile.Rotation, new Vector2(0.5f, 1)* projectile.Size * projectile.profile.ScaleMult, projectile.color);          

        }

    }
}
