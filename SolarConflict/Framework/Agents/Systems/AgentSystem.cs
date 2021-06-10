using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XnaUtils;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject;
using XnaUtils.Graphics;

namespace SolarConflict.Framework.Agents.Systems {
    [Serializable]
    public abstract class AgentSystem:IEmitter
    {
        public string ID { get; set; }

        //void Equipped  // 
        //void Unequipped      
        //onZeroTime
        public abstract bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate = false);
        public virtual void Draw(Camera camera, Agent agent, Vector2 initPosition, float initRotation, DrawType drawType = DrawType.Alpha) { }
        // bool IsActive();

        public virtual float GetCooldown()
        {
            return 0;
        }

        public virtual float GetCooldownTime()
        {
            return 0;
        }

        public virtual LightObject GetSelfLights(GameEngine gameEngine)
        {
            return null;
        }

        public abstract AgentSystem GetWorkingCopy();

        /// <remarks>Generally called when the owning agent is spawned, changes sectors, etc.</remarks>
        public virtual void Reset() { }

        public virtual GameObject Emit(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0, int maxLifetime = 0, float? size = null, Color? color = null, float param = 0)
        {
            Agent agent = parent as Agent;
            if(agent != null)
            {
                AgentSystem systemToAdd = this.GetWorkingCopy();
                systemToAdd.Reset();
                agent.AddSystem(systemToAdd);                
            }
            return null;
        }
    }    
}
