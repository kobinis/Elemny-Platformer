using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaintPlay.XnaUtils
{
    class FxParticleEngine
    {
        List<Particle> addList;
        List<Particle> removeList;   
        List<Particle> particles;

        public FxParticleEngine()
        {
            addList = new List<Particle>();
            removeList = new List<Particle>();
            particles = new List<Particle>();
        }

        public void AddParticle(Particle particle)
        {
            particles.Add(particle);
        }

        public void Update()
        {
            
            foreach (Particle particle in particles)
            {

                if (!particle.IsDead())
                {
                    particle.Update();
                }
                else
                {
                    removeList.Add(particle);
                }                               
            }

            foreach (Particle particle in removeList)
            {
                particles.Remove(particle);                
            }
            removeList.Clear();
            //ToDo: add list

        }

        public void Draw()
        {
            foreach (Particle particle in particles)
            {
                particle.Draw();
            }
        }


        
    }
}
