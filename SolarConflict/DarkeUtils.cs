using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SolarConflict
{
    class ExplostionTest
    {
        static Vector3 particleVelocity = new Vector3(0, 0, 0.1f);
        /// <summary>
        /// Creates explosion, this is static stateless function, only created shards are persistent.
        /// Intensity is expected between 0.1 and 1.0 where 0.1 is fairly small explosion and 1.0 very large
        /// </summary>
        /// <param name="position"></param>
        /// <param name="intensity"></param>
        /// <param name="particleSystem"></param>
        public static void MakeNormalExplosion(Vector2 position, float intensity, ParticleSystem particleSystem, SolarConflict.GameContent.Activities.PromoAndTest.TestScene scene)
        {
            //intensity = 1.1f;
            //Clamp intensity
            float clampedIntensity = Math.Max(intensity, 0.1f);
            float radius = clampedIntensity * 200;

            Vector3 v3position = new Vector3(position.X, position.Y, 0);


            //Create core of the explosion
            //Based on radius let's decide how many particles we need, surface of circle is PI * r ^ 2, so let's consider slower growing pow of r with whatever constant suites us and compensante with size of particles
            int coreCount = (int)(Math.Pow(radius, 1.2f) * 0.2f);

            for (int i = 0; i < coreCount; i++)
            {
                float distance;
                Vector3 offset = DarkeUtils.getCircleSurfacePoint3(1.0f, out distance) * radius; //considering unit half sphere here
                float temperature = 8000 + (1 - distance) * 7000; //hotter at the center
                float size = 5f + (1 - distance) * 10f; //bigger at center with minimum size 5 at the edge
                particleSystem.addParticle(v3position + offset, particleVelocity, temperature, size);
            }

            //for shard purpose we will remap our intensity range to 1-5 range and we create randomness +/- 1, so we need range 1-4 and cap is +2 as c# rng is ceil exclusive
            //feel free to increase randomness range to + 3 or whatever you like
            int shardCountLow = (int)Math.Ceiling(intensity * 3) + 1;
            int shardCount = DarkeUtils.random.Next(shardCountLow, shardCountLow + 2);

            for (int i = 0; i < shardCount; i++)
            {
                Vector3 direction = DarkeUtils.GetCirclePoint3(1); //position on unit circle
                Vector3 positionOffset = direction * radius * 0.75f;   //Random on circle
                Vector3 velocity = direction * (float)DarkeUtils.GetRandomNumber(300 + clampedIntensity * 200, 500 + clampedIntensity * 200) ;
                float temperature = (float)DarkeUtils.GetRandomNumber(9000, 15500);
                float size = (float)DarkeUtils.GetRandomNumber(5, 13) + clampedIntensity * 8;
                float lifeSpan = (float)DarkeUtils.GetRandomNumber(0.4f, 1.5f); //Between one to two seconds coz why not

                //I will just use reference to scene to add burning shards here, final system should of course fully respect your game object system but this was fastest for me to design
                scene.burningShards.Add(new BurningShard(v3position + positionOffset, velocity, lifeSpan, temperature, size, particleSystem));
            }

         


        }
    }

    class BurningShard
    {
        float lifeSpan;
        float timer;
        Vector3 velocity;
        Vector3 position;
        float startSize = 5f;
        float startTemperature = 12000f;

        Vector3 particleVelocity = new Vector3(0, 0, 0.1f); //just a little nudge up that wont be visible without perspective camera but that doesn't matter
        ParticleSystem particleSystem;
        float elapsed = 1f / 60f; //Fixed time step, expecting one step to be 16.666~ ms 
       
        public BurningShard(Vector3 position, Vector3 velocity, float lifeSpan, float startTemperature, float startSize, ParticleSystem particleSystem)
        {
            this.particleSystem = particleSystem;
            this.position = position;
            this.velocity = velocity;
            this.startTemperature = startTemperature;
            this.lifeSpan = lifeSpan;
            this.startSize = startSize;
        }

        public void Update()
        {
            if (timer > lifeSpan)
                return; //Ofc at this point gameObject should be recycled or treated as garbage

            timer += elapsed; 
            position += velocity * elapsed;

            //phase of shards life, we are using it to make particles smaller towards end of lifetime as well as colder;
            float inversePhase = 1 - timer / lifeSpan;
            float temperature = (inversePhase / (1 + inversePhase)) * 2 * startTemperature; //google: plot (x / (1 + x)) * 2
            float size = inversePhase * startSize + 1f; //just clamping min size, no point to spawn zero scaled particles

            particleSystem.addParticle(position, particleVelocity, temperature, size);
        }
    }


    public static class DarkeUtils
    {

        public static Random random;


        static public void Init()
        {
            random = new Random();
        }

        static public Vector3 GetCirclePoint3(float r)
        {
            double angle = MathHelper.TwoPi * random.NextDouble();

            float x = (float)(r * Math.Sin(angle));
            float y = (float)(r * -Math.Cos(angle));

            return new Vector3(x, y, 0);
        }

        static public Vector3 getCircleSurfacePoint3(float r)
        {
            double angle = MathHelper.TwoPi * random.NextDouble();

            float distance = (float)GetRandomNumberUniform(0, r);

            float x = (float)(Math.Sin(angle) * distance);
            float y = (float)(-Math.Cos(angle) * distance);

            return new Vector3(x, y, 0);
        }

        static public Vector3 getCircleSurfacePoint3(float r, out float distance)
        {
            double angle = MathHelper.TwoPi * random.NextDouble();
            distance = (float)GetRandomNumberUniform(0, r);

            float x = (float)(Math.Sin(angle) * distance);
            float y = (float)(-Math.Cos(angle) * distance);

            return new Vector3(x, y, 0);
        }

        static public double GetRandomNumberUniform(double minimum, double maximum)
        {
            return Math.Sqrt(random.NextDouble()) * (maximum - minimum) + minimum;
        }

        static public Vector3 getHalfSphereSurfacePoint(float r)
        {
            float x, y, z;
            double alpha, beta;
            alpha = GetRandomNumber(-Math.PI / 2, Math.PI / 2);
            beta = GetRandomNumber(0, 2 * Math.PI);

            x = (float)(r * Math.Sin(alpha) * Math.Cos(beta));
            y = (float)(r * Math.Sin(alpha) * Math.Sin(beta));
            z = (float)(r * Math.Cos(alpha));

            return new Vector3(x, y, z);
        }

        static public double GetRandomNumber(double minimum, double maximum)
        {
            return random.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}
