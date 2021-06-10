using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SolarConflict.Framework;
using SolarConflict.NodeGeneration.PlanetGeneration;
using XnaUtils;
using XnaUtils.Graphics;

namespace SolarConflict.NodeGeneration.PlanetGeneration
{
    [Serializable]
    public class Planet : GameObject, IEmitter
    {
        public Sprite MainSprite;
        //Visiual
        public Texture2D PlanetTexture;
        public Texture2D CityMap;
        public float PlanetRotationSpeed;
        public Vector3 AtmosphereColor;
        public Vector3 CityColor;
        public float AtmosphereIntensity;        
        //Activity
        public string ActivityName;
        public ActivityParameters ActivityParameters;
        

        //Parameters for procedural planets
        public bool procedural;

        public Vector3 HeightPhases;

        public Vector3 Color1;
        public Vector3 Color2;
        public Vector3 Color3;

        public PlanetType planetType;
        public float FluidLevel;
        public float FluidEmittivity = 0;
        public Vector3 FluidColor;

        public Texture2D surfaceTextures;
        public Texture2D heightTexture;   
        
        public EffectTechnique effectTechnique;

        private float scaleMult;


        //[OnDeserialized]
        //public void OnDeserializedMethod(StreamingContext context)
        //{
        //    effectTechnique = Camera.NormalMapEffect.Techniques["PlanetSurface"];
        //}

        public override void Update(GameEngine gameEngine)
        {
            Lifetime++;

        }

        public override void Draw(Camera camera)
        {

            //Shader parameters       
            Camera.NormalMapEffect.Parameters["Rotation"].SetValue(Lifetime * PlanetRotationSpeed);
            //Camera.NormalMapEffect.Parameters["DominantLightColor"].SetValue(lightColor); //To Do: Uncomment and hook up Vector 3 light color here
            Vector2 lightPos = new Vector2(0, 0) - camera.GetWorldPos(ActivityManager.ScreenCenter);
            Camera.NormalMapEffect.Parameters["DominantLightPos"].SetValue(new Vector3(lightPos.X, lightPos.Y, 10000f));
            Camera.NormalMapEffect.Parameters["CityColor"].SetValue(CityColor);
            Camera.NormalMapEffect.Parameters["AtmosphereIntensity"].SetValue(AtmosphereIntensity * 0.8f);
            Camera.NormalMapEffect.Parameters["AtmosphereColor"].SetValue(AtmosphereColor);

            if (procedural)
            {
                Camera.NormalMapEffect.CurrentTechnique = effectTechnique;

                //Procedural planet parameters
                Camera.NormalMapEffect.Parameters["Color1"].SetValue(Color1);
                Camera.NormalMapEffect.Parameters["Color2"].SetValue(Color2);
                Camera.NormalMapEffect.Parameters["Color3"].SetValue(Color3);

                Camera.NormalMapEffect.Parameters["Phase"].SetValue(HeightPhases);
               

                Camera.NormalMapEffect.Parameters["FluidColor"].SetValue(FluidColor * FluidEmittivity);
                Camera.NormalMapEffect.Parameters["FluidLevel"].SetValue(FluidLevel);

                Camera.NormalMapEffect.Parameters["NormalMap"].SetValue(surfaceTextures); //We hijack normal sampler here
                ActivityManager.GraphicsDevice.BlendState = BlendState.AlphaBlend;
                scaleMult = 2f / heightTexture.Height;
                camera.CameraDraw(heightTexture, Position, Rotation, new Vector2(0.5f, 1) * Size * scaleMult, Color.White);

                ActivityManager.GraphicsDevice.BlendState = BlendState.Additive;
                Camera.NormalMapEffect.CurrentTechnique = Camera.NormalMapEffect.Techniques["PlanetAtmosphere"];
                camera.CameraDraw(heightTexture, Position, Rotation, new Vector2(0.5f, 1) * Size * scaleMult, Color.White);

            }
            else
            {
                Camera.NormalMapEffect.CurrentTechnique = effectTechnique;
                scaleMult = 2f / MainSprite.Height;
                //Static planet parameters
                ActivityManager.GraphicsDevice.BlendState = BlendState.AlphaBlend;
                Camera.NormalMapEffect.Parameters["RoughnessMap"].SetValue(CityMap); //Hijacked for city mask

                camera.CameraDraw(PlanetTexture, Position, Rotation, new Vector2(0.5f, 1) * Size * scaleMult, Color.White);

                ActivityManager.GraphicsDevice.BlendState = BlendState.Additive;
                Camera.NormalMapEffect.CurrentTechnique = Camera.NormalMapEffect.Techniques["PlanetAtmosphere"];
                camera.CameraDraw(PlanetTexture, Position, Rotation, new Vector2(0.5f, 1) * Size * scaleMult, Color.White);
            }


        }

        public override EffectTechnique GetEffectTechnique()
        {            
            return effectTechnique;
        }

        public override float Mass { get; set; }
        public override string Name { get; set; }

        public override string Tag { get { return Name; } }

        public override CollisionSpec CollisionInfo { get { return CollisionSpec.SpecEmpty; } set { } }
        public string ID { get; set; }

        private FactionType _faction;

        public override void ApplyCollision(GameObject collidingObject, GameEngine gameEngine)
        {            
        }

        public override void ApplyForce(Vector2 force, float speedLimit)
        {            
        }

        public override GameObject GetAgentAncestor()
        {
            return Parent?.GetAgentAncestor();
        }

        public override string GetId()
        {
            return ID;
        }

        public override float GetMeterValue(MeterType type)
        {
            return 0;
        }

        public override GameObjectType GetObjectType()
        {
            return GameObjectType.PlaceOfIntrest;
        }

        public override Sprite GetSprite()
        {
            return MainSprite;
        }

        public override GameObject GetTarget(GameEngine gameEngine, TargetType targetType)
        {
            return null;
        }

        public override void SetMeterValue(MeterType type, float value)
        {            
        }

        public override CollisionType ListType
        {
            get { return CollisionType.CollideAll; }
            set { }
        }

        public override DrawType DrawType
        {
            get { return DrawType.Planets; }
            set { }
        }

        public override GameObjectType GetCollideWithMask()
        {
            return GameObjectType.None;
        }

        public GameObject Emit(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0, int maxLifetime = 0, float? size = null, Color? color = null, float param = 0)
        {
            var clone = GetWorkingCopy();
            clone.Parent = parent;
            clone._faction = faction;
            clone.Position = refPosition;
            clone.Velocity = refVelocity;
            clone.Rotation = refRotation;
            clone.RotationSpeed = refRotationSpeed;
            //Size??
            gameEngine.AddList.Add(clone);
            return clone;
        }

        public Planet GetWorkingCopy()
        {
            var clone = MemberwiseClone() as Planet;            
            return clone;
        }       
    }
}
