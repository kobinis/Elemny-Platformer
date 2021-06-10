using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PaintPlay.XnaUtils.Input
{
    public class TouchState //or struct;
    {
        public Vector2 Position { set; get; }
        public bool IsFingerOrPen { set; get; }
        public float Rotation { set; get; }
        public Vector2 PreviousPosition { set; get; }
        public float PreviousRotation { set; get; }
        public int Size { set; get; }
        public Vector2 FirstPosition {set; get;}
        public float FirstRotation { set; get; }
        public int ID { set; get; }
        public bool OnPress { set; get; }
        public bool OnRelease { set; get; }
        public int UserData { set; get; }
       

        public Vector2 GetSpeed()
        {
            return Position - PreviousPosition;
        }
    }

    abstract class InputFacade
    {        
        public abstract void Update();
        public abstract void Draw();
        public abstract List<TouchState> GetTouchColection();        
        
    }
}
