using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using XnaUtils;

namespace SolarConflict
{
    [Serializable]
    class MinerAI : IAgentControl
    {
        public int ID { get; set; }
        public int ShotRange { get; set; }
        private bool _isInventoryFull = false;


        public bool attack = true;

        public MinerAI(int shotRange = 1700)
        {
            ShotRange = shotRange;
        }

        public ControlSignals Update(Agent agent, GameEngine gameEngine, ref Vector2[] analogDirections)
        {
            
            ControlSignals controlSignals = 0;
            agent.targetSelector.DefaultTarget = GameObjectType.Mine;

            if (agent.Inventory != null && agent.Inventory.IsInventoryFull() && !_isInventoryFull)
            {
                _isInventoryFull = true;
                agent.SetTarget(agent.Parent, TargetType.Goal); // todo: maybe find closet base
            }

            if(agent.GetTarget(gameEngine, TargetType.Goal) == null)
            {
                _isInventoryFull = false;
            }

            if (_isInventoryFull)
            {
                agent.SetTarget(agent.Parent, TargetType.Goal);
                if(agent.Parent != null && agent.Parent.IsActive)
                {
                    agent.SetTarget(null, TargetType.Enemy);
                }
                if (GameObject.DistanceFromEdge(agent, agent.GetTarget(gameEngine, TargetType.Goal)) < 500)
                {
                    controlSignals |= ControlSignals.Action3;
                    _isInventoryFull = false;
                }             
            }
            else
            {
                agent.SetTarget(null, TargetType.Goal);
                var mine = agent.targetSelector.GetPotentialTargets(gameEngine, GameObjectType.Mine, agent.Position);
                agent.SetTarget(mine, TargetType.Goal);

                if (agent.GetTarget(gameEngine, TargetType.Goal) != null && GameObject.DistanceFromEdge(agent, agent.GetTarget(gameEngine, TargetType.Goal)) < 1000)
                {
                    controlSignals |= ControlSignals.Action1;
                }

            }

            float shotAngleRange = 0.25f;
            // float shotSpeed = 14;


            // float disRange = 2400;
            float minDis = 10850;


            
            GameObject target = agent.GetTarget(gameEngine, TargetType.Goal);

            if (target != null)
            {
                Vector2 relPosReal = Vector2.Zero;
                if (target != null)
                    relPosReal = target.Position - agent.Position;
                float targetDis = Math.Max( relPosReal.Length() - target.Size - agent.Size, 0 );

                Vector2 relPos = Vector2.Zero;
                if (target != null)
                    relPos = target.Position - agent.Position; //+ target.Velocity / targetDis * 10f
                float targetDeg = (float)Math.Atan2(relPos.Y, relPos.X);


                float targetDegDiff = (float)FMath.AngleDiff(targetDeg, agent.Rotation);

                Vector2 head = relPosReal;
                if (head != Vector2.Zero)
                    head.Normalize();
                if (float.IsNaN(head.X))
                    throw new Exception();
                for (int i = 0; i < analogDirections.Length; i++)
                {
                    analogDirections[i] = head;
                }

                if (FMath.Bern(Math.Max(targetDegDiff * 4f, 0), gameEngine.Rand))
                {
                    controlSignals |= ControlSignals.Left;
                }

                if (FMath.Bern(Math.Max(-targetDegDiff * 4f, 0), gameEngine.Rand))
                {
                    controlSignals |= ControlSignals.Right;
                }


                if ( FMath.Bern((targetDis - 260) / 1100f, gameEngine.Rand))
                {
                    controlSignals |= ControlSignals.Up; // !!!!!!!!!!!!!!!!!!!
                }
            }

            target = agent.GetTarget(gameEngine, TargetType.Enemy);
            if (target != null && target.IsActive)
            {

                Vector2 relPosReal = Vector2.Zero;

                if (target != null)
                    relPosReal = target.Position - agent.Position;
                float targetDis = Math.Max( relPosReal.Length() - target.Size, 0);

                Vector2 relPos = Vector2.Zero;
                if (target != null)
                    relPos = target.Position - agent.Position; //+ target.Velocity / targetDis * 10f

                float targetDeg = (float)Math.Atan2(relPos.Y, relPos.X);
                float targetDegDiff = (float)FMath.AngleDiff(targetDeg, agent.Rotation);

                Vector2 head = relPosReal;
                if (head != Vector2.Zero)
                    head.Normalize();
                if (float.IsNaN(head.X))
                    throw new Exception();
                for (int i = 0; i < analogDirections.Length; i++)
                {
                    analogDirections[i] = head;
                }

                if (FMath.Bern(Math.Max(targetDegDiff * 4f, 0), gameEngine.Rand))
                {
                    controlSignals |= ControlSignals.Left;
                }


                if (FMath.Bern(Math.Max(-targetDegDiff * 4f, 0), gameEngine.Rand))
                {
                    controlSignals |= ControlSignals.Right;
                }




                /*if (Math.Abs(targetDegDiff * 5f) < shotDegRange && targetDis < shotRange)
                    controlSignals = BitVector.AddBit(controlSignals, (int)ControlSignals.Action2);*/

                if (Math.Abs(targetDegDiff * 4f) < shotAngleRange && targetDis < ShotRange && attack)
                    controlSignals |= ControlSignals.Action1;

                if (Math.Abs(targetDegDiff * 1f) < shotAngleRange && targetDis < agent.Size + 550 && attack)
                    controlSignals |= ControlSignals.Action3;


                if (agent.ClosesetDanger != null && !agent.ClosesetDanger.IsNotActive)
                {
                    float projDis = (agent.ClosesetDanger.Position - agent.Position).Length();
                    if (projDis < 300)
                    {

                        //Vector2 enemyRelPos = ship.ClosesetDanger.Position - ship.Position;// +ship.ClosesetDanger.Velocity * (projDis / ship.ClosesetDanger.Velocity.Length()) - ship.Position;
                        //float deg = (float)Math.Atan2(enemyRelPos.Y, enemyRelPos.X);

                        //if (FMath.AngleDiff(deg, ship.Rotation) > 0)
                        //    controlSignals |= ControlSignals.Right;
                        //else
                        //    controlSignals |= ControlSignals.Left;
                    }
                }

                /*
                    if (MyMath.rand.Next(2) == 0)
                     {
                         if (targetDegDiff > 0)
                             controlSignals = BitVector.AddBit(controlSignals, (int)ControlSignals.StrifeR);
                         else
                             controlSignals = BitVector.AddBit(controlSignals, (int)ControlSignals.StrifeL);
                     }*/



                if (targetDis > 1000)  //MyMath.Bern((targetDis - minDis) / disRange - 0.1f))
                {
                    controlSignals |= ControlSignals.Up; // !!!!!!!!!!!!!!!!!!!
                }

                if (targetDis < minDis)
                {
                    controlSignals |= ControlSignals.Action2;
                }
            }
            else
            {
                if (agent.ClosesetDanger != null && !agent.ClosesetDanger.IsNotActive)
                {
                    float projDis = (agent.ClosesetDanger.Position - agent.Position).Length();
                    if (projDis < 500)
                    {
                        Vector2 enemyRelPos = agent.ClosesetDanger.Position + agent.ClosesetDanger.Velocity * (projDis / (agent.ClosesetDanger.Velocity.Length() + 0.01f)) - agent.Position;
                        float deg = (float)Math.Atan2(enemyRelPos.Y, enemyRelPos.X);

                        if (FMath.AngleDiff(deg, agent.Rotation) < 0)
                            controlSignals |= ControlSignals.Right;
                        else
                            controlSignals |= ControlSignals.Left;
                    }
                }

                GameObject idleTarget = agent.GetIdleTarget();
                if (idleTarget != null)
                {
                    Vector2 position = idleTarget.Position - agent.Position;
                    float angle = (float)Math.Atan2(position.Y, position.X);
                    float distance = position.Length();

                    if (FMath.Bern(distance / 10000, gameEngine.Rand)) //change it to
                        analogDirections[0] = FMath.ToCartesian(1, angle); //change to turn smooth


                    if (distance > 700)
                        controlSignals |= ControlSignals.Up;
                }


            }
            controlSignals |= ControlSignals.Brake;
            return controlSignals;
        }

        public ControlAI GetWorkingCopy()
        {
            ControlAI clone = (ControlAI)MemberwiseClone();
            return clone;
        }

        public int GetCameraPriority()
        {
            return 1;
        }


        IAgentControl IAgentControl.GetWorkingCopy()
        {
            return MemberwiseClone() as IAgentControl;
        }
    }
}
