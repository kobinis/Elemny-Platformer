//using Microsoft.Xna.Framework;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using XnaUtils;

//namespace SolarConflict
//{
//    public enum SensorType
//    {
//        /// <summary>
//        /// The distance from the enemy /1000
//        /// </summary>
//        EnemyDistance,
//        EnemyAngle, //The angle between the object head and the enemy (maybe to change AngleToEnemy) in radient unit (-pi,pi)
//        EnemyAngleAbslute,//TODO:change name - the ABS(EnemyAngle)
//        DangerDistance, //The most closest danger distance
//        DangerAngle,
//        DangerAngleAbsulute,
//        Freq1,
//        Energy,
//        //AllyDistance,
//        //AllyAngle,
//        //AllyAngleDistance,
//        //DangerDistance, //The most closest danger distance
//        //DangerAngle,
//        //DangerAngleDistance,
//        //EnemyUpDown, //is the enemy is up or down: 1 Up, -1 Down
//        //EnemyLeftRight, //is the enemy is left or right: 1 left, -1 right
//        //AllyUpDown, //is the closet Ally is up or down: 1 Up, -1 Down
//        //AllyLeftRight, //is the closet Ally is left or right: 1 left, -1 right
//        //DangerUpDown,//is the closet danger is up or down: 1 Up, -1 Down
//        //DangerLeftRight, //is the closet danger is left or right: 1 left, -1 right
//        //Energy, // player Energy
//        //HitPoints,  // Player HitPoints
//        //EnemyHitpoints,// Enemy HitPoints
//        //EnemyShield, // The Enemy shield energy
//        //Shield, // player shield energy
//        //NeutralUpDown, 
//        //NeutralLeftRight,
//        //GoalDistance,
//        //GoalAngle,
//        //GoalAngleDistance, 
//    }

//    [Serializable]
//    public struct AiFloat
//    {
//        public AiFloat(float value, bool isDefault = false)
//        {
//            Value = value;
//            IsDefault = isDefault;
//        }
//        public float Value;
//        public bool IsDefault;

//        public static implicit operator AiFloat(float value) //TODO: maybe remove
//        {
//            AiFloat o = new AiFloat(value);
//            return o;
//        }
//    }

//    public class Sensors //singelton class that holds all the sonsors
//    {
//        public delegate AiFloat SensorFunc(Agent agent, GameEngine gameEngine);

//        private static Sensors inst = null;

//        public static Sensors Inst
//        {
//            get
//            {
//                if (inst == null)
//                    inst = new Sensors();
//                return inst;
//            }
//        }

//        private Dictionary<SensorType, SensorFunc> sensorFunctions; //can be an array


//        private Sensors()
//        {
//            sensorFunctions = new Dictionary<SensorType, SensorFunc>(); //can be array
//            //TODO: gett all static methods, add them to the dictionery/array

//            sensorFunctions.Add(SensorType.EnemyDistance, new SensorFunc(GetEnemyDistance));
//            sensorFunctions.Add(SensorType.EnemyAngle, new SensorFunc(GetEnemyAngle));
//            sensorFunctions.Add(SensorType.EnemyAngleAbslute, new SensorFunc(GetEnemyAngleDistance));
//            sensorFunctions.Add(SensorType.DangerDistance, new SensorFunc(GetDangerDistance));
//            sensorFunctions.Add(SensorType.DangerAngle, new SensorFunc(GetDangerAngle));
//            sensorFunctions.Add(SensorType.DangerAngleAbsulute, new SensorFunc(GetDangerAngleDistance));
//            sensorFunctions.Add(SensorType.Freq1, new SensorFunc(GetFreq1));
//            sensorFunctions.Add(SensorType.Energy, new SensorFunc(GetEnergy));

