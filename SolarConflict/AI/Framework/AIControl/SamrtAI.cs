//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Audio;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Media;
//using XnaUtils;

//namespace SolarConflict
//{
//    [Serializable]
//    class SmartAI : IAgentControl
//    {

//        public int ID { get; set; }

//        public float ShotRange = 2000;
//        public float ShotAngleRange = MathHelper.ToRadians(45);
//        public float ShotSpeed = 35;

//        public float OptimalDistance = 1500;
//        public float DistanceSlack = 500;
//        public float MinimalDistance = 200; //For engaging DOWN

//        public float MinimalDistanceToGoal = 10f;

//        public bool PrioritizeGoal = false;
//        public bool IsUsingBreaks = true;
//        public bool IsEvasive = true;
//        // public bool PatrolOnIdle = false;

//        public ControlSignals Update(Agent agent, GameEngine engine, ref Vector2[] analogDirections)
//        {
//            ControlSignals controlSignals = 0;

//            GameObject goalTarget = agent.GetTarget(engine, TargetType.Goal);
//            GameObject enemyTarget = agent.GetTarget(engine, TargetType.Enemy);
//            //Vector2 relPosReal = Vector2.Zero;

//            if (enemyTarget != null)
//            {
//                float targetDis = GameObject.DistanceFromEdge(agent, enemyTarget); ;
//                float factor = (float)Math.Cos(agent.Lifetime * 0.05f) * Math.Min(targetDis / (ShotSpeed + (float)Math.Cos(agent.Lifetime * 0.3f) * 5), 1000);
//                Vector2 relPosReal = enemyTarget.Position + (enemyTarget.Velocity - agent.Velocity) * factor - agent.Position;

//                if (targetDis > 0)
//                {
//                    analogDirections[0] = relPosReal.Normalized();
//                    analogDirections[1] = analogDirections[0];
//                }

//                if (FMath.Bern((targetDis - MinimalDistance) / 1000 - 0.1f, engine.Rand))
//                {
//                    controlSignals |= ControlSignals.Up;
//                }

//                if (targetDis < ShotRange)
//                {
//                    float targetAngle = (float)Math.Atan2(relPosReal.Y, relPosReal.X);
//                    float targetDegDiff = (float)FMath.AngleDiff(targetAngle, agent.Rotation);
//                    if (targetDegDiff < ShotAngleRange)
//                    {
//                        controlSignals |= ControlSignals.Action1;
//                    }

//                    if (targetDegDiff < ShotAngleRange * 1.3f && targetDis < ShotRange * 0.8f)
//                    {
//                        controlSignals |= ControlSignals.Action2;
//                    }
//                }

//                if (agent.GetDamageTaken() > 0)
//                {
//                    controlSignals |= ControlSignals.Action4;
//                }
//            }

//            if (goalTarget != null && (enemyTarget == null || PrioritizeGoal))
//            {
//                Vector2 relPosReal = goalTarget.Position - agent.Position;
//                float targetDis = GameObject.DistanceFromEdge(agent, goalTarget); ;
//                if (targetDis > MinimalDistanceToGoal)
//                    analogDirections[0] = relPosReal.Normalized();

//                if (FMath.Bern((targetDis - MinimalDistanceToGoal) / 1000 - 0.1f, engine.Rand))
//                {
//                    controlSignals |= ControlSignals.Up;
//                }
//            }

//            if (IsEvasive)
//                controlSignals |= UpdateEvasive(agent, engine, analogDirections);


//            //controlSignals |= ControlSignals.Brake;
//            return controlSignals;
//        }

//        private ControlSignals UpdateEvasive(Agent agent, GameEngine engine, Vector2[] analogDirections)
//        {
//            ControlSignals controlSignals = 0;

//            // Evasion (right?)
//            float dangerDistance = float.MaxValue;
//            if (agent.ClosesetDanger != null && agent.ClosesetDanger.IsActive)
//                dangerDistance = (agent.ClosesetDanger.Position - agent.Position).Length() - agent.ClosesetDanger.Size - agent.Size;
//            if (dangerDistance < 1500) //can add pretication to were the shot will be
//            {
//                Vector2 dangerPos = agent.ClosesetDanger.Position + agent.ClosesetDanger.Velocity * dangerDistance - agent.Position;
//                float dangerAngle = (float)Math.Atan2(dangerPos.Y, dangerPos.X);
//                float dangerAngelDiff = FMath.AngleDiff(dangerAngle, agent.Rotation);
//                if (dangerDistance < 60) //mainly for sun safty /add check for facing danger
//                {
//                    controlSignals &= ~ControlSignals.Up;
//                    controlSignals |= ControlSignals.Down;
//                }
//                if (dangerAngelDiff > 0)
//                {
//                    controlSignals |= ControlSignals.Right;
//                }
//                else
//                {
//                    controlSignals |= ControlSignals.Left;
//                }

//            }
//            return controlSignals;
//        }

//        public IAgentControl GetWorkingCopy()
//        {
//            var clone = (SmartAI)MemberwiseClone();
//            return clone;
//        }
//    }
//}
