using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Xml.Serialization;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;

namespace SolarConflict
{   
    [Serializable]
    public class InitColorConst:BaseInitColor
    {
        
        public Color color;

        public InitColorConst(Color color)
        {
            this.color = color;
        }

        public override Color Init(Projectile projectile, GameEngine gameEngine)
        {
            return color;
        }

        
    }
}
