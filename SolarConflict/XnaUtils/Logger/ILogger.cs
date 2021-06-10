using System;

namespace SolarConflict.Framework.Logger
{
   public interface ILogger : IDisposable
   {
      void Write(string Message, Exception Exception);
      void Write(string Message);
   }
}
