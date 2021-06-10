using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;

namespace SolarConflict
{
    [Serializable]
    public class UpdateColorFadeInOut : ColorUpdater
    {
        public float bais1, bais2, multiply1, multiply2;


      /*  public UpdateColorFadeInOut()
        {
            bais1 = 1;
            multiply1 = -1;
            bais2 = 0;
            multiply2 = 1;
        }*/

        
        public UpdateColorFadeInOut()
        {
            bais1 = 2;
            multiply1 = -2;
            bais2 = 0;
            multiply2 = 2;
        }

        public override void Update(Projectile projectile, float normalizedLifeTime, GameEngine gameEngine)
        {
            float alpha = MathHelper.Clamp((bais1 + normalizedLifeTime * multiply1)*(bais2 + normalizedLifeTime*multiply2), 0, 1);
            //float alpha = 1;
            projectile.SetColorAlpha((byte)(alpha * 255));

            /*if (normalizedLifeTime > 0.8)
            {
                byte b = (byte)((1 - normalizedLifeTime) * 255);
            }*/

            //  projectile.SetColorAlpha(0);
        }

    }
}
