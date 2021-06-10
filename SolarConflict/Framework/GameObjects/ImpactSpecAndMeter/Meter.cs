using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SolarConflict
{
    [Serializable]
    public class Meter //ToDo: change name
    {
        public float Value; //maybe cahnge to private?
        public float MaxValue; 
        //float generation; //Remove ?
        //float minValue; //ToDo: ???replace with zero

        public float NormalizedValue
        {
            get
            {
                return Value / MaxValue;
            }
        }

        public Meter()
        {
            Value = 0;
            MaxValue = float.MaxValue;
        }

        public Meter(float maxValue)
        {
            this.MaxValue = maxValue;            
            Value = maxValue;
        }

        public Meter(float value, float maxValue)
        {
            this.MaxValue = maxValue;
            Value = value;
        }

        
        /*public void Update() //think if you need it, Dont get called!
        {           
            Value = Math.Min(Math.Max(Value + generation, 0), MaxValue);
        }*/

        public void AddValue(float addValue)
        {                       
            Value = MathHelper.Clamp(Value + addValue, 0, MaxValue);
        }

        public void SetValue(float value)
        {
            Value = MathHelper.Clamp(value, 0, MaxValue);
        }

        public Meter GetWorkingCopy()
        {
            Meter clone = new Meter(this.Value, this.MaxValue);
            return clone;
        }


       

    }
}
