using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict
{
    [Serializable]
    public struct StarDate
    {
        private const int HourInFrames = 60 * 60;
        
        public int Frames { get; set; }

        public int DateInHours
        {
            get
            {
                return Frames / HourInFrames;
            }
        }

        public int DateInDays
        {
            get
            {
                return Frames / (HourInFrames * 10);
            }
        }

        public int Hour
        {
            get
            {
                return DateInHours % 10;
            }
        }

        public int Day
        {
            get
            {
                return DateInDays % 100;
            }
        }

        public TimeSpan GetTimeSpan()
        {
            TimeSpan ts = new TimeSpan(0, 0, 0, seconds: Frames / 60, milliseconds: (Frames - Frames / 60) * 1000);
            return ts;
        }

        public int Year {
            get
            {
                return DateInDays / 100;
            }
        }

      

        public override string ToString()
        {
            return Year.ToString() + "." + Day.ToString() + "." + Hour.ToString();
        }
    }
}
