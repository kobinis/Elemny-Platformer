using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict.Framework
{
    /// <summary>
    /// Checks collisions between GameObjects
    /// </summary>
    [Serializable]
    public class CollisionManager
    {
        private Point _gridSize;        
        private List<GameObject>[,] _grid;
        private float _cellSize;
        private float _invCellSize;
        private GameEngine _gameEngine;
        private List<GameObject> _bigGameObjects;

        public CollisionManager(GameEngine gameEngine, int cellSize = 1500)
        {
            _bigGameObjects = new List<GameObject>();
            _gameEngine = gameEngine;
            _gridSize = new Point(32, 32);
            _cellSize = cellSize;
            _invCellSize = 1f / _cellSize;
            _grid = new List<GameObject>[_gridSize.X, _gridSize.Y];
            for (int x = 0; x < _gridSize.X; x++)
            {
                for (int y = 0; y < _gridSize.Y; y++)
                {
                    _grid[x, y] = new List<GameObject>();
                }
            }
        }

        public Vector2 RayCast(Vector2 origin, Vector2 direction, float maxLength, out GameObject gameObjectHit, GameObjectType gameObjectType, GameObject parent = null,  GameObjectType mask = GameObjectType.All)
        {
         //   maxLength = 1000;
            //Get all objects in possible range
            List<GameObject> objects = new List<GameObject>();
            direction.Normalize();
            GetAllObjectInRange(origin + direction *maxLength *0.5f, maxLength * 0.5f, objects, mask);
            
            
            //Vector2 dir = direction;
            direction *= maxLength;

            gameObjectHit = null;

            float A = direction.X * direction.X + direction.Y * direction.Y;

            float hitDistance = 1;
            for (int i = 0; i < objects.Count; i++)
            {
                if ((objects[i].GetCollideWithMask() & gameObjectType) == 0 || objects[i] == parent ) //TODO: add mask
                    continue;

                float B = 2 * (direction.X * (origin.X - objects[i].Position.X) + direction.Y * (origin.Y - objects[i].Position.Y));
                float C = (origin.X - objects[i].Position.X) * (origin.X - objects[i].Position.X) +
                          (origin.Y - objects[i].Position.Y) * (origin.Y - objects[i].Position.Y) -
                           objects[i].Size * objects[i].Size;

                float det = B * B - 4 * A * C;

               
                if ((A <= 0.0000001) || (det < 0))
                {   
                    //No hit
                }
                else
                 
                if (det == 0)
                {
                    //We touched the circle exactly
                    float newHitDistance = -B / (2 * A);
                    //If we have no hit beforehand or our hit distance improved (shorter is better)
                    if((gameObjectHit == null || newHitDistance < hitDistance) && newHitDistance > 0 && newHitDistance <= 1)
                    {
                        //Asign new candidate
                        hitDistance = newHitDistance;
                        gameObjectHit = objects[i];
                    }
                }
                else
                {
                    //We have two intersections and we want closer one
                    float newHitDistanceA = (float)((-B + Math.Sqrt(det)) / (2 * A));
                    float newHitDistanceB = (float)((-B - Math.Sqrt(det)) / (2 * A));

                    if((newHitDistanceA < 0 || newHitDistanceA > 1) && (newHitDistanceB < 0 || newHitDistanceB > 1))
                    {
                        continue;
                        //gameObject = null;
                        //hitDistance;
                    }

                   
                    //bit closer intersection point
                    if (newHitDistanceA < newHitDistanceB)
                    {
                        if (gameObjectHit == null || newHitDistanceA < hitDistance)
                        {
                            hitDistance = newHitDistanceA;
                            gameObjectHit = objects[i];
                        }      
                    }
                    else
                    {
                        if (gameObjectHit == null || newHitDistanceB < hitDistance)
                        {
                            hitDistance = newHitDistanceB;
                            gameObjectHit = objects[i];
                        }
                    }

                    
                }
            }
            if (gameObjectHit == null)
            {
                hitDistance = 1;
            }
            return origin + direction * hitDistance;//;  new Vector2(origin.X + hitDistance * direction.X, origin.Y + hitDistance * direction.Y);           
        }

        public void Update(IEnumerable<GameObject> gameObjects)
        {
            ClearGrid();
            _bigGameObjects.Clear();
            foreach (var gameObject in gameObjects)
            {                
                if (gameObject.Size * 2 < _cellSize) 
                {
                    AddGameObjectToGrid(gameObject); 
                }
                else
                {
                    _bigGameObjects.Add(gameObject);
                }
            }
            ApplyCollisions();
            CheckAndApplyWithList(_bigGameObjects, false); 
        }

        public void CheckAndApplyWithList(IEnumerable<GameObject> gameObjects, bool checkWithBig = true) //TODO: we can optimize here
        {
            foreach (var object1 in gameObjects)
            {
                Vector2 gridPos = object1.Position * _invCellSize;
                int x = FMath.Mod((int)gridPos.X, _gridSize.X);
                int y = FMath.Mod((int)gridPos.Y, _gridSize.Y);
                int range = (int)Math.Ceiling(object1.Size * _invCellSize + 0.1f);
                for (int dy = -range; dy <= range; dy++)
                {
                    for (int dx = -range; dx <= range; dx++)
                    {
                        int indexX = FMath.Mod(x + dx, _gridSize.X);
                        int indexY = FMath.Mod(y + dy, _gridSize.Y);
                        foreach (var object2 in _grid[indexX, indexY])
                        {
                            if (GameObject.CheckCollision(object1, object2))
                            {
                                object2.ApplyCollision(object1, _gameEngine);
                                object1.ApplyCollision(object2, _gameEngine);
                            }
                        }
                    }
                }
                if (checkWithBig)
                {
                    foreach (var object2 in _bigGameObjects)
                    {
                        if (GameObject.CheckCollision(object1, object2))
                        {
                            object2.ApplyCollision(object1, _gameEngine);
                            object1.ApplyCollision(object2, _gameEngine);
                        }
                    }
                }
            }
        }
        

        private void AddGameObjectToGrid(GameObject gameObject)
        {
            Vector2 gridPos = gameObject.Position * _invCellSize;
            int indexX = FMath.Mod((int)gridPos.X, _gridSize.X);
            int indexY = FMath.Mod((int)gridPos.Y, _gridSize.Y);
            _grid[indexX, indexY].Add(gameObject);
        }

        private void ClearGrid()
        {
            for (int y = 0; y < _gridSize.Y; y++)
            {
                for (int x = 0; x < _gridSize.X; x++)
                {
                    _grid[x, y].Clear();
                }
            }
        }

        private void InCellsCollisions(List<GameObject> cell)
        {
            for (int i = 0; i < cell.Count; i++)
            {
                for (int j = i+1; j < cell.Count; j++)
                {
                    if (GameObject.CheckCollision(cell[i], cell[j]))
                    {
                        cell[i].ApplyCollision(cell[j], _gameEngine);
                        cell[j].ApplyCollision(cell[i], _gameEngine);
                    }
                }
            }
        }

        private void TwoCellsCollisions(List<GameObject> cell1, List<GameObject> cell2) //TODO: change name to check two lists
        {
            foreach (var object1 in cell1)
            {
                foreach (var object2 in cell2)
                {
                    if (GameObject.CheckCollision(object1, object2))
                    {
                        object1.ApplyCollision(object2, _gameEngine);
                        object2.ApplyCollision(object1, _gameEngine);                        
                    }
                }
            }
        }

        private void ApplyCollisions()
        {
            for (int y = 0; y < _gridSize.Y; y++)
            {
                for (int x = 0; x < _gridSize.X; x++)
                {
                    InCellsCollisions(_grid[x, y]);
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        for (int dx = Math.Min(dy+1,1); dx <= 1; dx++)
                        {
                            int indexX = FMath.Mod(x + dx, _gridSize.X);
                            int indexY = FMath.Mod(y + dy, _gridSize.Y);
                            TwoCellsCollisions(_grid[x, y], _grid[indexX, indexY]);
                        }
                    }                   
                }
            }
        }

               
        public void GetAllObjectPossiblyInRange(Vector2 postion, float distance, List<GameObject> gameObjectList)
        {
            gameObjectList.Clear(); //TODO: remove
            Vector2 gridPos = postion * _invCellSize; //TODO: change to inverse
            int x = FMath.Mod((int)gridPos.X, _gridSize.X);
            int y = FMath.Mod((int)gridPos.Y, _gridSize.Y);
            int range = (int)Math.Ceiling(distance * _invCellSize + 0.1f);
            for (int dy = -range; dy <= range; dy++) //TODO: also do a size check
            {
                for (int dx = -range; dx <= range; dx++)
                {
                    int indexX = FMath.Mod(x + dx, _gridSize.X);
                    int indexY = FMath.Mod(y + dy, _gridSize.Y);
                    gameObjectList.AddRange(_grid[indexX,indexY]);
                }
            }
            gameObjectList.AddRange(_bigGameObjects);
        }        


        public void GetAllObjectInRange(Vector2 postion, float distance, List<GameObject> gameObjectList, GameObjectType type = GameObjectType.All)
        {         
            Vector2 gridPos = postion * _invCellSize; //TODO: change to inverse
            int x = FMath.Mod((int)gridPos.X, _gridSize.X);
            int y = FMath.Mod((int)gridPos.Y, _gridSize.Y);
            int range = (int)Math.Ceiling(distance * _invCellSize + 0.1f);
            for (int dy = -range; dy <= range; dy++) //TODO: also do a size check
            {
                for (int dx = -range; dx <= range; dx++)
                {
                    int indexX = FMath.Mod(x + dx, _gridSize.X);
                    int indexY = FMath.Mod(y + dy, _gridSize.Y);
                    foreach (var gameObject in _grid[indexX, indexY])
                    {
                        if(GameObject.DistanceFromEdge(postion, gameObject.Position, 0, gameObject.Size) <= distance && (gameObject.GetObjectType() & type) > 0)
                        {
                            gameObjectList.Add(gameObject);
                        }
                    }
                }
            }
            
            foreach (var gameObject in _bigGameObjects)
            {
                if (GameObject.DistanceFromEdge(postion, gameObject.Position, 0, gameObject.Size) <= distance && (gameObject.GetObjectType() & type) > 0)
                {
                    gameObjectList.Add(gameObject);
                }
            }
        }
        


        public GameObject FindClosestTarget(Vector2 postion, float range, GameObjectType gameObjectType, Faction faction)
        {
            float minDistance = float.MaxValue;
            GameObject closestTarget = null;
            Vector2 gridPos = postion * _invCellSize; //TODO: change to inverse
            int x = FMath.Mod((int)gridPos.X, _gridSize.X);
            int y = FMath.Mod((int)gridPos.Y, _gridSize.Y);
            int cellRange = (int)Math.Ceiling(range * _invCellSize + 0.1f);
            for (int dy = -cellRange; dy <= cellRange; dy++) //TODO: also do a size check
            {
                for (int dx = -cellRange; dx <= cellRange; dx++)
                {
                    int indexX = FMath.Mod(x + dx, _gridSize.X);
                    int indexY = FMath.Mod(y + dy, _gridSize.Y);
                    foreach (var gameObject in _grid[indexX, indexY])
                    {
                        if ((gameObject.GetObjectType() & gameObjectType) > 0 && 
                            ( faction == null || faction.GetRelationToFaction(gameObject.GetFactionType()) < 0) )
                        {
                            float distance = GameObject.DistanceFromEdge(postion, gameObject.Position, 0, gameObject.Size); //Can be improved to square distance
                            if(distance < minDistance)
                            {
                                minDistance = distance;
                                closestTarget = gameObject;
                            }
                        }
                    }
                }
            }

            foreach (var gameObject in _bigGameObjects)
            {
                if ((gameObject.GetObjectType() & gameObjectType) > 0 &&
                           (faction == null || faction.GetRelationToFaction(gameObject.GetFactionType()) < 0) )
                {
                    float distance = GameObject.DistanceFromEdge(postion, gameObject.Position, 0, gameObject.Size); //Can be improved to square distance
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closestTarget = gameObject;
                    }
                }
            }

            if(minDistance > range)
            {
                closestTarget = null;
            }

            return closestTarget;
        }

    }
}
