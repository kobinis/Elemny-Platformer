using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SolarConflict.Framework;
using SolarConflict.Framework.Emitters;

namespace SolarConflict
{

    class PARTICLE_MANAGER
    {
        public static List<ParticleSystem> particleSystems = new List<ParticleSystem>();
        public static Dictionary<string, ParticleSystem> systems;

        public static ParticleSystem Get(string id)
        {
            return systems[id];
        }

        public static void Initialize(ContentManager content, GraphicsDevice device)
        {
            systems = new Dictionary<string, ParticleSystem>();
            //AddParticleSystem(new SmokePS(content, device), 11000, 10);
            //AddParticleSystem(new FirePS(content, device), 11000, 10);
            //AddParticleSystem(new GreenSmokePS(content, device), 12000, 10);

        }

        public static ParticleSystem AddParticleSystem(ParticleSystem system, float defaultParamA, float defaultParamB)//, ?float defaultParameterA,)
        {
            particleSystems.Add(system);
            ContentBank.Inst.AddContent(new ParticleSystemEmitter(system.GetType().Name, system, defaultParamA, defaultParamB));
            systems.Add(system.GetType().Name, system);
            return system;
        }

        public static void Update(float elapsed)
        {
            foreach (ParticleSystem system in particleSystems)
            {
                system.update(elapsed);
            }
        }

        public static void Draw(Matrix view, Matrix projection)
        {
            foreach (ParticleSystem system in particleSystems)
            {
                system.setCamera(view);
                system.setProjection(projection);
                system.draw();
            }
        }
    }
}
