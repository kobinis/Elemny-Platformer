using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaintPlay.XnaUtils.Input
{
    enum Gesture {None, Pinch, Expand};
    class InputHelper
    {
        public static Gesture GetGesture(List<TouchState> touches)
        {
            float T = 400;
            if (touches.Count >= 2)
            {
                if ((touches[0].Position - touches[0].FirstPosition).LengthSquared() > T || (touches[1].Position - touches[1].FirstPosition).LengthSquared() > T)
                {
                    float ddis = (touches[0].Position - touches[1].Position).LengthSquared() - (touches[0].FirstPosition - touches[1].FirstPosition).LengthSquared();
                    if (ddis > 0)
                        return Gesture.Expand;
                    else
                        return Gesture.Pinch;
                }
                // 
            }

            return Gesture.None;
        }
    }
}
