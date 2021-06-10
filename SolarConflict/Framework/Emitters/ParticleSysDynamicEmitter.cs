using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarConflict.Framework.Emitters
{
    [Serializable]
    public class ParticleSysDynamicEmitter : IEmitter
    {
        public string ID { get; set; }
        [NonSerialized]
        ParticleSystem system;
        public ValueUpdader ParamA;
        public ValueUpdader ParamB;
        public float VelocityMult;


        public ParticleSysDynamicEmitter(string id, ParticleSystem system, ValueUpdader paramA, ValueUpdader paramB)
        {
            ID = id;
            ParamA = paramA;
            ParamB = paramB;
            this.system = system;
            VelocityMult = 1;
        }

        public GameObject Emit(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0, int maxLifetime = 0, float? size = null, Color? color = null, float param = 0)
        {
            system.addParticle(new Vector3(refPosition.X, refPosition.Y, 0), VelocityMult * new Vector3(refVelocity.X, refVelocity.Y, 0), ParamA.GetValue(gameEngine, parent), ParamA.GetValue(gameEngine, parent));
            return null;
        }
    }
}
