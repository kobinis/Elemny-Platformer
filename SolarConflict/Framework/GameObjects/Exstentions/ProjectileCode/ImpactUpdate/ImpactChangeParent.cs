using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;

namespace SolarConflict
{
    [Serializable]
    public class ImpactChangeParent : BaseImpactUpdate 
    {        
        public override void Update(Projectile projectile, GameObject collidingObject, GameEngine gameEngine)
        {
            if (projectile != collidingObject && projectile != collidingObject.Parent) //need to add more checks
                projectile.Parent = collidingObject; //or get ancestor
        }
    }
}

