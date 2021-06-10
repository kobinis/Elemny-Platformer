using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict.Framework.InGameEvent.GenericProcess
{
    [Serializable]
    class FireflysProcess:GameProcess
    {
        GameObject target;
        DummyObject centerObject;
        GameObject[] fireflys;
        IEmitter fireflyEmitter;
        float PositionRad = 10000;
        int timeOffset;

        public FireflysProcess(int num)
        {
            fireflyEmitter = ContentBank.Get("Fireflys");
            fireflys = new GameObject[num];
            centerObject = new DummyObject();
            centerObject.SetFactionType(FactionType.Player);
            timeOffset = FMath.Rand.Next(90 * 10);
        }

        private void CreateFireflys(GameEngine gameEngine)
        {
            for (int i = 0; i < fireflys.Length; i++)
            {
                if(fireflys[i] == null || fireflys[i].IsNotActive)
                {
                    Vector2 velocitiy = FixedVelocity(gameEngine.PlayerAgent?.Velocity ?? Vector2.Zero, gameEngine.Rand);
                    Vector2 position = gameEngine.Camera.Position - velocitiy * PositionRad;
                    fireflys[i] = fireflyEmitter.Emit(gameEngine, centerObject, FactionType.Neutral, position, Vector2.Zero, 0);
                }
            }
        }

        public override void Update(GameEngine gameEngine)
        {
            if((gameEngine.FrameCounter+ timeOffset) % 90 == 0)
            {
                CreateFireflys(gameEngine);

                if (gameEngine.Rand.Next(5) != 0)
                {
                    List<GameObject> objects = new List<GameObject>();
                    gameEngine.CollisionManager.GetAllObjectInRange(gameEngine.Camera.Position, 6000, objects);
                    if(objects.Count > 0)
                    {
                        target = objects[gameEngine.Rand.Next(objects.Count)]; 
                    }
                }
                else
                {
                    target = null;
                }

            }
            if (target != null)
                centerObject.Position = target.Position;
            else
                centerObject.Position = gameEngine.PlayerAgent?.Position ?? gameEngine.Camera.Position;

        }

        public override GameProcess GetWorkingCopy()
        {
            return new FireflysProcess(this.fireflys.Length);
        }

        private Vector2 FixedVelocity(Vector2 velocity, Random random)
        {
            return velocity == Vector2.Zero ? FMath.ToCartesian(1, random.NextFloat() * MathHelper.TwoPi) : velocity.Normalized();
        }
    }
}
