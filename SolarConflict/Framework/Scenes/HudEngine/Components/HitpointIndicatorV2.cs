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
    public class HitpointIndicatorV2 : IHudComponent
    {
        public GameObjectType ObjectsToShow { get; set; }

        float segmentCoreWidth = 13;
        int segmentOffset = 5;
       // int barPadding = 1;

        Rectangle shieldSourceRectangle = new Rectangle(2, 20, 2, 35);
        Rectangle barSourceRectangle = new Rectangle(2, 0, 1, 15);
        Rectangle segmentSource = new Rectangle(125, 0, 29, 29);
        Color shieldColor = new Color(200, 200, 48, 180);

        public HitpointIndicatorV2() 
        {
            ObjectsToShow = GameObjectType.Ship | GameObjectType.Mothership; 
        }

        

        public void Update(Scene scene, Agent player)
        {
          
        }

        public void Draw(SpriteBatch spriteBatch, Scene scene, Agent player, Vector2 pos)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            var gameEngine = scene.GameEngine;
            var camera = scene.Camera;
            //FactionType faction = FactionType.Player;

            var allobjects = new List<GameObject>(128);
            gameEngine.CollisionManager.GetAllObjectInRange(gameEngine.Camera.Position, 10000, allobjects);

            foreach (var gameObject in allobjects) //someday: refactor this class
            {
                if (gameObject.GetCollideWithMask() != GameObjectType.None &&  camera.IsOnScreen(gameObject.Position, gameObject.Size) && gameObject != player && ( gameObject.GetFactionType() != FactionType.Player || gameObject.GetMeter(MeterType.Hitpoints)?.NormalizedValue < 1 || gameObject.GetMeter(MeterType.Shield)?.NormalizedValue <1) )
                {
                    if (!gameObject.IsCloaked && (gameObject.GetObjectType() & ObjectsToShow) > 0)
                    {
                        Color color = Palette.SameFactionColor;
                        if (gameObject.IsHostileToPlayer(gameEngine))
                        {
                            color = Palette.HostileColor;
                        }
                        else
                        {
                            color = Color.Green; //Palette.NonHostileColor;
                        }



                        //color = FactionColorIndicator.FactionToColor(gameObject.GetFactionType());
                        var hitPointMeter = gameObject.GetMeter(MeterType.Hitpoints);
                        float value = hitPointMeter.Value;
                        float maxValue = hitPointMeter.MaxValue;
                        var shieldMeter = gameObject.GetMeter(MeterType.Shield);
                        if (shieldMeter != null)
                        {
                            value += shieldMeter.Value;
                            maxValue += shieldMeter.MaxValue;
                        }
                        DrawHpMeterDisplay(camera, gameObject, value/maxValue, color, 1);
                        Meter energyMeter = gameObject.GetMeter(MeterType.Energy);
                        if(energyMeter != null)
                            DrawShieldMeterDisplay(camera, gameObject, energyMeter, shieldColor, 0);

                        //if (gameObject == player)
                        //{
                        //    CreateIndicator(camera, gameObject, Color.Yellow, MeterType.Energy, 2);
                        //}
                    }
                }
            }
            spriteBatch.End();
        }
    

        private float GetUnderlayBarLength(int segmentCount, float zoomFactor)
        {

            //this will result in correct on screen length without rounding error, we basically do efficient simulation of rounding that would happen during placement of segmenets
            return ((float)((segmentCoreWidth + segmentOffset) * zoomFactor)) * (segmentCount - 1) + (segmentCoreWidth * zoomFactor); //+ (int)(3 * barPadding * zoomFactor);

            
        }
        
        //Segmented bar
        private void DrawHpMeterDisplay(Camera camera, GameObject gameObject, float normalizedValue, Color color, int location)
        {
            Vector2 position = camera.GetScreenPos(gameObject.Position);

            float zoomFactor = camera.Zoom ; //we are at 1.0 at maximal zoom


            //Calculate bar total width
            float width = (gameObject.Size * 2);
            //now we need to round up width to nearest amount of segments
            float segmentCoreWidth = 13;
            int offset = 5;

            int maxSegments = (int)Math.Floor(width / (segmentCoreWidth + offset));
            //now we get actuall width
            width = GetUnderlayBarLength(maxSegments, zoomFactor); 

            Vector2 finalPos = new Vector2(position.X - width * 0.5f, position.Y - gameObject.Size * 1.5f * zoomFactor + 35 * zoomFactor);
            Rectangle targetRectangle = new Rectangle((int)finalPos.X,(int)(finalPos.Y), (int)(width), (int)(15 * zoomFactor));

     
            HudUtils.BasicMeterDisplay(camera.SpriteBatch, targetRectangle, barSourceRectangle, color);

            Rectangle segmentTarget = new Rectangle((int)position.X - (int)(7 * zoomFactor), (int)(position.Y - gameObject.Size * zoomFactor) - (int)(7 * zoomFactor), (int)(segmentSource.Width * zoomFactor), (int)(segmentSource.Height * zoomFactor));

            Vector2 segmentPosition = new Vector2(finalPos.X - (7 * zoomFactor), (finalPos.Y) - (7 * zoomFactor));

            int toDraw = (int)Math.Ceiling(maxSegments * normalizedValue);
            for(int i = 0; i < toDraw; i++)
            {
               
                HudUtils.HpSegmentDisplay(camera.SpriteBatch, segmentPosition, segmentSource, color, zoomFactor);
                segmentPosition.X += ((segmentCoreWidth + offset) * zoomFactor);
            }

        }

        private void DrawShieldMeterDisplay(Camera camera, GameObject gameObject, Meter meter, Color color, int location)
        {
            Vector2 position = camera.GetScreenPos(gameObject.Position);

            float zoomFactor = camera.Zoom;

            //Calculate bar total width
            float width = (int)(gameObject.Size * 2);
            //now we need to round up width to nearest amount of segments

            int maxSegments = (int)Math.Floor(width / (segmentCoreWidth + segmentOffset));
            //now we get actuall width
            width = GetUnderlayBarLength(maxSegments, zoomFactor);

            Vector2 finalPos = new Vector2(position.X - width * 0.5f, position.Y - gameObject.Size * 1.5f * zoomFactor + 35 * zoomFactor);

            float value = meter.Value / meter.MaxValue;

            Rectangle targetRectangle = new Rectangle((int)finalPos.X, (int)(finalPos.Y) - (int)(zoomFactor * 10) + (int)(zoomFactor * 1), (int)(width * value), (int)(35 * zoomFactor)); 

            HudUtils.BasicMeterDisplay(camera.SpriteBatch, targetRectangle, shieldSourceRectangle, color);
        }

        private void DrawMeterDisplay(Camera camera, GameObject gameObject, Meter meter, Color color, int location)
        {
            float radius = gameObject.Size * camera.Zoom + 10;
            float height = 20 * camera.Zoom;
            float yOffset = gameObject.Size * camera.Zoom * 1.2f + 30 * camera.Zoom + height * location;
            Vector2 position = camera.GetScreenPos(gameObject.Position);
            Rectangle meterRectangle = new Rectangle((int)(position.X - radius), (int)(position.Y - yOffset), (int)(radius * 2), (int)height);

            float value = meter.Value / meter.MaxValue;
            HudUtils.MeterDisplay(camera.SpriteBatch, value, meterRectangle, color);
        }


        public Rectangle GetSize() { return new Rectangle(); }
    }
}
