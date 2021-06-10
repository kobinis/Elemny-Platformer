using SolarConflict.Framework.World.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolarConflict.GameWorld;
using Microsoft.Xna.Framework;

namespace SolarConflict.NodeGeneration.Features
{
    class EmitterFeature : GenerationFeature
    {
        public IEmitter Emitter;
        public string EmitterID { set { Emitter = ContentBank.Inst.GetEmitter(value); } }
        public GameObject Parent;

        public EmitterFeature()
        {

        }

        public EmitterFeature(IEmitter emitter, GameObject parent = null)
        {
            Parent = parent;
            Emitter = emitter;
        }

        public override GameObject GenerationLogic(Scene scene, SceneGenerator generator)
        {
            return Emitter.Emit(scene.GameEngine, Parent, Faction, Position, Vector2.Zero, Rotation, param: Level);
        }

        public override void SetParentObject(GameObject gameobject)
        {
            Parent = gameobject;
        }
    }
}
