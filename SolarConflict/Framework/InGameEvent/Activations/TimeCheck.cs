using SolarConflict.Framework.InGameEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.Framework.InGameEvent.Activations {
    [Serializable]
    class TimeCheck : IEventActivationCheck {

        /// <summary>Governs what happens when the timer's conditional evaluates to false</summary>
        public enum BehaviorOnFail {
            Pause,
            Reset,
            Tickdown
        }

        /// <summary>An additional activation condition</summary>
        /// <remarks>If null, always considered true</remarks>
        public IEventActivationCheck Condition;
        public BehaviorOnFail OnFail;

        public int TimeInSeconds;
        int _value;



        public bool CheckActivation(Agent agent, GameEngine gameEngine) {
            if (!(Condition?.CheckActivation(agent, gameEngine) == false)) {
                // Precondition satisfied, tick on
                ++_value;

                if (_value >= TimeInSeconds) {
                    // Time elapsed
                    _value = 0;
                    return true;
                }

                return false;
            }

            // Precondition failed, respond accordingly
            switch (OnFail) {
                case BehaviorOnFail.Pause:
                    // Do nothing
                    break;

                case BehaviorOnFail.Reset:
                    _value = 0;
                    break;

                case BehaviorOnFail.Tickdown:
                    --_value;
                    break;
            }


            return false;            
        }

        public IEventActivationCheck GetWorkingCopy() {
            var result = MemberwiseClone() as TimeCheck;
            result.Condition = Condition.GetWorkingCopy();

            return result;
        }
    }
}
