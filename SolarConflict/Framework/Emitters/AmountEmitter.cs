using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SolarConflict.Framework.Emitters
{
    /// <summary>
    /// Calls inner emitter N times
    /// </summary>
    [Serializable]
    class AmountEmitter : IEmitter
    {
        public string ID { get; set; }
        public int N;
        public IEmitter Emitter;

        public AmountEmitter(string emitterID, int n)
        {
            Emitter = ContentBank.Inst.GetEmitter(emitterID);
            N = n;
        }

        public GameObject Emit(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0, int maxLifetime = 0, float? size = null, Color? color = null, float param = 0)
        {
            for (int i = 0; i < N; i++)
            {
                Emitter.Emit(gameEngine, parent, faction, refPosition, refVelocity, refRotation, refRotationSpeed, maxLifetime, size, color, param);
            }
            return null;
        }
    }
}
