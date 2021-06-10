using Microsoft.Xna.Framework;
using SolarConflict.NewContent.Emitters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Utils.QuickStart
{
    //basic- armor
    //Lava - mines
    //Worms
    //
    public class AsteroidData
    {        
        public string ID;
        public string Name;
        public float Hitpoints = 250;
        public float Armor;

        public IEmitter Loot;
        public IEmitter DestroyedEmitter;
                
        public Color? Color;
        public Color LightColor;
        public float SelfIlumination;
        public float Size = 100;
        public float SizeVariance = 0.2f;
        //public bool IsDestroyedByAgent;

        public bool GenerateSmallAsteroid;
        public IEmitter SmallLoot;
        public IEmitter SmallDestroyedEmitter;
        public float SmallSizeMult = 0.8f;
        public float SmallHitpointMult = 0.6f;
        public int Level;  
        
        public AsteroidData(string id, int level = 0)
        {
            ID = id;
            Color = default(Color);
            Loot = new LootEmitter();
            SmallLoot = new LootEmitter();
            SelfIlumination = 0;
            Level = level;
        }
    }



    class AsteroidQuickStart
    {
        public static List<ProjectileProfile> MakeAndAdd(AsteroidData data)
        {
            List<ProjectileProfile> profiles = new List<ProjectileProfile>();
            
            GroupEmitter hitpointZeroEmitter = new GroupEmitter();
            hitpointZeroEmitter.AddEmitter(data.Loot);
            hitpointZeroEmitter.AddEmitter(data.DestroyedEmitter);
            hitpointZeroEmitter.AddEmitter(typeof(FxEmitterRockExp).Name);
            hitpointZeroEmitter.AddEmitter(typeof(FxEmitterRockExp).Name);

            ProjectileProfile asteroid = AsteroidQuickStart.MakeEmptyAsteroid(data.Color, data.Hitpoints, data.Size, data.Size * data.SizeVariance);
            asteroid.Name = data.Name;
            asteroid.ID = data.ID;
            asteroid.HitPointZeroEmiiter = hitpointZeroEmitter;
            ContentBank.Inst.AddContent(asteroid);
            profiles.Add(asteroid);
            PointLight light = null;
            PointLight smallLight = null;
            if(data.SelfIlumination > 0)
            {
                Color color = Color.White;
                if (data.Color.HasValue)
                    color = data.Color.Value;
                light = new PointLight(color.ToVector3(), 3000,  data.SelfIlumination);
                smallLight = new PointLight(color.ToVector3(), 3000, data.SelfIlumination);
                asteroid.Light = light;
            }
            
            if (data.GenerateSmallAsteroid)
            {
                GroupEmitter smallHitpointZeroEmitter = new GroupEmitter();
                smallHitpointZeroEmitter.AddEmitter(data.SmallLoot);
                smallHitpointZeroEmitter.AddEmitter(data.SmallDestroyedEmitter);
                smallHitpointZeroEmitter.AddEmitter(typeof(FxEmitterRockExp).Name);

                float smallBaseSize = data.Size * data.SmallSizeMult;
                ProjectileProfile smallAsteroid = AsteroidQuickStart.MakeEmptyAsteroid(data.Color, data.Hitpoints * data.SmallHitpointMult, smallBaseSize, smallBaseSize * data.SizeVariance);
                smallAsteroid.Name = data.Name;
                smallAsteroid.ID = "Small" + data.ID;
                smallAsteroid.HitPointZeroEmiiter = smallHitpointZeroEmitter;
                smallAsteroid.Light = smallLight;
                ContentBank.Inst.AddContent(smallAsteroid);                
                profiles.Add(smallAsteroid);


                var asteroEmitter = new ParamEmitter();
                asteroEmitter.Emitter = smallAsteroid;
                asteroEmitter.MinNumberOfGameObjects = 3;
                asteroEmitter.RangeNumberOfGameObject = 2;
                asteroEmitter.PosRadType = ParamEmitter.EmitterPosRad.Random;
                asteroEmitter.PosRadMin = 10;
                asteroEmitter.PosRadRange = 6;
                asteroEmitter.PosAngleType = ParamEmitter.EmitterPosAngle.Random;
                asteroEmitter.PosAngleRange = 360;
                hitpointZeroEmitter.AddEmitter(asteroEmitter);
            }

            profiles.Do(p => p.Level = data.Level);
            
            return profiles;
        }

        public static ProjectileProfile MakeEmptyAsteroid(Color? color = null, float hitpoints = 250, float size = 100, float sizeRange = 20, float damage = 0.1f)
        {
            ProjectileProfile profile = new ProjectileProfile();           
            
            #region Display
            if(color != null)
            {
                profile.InitColor = new InitColorConst(color.Value);                
            }
            profile.DrawType = DrawType.Lit;
            profile.TextureID = "spacerock10000";
            profile.CollisionWidth = profile.Sprite.Width - 180;
            profile.InitParam = new InitFloatRandom(0, 10000);



            var multiSheetDraw = new ProjectileDrawMultipleSpritesheets(16, new Spritesheet("Rock1Sheet0", 453, 429, 16),
                new Spritesheet("Rock1Sheet1", 453, 429, 16), new Spritesheet("Rock1Sheet2", 453, 429, 16),
                new Spritesheet("Rock1Sheet3", 453, 429, 16), new Spritesheet("Rock1Sheet4", 453, 429, 16),
                new Spritesheet("Rock1Sheet5", 453, 429, 10));
        //    multiSheetDraw.paramMult = 0.1f;
            multiSheetDraw.lifeTimeMult = 0.3f;

              profile.Draw = multiSheetDraw;            

           // profile.UpdateList.Add(new UpdateParamSumVelocity());
            #endregion                                   
            profile.InitHitPoints = new InitFloatConst(hitpoints);
            profile.InitSize = new InitFloatRandom(size, sizeRange);

            profile.CollisionSpec = new CollisionSpec(damage, ImpactType.Velocity, 1f);
            profile.CollisionSpec.Flags |= CollisionSpecFlags.AffectsAllies | CollisionSpecFlags.IsRotating;
            profile.Mass = 2; //??
            profile.RotationMass = 10; //TODO: according to size
            
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = true;
            profile.IsTurnedByForce = true;
            profile.RotationInertia = 0.995f;
            profile.VelocityInertia = 0.99f;
            profile.ObjectType |= GameObjectType.Asteroid;
            profile.CollisionType = CollisionType.UpdateOnlyOnScreen; //updateOnScreen

            return profile;
        }
    }
}
