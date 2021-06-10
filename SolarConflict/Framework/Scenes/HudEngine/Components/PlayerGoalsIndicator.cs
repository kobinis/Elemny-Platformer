using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SolarConflict.Session.World.MissionManagment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict.Framework.Scenes.HudEngine.Components
{
    /// <summary>
    /// Shows main mission on HUD
    /// </summary>
    [Serializable]
    class PlayerGoalsIndicator : IHudComponent //TODO: change name
    {
        Rectangle _rectangle;
        Mission _mission; //Remove
        string _text; //Remove
        RichTextParser _parser;

        public PlayerGoalsIndicator()
        {
            _parser = new RichTextParser(Game1.font);
        }

        public void Draw(SpriteBatch spriteBatch, Scene scene, Agent player, Vector2 pos)
        {
            spriteBatch.Begin(blendState: BlendState.AlphaBlend);
            _parser.Draw(spriteBatch, pos);
            spriteBatch.End();
        }

        public Rectangle GetSize()
        {
            return _rectangle;
        }

        public void Update(Scene scene, Agent player)
        {
            var missions = scene.GetMissions().Where(t => !t.IsHidden && !t.IsDismissable).ToList(); //Change to show the main mission, / selected mission
            _text = null;
            if (missions.Count >= 1)
            {
                _mission = missions[0];
                _text = _mission.GetActiveText();
               
            }

            if (_text != null)
            {
                _parser.Text = _text;
                Vector2 size = _parser.MeasureText();
                _rectangle = new Rectangle(0, 0, (int)size.X, (int)size.Y);
            }
            else
            {
                _text = string.Empty;
                _parser.Text = _text;
                _rectangle = new Rectangle(0, 0, 1, 1);
            }
        }
    }
}