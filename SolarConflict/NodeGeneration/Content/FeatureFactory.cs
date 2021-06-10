using SolarConflict.Framework.World.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.NodeGeneration.Features
{
    public static class FeatureFactory
    {
        public static GenerationFeature MakePlanetsFature()
        {
            PositionFeature planets = new PositionFeature();
            planets.AddChild("Earth");
            planets.AddChild("Moon");
            planets.AddChild("Earth");
            planets.AddChild("Moon");
            planets.AddChild("Earth");
            planets.AddChild("Earth");
            return planets;
        }

        public static GenerationFeature MakeDemonAlterFeature()
        {
            GenerationFeature feature = "DemonAlter";
            ParamEmitter asteroids = ParamEmitter.MakePositionSpreadParam(5, 0);
            asteroids.PosAngleBase = -60;
            asteroids.EmitterID = "SmallAsteroid1";
            asteroids.PosRadMin = 750;
            feature.AddChild(new EmitterFeature(asteroids));
            return feature;
        }
    }

}
