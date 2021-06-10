using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict.Framework
{
    public static class GameEngineExtensions
    {

        public static List<GameObject> AddObjectsInFormation(this GameEngine gameEngine, List<string> objectsIDs, FactionType factionType, Vector2 initPos, float rotation, float spacing = 40, int num = 0, int size = 0 , bool isSetGoal = true)
        {
            int number = objectsIDs.Count;
            if (num > 0)
            {
                number = num;
            }
            float maxSize = 0;

            List<GameObject> gameObjects = new List<GameObject>(number);
            for (int i = 0; i < number; i++)
            {
                var gameObject = gameEngine.AddGameObject(objectsIDs[i % objectsIDs.Count], factionType, Vector2.Zero, rotation);
                gameObjects.Add(gameObject);
                maxSize = Math.Max(maxSize, gameObject.Size * 2 + spacing);
            }
            float realSize = size == 0 ? maxSize : size;
            for (int i = 0; i < number; i++)
            {
                Vector2 position = initPos + FMath.GetFormationPosition(i, MathHelper.ToRadians(rotation)) * realSize;
                gameObjects[i].Position = position;
                if(isSetGoal)
                {
                    gameObjects[i].SetTarget(gameObjects[0], TargetType.Goal);
                }
            }

            return gameObjects;
        }

        
    }
}
