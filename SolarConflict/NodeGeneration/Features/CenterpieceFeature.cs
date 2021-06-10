using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolarConflict.GameWorld;
using Microsoft.Xna.Framework;

namespace SolarConflict.Framework.World.Generation.Profiles
{
    class CenterpieceFeature : GenerationFeature
    {
        public float SizeMult { get; set; }
        GenerationFeature feature;
        public CenterpieceFeature(GenerationFeature feature, float size = 0):base()
        {
            Size = size;
            this.feature = feature;
            SizeMult = 1.1f;
        }

        public override GameObject GenerationLogic(Scene scene, SceneGenerator generator)
        {
            float? size = Size;
            if (Size == 0)
                size = null;
            feature.Parent = this;
            feature.LocalPosition = Vector2.Zero;

            GameObject centerpiece = feature.GenerationLogic(scene, generator);
            float centerpiceSize = Size;
            if (centerpiece != null)
               centerpiceSize = centerpiece.Size;

            generator.SunRadius = (int)(centerpiceSize * SizeMult);
            generator.Centerpice = centerpiece;
            return centerpiece;
        }
    }
}
