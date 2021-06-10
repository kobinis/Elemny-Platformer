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
    /// Rotates a system according to analong input
    /// </summary>
    [Serializable]
    public class TurretSystemHolder : AgentSystem
    {
        public AgentSystem system;
        public Vector2 position;
        public int AnalogIndex;
        public bool FixToCenter = false;

        public Sprite Sprite;
        public float scale;
        private Vector2 pos;
        private float angle;

        public TurretSystemHolder(AgentSystem system, Vector2 position, string textureId)
        {
            AnalogIndex = 1;
            Sprite = Sprite.Get(textureId);
            this.system = system;
            this.position = position;
            scale = 1;
        }

        public override bool Update(Agent agent, GameEngine engine, Vector2 initPosition, float initRotation, bool tryActivate)
        {
            if (FixToCenter)
            {
                pos = agent.Position;
            }
            else
            {
                pos = new Vector2(initPosition.X + position.X * agent.Heading.X - position.Y * agent.Heading.Y, initPosition.Y + position.X * agent.Heading.Y + position.Y * agent.Heading.X);
            }
            angle = (float)Math.Atan2(agent.analogDiractions[AnalogIndex].Y, agent.analogDiractions[AnalogIndex].X);
            return system.Update(agent, engine, pos, angle, tryActivate);
        }

        public override void Draw(Camera camera, Agent agent, Vector2 initPosition, float initRotation, DrawType drawType = DrawType.Alpha)
        {
            if (Sprite != null)
            {
                camera.CameraDraw(Sprite, pos, angle, 1f, Color.White);
            }
        }

        public override AgentSystem GetWorkingCopy()
        {
            TurretSystemHolder systemHolderClone = (TurretSystemHolder)MemberwiseClone();
            systemHolderClone.system = system.GetWorkingCopy();
            // systemHolderClone.Sprite = Sprite?.Clone() as Sprite;
            return systemHolderClone;
        }

        public override float GetCooldown()
        {
            return system.GetCooldown();
        }

        public override float GetCooldownTime()
        {
            return system.GetCooldownTime();
        }

    }
}
