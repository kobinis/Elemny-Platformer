using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.Framework.Logger
{
    public class LoggerException : Exception
    {
        public LoggerException(string message) 
            : base(message)
        {
        }

        public LoggerException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
