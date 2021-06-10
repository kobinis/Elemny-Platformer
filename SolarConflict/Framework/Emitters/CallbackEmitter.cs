using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.Framework.Emitters
{
    [Serializable]
    public class CallbackEmitter : IEmitter
    {

        public Func<GameEngine, GameObject, FactionType, Vector2, Vector2, float, GameObject> Action;

        public string ID { get; set; }

        public CallbackEmitter(string id, Func<GameEngine, GameObject, FactionType, Vector2, Vector2, float, GameObject> action = null)
        {
            ID = id;
            Action = action;
        }

        public GameObject Emit(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0, int maxLifetime = 0, float? size = default(float?), Color? color = default(Color?), float param = 0)
        {
            return Action(gameEngine, parent, faction, refPosition, refVelocity, refRotation);
        }
    }
}
