using SolarConflict.Framework.World.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolarConflict.GameWorld;
using Microsoft.Xna.Framework;
using XnaUtils;
using SolarConflict.Framework.Emitters;

namespace SolarConflict.NodeGeneration.Features
{
    class PositionFeature:GenerationFeature
    {
        ParamEmitter paramEmitter;
        FeatureWrapperEmitter emitterWrapper;
        int numberOfCallsPerChiled = 1;
        public GameObject parent;

        public PositionFeature()
        {
            emitterWrapper = new FeatureWrapperEmitter();
            paramEmitter = new ParamEmitter();
            paramEmitter.PosAngleType = ParamEmitter.EmitterPosAngle.Range;
            paramEmitter.PosAngleBase = 0;
            paramEmitter.PosAngleRange = 360;
            paramEmitter.PosRadMin = 20000;
            paramEmitter.PosRadType = ParamEmitter.EmitterPosRad.Range;
        }


        public override GameObject Generation(Scene scene, SceneGenerator generator)
        {
            parent = generator.Centerpice;
            paramEmitter.PosRadMin = generator.SunRadius * 3; //Temp
            paramEmitter.PosRadRange = generator.SceneRadius; //Temp;

            GenerationLogic(scene, generator);
            Vector2 oldLocalPos = this.LocalPosition;
            emitterWrapper.SceneGenerator = generator;
            emitterWrapper.features = children;
            paramEmitter.Emitter = emitterWrapper;
            paramEmitter.MinNumberOfGameObjects = children.Count* numberOfCallsPerChiled; //move to add chiled
            paramEmitter.Emit(scene.GameEngine, parent, Faction, Vector2.Zero, Vector2.Zero, Rotation);
            //for (int i = 0; i < children.Count * numberOfCallsPerChiled; i++)
            //{
            //    int childIndex = (i / IndexDiv) % children.Count;
            //    float rad = FMath.TransformToRadius(Rand.NextFloat(), maxRad, minRad);                
            //    PraramEmitter.Emit(scene.GameEngine, null, Faction, )
            //    //children[i].Generation(scene, generator);
            //}           
            LocalPosition = oldLocalPos;
            return null;
        }
    }
}
