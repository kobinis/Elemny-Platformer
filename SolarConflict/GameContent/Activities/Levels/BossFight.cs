using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.Agents.Systems;
using XnaUtils.Framework.Graphics;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Activities.Levels
{

    public class ControlSignalTransferSystem : AgentSystem
    {

        public override bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate = false)
        {
            if (agent.Parent != null && agent.Parent.IsActive)
            {
                Agent parent = agent.Parent as Agent;
                agent.ControlSignal = parent.ControlSignal;
                agent.analogDiractions[0] = parent.analogDiractions[0];
                agent.analogDiractions[1] = parent.analogDiractions[1];
            }
            return false;
        }
        public override AgentSystem GetWorkingCopy()
        {
            return this;//(AgentSystem)MemberwiseClone();
        }
    }

    public class ReletivePositionSystem : AgentSystem
    {
        //Also take controlsignals form parent, transfer force
        public Vector2 ReletivePosition;
        private SetPixel _pixel;
        private Camera _camera;
        private Sprite _sprite;
        private float _spriteRotation; //remove
        
        public ReletivePositionSystem(Vector2 reletivePosition)
        {
            ReletivePosition = reletivePosition;
            _pixel = new SetPixel(DrawPoint);
            _sprite = Sprite.Get("strat");
        }

        public override bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate = false)
        {            
            if(agent.Parent != null && agent.Parent.IsActive)
            {
                //if(ReletivePosition = )
                Vector2 rotatedVec = (agent.Parent as Agent).RotateVector(ReletivePosition);
                agent.Position = agent.Parent.Position + rotatedVec;
                agent.Velocity = agent.Parent.Velocity;
                agent.Rotation = agent.Parent.Rotation;                
                if (agent.appliedForce != Vector2.Zero)
                     agent.Parent.ApplyForce(agent.appliedForce, 150);
            }
            return false;
        }

        

        public override void Draw(Camera camera, Agent agent, Vector2 initPosition, float initRotation, DrawType drawType = DrawType.Alpha)
        {
            _camera = camera;
            _spriteRotation = FMath.GetRotation(agent.Parent.Position - agent.Position);
            GraphicsUtils.Line(camera.SpriteBatch, agent.Position, agent.Parent.Position, Color.White, _sprite.Width*2, _pixel);
        }

        public override AgentSystem GetWorkingCopy()
        {
            return (AgentSystem)MemberwiseClone();
        }

        public void DrawPoint(SpriteBatch sb, Vector2 point, Color color)
        {
            if (_camera != null)
                _camera.CameraDraw(_sprite, point, _spriteRotation, 1f, color);
        }
    }

    class BossFight : Scene
    {
       

        public override void InitScript(string parameters = null, ActivityParameters activityParameters = null)
        {
            base.InitScript(parameters, activityParameters);

            this.AddObjectRandomlyInLocalCircle("Asteroid1", 1000, 40000);            
            AddGameObject("PlayerClone", Framework.FactionType.Pirates1, -Vector2.UnitY * 1000);

            //var parent = ContentBank.Inst.GetGameObjectFactory("PrologShip1").MakeGameObject(GameEngine, null, Framework.FactionType.Player);
            //parent.SetControlType(AgentControlType.Player);
            var parent = AddGameObject("PrologShip1", Vector2.UnitY * 1000, faction: Framework.FactionType.Player, controlType: AgentControlType.Player);
            for (int i = 0; i < 2; i++)
            {
                var kid = AddGameObject("PrologShip1", Vector2.UnitY * 1000 + new Vector2( (i*2 -1) * 300, 300), faction: Framework.FactionType.Player, controlType: AgentControlType.None) as Agent;
                kid.Parent = parent;
                kid.AddSystem(new ControlSignalTransferSystem());
                kid.AddAfterSystem(new ReletivePositionSystem(new Vector2( -1, (i * 2 - 1)) * 100));
            }

            //GameEngine.AddList.Add(parent);
            
        }

        public static Activity ActivityProvider(string parameters = "")
        {
            return new BossFight();
        }
    }
}
