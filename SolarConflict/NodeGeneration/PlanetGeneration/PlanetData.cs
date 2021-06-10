using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XnaUtils;

namespace SolarConflict.NodeGeneration.PlanetGeneration
{
    public class PlanetData
    {
        public string Name;
        //Visiual
        public string PlanetTextureID;
        public string CityMapID;
        public float RotationSpeed;
        public Vector3 AtmosphereColor;
        public Vector3 CityColor;
        public float AtmosphereIntensity;
        public float Size;
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

        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public EffectTechnique effectTechnique;


        public PlanetData(string name, string planetTextureID, string cityMapID, float size)
        {
            Name = name;
            PlanetTextureID = planetTextureID;
            CityMapID = cityMapID;
            RotationSpeed = 0.001f;
            AtmosphereColor = new Vector3(1.0f, 0.62f, 0.35f);
            AtmosphereIntensity = 0.7f;
            CityColor = (new Vector3(1.0f, 0.5f, 0.25f)) * 0.8f;
            Size = size;
            effectTechnique = Camera.NormalMapEffect.Techniques["PlanetSurface"];
        }

        public PlanetData(string name, string planetTextureID, string cityMapID, float size, Vector3 atmosphereColor, Vector3 cityColor)
        {
            Name = name;
            PlanetTextureID = planetTextureID;
            CityMapID = cityMapID;
            RotationSpeed = 0.001f;
            AtmosphereColor = atmosphereColor;
            AtmosphereIntensity = 0.7f;
            CityColor = cityColor;
            Size = size;
            effectTechnique = Camera.NormalMapEffect.Techniques["PlanetSurface"];
        }

        public PlanetData(int seed)
        {
            procedural = true;
            RotationSpeed = 0.001f;
            Random rnd = new Random(seed);
            Array values = Enum.GetValues(typeof(PlanetType));
            planetType = (PlanetType)values.GetValue(rnd.Next(values.Length));
            GeneratePlanet(rnd);

            Name = GenerateName(rnd);
        }

        public PlanetData(int seed, PlanetType planetType)
        {
            procedural = true;
            RotationSpeed = 0.001f;
            Random rnd = new Random(seed);
            this.planetType = planetType;
            GeneratePlanet(rnd);
            
            Name = GenerateName(rnd);
        }

        /// <summary>Generates completely random planet</summary>
        public PlanetData GeneratePlanet(Random rnd)
        {


            //pick random height texture
            heightTexture = Game1.planetHeightTexture[rnd.Next(Game1.planetHeightTexture.Length)];

            //This can be randomized as well if there are more surface textures to pick from
            surfaceTextures = Game1.planetSurfaceTexture;
            //pick size
            Size = MathHelper.Lerp(PlanetParam.sizeLimit.X, PlanetParam.sizeLimit.Y, rnd.NextFloat());

            //pick transitions phases
            HeightPhases.X = MathHelper.Lerp(PlanetParam.phaseLimit1.X, PlanetParam.phaseLimit1.Y, rnd.NextFloat());
            HeightPhases.Y = MathHelper.Lerp(PlanetParam.phaseLimit2.X, PlanetParam.phaseLimit2.Y, rnd.NextFloat());
            HeightPhases.Z = MathHelper.Lerp(PlanetParam.phaseLimit3.X, PlanetParam.phaseLimit3.Y, rnd.NextFloat());



            switch(planetType)
            {
                case PlanetType.Barren:
                    GenerateBarren(rnd);
                    break;
                case PlanetType.Magma:
                    GenerateMagma(rnd);
                    break;
                case PlanetType.Terra:
                    GenerateTerran(rnd);
                    break;
                case PlanetType.Toxic:
                    GenerateToxic(rnd);
                    break;
            }

            return null;
        }

        private void GenerateBarren(Random rnd)
        {
            effectTechnique = Camera.NormalMapEffect.Techniques["PlanetRandomSurfaceBarren"];
            //Surface colors
            Color1 = Vector3.Lerp(PlanetParam.blackMin, PlanetParam.redMax, rnd.NextFloat());
            Color2 = Vector3.Lerp(PlanetParam.brownMin, PlanetParam.brownMax, rnd.NextFloat());
            Color3 = Vector3.Lerp(PlanetParam.sandMin, PlanetParam.sandMax, rnd.NextFloat());

            //Atmosphere color
            AtmosphereIntensity = MathHelper.Lerp(PlanetParam.atmoIntLimits.X, PlanetParam.atmoIntLimits.Y, rnd.NextFloat());
            AtmosphereColor = Vector3.Lerp(PlanetParam.atmoBarrensMin, PlanetParam.atmoBarrensMax, rnd.NextFloat());
            //Pick texture pack

            //Fluids
            FluidLevel = -2.0f;
            FluidColor = Vector3.Zero;
        }

