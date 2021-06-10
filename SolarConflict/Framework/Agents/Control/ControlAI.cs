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
    class ControlAI : IAgentControl
    {
        public int ID { get; set; }
        public int ShotRange { get; set; }

        public bool attack = true;

        public ControlAI(int shotRange = 1700)
        {
            ShotRange = shotRange;
        }

        public ControlSignals Update(Agent ship, GameEngine engine, ref Vector2[] analogDirections)
        {


            float shotAngleRange = 0.25f;
            // float shotSpeed = 14;


            // float disRange = 2400;
            float minDis = 10850;


            ControlSignals controlSignals = 0;
            GameObject target = ship.GetTarget(engine, TargetType.Goal);
            if (target != null)
            {
                Vector2 relPosReal = Vector2.Zero;
                relPosReal = target.Position - ship.Position;
                float targetDis = Math.Max(relPosReal.Length() - target.Size, 0);

                Vector2 relPos = Vector2.Zero;
                if (target != null)
                    relPos = target.Position - ship.Position; //+ target.Velocity / targetDis * 10f
                float targetDeg = (float)Math.Atan2(relPos.Y, relPos.X);


                float targetDegDiff = (float)FMath.AngleDiff(targetDeg, ship.Rotation);

                Vector2 head = relPosReal;
                if (head != Vector2.Zero)
                    head.Normalize();
                if (float.IsNaN(head.X))
                    throw new Exception();
                for (int i = 0; i < analogDirections.Length; i++)
                {
                    analogDirections[i] = head;
                }

                if (FMath.Bern(Math.Max(targetDegDiff * 4f, 0), engine.Rand))
                {
                    controlSignals |= ControlSignals.Left;
                }


                if (FMath.Bern(Math.Max(-targetDegDiff * 4f, 0), engine.Rand))
                {
                    controlSignals |= ControlSignals.Right;
                }

                if (FMath.Bern((targetDis - minDis) / 1000 - 0.1f, engine.Rand))
                {
                    controlSignals |= ControlSignals.Up; // !!!!!!!!!!!!!!!!!!!
                }

            }

            target = ship.GetTarget(engine, TargetType.Enemy);
            if (target != null && target.IsActive)
            {

                Vector2 relPosReal = Vector2.Zero;



                if (target != null)
                    relPosReal = target.Position - ship.Position;
                float targetDis = relPosReal.Length();

                Vector2 relPos = Vector2.Zero;
                if (target != null)
                    relPos = target.Position - ship.Position; //+ target.Velocity / targetDis * 10f
                float targetDeg = (float)Math.Atan2(relPos.Y, relPos.X);


                float targetDegDiff = (float)FMath.AngleDiff(targetDeg, ship.Rotation);

                Vector2 head = relPosReal;
                if (head != Vector2.Zero)
                    head.Normalize();
                if (float.IsNaN(head.X))
                    throw new Exception();
                for (int i = 0; i < analogDirections.Length; i++)
                {
                    analogDirections[i] = head;
                }

                if (FMath.Bern(Math.Max(targetDegDiff * 4f, 0), engine.Rand))
                {
                    controlSignals |= ControlSignals.Left;
                }


                if (FMath.Bern(Math.Max(-targetDegDiff * 4f, 0), engine.Rand))
                {
                    controlSignals |= ControlSignals.Right;
                }




                /*if (Math.Abs(targetDegDiff * 5f) < shotDegRange && targetDis < shotRange)
                    controlSignals = BitVector.AddBit(controlSignals, (int)ControlSignals.Action2);*/

                if (Math.Abs(targetDegDiff * 4f) < shotAngleRange && targetDis < ShotRange && attack)
                    controlSignals |= ControlSignals.Action1;

                if (Math.Abs(targetDegDiff * 1f) < shotAngleRange && targetDis < ship.Size + 550 && attack)
                    controlSignals |= ControlSignals.Action3;


                if (ship.ClosesetDanger != null && !ship.ClosesetDanger.IsNotActive)
                {
                    float projDis = (ship.ClosesetDanger.Position - ship.Position).Length();
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



                if (targetDis > 600)  //MyMath.Bern((targetDis - minDis) / disRange - 0.1f))
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
                if (ship.ClosesetDanger != null && !ship.ClosesetDanger.IsNotActive)
                {
                    float projDis = (ship.ClosesetDanger.Position - ship.Position).Length();
                    if (projDis < 500)
                    {
                        Vector2 enemyRelPos = ship.ClosesetDanger.Position + ship.ClosesetDanger.Velocity * (projDis / (ship.ClosesetDanger.Velocity.Length() + 0.01f)) - ship.Position;
                        float deg = (float)Math.Atan2(enemyRelPos.Y, enemyRelPos.X);

                        if (FMath.AngleDiff(deg, ship.Rotation) < 0)
                            controlSignals |= ControlSignals.Right;
                        else
                            controlSignals |= ControlSignals.Left;
                    }
                }

                GameObject idleTarget = ship.GetIdleTarget();
                if (idleTarget != null)
                {
                    Vector2 position = idleTarget.Position - ship.Position;
                    float angle = (float)Math.Atan2(position.Y, position.X);
                    float distance = position.Length();

                    if (FMath.Bern(distance / 10000, engine.Rand)) //change it to
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
