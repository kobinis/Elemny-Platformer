using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SolarConflict
{
   
    [Flags]
    public enum ControlSignals:uint //Maybe split?
    {
        None = 0,
        Action1 = 1 << 0, //ControlSignals
        Action2 = 1 << 1,
        Action3 = 1 << 2,
        Action4 = 1 << 3,
        Up = 1 << 4, 
        Down = 1 << 5, 
        Left = 1 << 6,  
        Right = 1 << 7,     
        AlwaysOn = 1 << 8,

        QuickUse1 = 1 << 9,
        QuickUse2 = 1 << 10,
        QuickUse3 = 1 << 11,
        QuickUse4 = 1 << 12,
        MoveToCursor = 1 << 13,
        
        OnCreate = 1<< 14 , //ControlIndicators
        OnDestroyed = 1 << 15,
        OnTakingDamage = 1 << 16,  //Not working?
        OnDamageToShield = 1<< 17,
        OnDamageToHull = 1 << 18,
        OnStun = 1<< 19,
        OnLowHitpoints = 1<< 20,
        OnHitpointsNotFull = 1 << 21,
        OnLowEnergy = 1<< 22,
        OnCloak = 1<< 23,
        OnDecloak = 1<< 24,
        OnColision = 1<< 25,
        OnCombat = 1<< 26,
        OnInventoryHasRoom = 1 << 27,

        Brake = 1 << 30, // Braking is presently a kludge for White Nights Prague, only usable by player ships
        //MoveForward = (uint)(1 << 31),
        
        //up to 31
    } 
    
    public enum AnalogDefaltNames { Left, Right, None }

    public static class InputUtils //change it
    {
        public static ControlSignals InputMask = (ControlSignals)((uint)ControlSignals.QuickUse4 * 2 - 1);
        public static ControlSignals InputNotMask = ~InputMask; //maybe remove?
        public const int InputNumber = 8; 

        //////cange from Keys to KeyMouse (holds Key or mouse and name of action)
        //public static Keys[] keys1 = new Keys[13] { Keys.Up, Keys.Down, Keys.Q, Keys.E,Keys.W, Keys.S, Keys.A, Keys.D, Keys.None, Keys.D1, Keys.D2, Keys.D3, Keys.D4};
        //////keys only
        //public static Keys[] keys2 = new Keys[10] { Keys.W, Keys.S, Keys.A, Keys.D, Keys.Up, Keys.Down, Keys.Q, Keys.E, Keys.Left, Keys.Right };
        ////public static Keys[] keys2 = new Keys[10] { Keys.Up, Keys.Down, Keys.A, Keys.D, Keys.Q, Keys.W, Keys.E , Keys.S,Keys.Left, Keys.Right };
              
    }    

    /* class ControlSignals
     {
         public Vector2[] analogDirections; //2
         public float[] analogSignals; //2
         public UInt32 digitalSignals;
         public UInt32 digitalIndicators;
     }*/
}
