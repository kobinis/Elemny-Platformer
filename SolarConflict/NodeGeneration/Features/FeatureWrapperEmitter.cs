using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using SolarConflict.Framework.World.Generation;
using SolarConflict.GameWorld;

namespace SolarConflict.NodeGeneration.Features
{
    public class FeatureWrapperEmitter : IEmitter
    {
        public string ID { get; set;}
        public List<GenerationFeature> features;
        public SceneGenerator SceneGenerator;
        public bool IsSetFaction;
        public bool IsGlobalPosition;
        public bool IsSetRotation;
        public int Index;                

        public GameObject Emit(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0, int maxLifetime = 0, float? size = default(float?), Color? color = default(Color?), float param = 0)
        {
            var feature = features[Index];
            Index++;

            if (IsGlobalPosition)
                feature.Position = refPosition;
            else
                feature.LocalPosition = refPosition;
            if (IsSetRotation)
                feature.Rotation = refRotation;
            if (size != null)
                feature.Size = size.Value;
            if (IsSetFaction)
                feature.SetFaction(faction);
            feature.SetParentObject(parent);
            return feature.Generation(gameEngine.Scene, SceneGenerator);            
        }
    }
}
