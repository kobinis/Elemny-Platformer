using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;

namespace SolarConflict
{
    [Serializable]
    public class UpdateSizeFunc : BaseUpdate
    {
        public float A, B, C;
        public float MinSize, MaxSize;
        
        public UpdateSizeFunc()
        {
            MinSize = 0;
            MaxSize = 2500;
        }        

        public override void Update(Projectile projectile, float normalizedLifeTime, GameEngine gameEngine)
        {
            float size = A * normalizedLifeTime * normalizedLifeTime + B *normalizedLifeTime + C;
            projectile.Size = MathHelper.Clamp(size, MinSize, MaxSize);
        }

        public static UpdateSizeFunc MidValues(float start, float mid, float end)
        {
            UpdateSizeFunc func = new UpdateSizeFunc();
            func.C = start;
            func.B = 4 * mid - 3 * start - end;
            func.A = 2 * end - 4 * mid + 2 * start;
            return func;            
        }
    }
}

