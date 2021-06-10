using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.SimpleGui;
using XnaUtils.SimpleGui.Controllers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject;

namespace SolarConflict
{
    [Serializable]
    class HullControl : GuiControl //TODO: change it
    {
        private Agent ship;

        public Agent Ship
        {
            get { return ship; }
            set { ship = value; TooltipText = AgentTooltipText();   }
        }

        ImageControl image;

        public HullControl(Agent ship, Vector2 pos, Vector2 size)
            : base(pos, size)
        {
            Ship = ship;          

            image = new ImageControl(null, Vector2.Zero, size * 0.9f);

            this.CursorOverColor = new Color(240, 240, 20, 200);
        }

        private string AgentTooltipText()
        {
            if(ship != null)
                return "#simage{" + ship.GetSprite().ID + ",300,300}#nl{}" + AgentUtils.DescribeAgentHull(ship);
            return null;
        }

        public override void UpdateLogic(InputState inputState)
        {
            if(ship != null)
                image.Sprite = ship.GetSprite();

            //if (IsCursorOn) //TODO: change
            //{
            //    if (ship != null)
            //    {
            //        TooltipText = "#simage{" + ship.GetSprite().ID +"}#nl{}"+ AgentUtils.DescribeAgentHull(ship);                     
            //    }
            //    else
            //        TooltipText = string.Empty;
            //}
        }

        protected override void DrawLogic(SpriteBatch sb, Color? color = null)
        {
            base.DrawLogic(sb, color);
            if (ship != null)
            {
                image.Position = this.Position;
                image.Draw(sb);                
            }
        }
    }
}
