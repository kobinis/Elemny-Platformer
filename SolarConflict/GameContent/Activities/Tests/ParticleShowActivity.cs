using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.CameraControl.Zoom;

namespace SolarConflict.GameContent.Activities.Tests
{
   

    class ParticleShowActivity: Activity
    {
        GameEngine gameEngine;
        List<IEmitter> emitters;
        int currentEmitterIndex;
        IEmitter gravityEmitter;
        int radX = 30;
        int radY = 20;
        Vector2 space = Vector2.One * 50;
        GameObject[,] grid;
        ManualZoom zoom;

        public ParticleShowActivity()
        {
            Camera camera = new Camera();
            zoom = new ManualZoom();
            gameEngine = new GameEngine(camera);
            gravityEmitter = MakeGravityWell();//ContentBank.Inst.GetEmitter("FullExplosionFx1");
                //MakeGravityWell();
            //gravityEmitter.InitMaxLifetimeID = "1000";
            emitters = new List<IEmitter>();
            emitters.Add(MakePush());
            emitters.Add(MakeGlowParticle());
            emitters.Add(MakeSparkEmitter());
            // emitters.Add(MakeGravityWell());
           
            
            grid = new GameObject[2 * radX, 2 * radY];

            //for (int x = -0; x < 2 * radX; x++)
            //{
            //    for (int y = 0; y < 2 * radY; y++)
            //    {
            //        Vector2 pos = new Vector2(x - radX, y - radY) * space;
            //        GameObject go = emitters[1].Emit(gameEngine, null, Framework.FactionType.Neutral, pos, Vector2.Zero, 0);
            //        grid[x, y] = go;
            //    }
            //}

        }


        public override void Update(InputState inputState)
        {
            if (inputState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape))
                ActivityManager.Inst.Back();
            zoom.Update(gameEngine.Camera, null, null, gameEngine, inputState);
            for (int i = 0; i < DebugUtils.DebugKeys.Length; i++)
            {
                if(inputState.IsKeyPressed(DebugUtils.DebugKeys[i]))
                {
                    currentEmitterIndex = i % emitters.Count;
                }
            }
            Vector2 position = gameEngine.Camera.GetWorldPos(inputState.Cursor.Position);
            Vector2 velocity = (position - gameEngine.Camera.GetWorldPos(inputState.Cursor.PreviousPosition))* 2;
            if(inputState.Cursor.IsPressedLeft)
            {
                emitters[currentEmitterIndex].Emit(gameEngine, null, Framework.FactionType.Neutral, position , velocity, 0);
            }
            if (inputState.Cursor.OnPressRight)
            {
                gravityEmitter.Emit(gameEngine, null, Framework.FactionType.Neutral, position, Vector2.Zero, 0);
            }

            //for (int x = -0; x < 2 * radX; x++)
            //{
            //    for (int y = 0; y < 2 * radY; y++)
            //    {
            //        Vector2 pos = new Vector2(x - radX, y - radY) * space;
            //        GameObject go = grid[x, y];
            //        Vector2 diff = pos - go.Position;
            //        go.Velocity += diff * 0.1f;
            //    }
            //}

            gameEngine.Update(inputState);
            
        }

        public override void Draw(SpriteBatch sb)
        {
            gameEngine.Draw(sb);
        }

        public static Activity ActivityProvider(string parameters = "")
        {
            return new ParticleShowActivity();
        }

        public static ProjectileProfile MakeGravityWell()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.CollisionType = CollisionType.CollideAll;
            profile.DrawType = DrawType.Additive;
            //profile.UpdateColor = ProjectileProfile.ColorUpdate.f;
            profile.TextureID = "shockwave2";
            profile.CollisionWidth = profile.Sprite.Width - 10;

            /*profile.InitSizeId = "40";
            profile.UpdateSize = new UpdateSizeGrow(5, 1.1f);*/

            profile.InitSizeID = "150";
            //profile.UpdateSize = new UpdateSizeGrow(-5, 1f);

