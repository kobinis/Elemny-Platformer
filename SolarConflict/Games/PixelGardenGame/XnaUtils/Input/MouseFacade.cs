using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace PaintPlay.XnaUtils.Input
{
    class MouseFacade:InputFacade
    {
        MyMouse mouse;
        Vector2 firstPosition, prevPos;
        List<TouchState> touchList;

        public MouseFacade()
        {
            mouse = new MyMouse();
            touchList = new List<TouchState>(1);
            firstPosition = new Vector2();
        }

        public override void Update()
        {
            mouse.Update();
            if (mouse.LeftClick)
            {
                firstPosition = mouse.Pos;
                prevPos = mouse.Pos;
            }
        }

        public override void Draw()
        {
            foreach (TouchState touch in touchList)
            {
                //MyGraphics.Circle(touch.Position, 1, Color.Red);
            }
        }


        public override List<TouchState> GetTouchColection() //return a copy
        {
            touchList.Clear();
            TouchState state;
            if (mouse.GetLeft() || mouse.lastMouse.LeftButton == ButtonState.Pressed)
            {
                state = new TouchState();
                state.ID = 1;
                state.FirstPosition = firstPosition;
                state.Position = mouse.Pos;
                state.Rotation = 0;
                state.PreviousPosition = prevPos;
                state.PreviousRotation = 0;
                state.Size = 4;
                state.IsFingerOrPen = true;
                state.OnPress = mouse.LeftClick;
                state.OnRelease = (mouse.lastMouse.LeftButton == ButtonState.Pressed) && !mouse.GetLeft();
                touchList.Add(state);
                prevPos = mouse.Pos;
            }            
            return touchList;
        }
    }
}