//            /* sensorFunctions.Add(SensorType.HitPoints, new SensorFunc(GetHitpoints));
//             sensorFunctions.Add(SensorType.AllyDistance, new SensorFunc(GetAllyDistance));
//             sensorFunctions.Add(SensorType.AllyAngle, new SensorFunc(GetAllyAngle));
//             sensorFunctions.Add(SensorType.AllyAngleDistance, new SensorFunc(GetAllyAngleDistance));
//             sensorFunctions.Add(SensorType.DangerDistance, new SensorFunc(GetDangerDistance));
//             sensorFunctions.Add(SensorType.DangerAngle, new SensorFunc(GetDangerAngle));
//             sensorFunctions.Add(SensorType.DangerAngleDistance, new SensorFunc(GetDangerAngleDistance));
//             sensorFunctions.Add(SensorType.EnemyUpDown, new SensorFunc(GetEnemyUpDown));
//             sensorFunctions.Add(SensorType.EnemyLeftRight, new SensorFunc(GetEnemyLeftRight));
//             sensorFunctions.Add(SensorType.AllyUpDown, new SensorFunc(GetAllyUpDown));
//             sensorFunctions.Add(SensorType.AllyLeftRight, new SensorFunc(GetAllyLeftRight));
//             sensorFunctions.Add(SensorType.DangerUpDown, new SensorFunc(GetDangerUpDown));
//             sensorFunctions.Add(SensorType.DangerLeftRight, new SensorFunc(GetDangerLeftRight));
         
//             sensorFunctions.Add(SensorType.EnemyHitpoints, new SensorFunc(GetEnemyHitpoints));
//             sensorFunctions.Add(SensorType.EnemyShield, new SensorFunc(GetEnemyShield));
//             sensorFunctions.Add(SensorType.Shield, new SensorFunc(GetShield));*/
//        }

//        public AiFloat GetSensorValue(Agent agent, GameEngine gameEngine, SensorType sensorType)
//        {
//            return sensorFunctions[sensorType].Invoke(agent, gameEngine);
//        }


//        public static AiFloat GetFreq1(Agent agent, GameEngine gameEngine) //HOW to make it with param??
//        {
//            return (float)Math.Cos(agent.Lifetime * 0.1f); //Add param
//        }


//        //EnemyDistance
//        /// <summary>
//        /// The distance from the enemy /1000
//        /// </summary>        
//        public static AiFloat GetEnemyDistance(Agent agent, GameEngine gameEngine)
//        {            
//            GameObject target = agent.targetSelector.GetTarget(TargetType.Enemy);
//            if (target == null)
//            {
//                return new AiFloat(2, true);
//            }
//            return new AiFloat((target.Position - agent.Position).Length() * 0.001f);
//        }

//        /// <summary>
//        /// The facing angle to the enemy in radient- if you are facing it returns 0 
//        /// </summary>        
//        public static AiFloat GetEnemyAngle(Agent agent, GameEngine gameEngine)
//        {            
//            GameObject target = agent.targetSelector.GetTarget(TargetType.Enemy);
//            if (target == null)
//                return new AiFloat(0,true);
//            Vector2 enemyRelPos = target.Position - agent.Position;
//            float angle = (float)Math.Atan2(enemyRelPos.Y, enemyRelPos.X);
//            return new AiFloat(FMath.AngleDiff(angle, agent.Rotation)); //TODO: check, change, find bit on rotation
//        }

//        //public static AiFloat GetEnemyAngleDistance(Agent agent, GameEngine gameEngine)
//        //{
//        //    AiFloat value = agent.control.GetSensorValue(agent, gameEngine, SensorType.EnemyAngle).Value;
//        //    value.Value = Math.Abs(value.Value);
//        //    return value;
//        //}

//        public static AiFloat GetDangerAngle(Agent agent, GameEngine gameEngine)
//        {
         
//           GameObject target = agent.ClosesetDanger;           
//            if (target == null)
//                return new AiFloat(0, true);
//            Vector2 enemyRelPos = agent.Position - target.Position;
//            float angle = (float)Math.Atan2(enemyRelPos.Y, enemyRelPos.X);
//            return new AiFloat(FMath.AngleDiff(angle, agent.Rotation));
//        }

//        public static AiFloat GetDangerDistance(Agent agent, GameEngine gameEngine)
//        {
//            GameObject target = agent.ClosesetDanger;
//            if (target == null)
//            {
//                return new AiFloat(2, true);
//            }
//            return new AiFloat((target.Position - agent.Position).Length() * 0.001f);
//        }

