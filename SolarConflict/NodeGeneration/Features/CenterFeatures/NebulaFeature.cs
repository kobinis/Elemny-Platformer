//using SolarConflict.Framework.World.Generation;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using SolarConflict.GameWorld;
//using Microsoft.Xna.Framework;
//using SolarConflict.Framework;
//using XnaUtils;

//namespace SolarConflict.NodeGeneration.Features {
//    class NebulaFeature : GenerationFeature {
//        //public float Density;
//        public float MaxSize;
//        public float MinSize;        

//        public override GameObject GenerationLogic(Scene scene, SceneGenerator generator) {
//            Size = Rand.NextFloat(MinSize, MaxSize);

//            // METHOD I: add effects randomly in circle
//            /*var numObjects = (int)(Math.PI * Size * Size * Density);
//            scene.AddObjectRandomlyInLocalCircle("NebulaFx1", numObjects, Size, Position, seed: Rand.Next());
//            scene.AddObjectRandomlyInLocalCircle("NebulaFx2", numObjects, Size, Position, seed: Rand.Next());*/

//            // METHOD II: draw a circle using effects as pixels, give each such pixel a random offset
//            var increment = 350f;
//            var maxWobble = 1.5f * increment;
//            for (var y = -Size; y <= Size; y += increment) {
//                var xSize = (float) Math.Sqrt(Size * Size - y * y);
//                for (var x = -xSize; x <= xSize; x += increment) {
//                    var wobble = FMath.ToCartesian(Rand.NextFloat(-1f, 1f) * maxWobble, Rand.NextFloat(0, 360f));
//                    scene.AddGameObject("NebulaFx1", Position + new Vector2(x, y) + wobble * 2, Rand.NextFloat(0f, 360f));
//                }
//            }

//            scene.GameEngine.AddGameProcces(new NebulaStorm() { Position = Position, Radius = Size * 0.9f });

//            return base.GenerationLogic(scene, generator);
//        }
//    }

//    [Serializable]
//    public class NebulaStorm : GameProcess {
//        public string ID { get; set; }

//        float ChancePerFrame = 0.1f;
//        float ActivationRadius = 50000;
//        public Vector2 Position;
//        public float Radius;

//        public override void Update(GameEngine gameEngine) {
//            if (gameEngine.PlayerAgent == null || ((gameEngine.PlayerAgent.Position - Position).LengthSquared() > ActivationRadius * ActivationRadius))
//                // No witnesses, no storm
//                return;
//            if (gameEngine.Rand.NextFloat() <= ChancePerFrame)
//                gameEngine.Scene.AddObjectRandomlyInLocalCircle("NebulaLightningEmitter", 1, Radius, Position, seed: gameEngine.Rand.Next());                
//        }

//        public override GameProcess GetWorkingCopy()
//        {
//            throw new NotImplementedException();
//        }
//    }

//}
