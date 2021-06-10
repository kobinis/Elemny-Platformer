using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SolarConflict.Framework.GameObjects.Emitters
{
    [Serializable]
    class PlayerEmitter : IEmitter
    {

        public string ID
        {
            get { return "Player"; }
            set { }
        }


        public GameObject Emit(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0,
            int maxLifetime = 0, float? size = null, Color? color = null, float param = 0)
        {
            if (MetaWorld.Inst.PlayerShip != null)
            {
                Agent player = (Agent)MetaWorld.Inst.PlayerShip;
                player.Position = refPosition;
                player.FactionType = faction;
                player.Rotation = refRotation;
                player.Velocity = refVelocity;
                player.RotationSpeed = refRotationSpeed;
                player.SetColor(color);

                gameEngine.AddGameObject(player);
                return player;
            }
            else
            {
                return null;
            }
        }
    }
}
