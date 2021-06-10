using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.Scenes;

namespace SolarConflict.Framework.Emitters.SceneRelated
{
    [Serializable]
    class SceneComponentEmitter : IEmitter
    {
        public SceneComponentType ComponentType;        
        public string ID { get; set; }
        bool AlwaysAdd = false;
        bool IsGlowing;
        public SceneComponentEmitter(SceneComponentType componentType, bool isGlowing = true)
        {
            ComponentType = componentType;
            IsGlowing = isGlowing;
        }


        public GameObject Emit(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0, int maxLifetime = 0, float? size = default(float?), Color? color = default(Color?), float param = 0)
        {
            if(gameEngine.Scene != null)
            {
                if(AlwaysAdd || gameEngine.Scene.SceneComponentSelector.GetProviderControl(ComponentType) == null)
                    gameEngine.Scene.SceneComponentSelector.AddComponent(ComponentType);
                if(IsGlowing)
                    gameEngine.Scene.SceneComponentSelector.GetProviderControl(ComponentType).IsGlowing = true;
            }
            return null;
        }
    }
}
