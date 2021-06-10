using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XnaUtils;

namespace SolarConflict.Framework.Emitters
{
    [Serializable]
    public class HierarchyEmitter : IEmitter
    {
        public string ID { get; set; }
        public int NumberOfObjects { get; set; }
        public int NumberOfObjectsRange { get; set; }
        public bool PassParent { get; set; }
        public int Size = 1;

        private List<IEmitter> _emitterList; //First, middle last

        
        public HierarchyEmitter()
        {
            PassParent = true;
            _emitterList = new List<IEmitter>();
        }

        public void AddEmitter(IEmitter emitter)
        {            
            _emitterList.Add(emitter);
            NumberOfObjects++;
        }

        public void AddEmitter(String id)
        {
            _emitterList.Add(ContentBank.Inst.GetEmitter(id));
        }

        public GameObject Emit(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0, int maxLifetime = 0, float? size = null, Color? color = null, float param = 0)
        {
            Vector2 direction = FMath.ToCartesian(1, refRotation);
            GameObject gameObject = null;
            GameObject prevGameObject = null;
            Vector2 postion = refPosition;
            int numberOfObjects;
            if (NumberOfObjectsRange > 0)
                numberOfObjects = NumberOfObjects + gameEngine.Rand.Next(NumberOfObjectsRange);
            else
                numberOfObjects = NumberOfObjects;
            for (int i = 0; i < NumberOfObjects; i++)
            {
                //TODO: update the index
                //TODO: can add sinus perpendicular to direction ??
                prevGameObject = gameObject;
                gameObject = _emitterList[i % _emitterList.Count].Emit(gameEngine, null, faction, postion, refVelocity, refRotation, refRotationSpeed, maxLifetime, size, color, param);
                if(prevGameObject != null)
                    prevGameObject.Parent = gameObject;
                postion += direction * Size;
            }
            if(PassParent && gameObject != null)
                gameObject.Parent = parent;
            return gameObject;
        }
    }
}
