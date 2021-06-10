using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XnaUtils.SimpleGui;

namespace XnaUtils.Input
{
    [Serializable]
    public struct CursorInfo //change name //maybe change to class?
    {
        //add Empty
        //Add Key
        //mybe add ?Keys
        public int ID { set; get; }
        public Vector2 Position { set; get; }
        public Vector2 PreviousPosition { set; get; }
        public Vector2 FirstPosition { set; get; }
        public bool IsActive { set; get; }
        public bool IsPressedLeft { set; get; }
        public bool IsLastPressedLeft { set; get; }
        public bool IsPressed { get { return IsPressedLeft || IsPressedRight; } }
        public bool WasNotStartedOnGui { set; get; }
        public GuiControl ActiveGuiControl { get; set; }

        public Point PostionAsPoint { get { return new Point((int)Position.X, (int)Position.Y);  } }

        public bool OnPressLeft
        {
            get
            {
                return IsPressedLeft && !IsLastPressedLeft;
            }
        }

        public bool OnReleaseLeft 
        {
            get
            {
                return !IsPressedLeft && IsLastPressedLeft;
            }
        }


        public bool IsPressedRight { set; get; }
        public bool IsLastPressedRight { set; get; }

        public bool OnPressRight
        {
            get
            {
                return IsPressedRight && !IsLastPressedRight;
            }
        }
        public bool OnReleaseRight
        {
            get
            {
                return !IsPressedRight && IsLastPressedRight;
            }
        }


        public int ScrollValue { set; get; }
        public int LastScrollValue { set; get; }



        public int GetDScroolWheel()
        {
            return ScrollValue - LastScrollValue;
        }


        public string UserData { set; get; }

        public TimeSpan Timestamp { set; get; }
        public TimeSpan PreviousTimestamp { set; get; }

        public double DeltaTime
        {
            get
            {
                return Timestamp.TotalMilliseconds - PreviousTimestamp.TotalMilliseconds;
            }
        }


        public Vector2 Speed
        {
            get
            {  
                return (Position - PreviousPosition) / (float)DeltaTime; //???
            }
        }

        //bool HasMoved  Speed > 0.5;

    }
}
