using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode.Init.InitColor
{
    [Serializable]
    class InitColorParent : BaseInitColor
    {
        public Color Color = Color.White;

        public override Color Init(Projectile projectile, GameEngine gameEngine) 
        {
            return projectile.Parent != null ? projectile.Parent.GetColor() : Color;            
        }
    }
}
