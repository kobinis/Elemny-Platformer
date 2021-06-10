using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.Framework.Emitters
{
    [Serializable]
    public class MeterChangeEmitter : IEmitter
    {
        public string ID { get; set; }


        public GameObject Emit(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0, int maxLifetime = 0, float? size = default(float?), Color? color = default(Color?), float param = 0)
        {
            if (parent != null)
            {
                parent.GetMeter(MeterType.Hitpoints).AddValue(300);
                parent.SetMeterValue(MeterType.Shield, 6000);
                parent.SetMeterValue(MeterType.StunTime, 60*5);
            }
            return null;
        }
    }
}
