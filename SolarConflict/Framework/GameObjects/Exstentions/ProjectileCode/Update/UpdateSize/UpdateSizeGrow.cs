using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;

namespace SolarConflict
{
    [Serializable]
    public class UpdateSizeGrow:BaseUpdate
    {
        public float dSize;
        public float mSize;
        public float MaxSize = float.MaxValue;
        public float MinSize = 0;


        public UpdateSizeGrow(): this(0,1)
        {           
        }

        public UpdateSizeGrow(float dSize): this(dSize, 1)
        {            
        }

        public UpdateSizeGrow(float dSize, float mSize, float maxSize = float.MaxValue, float minSize = 0)
        {
            this.dSize = dSize;
            this.mSize = mSize;
            this.MaxSize = maxSize;
            this.MinSize = minSize;
        }

        public override void Update(Projectile projectile, float normalizedLifeTime, GameEngine gameEngine)
        {
            projectile.Size = MathHelper.Clamp(projectile.Size * mSize + dSize, MinSize, MaxSize);
        }
    }
}
