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
    //can hold state
    [Serializable]
    class PlayerXbox : IPlayerControl
    {

        public static float vibrationTimer = 0;


        PlayerIndex playerIndex = PlayerIndex.One;

        [NonSerialized]
        GamePadState xBoxState;
        [NonSerialized]
        GamePadState prevXBoxState;
            
        Vector2 analog1, analog2;

        public PlayerXbox(PlayerIndex playerIndex)
        {
            this.playerIndex = playerIndex;
            vibrationTimer = 0;
            xBoxState = new GamePadState();
            prevXBoxState = new GamePadState();
        }       

        public  ControlSignals UpdateAgent(int index, Agent agent, GameEngine gameEngine, ref Vector2[] analogDirections)
        {

            ControlSignals controlSignals = 0;
                       
            if (xBoxState.Buttons.LeftShoulder == ButtonState.Pressed)
            {
                controlSignals |= ControlSignals.Left;
            }

            if (xBoxState.Buttons.RightShoulder == ButtonState.Pressed)
            {
                controlSignals |= ControlSignals.Right;
            }


            if (xBoxState.Triggers.Left > 0f)
                controlSignals |= ControlSignals.Up;
            if (xBoxState.Triggers.Right > 0f)
                controlSignals |= ControlSignals.Down;

            if (xBoxState.Buttons.X == ButtonState.Pressed)
                controlSignals |= ControlSignals.Action1;
            if (xBoxState.Buttons.A == ButtonState.Pressed)
                controlSignals |= ControlSignals.Action2;
            if (xBoxState.Buttons.Y == ButtonState.Pressed)
                controlSignals |= ControlSignals.Action3;
            if (xBoxState.Buttons.B == ButtonState.Pressed)
                controlSignals |= ControlSignals.Action4;




            if (xBoxState.IsButtonDown(Buttons.RightStick))
                controlSignals |= ControlSignals.Action1;
            bool isLeftThumstickOn = false;
            if (xBoxState.ThumbSticks.Left.LengthSquared() > 0.01f)
            {
                analog1 = new Vector2(xBoxState.ThumbSticks.Left.X, -xBoxState.ThumbSticks.Left.Y);
                isLeftThumstickOn = true;
                analog2 = analog1;                
            }
            analogDirections[0] = analog1;            

            if (xBoxState.ThumbSticks.Right.LengthSquared() > 0.01f)
            {                
                analog2 = new Vector2(xBoxState.ThumbSticks.Right.X, -xBoxState.ThumbSticks.Right.Y);
                if(!isLeftThumstickOn)
                {
                    analog1 = analog2;
                }
            }

            analogDirections[1] = analog2;

            //if (xBoxState.IsButtonDown(Buttons.LeftStick))
            //{
            //    float rotation = (float)Math.Atan2(analog1.Y, analog1.X);
            //    if(Math.Abs( FMath.AngleDiff(rotation, agent.Rotation)) < 0.1f)
            //        controlSignals |= ControlSignals.Up;
            //}

            if (xBoxState.DPad.Right == ButtonState.Pressed)
            {
                controlSignals |= ControlSignals.Right;
            }

            if (xBoxState.DPad.Left == ButtonState.Pressed)
            {
                controlSignals |= ControlSignals.Left;
            }

            if (xBoxState.ThumbSticks.Left.LengthSquared() > 0.7f)//(xBoxState.DPad.Up == ButtonState.Pressed )
            {
                float rotation = (float)Math.Atan2(analog1.Y, analog1.X);
                if(xBoxState.ThumbSticks.Left.LengthSquared() > 0.9f)
                    controlSignals |= ControlSignals.Up;
            }

            if (xBoxState.ThumbSticks.Right.LengthSquared() > 0.7f)//(xBoxState.DPad.Up == ButtonState.Pressed )
            {
                controlSignals |= ControlSignals.Action1;
            }

            if (xBoxState.DPad.Down == ButtonState.Pressed)
            {
                controlSignals |= ControlSignals.Right;
            }
            //vibrationTimer -= 0.1f;
            //GamePad.SetVibration(playerIndex, MathHelper.Clamp(vibrationTimer, 0, 1), MathHelper.Clamp(vibrationTimer, 0, 1));
            //controlSignals |= ControlSignals.Brake;
            return controlSignals;
        }

        public void CommandUpdate(Scene scene)
        {
            prevXBoxState = xBoxState;
            xBoxState = GamePad.GetState(playerIndex, GamePadDeadZone.Circular); //change, move it to inputstate
            
        }

        public bool IsCommandOn(PlayerCommand command)
        {
            return false;
        }

        public bool IsCommandClicked(PlayerCommand command)
        {
            if (command == PlayerCommand.SwapUp)
            {
                return xBoxState.IsButtonDown(Buttons.Start) && prevXBoxState.IsButtonUp(Buttons.Start);
            }

            if (command == PlayerCommand.Use)
            {
                return xBoxState.IsButtonDown(Buttons.LeftTrigger) && prevXBoxState.IsButtonUp(Buttons.LeftTrigger);
            }
            return false;
        }

        public string GetCommandTag(PlayerCommand command)
        {
            return null;
        }

        public string GetControlTag(ControlSignals signal)
        {
            return null;
        }
    }
}
