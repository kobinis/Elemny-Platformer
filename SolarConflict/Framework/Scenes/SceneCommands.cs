using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using XnaUtils;

namespace SolarConflict
{
    //ExtensionMethods ???
    public static class SceneCommands
    {

        public static void AddObjectInLine(this Scene scene, string objectId, Vector2 start, Vector2 end, FactionType faction = FactionType.Neutral, float spacingMultiplier = 1)
        {            
            var diff = end - start;            
            var rotation = MathHelper.ToDegrees((float)Math.Atan2(diff.Y, diff.X));

            // Spawn one object
            var firstObject = scene.AddGameObject(objectId, faction, start, rotation);
            
            var spacing = firstObject.Size * spacingMultiplier;

            // Spawn the rest
            var distance = diff.Length();
            if (diff != Vector2.Zero) {                
                int n = (int)Math.Ceiling(distance / spacing * 2);
                Vector2 step = diff / (float)n;
                for (int i = 1; i < n; i++) {
                    scene.AddGameObject(objectId, faction, start + i * step, rotation);
                }
            }
                
            
        }

        public static void AddObjectRandomlyInLocalCircle(this Scene scene, string objectId, int n, float rad, Vector2? center = null, float minRad = 0, int seed = 0, FactionType faction = FactionType.Neutral)
        {
            Random random;
            if (seed == 0)
                random = scene.GameEngine.Rand;
            else
                random = new Random(seed);

            for (int i = 0; i < n; i++)
            {
                float angle = (float)random.NextDouble() * MathHelper.TwoPi;
                float radius = (float)Math.Sqrt(random.NextDouble() * (rad * rad - minRad * minRad) + minRad * minRad);
                Vector2 position = FMath.ToCartesian(radius, angle);
                if (center != null)
                    position += (Vector2)center;
                //ToDo      
                float rotation = (float)random.NextDouble() * 360;
                scene.AddGameObject(objectId, faction, position, rotation); //change it
            }
        }

        public static void AddObjectRandomlyInCircle(this Scene scene, string objectID, int amount, float maxRadius, float minRadius = 0, int randomSeed = 0, FactionType faction = FactionType.Neutral)
        {
            Random random;

            if(randomSeed == 0)
                random = scene.GameEngine.Rand;
            else
                random = new Random(randomSeed);

            for (int i = 0; i < amount; i++)
            {
                float angle = (float)random.NextDouble() * MathHelper.TwoPi;
                float radius = (float)Math.Sqrt(random.NextDouble() * (maxRadius * maxRadius - minRadius * minRadius) + minRadius * minRadius);
                Vector2 position = FMath.ToCartesian(radius, angle);
                //ToDo      
                float rotation = (float)random.NextDouble() * 360;
                scene.AddGameObject(objectID, faction, position, rotation); //change it
            }
        }

        public delegate GameObject FactoryFunc(FactionType faction, Vector2 position, float rotation);
        
        private static void AddObjectRandomlyInCircle(this Scene scene, FactoryFunc func, int n, float rad, float minRad = 0, int seed = 0, FactionType faction = FactionType.Neutral)
        {
            Random random;
            if (seed == 0)
                random = scene.GameEngine.Rand;
            else
                random = new Random(seed);

            for (int i = 0; i < n; i++)
            {
                float angle = (float)random.NextDouble() * MathHelper.TwoPi;
                float radius = (float)Math.Sqrt(random.NextDouble() * (rad * rad - minRad * minRad) + minRad * minRad);
                Vector2 position = FMath.ToCartesian(radius, angle);
                //ToDo      
                float rotation = (float)random.NextDouble() * MathHelper.TwoPi;
                GameObject gameObject = func.Invoke(faction, position, rotation);
                gameObject.SetFactionType(faction);
                scene.GameEngine.AddGameObject(gameObject);                                
                //scene.AddGameObject(objectId, faction, position, rotation);

            }
        }


        public static void AddObjectRandomlyInCircle(this GameEngine gameEngine, string objectID, int amount, float maxRadius, float minRadius = 0, int randomSeed = 0, FactionType faction = FactionType.Neutral)
        {
            Random random;

            if (randomSeed == 0)
                random = gameEngine.Rand;
            else
                random = new Random(randomSeed);

            for (int i = 0; i < amount; i++)
            {
                float angle = (float)random.NextDouble() * MathHelper.TwoPi;
                float radius = (float)Math.Sqrt(random.NextDouble() * (maxRadius * maxRadius - minRadius * minRadius) + minRadius * minRadius);
                Vector2 position = FMath.ToCartesian(radius, angle);
                //ToDo      
                float rotation = (float)random.NextDouble() * 360;
                gameEngine.AddGameObject(objectID, faction, position, rotation); //change it
            }
        }






    }
}