//        public static AiFloat GetDangerAngleDistance(Agent agent, GameEngine gameEngine)
//        {
//            AiFloat value = agent.control.GetSensorValue(agent, gameEngine, SensorType.DangerAngle).Value;
//            value.Value = Math.Abs(value.Value);
//            return value;
//        }

//        public static AiFloat GetEnergy(Agent agent, GameEngine gameEngine)
//        {            
//            Meter meter = agent.GetMeter(MeterType.Energy);
//            if (meter == null)
//                return 0;
//            return meter.Value / meter.MaxValue;
//        }



//        ///* Get the closest danger distance */
//        //public static float? GetDangerDistance(Agent agent, GameEngine gameEngine, out SensorType sensorType)
//        //{
//        //    sensorType = SensorType.DangerDistance;
//        //    if (agent.ClosesetDanger == null)
//        //    {
//        //        return null;
//        //    }
//        //    return (agent.ClosesetDanger.Position - agent.Position).Length() * 0.001f;
//        //}

//        //public static float? GetDangerAngle(Agent agent, GameEngine gameEngine, out SensorType sensorType)
//        //{
//        //    sensorType = SensorType.DangerAngle;
//        //    if (agent.ClosesetDanger == null)
//        //    {
//        //        return null;
//        //    }


//        //    Vector2 enemyRelPos = agent.ClosesetDanger.Position - agent.Position;
//        //    float angle = (float)Math.Atan2(enemyRelPos.Y, enemyRelPos.X);
//        //    return FMath.AngleDiff(angle, agent.Rotation);


//        //}

//        //public static float? GetDangerAngleDistance(Agent agent, GameEngine gameEngine, out SensorType sensorType)
//        //{
//        //    sensorType = SensorType.DangerAngleDistance;

//        //    if (agent.ClosesetDanger == null)
//        //    {
//        //        return null;
//        //    }


//        //    Vector2 enemyRelPos = agent.ClosesetDanger.Position - agent.Position;
//        //    float angle = (float)Math.Atan2(enemyRelPos.Y, enemyRelPos.X);
//        //    return Math.Abs(FMath.AngleDiff(angle, agent.Rotation));
//        //}


//        /*
//        public static float? GetHitpoints(Agent agent, GameEngine gameEngine, out SensorType sensorType)
//        {
//            sensorType = SensorType.HitPoints; //ToDo, normalize
//            return agent.GetMeterValue(MeterType.Hitpoints); // /max+1
//        }


//        public static float? GetAllyDistance(Agent agent, GameEngine gameEngine, out SensorType sensorType)
//        {
//            sensorType = SensorType.AllyDistance;

//            GameObject target = agent.targetSelector.GetTarget(TargetType.Ally);

//            if (target == null)
//            {
//                return null;

//            }

//            return (target.Position - agent.Position).Length() * 0.001f;

//        }


//        public static float? GetAllyAngle(Agent agent, GameEngine gameEngine, out SensorType sensorType)
//        {
//            sensorType = SensorType.AllyAngle;

//            GameObject target = agent.targetSelector.GetTarget(TargetType.Ally);
//            if (target == null)
//                return null;
//            Vector2 enemyRelPos = target.Position - agent.Position;
//            float angle = (float)Math.Atan2(enemyRelPos.Y, enemyRelPos.X);
//            return FMath.AngleDiff(angle, agent.Rotation);
//        }

        
//        public static float? GetAllyAngleDistance(Agent agent, GameEngine gameEngine, out SensorType sensorType)
//        {
//            sensorType = SensorType.AllyAngleDistance;

//            GameObject target = agent.targetSelector.GetTarget(TargetType.Ally);
//            if (target == null)
//                return null;
//            Vector2 enemyRelPos = target.Position - agent.Position;
//            float angle = (float)Math.Atan2(enemyRelPos.Y, enemyRelPos.X);
//            return Math.Abs(FMath.AngleDiff(angle, agent.Rotation));
//        }

    

