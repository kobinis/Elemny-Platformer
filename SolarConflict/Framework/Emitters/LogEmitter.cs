using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SolarConflict.Framework.Emitters
{
    /// <summary>
    /// Needs to write a am error message to log
    /// </summary>
    class LogEmitter : IEmitter
    {
        public string ID { get; set; }
        
        private string message;
        public LogEmitter(string messeage)
        {
            this.message = messeage;
        }

        public GameObject Emit(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0, int maxLifetime = 0, float? size = default(float?), Color? color = default(Color?), float param = 0)
        {
            //TODO: add to log
            return null;
        }
    }
}
