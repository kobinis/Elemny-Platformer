using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using SolarConflict.Framework;
using System.Runtime.Serialization;
using XnaUtils;

namespace SolarConflict
{    
    [Serializable]
    public class SoundEmitter : IEmitter {
        private string id;

        public string ID {
            get { return id; }
            set { id = value; }
        }

        public string EffectID {
            get {
                return _effectID;
            }
            set {
                _effectID = value;                
                _effect = _effectID == null ? null : AudioBank.Inst.GetSound(_effectID);
            }
        }
        string _effectID;

        [NonSerialized]
        private SoundEffect _effect; //add pitch
        public float Volume;


        public SoundEmitter() {
            Volume = 1;
        }

        public SoundEmitter(string effectId, float volume) {
            Volume = volume;
            EffectID = effectId;
        }        

        public GameObject Emit(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0,
            int maxLifetime = 0, float? size = null, Color? color = null, float param = 0) {
            if(gameEngine.SoundEngine != null)
                gameEngine.SoundEngine.AddSoundToQue(gameEngine.Camera, _effect, refPosition, Volume); 
            return null; //maybe return a "soundGameObject that can stop the sound??"
        }

        public IEmitter Load(string path) {
            return null;
        }

        [OnDeserialized]
        public void OnDeserializedMethod(StreamingContext context) {
            this.EffectID = EffectID;
        }
    }
}
