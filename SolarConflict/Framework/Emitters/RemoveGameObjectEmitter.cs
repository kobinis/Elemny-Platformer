using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SolarConflict.Framework.Emitters
{
    [Serializable]
    class RemoveGameObjectEmitter : IEmitter
    {
        public string ID { get { return "RemoveGameObjectEmitter"; } set { } }

        public GameObject Emit(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0, int maxLifetime = 0, float? size = null, Color? color = null, float param = 0)
        {
            if(parent != null)
            {
                parent.IsActive = false;
            }
            return null;
        }
    }
}