        private void GenerateMagma(Random rnd)
        {
            effectTechnique = Camera.NormalMapEffect.Techniques["PlanetRandomSurfaceMagma"];
            //Surface colors
            Color1 = Vector3.Lerp(PlanetParam.redMin, PlanetParam.redMax, rnd.NextFloat());
            Color2 = Vector3.Lerp(PlanetParam.brownMin, PlanetParam.brownMax, rnd.NextFloat());
            Color3 = Vector3.Lerp(PlanetParam.grayMin, PlanetParam.grayMax, rnd.NextFloat());

            //Atmosphere color
            AtmosphereIntensity = MathHelper.Lerp(PlanetParam.atmoIntLimits.X, PlanetParam.atmoIntLimits.Y, rnd.NextFloat());
            AtmosphereColor = Vector3.Lerp(PlanetParam.atmoMagmaMin, PlanetParam.atmoMagmaMax, rnd.NextFloat());
            //Pick texture pack

            //Fluids
            FluidLevel = MathHelper.Lerp(PlanetParam.magmaLimits.X, PlanetParam.magmaLimits.Y, rnd.NextFloat());
            FluidColor = Vector3.Lerp(PlanetParam.magmaColorMin, PlanetParam.magmaColorMax, rnd.NextFloat());
            FluidEmittivity = MathHelper.Lerp(PlanetParam.magmaEmittivity.X, PlanetParam.magmaEmittivity.Y, rnd.NextFloat());
        }

        private void GenerateTerran(Random rnd)
        {
            effectTechnique = Camera.NormalMapEffect.Techniques["PlanetRandomSurfaceTerran"];
            //Surface colors
            Color1 = Vector3.Lerp(PlanetParam.sandMin, PlanetParam.sandMax, rnd.NextFloat());
            Color2 = Vector3.Lerp(PlanetParam.greenMin, PlanetParam.greenMax, rnd.NextFloat());
            Color3 = Vector3.Lerp(PlanetParam.brownMin, PlanetParam.grayMax, rnd.NextFloat());


            //Atmosphere color
            AtmosphereIntensity = MathHelper.Lerp(PlanetParam.atmoIntLimits.X, PlanetParam.atmoIntLimits.Y, rnd.NextFloat());
            AtmosphereColor = Vector3.Lerp(PlanetParam.atmoTerraMin, PlanetParam.atmoTerraMax, rnd.NextFloat());
            //Pick texture pack

            //Fluids
            FluidLevel = MathHelper.Lerp(PlanetParam.waterLimits.X, PlanetParam.waterLimits.Y, rnd.NextFloat());
            FluidColor = Vector3.Lerp(PlanetParam.waterColorMin, PlanetParam.waterColorMax, rnd.NextFloat());
        }

        private void GenerateToxic(Random rnd)
        {
            effectTechnique = Camera.NormalMapEffect.Techniques["PlanetRandomSurfaceMagma"];
            //Surface colors
            Color1 = Vector3.Lerp(PlanetParam.sandMin, PlanetParam.sandMax, rnd.NextFloat());
            Color2 = Vector3.Lerp(PlanetParam.greenMin, PlanetParam.greenMax, rnd.NextFloat());
            Color3 = Vector3.Lerp(PlanetParam.brownMin, PlanetParam.grayMax, rnd.NextFloat());


            float toxicRnd = rnd.NextFloat();

            //Atmosphere color
            AtmosphereIntensity = MathHelper.Lerp(PlanetParam.atmoIntLimits.X, PlanetParam.atmoIntLimits.Y, rnd.NextFloat());
            AtmosphereColor = Vector3.Lerp(PlanetParam.atmoToxicMin, PlanetParam.atmoToxicMax, toxicRnd);
            //Pick texture pack

            //Fluids
            FluidLevel = MathHelper.Lerp(PlanetParam.toxicLimits.X, PlanetParam.toxicLimits.Y, rnd.NextFloat());
            FluidColor = Vector3.Lerp(PlanetParam.atmoToxicMin, PlanetParam.atmoToxicMax, toxicRnd);
            FluidEmittivity = MathHelper.Lerp(PlanetParam.toxicEmittivity.X, PlanetParam.toxicEmittivity.Y, rnd.NextFloat());
            FluidEmittivity = 1.5f;
        }

        private string GenerateName(Random rnd)
        {
            int length = rnd.IntBetween(4, 7);
            string name = new string(Enumerable.Repeat(chars, length).Select(s => s[rnd.Next(s.Length)]).ToArray());

            //Throw in dash
            int dashIndex = rnd.IntBetween(1, length - 1);
            name = name.Insert(dashIndex, "-");
            return name;


        }
    }
}
