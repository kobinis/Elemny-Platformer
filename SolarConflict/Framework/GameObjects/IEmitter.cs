using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework;

namespace SolarConflict
{        
    public interface IEmitter 
    {
        GameObject Emit(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0,
            int maxLifetime = 0, float? size = null, Color? color = null, float param = 0);  //?Instead of param or in addition add a function that get's called on the object

        string ID //??
        {
            get;
            set;
        }        
    }
}
