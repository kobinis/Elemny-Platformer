using System;
using Microsoft.Xna.Framework;
using XnaUtils;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace SolarConflict.Framework.Scenes.HudEngine.Components
{
    /// <summary>
    /// Shows the hitpoint and shield bar of near by agents not including player
    /// </summary>
    [Serializable]
    public class HitpointIndicator: IHudComponent
    {
        public GameObjectType ObjectsToShow { get; set; }

        public HitpointIndicator() 
        {
            ObjectsToShow = GameObjectType.Ship | GameObjectType.Mothership; 
        }


        public void Update(Scene scene, Agent player)
        {
          
        }

        public void Draw(SpriteBatch spriteBatch, Scene scene, Agent player, Vector2 pos)
        {
            spriteBatch.Begin();
            var gameEngine = scene.GameEngine;
            var camera = scene.Camera;
            //FactionType faction = FactionType.Player;

            var allobjects = new List<GameObject>(128);
            gameEngine.CollisionManager.GetAllObjectInRange(gameEngine.Camera.Position, 10000, allobjects);

            foreach (var gameObject in allobjects) //someday: refactor this class
            {
                if (gameObject.GetCollideWithMask() != GameObjectType.None  && (gameObject.GetFactionType() != FactionType.Player || gameObject.GetMeter(MeterType.Hitpoints)?.NormalizedValue < 1 || gameObject.GetMeter(MeterType.Shield)?.NormalizedValue <1) && camera.IsOnScreen(gameObject.Position, gameObject.Size))
                {
                    if ((!gameObject.IsCloaked || gameObject.GetFactionType() == FactionType.Player) && (gameObject.GetObjectType() & ObjectsToShow) > 0 )
                    {
                        Color color = Palette.SameFactionColor;

                        color = FactionColorIndicator.FactionToColor(gameObject.GetFactionType());
                    
                        if (gameObject.IsHostileToPlayer(gameEngine))
                        {
                            color = Palette.HostileColor;
                        }
                        else
                        {
                            if(gameObject.GetFactionType() == FactionType.Player)
                                 color = Palette.SameFactionColor;
                            else
                                color = Color.Green; //Palette.NonHostileColor;
                        }

                        var hitPointMeter = gameObject.GetMeter(MeterType.Hitpoints);
                        float value = hitPointMeter.Value;
                        float maxValue = hitPointMeter.MaxValue;
                        var shieldMeter = gameObject.GetMeter(MeterType.Shield);
                        if (shieldMeter != null)
                        {
                            value += shieldMeter.Value;
                            maxValue += shieldMeter.MaxValue;
                        }



                        Meter hitpoints = new Meter(value, maxValue);
                        DrawMeterDisplay(camera, gameObject,value/ maxValue,  color, 1);
                        DrawIndicator(camera, gameObject, Color.Yellow, MeterType.Energy, 0);

                        //if (gameObject == player)
                        //{
                        //    CreateIndicator(camera, gameObject, Color.Yellow, MeterType.Energy, 2);
                        //}
                    }
                }
            }
            spriteBatch.End();
        }

      

        /// <summary>
        /// Create Indicator line above the game object.
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="gameObject"> game object to add Indicator</param>
        /// <param name="color">color of the Indicator</param>
        /// <param name="meterType"></param>
        /// <param name="location">location of the Indicator, between the indicators (location 0 is the top)</param>
        private void DrawIndicator(Camera camera, GameObject gameObject, Color color, MeterType meterType, int location)
        {
            Meter meter = gameObject.GetMeter(meterType);

            if (meter != null && meter.MaxValue > 0)
            {
                DrawMeterDisplay(camera, gameObject, meter.Value/meter.MaxValue, color, location);
            }
        }
     
        private void DrawMeterDisplay(Camera camera, GameObject gameObject, float value, Color color, int location)
        {
            float radius = gameObject.Size * camera.Zoom + 10;
            float height = 20 * camera.Zoom;
            float yOffset = gameObject.Size * camera.Zoom * 1.2f + 30 * camera.Zoom + height * location;
            Vector2 position = camera.GetScreenPos(gameObject.Position);
            Rectangle meterRectangle = new Rectangle((int)(position.X - radius), (int)(position.Y - yOffset), (int)(radius * 2), (int)height);
            
            HudUtils.MeterDisplay(camera.SpriteBatch, value, meterRectangle, color);
        }

        public Rectangle GetSize() { return new Rectangle(); }
    }
}
