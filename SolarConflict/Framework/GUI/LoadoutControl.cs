using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.SimpleGui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils.SimpleGui.Controllers;
using XnaUtils;
using XnaUtils.Graphics;

namespace SolarConflict
{
    //change it
    class LoadoutControl : GuiControl
    {
        private AgentLoadout loadout;

        public AgentLoadout Loadout
        {
            get { return loadout; }
            set { loadout = value; if(loadout != null) TooltipText = loadout.FullDescription;  agent = loadout?.MakeGameObject(null) as Agent;  }
        }

        //ImageControl image;

        [NonSerialized]
        private Agent agent;
        [NonSerialized]
        private Camera camera;

        public LoadoutControl(AgentLoadout loadout, Vector2 pos, Vector2 size)
            : base(pos, size)
        {
            Loadout = loadout;

            Sprite sprite = null;
            if(loadout != null)
                 sprite = loadout.GetSprite();
            

            //image = new ImageControl(sprite, Vector2.Zero, size * 0.9f);
            
            this.CursorOverColor = new Color(240, 240, 20, 200);
            camera = new Camera();
            agent = loadout?.MakeGameObject(null) as Agent;
        }

        public override void UpdateLogic(InputState inputState)
        {
        }

        protected override void DrawLogic(SpriteBatch sb, Color? color = null)
        {
            base.DrawLogic(sb, color);

            if (agent != null)
            {
                agent.DrawInGui(sb, GetRectangle());    
            }          
        }

        private float CalcScale()
        {
            return FMath.FindScale(new Vector2(agent.Sprite.Width, agent.Sprite.Height), this.HalfSize * 2);
        }

    }
}
