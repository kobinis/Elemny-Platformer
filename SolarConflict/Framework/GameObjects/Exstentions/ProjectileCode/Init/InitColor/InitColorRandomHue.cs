using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XnaUtils.Framework.Graphics;

namespace SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode.Init.InitColor
{
    [Serializable]
    class InitColorRandomHue : BaseInitColor
    {

        private float s, v;

        public InitColorRandomHue(float s = 1, float v = 1)
        {
            this.s = s;
            this.v = v;
        }

        public override Color Init(Projectile projectile, GameEngine gameEngine)
        {
            float h = gameEngine.Rand.NextFloat() * 360;
            return GraphicsUtils.HsvToRgb(h, s, v);            
        }
    }
}
