using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SolarConflict.Framework.Emitters
{

    //public enum MeterSetType { ToValue, Additive, Max, Min, Mult, }

    class MeterSetterEmitter : IEmitter
    {
        public string ID { get; set; }

        /// <summary>
        /// The faction to applay the effect to, if null it will take the gaction passed by the emitter
        /// </summary>
        public FactionType? Faction;

        private List<Tuple<MeterType, float>> metersToSet;

        public void AddMeter(MeterType type, float value)
        {
            metersToSet.Add(new Tuple<MeterType, float>(type, value));
        }

        public MeterSetterEmitter()
        {
            metersToSet = new List<Tuple<MeterType, float>>();
        }

        public GameObject Emit(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0, int maxLifetime = 0, float? size = default(float?), Color? color = default(Color?), float param = 0)
        {
            if (Faction != null)
                faction = Faction.Value;
            foreach (var gameObject in gameEngine._collideAllCheckList)
            {
                if(faction == FactionType.None || faction == gameObject.GetFactionType())
                {
                    foreach (var meterValue in metersToSet)
                    {
                        gameObject.SetMeterValue(meterValue.Item1, meterValue.Item2);
                    }                    
                }
            }
            return null;
        }
    }
}
