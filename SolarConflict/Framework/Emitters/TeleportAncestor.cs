
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SolarConflict.Framework.GameObjects.Emitters
{
    [Serializable]
    class TeleportAncestor: IEmitter
    {

        public string ID
        {
            get { return "TeleportAncestor"; }
            set { }
        }


        public GameObject Emit(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0,
            int maxLifetime = 0, float? size = null, Color? color = null, float param = 0)
        {
            if(parent!= null)
            {
                GameObject ancestor = parent.GetAgentAncestor();
                if (ancestor != null)
                {
                    ancestor.Position = refPosition;
                    //parent.Velocity = refVelocity;
                    //parent
                }
            }             
            return parent;
        }

    
    }
}

