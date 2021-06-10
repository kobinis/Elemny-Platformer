//SystemGroup
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XnaUtils;
using XnaUtils.Graphics;
using SolarConflict.Framework.Agents.Systems;

namespace SolarConflict
{   
    [Serializable] 
    public class SystemGroup:AgentSystem
    {
        [Serializable]
        private struct InnerSystem
        {
            public InnerSystem(AgentSystem system, bool isActivating, bool isActivatable) //add cooldown
            {
                this.System = system;
                this.IsActivating = isActivating;
                this.IsActivatable = isActivatable;
            }            
            public AgentSystem System;
            public bool IsActivating; //??change name
            public bool IsActivatable;
            //Activate from if other was Activated

            public InnerSystem GetWorkingCopy()
            {
                InnerSystem innerSystemClone = (InnerSystem)MemberwiseClone();
                innerSystemClone.System = System.GetWorkingCopy();
                return innerSystemClone;               
            }
        }

        List<InnerSystem> systems;

        public SystemGroup()
        {
            systems = new List<InnerSystem>();
        }

        //public override void OnDamageSustained(float amount) {
        //    systems.Do(s => s.System.OnDamageSustained(amount));
        //}

        public override bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate)
        {
            bool wasActivated = false;
            foreach (var innerSytstem in systems)
            { 
                wasActivated |= (innerSytstem.System.Update(agent, gameEngine, initPosition, initRotation, tryActivate && innerSytstem.IsActivatable) & innerSytstem.IsActivating);
            }
            return wasActivated;
        }

        public override void Draw(Camera camera, Agent agent, Vector2 initPosition, float initRotation, DrawType drawType = DrawType.Alpha)
        {
            foreach (var innerSytstem in systems)
            {
                innerSytstem.System.Draw(camera, agent, initPosition, initRotation, drawType);
            }
        }

        public override AgentSystem GetWorkingCopy()
        {
            SystemGroup systemGroupClone = (SystemGroup)MemberwiseClone();
            systemGroupClone.systems = new List<InnerSystem>(systems.Count);
            foreach (var system in systems)
            {
                systemGroupClone.systems.Add(system.GetWorkingCopy());
            }
            return systemGroupClone;
        }

        public void AddSystem(AgentSystem system, bool isActivating = true, bool isActivatable = true)
        {
            systems.Add(new InnerSystem(system, isActivating, isActivatable));
        }

        public override float GetCooldownTime()
        {
            float cooldownTime = 0;

            foreach (var item in systems)
            {
                cooldownTime = Math.Max(cooldownTime, item.System.GetCooldownTime());
            }

            return cooldownTime;
        }

        public override float GetCooldown()
        {
            float cooldown = 0;

            foreach (var item in systems)
            {
                cooldown = Math.Max(cooldown, item.System.GetCooldown());
            }

            return cooldown;
        }

        public T GetSystem<T>() where T : AgentSystem {            
            return systems.Select(s => s.System as T).FirstOrDefault(s => s != null);
        }
    }
}
