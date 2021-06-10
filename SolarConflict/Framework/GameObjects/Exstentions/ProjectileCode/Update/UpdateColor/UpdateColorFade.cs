using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;

namespace SolarConflict
{
    [Serializable]
    public class UpdateColorFade:ColorUpdater
    {

        public float Offset;
        public float Multiply;

        public UpdateColorFade()
        {
            Offset = 1;
            Multiply = -1;
        }

        public UpdateColorFade(float bais, float multiply)
        {
            this.Offset = bais;
            this.Multiply = multiply;
        }

        public override void Update(Projectile projectile, float normalizedLifeTime, GameEngine gameEngine)
        {
            float alpha = MathHelper.Clamp(Offset + normalizedLifeTime * Multiply, 0, 1);
            projectile.SetColorAlpha((byte)(alpha*255));        
        }
        
    }
}
