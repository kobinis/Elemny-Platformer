using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XnaUtils;
using XnaUtils.Graphics;

namespace SolarConflict.NodeGeneration.PlanetGeneration
{
    public class PlanetQuickStart
    {
        public static IEmitter Make(PlanetData data)
        {
            Planet planet = new Planet();

            planet.AtmosphereColor = data.AtmosphereColor;
            planet.AtmosphereIntensity = data.AtmosphereIntensity;
            planet.PlanetRotationSpeed = data.RotationSpeed;
            planet.effectTechnique = data.effectTechnique;
            planet.Name = data.Name;
            planet.planetType = data.planetType;
            planet.CityColor = data.CityColor;

            if (data.procedural)
            { 
                planet.Color1 = data.Color1;
                planet.Color2 = data.Color2;
                planet.Color3 = data.Color3;

                planet.planetType = data.planetType;
                planet.FluidLevel = data.FluidLevel;
                planet.FluidEmittivity = data.FluidEmittivity;
                planet.FluidColor = data.FluidColor;

                planet.HeightPhases = data.HeightPhases;

                planet.surfaceTextures = data.surfaceTextures;
                planet.heightTexture = data.heightTexture;

                planet.procedural = true;
            }
            else
            {
                planet.MainSprite = TextureBank.Inst.GetSprite(data.PlanetTextureID);
                planet.PlanetTexture = planet.MainSprite.Texture;
                planet.CityMap = TextureBank.Inst.GetTexture(data.CityMapID);
                planet.effectTechnique = Camera.NormalMapEffect.Techniques["PlanetSurface"];
            }

           // planet.
            planet.Size = data.Size;
            return planet;

            //ProjectileProfile projectileProfile = new ProjectileProfile();
            //projectileProfile.Name = data.Name;
            //projectileProfile.TextureID = data.PlanetTextureID;
            //projectileProfile.CollisionType = CollisionType.Collide1;
            //MoveWithParent movement = new MoveWithParent();
            
            //projectileProfile.ScaleMult = 2f / Sprite.Get(data.PlanetTextureID).Height;
            //movement.RefRotationMult = 0;
            //movement.RotationSpeed = 200f;
            //projectileProfile.CollisionType = CollisionType.Collide1;
            //projectileProfile.DrawType = DrawType.Lit;
            //projectileProfile.MovementLogic = movement;
            //projectileProfile.InitSize = new InitFloatConst(data.Size);
            //projectileProfile.IsDestroyedOnCollision = false;
            //projectileProfile.IsEffectedByForce = false;
            //projectileProfile.CollisionSpec = CollisionSpec.SpecForce;

            //projectileProfile.Draw = new DrawPlanet(data.CityMapID, data.AtmosphereColor, data.AtmosphereIntensity, data.CityColor, data.RotationSpeed);
            
            //projectileProfile.RotationLogic = new UpdateRotationParent();
            //projectileProfile.CollideWithMask = GameObjectType.None;

            //projectileProfile.DrawType = DrawType.Planets;
            //return projectileProfile;
        }
    }
}
