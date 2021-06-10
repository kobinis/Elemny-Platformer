using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework;

namespace SolarConflict
{
    /// <summary>
    /// IGameObjectFactory - creates an instance of a GameObject
    /// </summary>
    public interface IGameObjectFactory 
    {

        GameObject MakeGameObject(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0,
            int maxLifetime = 0, float? size = null, Color? color = null, float param = 0);

        GameObject MakeGameObject(GameEngine gameEngine,GameObject parent = null, FactionType faction = FactionType.Neutral, int maxLifetime = 0, float? size = null, Color? color = null, float param = 0);

        string ID //??
        {
            get;
            set;
        }
    }
}
