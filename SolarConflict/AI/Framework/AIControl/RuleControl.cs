//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;

//namespace SolarConflict.AI.GameAI
//{
    
//    [Serializable]
//    public class AiRule
//    {
//        /// <summary>
//        /// Parameters that determin if an out 
//        /// </summary>
//        [Serializable]
//        public struct Term
//        {
//            public SensorType InputType;
//            public float MaxValue;
//            public float MinValue;
//            public Term(SensorType inputType, float maxValue, float minValue)
//            {
//                InputType = inputType;
//                MaxValue = maxValue;
//                MinValue = minValue;
//            }
//        }

//        public ControlSignals Signal;
//        public List<Term> Terms { get; private set; }

//        public AiRule()
//        {
//            Terms = new List<AiRule.Term>();
//        }

//        public bool EvaluateRule(Agent agent, GameEngine gameEngine)
//        {
//            bool ans = true;
//            foreach (var item in Terms)
//            {
//                float value = agent.control.GetSensorValue(agent, gameEngine, item.InputType).Value;
//                ans &= value >= item.MinValue && value <= item.MaxValue;
//            }
//            return ans;
//        }

//        public static AiRule GetCloserRule(float minRange)
//        {
//            AiRule rule = new AiRule();
//            rule.Signal = ControlSignals.Up;
//            rule.Terms.Add(new Term(SensorType.EnemyDistance, float.MaxValue, minRange));
//            // rule.Terms.Add(new Term(SensorType.EnemyAngleAbslute, 4, 0));
//            return rule;
//        }

//        public static AiRule InRaneRule(ControlSignals signal, float range, float angleRange, float minAngleRange = 0)
//        {
//            AiRule rule = new AiRule();
//            rule.Signal = signal;
//            rule.Terms.Add(new Term(SensorType.EnemyDistance, float.MaxValue, range));
//            rule.Terms.Add(new Term(SensorType.EnemyAngleAbslute, angleRange, minAngleRange));
//            return rule;
//        }
//    }

//    [Serializable]
//    public class RuleControl : IAgentControl
//    {
//        public int ID { get; set; }
//        public List<AiRule> Ruels;
//        //AddSupprationRules
//        public float TargetLeadingFactor = 1;

//        public RuleControl()
//        {
//            Ruels = new List<AiRule>();
//            Ruels.Add(AiRule.GetCloserRule(1f));
//            //  Ruels.Add(AiRule.InRaneRule(ControlSignals.Action1, 1500, 0.4f));
//        }


//        public ControlSignals Update(Agent agent, GameEngine gameEngine, ref Vector2[] analogDirections)
//        {
//            GameObject goalTarget = agent.GetTarget(gameEngine, TargetType.Goal);
//            GameObject enemyTarget = agent.GetTarget(gameEngine, TargetType.Enemy);
//            if (enemyTarget != null)
//            {
//                float targetDis = GameObject.DistanceFromEdge(agent, enemyTarget);
//                float factor = Math.Min(targetDis * TargetLeadingFactor, 10);
//                Vector2 relPosReal = enemyTarget.Position + enemyTarget.Velocity * factor - agent.Position; //+agent.size*Sway

//                if (targetDis > 0)
//                {
//                    analogDirections[0] = relPosReal.Normalized(); //movment 
//                    analogDirections[1] = analogDirections[0]; //turret guns
//                }
//            }

//            if (goalTarget != null)
//            {
//                Vector2 relPosReal = goalTarget.Position - agent.Position;
//                analogDirections[0] = relPosReal.Normalized();
//            }

//            ControlSignals signals = ControlSignals.None;
//            foreach (var rule in Ruels)
//            {
//                if (rule.EvaluateRule(agent, gameEngine))
//                    signals |= rule.Signal;
//            }
//            return signals;
//        }

//        public IAgentControl GetWorkingCopy()
//        {
//            return (IAgentControl)MemberwiseClone();
//        }


//    }
//}
