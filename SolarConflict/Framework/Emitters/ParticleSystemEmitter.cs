using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SolarConflict.Framework.Emitters
{
    [Serializable]
    public class ParticleSystemEmitter : IEmitter
    {
        public string ID { get; set; }
        public float parameterA;
        public float parameterB;
        [NonSerialized]
        ParticleSystem system;

        public ParticleSystemEmitter(string id, ParticleSystem system, float paramA, float paramB)
        {
            ID = id;
            parameterA = paramA;
            parameterB = paramB;
            this.system = system;
        }


        public GameObject Emit(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0, int maxLifetime = 0, float? size = null, Color? color = null, float param = 0)
        {
            //TODO: add rotation 
            float paramB = size.HasValue ? size.Value : parameterB;
            system.addParticle(new Vector3(refPosition.X, refPosition.Y, 0), new Vector3(refVelocity.X, refVelocity.Y, 0), parameterA, paramB);
            // system.addParticle(new Vector3(refPosition.X, -refPosition.Y, 0), new Vector3(refVelocity.X, -refVelocity.Y, 0), parent.GetColor().ToVector3(), parameterA, parameterB);
            return null;
        }

        [OnDeserialized]
        public void OnDeserializedMethod(StreamingContext context)
        {
            system = PARTICLE_MANAGER.Get(ID);
        }
    }
}
