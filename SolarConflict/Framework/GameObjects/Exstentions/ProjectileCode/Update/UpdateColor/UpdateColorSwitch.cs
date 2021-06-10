using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;

namespace SolarConflict
{
    /// <summary>
    /// Fades between colors
    /// </summary> 
    [Serializable]
    public class UpdateColorSwitch : ColorUpdater
    {
        public Color begin, end;

        public UpdateColorSwitch():this(Color.White, Color.Black)
        {

        }

        public UpdateColorSwitch(Color begin, Color end)
        {            
            this.begin = begin;
            this.end = end;
        }

        public override void Update(Projectile projectile, float normalizedLifetime, GameEngine gameEngine)
        {
            projectile.ParticleColor =new Color((int)(begin.R * (1 - normalizedLifetime) + end.R * normalizedLifetime),
                (int)(begin.G * (1 - normalizedLifetime) + end.G * normalizedLifetime),
                (int)(begin.B * (1 - normalizedLifetime) + end.B * normalizedLifetime),
                (int)(begin.A * (1 - normalizedLifetime) + end.A * normalizedLifetime));                        
        }

        
    }
}
