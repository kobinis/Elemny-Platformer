using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SolarConflict.Framework.GameObjects.Emitters
{
    [Serializable]
    public class PlayerCloneEmitter : IEmitter //add option to clone without cargo/Maybe difffrent emitter??? //maybe combine all
    {        
                
        public string ID
        {
            get { return "PlayerClone"; }
            set { }
        }


        public GameObject Emit(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0,
            int maxLifetime = 0, float? size = null, Color? color = null, float param = 0)      
        {
            if (MetaWorld.Inst.PlayerShip != null)
            {
                Agent player = (Agent)MetaWorld.Inst.PlayerShip;                
                GameObject playerClone = player.Emit(gameEngine, parent, faction, refPosition, refVelocity, refRotation, refRotationSpeed, maxLifetime, size, color);
                return playerClone;
            }
            else
            {
                return null;
            }
        }

        public IEmitter Load(string path)
        {
            return null;
        }
    }
}
