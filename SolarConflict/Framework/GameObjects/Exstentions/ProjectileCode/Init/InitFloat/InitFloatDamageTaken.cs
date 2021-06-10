using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict
{
    [Serializable]
    public class InitFloatParentDamageTaken : BaseInitFloat
    {
        public override float Init(Projectile projectile, GameEngine gameEngine)
        {
            if (projectile.Parent != null)
                return projectile.Parent.GetDamageTaken();
            else
                return 0;
        }
    }
}
