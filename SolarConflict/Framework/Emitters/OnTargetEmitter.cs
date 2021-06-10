using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SolarConflict.Framework.Emitters
{
    [Serializable]
    class OnTargetEmitter : IEmitter
    {
        public string ID { get; set; }
        public IEmitter emitter;
        public float range;

        public OnTargetEmitter(IEmitter emitter, float range)
        {
            this.emitter = emitter;
            this.range = range;
        }

        

        public GameObject Emit(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0, int maxLifetime = 0, float? size = null, Color? color = null, float param = 0)
        {
            var agent = parent.GetAgentAncestor();
            var target = agent.GetTarget(gameEngine, TargetType.Enemy);
            if(target != null)
            {
                Vector2 diff = target.Position - parent.Position;                
                if (diff.LengthSquared() <= range * range)
                {
                    refPosition = target.Position;
                }
                else
                {
                    refPosition = diff / (diff.Length() + 0.01f) * range;
                }
            }
            return emitter.Emit(gameEngine, parent, faction, refPosition, refVelocity, refRotation, refRotationSpeed, maxLifetime, size, color, param);
        }
    }
}
