using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using System.Diagnostics;

namespace SolarConflict
{
    [Serializable]
    class EmitterIdHolder : IEmitter
    {
        private string id;
        private IEmitter emitter;

        public IEmitter Emitter
        {
            get {
                if (emitter == null)
                    emitter = ContentBank.Inst.GetEmitter(id);
                return emitter;
            }
        }

        public string ID
        {
            get { return id; }
            set { id = value; }
        }
       
        public EmitterIdHolder(string id)
        {
            this.id = id;
        }

        public EmitterIdHolder(string id, IEmitter emitter)
        {
            this.id = id;
            this.emitter = emitter;
        }

        public GameObject Emit(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0,
            int maxLifetime = 0, float? size = null, Color? color = null, float param = 0)
        {
            if (emitter == null)
            {
                emitter = ContentBank.Inst.GetEmitter(id);
                //Debug.Assert(emitter.GetType() != typeof(EmitterIdHolder), "Emitter ID: " + id + " Was not found!");                
            }
            return emitter.Emit(gameEngine, parent, faction, refPosition, refVelocity, refRotation, refRotationSpeed, maxLifetime, size, color, param);
        }   

    }
}