            profile.MovementLogic = new MoveToTarget(ProjectileTargetType.Parent, 0, 0);

            profile.InitMaxLifetime = new InitFloatConst(1000);
            profile.Mass = 0.1f;
            //profile.ImpactEmitterId = "EmitterImpactFx1";
            profile.CollisionSpec = new CollisionSpec(0, 8f);
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = false;
            profile.CollideWithMask = GameObjectType.All;
            return profile;
        }

        public static ProjectileProfile MakePush()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.TextureID = "shockwave2";
            profile.DrawType = DrawType.None;
            profile.InitColor = new InitColorConst(Color.White);
            profile.CollisionType = CollisionType.CollideAll;
            profile.InitSize = new InitFloatConst(200); //100
            profile.InitMaxLifetimeID = "2";
            profile.CollisionSpec = new CollisionSpec();
            profile.CollisionSpec.Force = 5; //3
            //profile.CollisionSpec.ForceType = ForceType.Gravity;
            profile.IsDestroyedWhenParentDestroyed = false;
            profile.CollisionType = CollisionType.CollideAll;
            profile.CollideWithMask = GameObjectType.All;
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = false;
            return profile;
        }

        public static ProjectileProfile MakeGlowParticle()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.CollisionType = CollisionType.CollideAll;

            profile.DrawType = DrawType.Additive;
            profile.TextureID = "smallGlow";// "lightGlow";  //
            profile.CollisionWidth = profile.Sprite.Width - 10;
           // profile.ColorLogicID = "FadeOutSlow";
            /*profile.InitSizeId = "40";
            profile.UpdateSize = new UpdateSizeGrow(5, 1.1f);*/

            profile.InitSizeID = "20";
            profile.InitHitPointsID = "100000";
            //profile.UpdateSize = new UpdateSizeGrow(-5, 1f);

            //profile.MovementLogic = new MoveToTarget(ProjectileTargetType.Parent, 0, 0);

            profile.InitMaxLifetime = new InitFloatConst(100000);
            profile.Mass = 1f;
            profile.VelocityInertia = 0.9f;
            //profile.ImpactEmitterId = "EmitterImpactFx1";
            profile.CollisionSpec = new CollisionSpec(0, 2f);
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = true;
            profile.CollideWithMask = GameObjectType.All;
            return profile;
        }

        public static ProjectileProfile MakeSpark()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.VelocityInertia = 1;
            profile.RotationInertia = 1;
            profile.TextureID = "add6";
            profile.CollisionWidth = 32;
            profile.InitMaxLifetimeID = "20";
            profile.InitSize = new InitFloatRandom(10, 10); //"InitFloatRandom,10,10";
            profile.ColorLogicID = "FadeInOut";
            profile.CollisionType = CollisionType.CollideAll;
            profile.DrawType = DrawType.Additive;
            profile.IsEffectedByForce = true;
            return profile;
        }

        public static ParamEmitter MakeSparkEmitter()
        {
            ParamEmitter emitter = new ParamEmitter();
            emitter.Emitter = MakeSpark();
            emitter.RefVelocityMult = 0.1f;
            emitter.PosRadType = ParamEmitter.EmitterPosRad.Random;
            emitter.PosRadMin = 50;
            emitter.PosRadRange = 100;
            emitter.PosAngleType = ParamEmitter.EmitterPosAngle.Random;
            emitter.PosAngleRange = 360;
            emitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Random;
            emitter.VelocityMagMin = 5f;
            emitter.VelocityMagRange = 3f;
            emitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.Random;
            emitter.VelocityAngleRange = 360;
            emitter.RotationType = ParamEmitter.EmitterRotation.Random;
            emitter.RotationRange = 360;
            emitter.RotationSpeedType = ParamEmitter.EmitterRotationSpeed.Random;
            emitter.RotationSpeedRange = 0.15f; //maybe more;
            emitter.LifetimeType = ParamEmitter.EmitterLifetime.Const;
            emitter.LifetimeMin = 30;
            emitter.MinNumberOfGameObjects = 13;
            return emitter;
        }
    }
}
