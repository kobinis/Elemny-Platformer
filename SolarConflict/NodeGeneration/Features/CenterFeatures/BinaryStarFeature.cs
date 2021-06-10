using SolarConflict.Framework.World.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolarConflict.GameWorld;
using Microsoft.Xna.Framework;
using SolarConflict.NewContent.Projectiles;

namespace SolarConflict.NodeGeneration.Features.CenterFeatures
{
    //Add Position
    //Two rotating stars and a portal( p = 0.3) that warps you to a diffrent node
    class BinaryStarFeature:GenerationFeature
    {
        IEmitter sunEmitter = ContentBank.Inst.GetEmitter(typeof(OrbitingSunWithBackground).Name);
        public override GameObject GenerationLogic(Scene scene, SceneGenerator generator)
        {
            float sunDistanceFromZero = 10000;
            DummyObject center = new DummyObject(Position);
            var sun1 = sunEmitter.Emit(scene.GameEngine, center, Framework.FactionType.Neutral, -Vector2.UnitX * sunDistanceFromZero, Vector2.Zero,0);
            var sun2 = sunEmitter.Emit(scene.GameEngine, center, Framework.FactionType.Neutral, Vector2.UnitX * sunDistanceFromZero, Vector2.Zero, 0);
            center.Size = (sun1.Position - sun2.Position).Length() + sun1.Size + sun2.Size;
            return center;
        }
    }
}
