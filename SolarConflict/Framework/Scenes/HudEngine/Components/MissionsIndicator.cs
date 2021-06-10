using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SolarConflict.Session.World.MissionManagment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;
using XnaUtils.Graphics;

namespace SolarConflict.Framework.Scenes.HudEngine.Components
{
    /// <summary>
    /// Shows arrows (+Distance and Icon) to current selected missions
    /// </summary>
    [Serializable]
    public class MissionsIndicator : IHudComponent
    {
        private Sprite _arrowTexture;
      //  private Dictionary<Mission, int> _fadeoutTimer;

        public MissionsIndicator()
        {
            _arrowTexture = Sprite.Get("missionTarget");// "smallFuzzyArrow");
           // _fadeoutTimer = new Dictionary<Mission, int>();
        }

        public void Update(Scene scene, Agent player)
        {

            foreach (var mission in scene.GetMissions())
            {
                if (player != null && mission.IsSelected && mission.GetPosition() != null )
                {
                    //if (scene.Camera.IsOnScreen(mission.GetPosition().Value, 0))
                    //{
                    //    if (_fadeoutTimer.ContainsKey(mission))
                    //    {
                    //        _fadeoutTimer[mission] += 1;
                    //    }
                    //    else
                    //    {
                    //        _fadeoutTimer.Add(mission, 0);
                    //    }
                    //}
                    //else
                    //{

                    //    if (_fadeoutTimer.ContainsKey(mission) && !scene.Camera.IsOnScreen(mission.GetPosition().Value, 500))
                    //    {
                    //        _fadeoutTimer[mission] = Math.Min(_fadeoutTimer[mission] - 1, 200);
                    //    }
                    //}

                }
            }
            //var timers = _fadeoutTimer.Keys.ToList();
            //foreach (var key in timers)
            //{
            //    if (_fadeoutTimer[key] < -10)
            //    {
            //        _fadeoutTimer.Remove(key);
            //    }
            //}
        }

        public void Draw(SpriteBatch spriteBatch, Scene scene, Agent player, Vector2 pos)
        {
            //List<Mission> missions = scene.MissionManager.GetSelectetMissions();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
            //Missions
            foreach (var mission in scene.GetMissions())
            {                
                if (mission.IsSelected && mission.GetPosition() != null && player != null)// && goal.IsSelected)
                {
                   // int fadetimer;
                    //_fadeoutTimer.TryGetValue(mission, out fadetimer);
                  //  byte alpha = (byte) Math.Min(Math.Max( 255 - fadetimer / 3,0),255);
                    Color color = mission.Color;
                    //color.A = alpha;
           
                    double distance = Math.Round(GameObject.DistanceFromEdge(mission.GetPosition().Value, player.Position, mission.GetRadius(), player.Size) / 100);
                    Sprite sprite = _arrowTexture;
                    HudUtils.DrawArrow(scene.Camera, mission.GetPosition().Value, mission.GetRadius(), sprite, color, 0.5f);
                }
            }
            spriteBatch.End();
        }

        public Rectangle GetSize()
        {
            return new Rectangle();
        }

    }
}
