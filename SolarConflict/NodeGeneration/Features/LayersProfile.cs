using Microsoft.Xna.Framework;
using SolarConflict.GameWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict.Framework.World.Generation.Profiles
{
    class LayersProfile : GenerationFeature
    {
        public Vector2 Center { get; set; }
        public int numberOfLevels = 3;
        public int startingLevel = 0;
        public string BaseName;
        public int MaxNumberOfBatches = 30;
        public int MinNumberOfBatches = 5;
        public bool IsAddLevel;


        public LayersProfile(bool isAddLevel) : base()
        {
            IsAddLevel = isAddLevel;            
        }

        public override GameObject GenerationLogic(Scene scene, SceneGenerator generator)
        {
            float maxRadius = generator.SceneRadius*1.1f;
            float minRadius = generator.SunRadius;
            for (int i = 0; i < numberOfLevels; i++)
            {
                string name = BaseName;
                if(IsAddLevel)
                    name += (i + startingLevel).ToString();
                float minRad = maxRadius - (i + 1) * (maxRadius - minRadius) / (float)(numberOfLevels);
                float maxRad = maxRadius - i * (maxRadius - minRadius) / (float)(numberOfLevels);

                int numberOfBataches = (int)Math.Round((1f - i / (float)numberOfLevels) * (MaxNumberOfBatches - MinNumberOfBatches) + MinNumberOfBatches);

                int amount = Rand.Next(5, 10); //TODO: change
                if (ContentBank.Inst.ContainsEmitter(name))
                {
                    for (int j = 0; j < numberOfBataches; j++)
                    {
                        Vector2 pos = Center + FMath.ToCartesian(FMath.TransformToRadius((float)Rand.NextDouble(), maxRad, minRad), (float)Rand.NextDouble() * MathHelper.TwoPi);
                        scene.AddGameObject(name, FactionType.Neutral, pos, Rand.NextFloat() * MathHelper.TwoPi);
                    }
                }
                
            }
            return null;
        }

    }
}
