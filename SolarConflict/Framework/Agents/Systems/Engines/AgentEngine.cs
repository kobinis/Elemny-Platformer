using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XnaUtils;
using SolarConflict.Framework.Agents.Systems;
using SolarConflict.Framework;

namespace SolarConflict
{
    [Serializable]
    public class AgentEngine : AgentSystem 
    {
        public ActivationCheck activationCheck;
        public float force;
        public float maxSpeed;

        //Display
        public LightObject LightObject; //TODO: add fadein and fadeout
        public IEmitter trailEmitter;
        public float trailSpeed;
        public Color color;        

        // private bool isActive;

        public AgentEngine(float force, float maxSpeed)
        {
            color = Color.White;
            activationCheck = new ActivationCheck();
            this.force = force;
            this.maxSpeed = maxSpeed;
            trailEmitter = ContentBank.Inst.GetEmitter("ProjEngineTrail"); //check if 
            trailSpeed = 6;

            LightObject = new LightObject(new Vector3(1f, 0.5f, 0.2f), 500, 0f, 0f);
        }

        public override bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate)
        {
            if (activationCheck.Check(agent, tryActivate))
            {
                activationCheck.DrainCost(agent);
                Vector2 initRotVector = new Vector2((float)Math.Cos(initRotation), (float)Math.Sin(initRotation));
                Vector2 rotatedForce = -FMath.RotateVector(initRotVector, Vector2.UnitX);
                float refRotationSpeed = (float)gameEngine.Rand.Next(2) - 1f;
                agent.ApplyForce(rotatedForce * force, maxSpeed);
                if (!agent.IsCloaked)
                {
                    trailEmitter?.Emit(gameEngine, agent, 0, initPosition, agent.Velocity - trailSpeed * rotatedForce, initRotation, refRotationSpeed);
                    LightObject.Position = initPosition;
                    if(gameEngine.Camera.IsOnScreen(LightObject)) // remove
                        gameEngine.LightsPerFrame.Add(LightObject); //Only if on screen
                    LightObject.Light.Intensity = Math.Min(LightObject.Light.Intensity + 0.1f, 5f);
                }
                return true;
            }
            else
                LightObject.Light.Intensity *= 0.9f;
            //ActivityManager.Inst.AddToast(LightObject.Light.Intensity.ToString(), 5);
            return false;
        }

        public override LightObject GetSelfLights(GameEngine gameEngine)
        {
            return LightObject;
        }

        public override AgentSystem GetWorkingCopy()
        {
            var engineSystem = (AgentEngine)MemberwiseClone();
            engineSystem.LightObject = this.LightObject.GetWorkingCopy();
            engineSystem.LightObject.Light = this.LightObject.Light.GetWorkingCopy();
            return engineSystem;
        }
    }
}
