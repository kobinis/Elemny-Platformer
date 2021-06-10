using Microsoft.Xna.Framework;
using SolarConflict.Framework.Agents.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict
{
    /// <summary>
    /// MeterGenerator - used to update a meter value and max value (used for energy generator and shield)
    /// </summary>
    /// <remarks>TODO: consider accounting for power expenditure, and not just damage, in integrity/disruption, so ships expending a bunch of power
    /// would be considered "in combat" and not benefit from boosted regeneration.</remarks>
    [Serializable]
    public class MeterGenerator : AgentSystem
    {
        /// <summary>
        /// The meter type that this Generator regenerates (Shield, Armor, Health...)
        /// </summary>
        public MeterType MeterType;
        /// <summary>
        /// The meter type that this Generator regenerates (Shield
        /// </summary>
        public MeterType MeterToTakeMaxValue;
        /// <summary>
        /// Maximum value that will be set to meter
        /// </summary>
        public float MaxValue;


        public int TickDelay = 2;

        public float GenerationAmountPerTick;
        public float DisruptedGenerationAmountPerTick;

        /// <summary>
        /// Number of units to be regenerated every sec
        /// </summary>
        public float GenerationAmountPerSec { get { return GenerationAmountPerTick * 60f / TickDelay; } set { GenerationAmountPerTick = value / 60f * TickDelay; } }

        

        ///// <summary>
        ///// The number of frames in a tick
        ///// </summary>
        //public int GenerationCooldownTime; //TODO: in case you add also fix MeterGeneratorTemplate

        /// <summary>The system's generation rate while it is disrupted (see Integrity)</summary>
        public float DisruptedGenerationAmountPerSec { get { return DisruptedGenerationAmountPerTick * 60f /TickDelay; } set { DisruptedGenerationAmountPerTick = value / 60f * TickDelay; } }





        /// <summary>
        /// Applied when meter reaches 0 to delay regenration on depletion
        /// </summary>
        //public int DepletionCooldownTime;

        /// <summary>
        /// Time that takes the Shield to recover after damage (While
        /// </summary>
        public int RechargeDelayInFrames;

        private bool isOld; 
     
      //  private int depletionCooldown;


        public MeterGenerator()
        {
            MeterType = MeterType.Shield;
            MeterToTakeMaxValue = MeterType.None;
            MaxValue = 0;

            GenerationAmountPerSec = 60;
         //   GenerationCooldownTime = 1;
        //    DepletionCooldownTime = 0;
            //isNew = true;

         //   generationCooldown = 0;
            //depletionCooldown = 0;
        }

        //public override void OnDamageSustained(float amount) {            
        //    Integrity?.OnDamageSustained(amount);
        //}

        public override bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate = false)
        {
            //Integrity?.Update();

            //Sets maximum value of the meter to MaxValue        
            if (MeterToTakeMaxValue != MeterType.None)
            {
                agent.AddMeterValue(MeterToTakeMaxValue, MaxValue);
            }

            Meter meter = agent.GetMeter(MeterType); //TODO: if null?
            if (meter == null)
            {
                meter = new Meter(MaxValue);
                agent.SetMeter(MeterType, meter);
            }

            //Sets The value of the meter to MaxValue
            if (!isOld && meter.MaxValue >= MaxValue)
            {
                agent.MaxMeterValue(MeterType, MaxValue);
                isOld = true;
            }

            //Depletion Cooldown 
            //if(meter.Value == 0)
            //{
            //    depletionCooldown = DepletionCooldownTime;
            //}
            //depletionCooldown--;

            //Apply Generation
            if (TickDelay != 0 && agent.Lifetime % TickDelay == 0)//TODO: add tick time // && depletionCooldown <= 0)
            {
                //ActivityManager.Inst.AddToast(agent.DamageTimer.ToString() + " - " +RechargeDelayInFrames.ToString(), 100);
                if (agent.DamageTimer < RechargeDelayInFrames)
                    meter.AddValue(DisruptedGenerationAmountPerTick);
                else
                    meter.AddValue(GenerationAmountPerTick);                
            }
            
           

            return false;
        }        

        public override AgentSystem GetWorkingCopy()
        {          
            var result = MemberwiseClone() as MeterGenerator;   
            return result;
        }

    }

}
