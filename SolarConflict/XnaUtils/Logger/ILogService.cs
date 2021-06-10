using System;

namespace SolarConflict.Framework.Logger
{
   public interface ILogService : IDisposable
   {
      void Write(string Message, params object[] args);
      void Write(Exception Exception);
      void Write(string Message, Exception Exception, params object[] args);
      void Init();
   }
}
