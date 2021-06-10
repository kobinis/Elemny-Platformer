using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;
using XnaUtils.Graphics;
using XnaUtils.SimpleGui;

namespace SolarConflict.Framework.GUI
{
    public class AgentGuiControl : GuiControl //TODO: add size
    {
        public Agent Agent;
        private Camera _camera;
        public AgentGuiControl(Agent agent) : base(Vector2.Zero, Vector2.One * 150)
        {
            Agent = agent;
            _camera = new Camera();
        }

        public override void UpdateLogic(InputState inputState)
        {
            //TooltipText = AgentUtils.DescribeStatsAndAbilities(Agent);
        }

        public override void Draw(SpriteBatch sb, Color? color = default(Color?))
        {
            base.Draw(sb, color);
            if (Agent != null)
            {
               
                

               
               
                Agent.DrawInGui(sb,GetRectangle());
                
                if(Agent.IsNotActive)
                {
                    base.Draw(sb, new Color(60, 60, 60, 155));
                    sb.Draw(TextureBank.Inst.GetSprite("UnderConstruction"), this.GetRectangle(), new Color(255,255,255,200));                    
                }
            }
        }
    }
}
