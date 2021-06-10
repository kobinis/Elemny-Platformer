using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using SolarConflict.NodeGeneration.PlanetGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict.GameContent.Activities.PromoAndTest
{
    class TestScene:Scene
    {
        GameObject earth;

        public List<BurningShard> burningShards;

        public override void InitScript(string parameters = null, ActivityParameters activityParameters = null)
        {
            background = new Background("bgvirtual");
            GameEngine.AmbientColor = Vector3.One * 0;
            SceneComponentSelector.AddComponent(Framework.Scenes.SceneComponentType.TacticalMap);
            //var player = AddGameObject("PrologShip2", Vector2.UnitX * 15500, 180, FactionType.Player, AgentControlType.Player);

            //  var sun = AddGameObject("Sun", Vector2.Zero);
            LightObject lightObj = new LightObject(new Vector3(1,1,0.8f), 1000000, 3);
            lightObj.Position = -Vector2.One * 50000;
            this.GameEngine.PermanentLights.Add(lightObj);
            earth = AddGameObject("Earth", Vector2.UnitX * 16500);
            //earth = AddGameObject("Mars", Vector2.UnitX * 18500);
            //earth.Parent = portal;
            //for (int i = 1; i < 6; i++)
            //{
            //    AddGameObject($"ResourceMine{i}", Vector2.UnitX * 1000 * (15 +i));
            //}

          

            CameraManager.MovmentType = CameraMovmentType.OnPlayer;
            AddGameObject("MatA1", Vector2.Zero);
            //this.AddObjectRandomlyInLocalCircle("LavaAsteroid1", 10, 6000, Vector2.Zero, 500);
            this.AddObjectRandomlyInLocalCircle("Asteroid1", 10, 6000, Vector2.Zero, 500);
            Camera.Position = earth.Position;

            //int planetNum = 5;
            //for (int i = 0; i < planetNum; i++)
            //{
            //    PlanetData data = new PlanetData("Earth" + i.ToString(), "earthV4", "earthV4cities", 1500, new Vector3(0.15f, 0.75f, 1.0f), new Vector3(0.141f, 1, 0.956f) * 0.8f);
            //    var planet = PlanetQuickStart.Make(data).Emit(GameEngine, sun, Framework.FactionType.Neutral, Vector2.One * (i + 1000 * 1.3f + i / (float)planetNum * 100000), Vector2.Zero, 0);
            //    //info.Name + (i + 1).ToString( = new PlanetData(info.Name + (i+1).ToString(), "Earth", )
            //    Camera.Position = planet.Position;
            //}

            AddGameObject("VoidTestShip", Camera.Position, faction: FactionType.Player);

            ParamEmitter emitter = new ParamEmitter();
            emitter.EmitterID = "Debris";
            emitter.RotationSpeedType = ParamEmitter.EmitterRotationSpeed.Random;
            emitter.RotationRange = 0.1f; //Test it
            emitter.RotationSpeedBase = -0.1f;
            emitter.PosAngleType = ParamEmitter.EmitterPosAngle.Random;
            emitter.PosAngleRange = 360;
            emitter.PosRadType = ParamEmitter.EmitterPosRad.Random;
            emitter.PosRadRange = 1000;
            emitter.PosRadMin = 30;
            emitter.MinNumberOfGameObjects = 500;
            emitter.RotationType = ParamEmitter.EmitterRotation.Random;
            emitter.RotationRange = 360;
            emitter.LifetimeType = ParamEmitter.EmitterLifetime.Random;
            emitter.LifetimeMin = 100;
            emitter.LifetimeRange = 1000;
            emitter.Emit(GameEngine, null, FactionType.Neutral, Camera.Position, Vector2.Zero, 0, 0);

            //Added just for purpose of explosion testing
            DarkeUtils.Init();
            burningShards = new List<BurningShard>();
        }

        public override void UpdateScript(InputState inputState)
        {
            if (inputState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Space))
                this.AddGameObject("SmallTargetShip", FactionType.Pirates1, this.CursorWorldPosition);



            if (inputState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Enter))
                this.AddGameObject("SmallEMPShip", FactionType.Pirates1, this.CursorWorldPosition);

            //if (inputState.Cursor.IsPressedLeft)
            //    this.AddGameObject("FirePS", FactionType.Pirates1, this.CursorWorldPosition);


            //if (!inputState.Cursor.IsLastPressedRight && inputState.Cursor.IsPressedRight)
            //    ExplostionTest.MakeNormalExplosion(this.CursorWorldPosition, (float)DarkeUtils.GetRandomNumber(0,1), PARTICLE_MANAGER.systems["SmokePS"], this);


            //if (inputState.Cursor.OnPressRight)
            //    ExplostionTest.MakeNormalExplosion(this.CursorWorldPosition, (float)DarkeUtils.GetRandomNumber(0, 1), PARTICLE_MANAGER.systems["SmokePS"], this);

            foreach (BurningShard shard in burningShards)
            {
                shard.Update();
            }
            //if (inputState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.P))
            //    PARTICLE_MANAGER.greenSmokePS.addParticle(new Vector3(Camera.Position.X, Camera.Position.Y, 0), new Vector3(1000, 0, 0), 55000, 10);
            //if (inputState.Cursor.OnPressLeft)
            //{
            //    earth.Position = Camera.GetWorldPos(inputState.Cursor.Position) ;
            //}
            //if (inputState.Cursor.OnPressRight)
            //{
            //    AddGameObject("GasHarvester", Camera.GetWorldPos(inputState.Cursor.Position), 0);
            //}
        }


        public static Activity ActivityProvider(string parameters) //TODO: change
        {
            return new TestScene();
        }

    }
}
