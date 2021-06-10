using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SolarConflict.Framework.Emitters
{
    class CursorEmitter : IEmitter
    {
        public CursorEmitter(IEmitter emitter)
        {
            this.emitter = emitter;
        }

        public string ID { get; set; }
        public IEmitter emitter;

        public GameObject Emit(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0, int maxLifetime = 0, float? size = null, Color? color = null, float param = 0)
        {
            refPosition = gameEngine.Camera.GetWorldPos( gameEngine.Scene.InputState.Cursor.Position);
            return emitter.Emit(gameEngine, parent, faction, refPosition, refVelocity, refRotation, refRotationSpeed, maxLifetime, size, color, param);
        }
    }
}
