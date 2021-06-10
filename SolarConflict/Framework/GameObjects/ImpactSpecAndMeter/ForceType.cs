using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict
{
    public enum ForceType : byte
    {
        FromCenter,
        Velocity,
        Rotation,
        Gravity,
        Mult,
        TargetRotation,
        GravityReversed,
        DirectionOfMovment,
    }
}
