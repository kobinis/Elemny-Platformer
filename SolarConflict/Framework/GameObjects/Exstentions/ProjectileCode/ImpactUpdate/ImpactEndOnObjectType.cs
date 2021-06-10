using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict
{
    [Serializable]
    class ImpactEndOnObjectType: BaseImpactUpdate
    {
        public GameObjectType gameObjectType = GameObjectType.Ship;
        public IEmitter emmiter;

        public ImpactEndOnObjectType()
        {
            emmiter = null;
        }

        public ImpactEndOnObjectType(GameObjectType gameObjectType)
        {
            this.gameObjectType = gameObjectType;
        }

        public override void Update(Projectile projectile, GameObject collidingObject, GameEngine gameEngine)
        {

            if ((collidingObject.GetObjectType() & gameObjectType) > 0) //change
            {
                if (emmiter != null)
                    emmiter.Emit(gameEngine, projectile, projectile.GetFactionType(), projectile.Position, projectile.Velocity, projectile.Rotation);                    
                projectile.IsActive = false;
            }
        }
    }
}