//        //Is the closest enemy is Up or down from us? 
//        public static float? GetEnemyUpDown(Agent agent, GameEngine gameEngine, out SensorType sensorType)
//        {
//            sensorType = SensorType.EnemyUpDown;
//            GameObject target = agent.targetSelector.GetTarget(TargetType.Enemy);
//            if (target == null)
//            {
//                return null;
//            }
//            if (target.Position.Y - agent.Position.Y > 0)
//            {
//                return -1; //enemy is down
//            }
//            else
//            {
//                return 1; //enemy is up
//            }
//        }

//        public static float? GetEnemyLeftRight(Agent agent, GameEngine gameEngine, out SensorType sensorType)
//        {
//            sensorType = SensorType.EnemyLeftRight;

//            GameObject target = agent.targetSelector.GetTarget(TargetType.Enemy);
//            if (target == null)
//            {
//                return null;
//            }
//            if (target.Position.X - agent.Position.X > 0)
//            {
//                return 1; //enemy is Left
//            }
//            else
//            {
//                return -1; //enemy is Right
//            }
//        }

//        public static float? GetAllyUpDown(Agent agent, GameEngine gameEngine, out SensorType sensorType)
//        {
//            sensorType = SensorType.AllyUpDown;

//            GameObject target = agent.targetSelector.GetTarget(TargetType.Ally);
//            if (target == null)
//            {
//                return null;
//            }
//            if (target.Position.Y - agent.Position.Y > 0)
//            {
//                return -1; //enemy is down
//            }
//            else
//            {
//                return 1; //enemy is up
//            }
//        }

//        public static float? GetAllyLeftRight(Agent agent, GameEngine gameEngine, out SensorType sensorType)
//        {
//            sensorType = SensorType.AllyLeftRight;

//            GameObject target = agent.targetSelector.GetTarget(TargetType.Ally);
//            if (target == null)
//            {
//                return null;
//            }
//            if (target.Position.X - agent.Position.X > 0)
//            {
//                return 1; //enemy is Left
//            }
//            else
//            {
//                return -1; //enemy is Right
//            }
//        }

//        public static float? GetDangerUpDown(Agent agent, GameEngine gameEngine, out SensorType sensorType)
//        {
//            sensorType = SensorType.DangerUpDown;

//            GameObject target = agent.ClosesetDanger;
//            if (target == null)
//            {
//                return null;
//            }
//            if (target.Position.Y - agent.Position.Y > 0)
//            {
//                return -1; //danger is down
//            }
//            else
//            {
//                return 1; //danger is up
//            }
//        }

//        public static float? GetDangerLeftRight(Agent agent, GameEngine gameEngine, out SensorType sensorType)
//        {
//            sensorType = SensorType.DangerLeftRight;

//            GameObject target = agent.ClosesetDanger;
//            if (target == null)
//            {
//                return null;
//            }
//            if (target.Position.X - agent.Position.X > 0)
//            {
//                return 1; //danger is left
//            }
//            else
//            {
//                return -1; //danger is right
//            }
//        }
     
//        public static float? GetEnemyHitpoints(Agent agent, GameEngine gameEngine, out SensorType sensorType)
//        {
//            sensorType = SensorType.EnemyHitpoints;

//            GameObject target = agent.targetSelector.GetTarget(TargetType.Enemy);
//            if (target == null)
//            {
//                return null;
//            }

//            return target.GetMeterValue(MeterType.Hitpoints);
//        }

        


//        public static float? GetEnemyShield(Agent agent, GameEngine gameEngine, out SensorType sensorType)
//        {
//            sensorType = SensorType.EnemyShield;
//            GameObject target = agent.targetSelector.GetTarget(TargetType.Enemy);
//            if (target == null)
//            {
//                return null;

//            }

//            return target.GetMeterValue(MeterType.Shield);

//        }
//        public static float? GetShield(Agent agent, GameEngine gameEngine, out SensorType sensorType)
//        {
//            sensorType = SensorType.Shield;
//            return agent.GetMeterValue(MeterType.Shield);
//        }
//        */

//    }
//}
