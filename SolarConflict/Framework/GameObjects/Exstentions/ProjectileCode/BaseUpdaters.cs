using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode
{
    [Serializable]
    public abstract class BaseEndOfLifeResolver //Runs in end of life (by time, impact or anything else)
    {
        public abstract bool Update(Projectile projectile, GameEngine gameEngine); 
    }

    [Serializable]
    public abstract class BaseInitColor
    {
        public abstract Color Init(Projectile projectile, GameEngine gameEngine);
    }

    [Serializable]
    public abstract class BaseImpactUpdate
    {
        public abstract void Update(Projectile projectile, GameObject collidingObject, GameEngine gameEngine);
    }

    /// <summary>
    /// Used as a base class for all updates on projectile state
    /// </summary>
    /// 
    [Serializable]
    public abstract class BaseUpdate
    {
        public abstract void Update(Projectile projectile, float normalizedLifeTime, GameEngine gameEngine);
    }

    [Serializable]
    public abstract class BaseDraw
    {
        public abstract void Draw(Camera camera, Projectile projectile);
    }

    /* public abstract class ImpectSpecGen
      {
          public abstract ImpactSpec GetImpectSpec(Projectile projectile, GameEngine gameEngine);
      }*/

}
