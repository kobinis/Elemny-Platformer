using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SolarConflict.Framework.Emitters
{
    /// <summary>
    /// TauntEmitter - Set parent as the target of all gameonjects in range
    /// that alrady target gameobjects from the same faction as parent
    /// </summary>
    [Serializable]
    class TauntEmitter : IEmitter
    {
        public float Range { get; set; }

        public string ID { get; set; }

        public TauntEmitter(float range)
        {
            Range = range;
        }

        public GameObject Emit(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0, int maxLifetime = 0, float? size = null, Color? color = null, float param = 0)
        {
            List<GameObject> objects = new List<GameObject>();
            gameEngine.CollisionManager.GetAllObjectInRange(parent.Position, Range, objects);
            foreach (var item in objects)
            {
                var target = item.GetTarget(gameEngine, TargetType.Enemy);
                if(target != null && target.GetFactionType() == parent.GetFactionType())
                {
                    item.SetTarget(parent, TargetType.Enemy);
                }
            }
            return null;
        }

        
    }
}
