using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict
{
    public enum ImpactType : byte
    {
        Additive,
        Max,
        Min,
        Mult,
        Velocity,
        OneOverRad, //Liniar down
        OneOverRadSquared,
        MySize, MyMass, TargetMass, TargetSize, TargetHitpoints
    } //add more
}
