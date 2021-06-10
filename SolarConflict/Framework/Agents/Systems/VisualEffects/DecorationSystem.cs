using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils;
using XnaUtils.Graphics;
using SolarConflict.Framework.Agents.Systems;

namespace SolarConflict
{
    /// <summary>
    /// Adds visual decoration to agents: Blinking lights etc...
    /// </summary>
    [Serializable]
    public class DecorationSystem : AgentSystem
    {
        [Serializable]
        private struct Decoration
        {
            public Vector2 Position;
            public float TimeOffset;
            public Color Color;
        }


        public float Scale { set; get; }
        
        public Sprite Sprite;
        public Color DecorationColor;

        List<Decoration> decorationList;

        public float deltaTime = 0.05f;

        float time = 0;

        public DecorationSystem()
        {
            Scale = 1;
            decorationList = new List<Decoration>();
            Sprite = Sprite.Get("smallLight");
            DecorationColor = new Color(255, 200, 200, 255);
        }

        public void AddDecoration(Vector2 position, float timeOffset, Color? color = null)
        {
            Decoration decoration = new Decoration();           
            decoration.Position = position;
            decoration.TimeOffset = timeOffset;
            decorationList.Add(decoration);
        }

        public override bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate = false)
        {
            time += deltaTime;            
            return false;
        }

        public override void Draw(Camera camera, Agent agent, Vector2 initPosition, float initRotation, DrawType drawType = DrawType.Alpha)
        {
            //int randomOffset = 0;
            if(agent.Lifetime > 0 && !agent.IsCloaked)
            {
                foreach (var item in decorationList)
                {
                    float alpha = MathHelper.Clamp((float)(Math.Sin(time + item.TimeOffset * MathHelper.TwoPi)),0,1);
                    Color color = DecorationColor;
                    color.A = (byte)(DecorationColor.A * alpha);
                    Vector2 position = agent.RotateVector(item.Position);
                    camera.CameraDraw(Sprite, position + agent.Position, 0, Scale , color);                    
                }
            }            
        }
             
        public override AgentSystem GetWorkingCopy()
        {
            return (AgentSystem)MemberwiseClone();
        }
    }
}
