//using System;
//using System.Diagnostics;

//namespace SolarConflict.Framework.Logger
//{
//    /// <summary>
//    /// Provide services to write log messages. (thread safe)
//    /// </summary>
//    public class LogService : ILogService
//    {
//        #region Fields

//        private ILogger _logger;

//        #endregion


//        #region Public Methods

//        /// <summary>
//        /// Initialize logger writer, this method must be called before first use of LogService. 
//        /// </summary>
//        /// <remarks>
//        /// We Initialize logger in special method and not in the constructor, 
//        /// because the logger initialization require to get keys from configuration and if the keys don't exist,
//        /// configuration service write message to the log which is not created yet.
//        /// To avoid this circular dependency we first create empty LogService without Logger instance.
//        /// That way Configuration will get LofService instance when if he need to call it.
//        /// </remarks>
//        /// <exception cref="LoggerException">Can't write message to the log file.</exception>
//        public virtual void Init()
//        {
//            _logger = new TextWriterLogger();
//        }

//        /// <summary>
//        /// Used only in unit testing for mocking Logger.
//        /// </summary>
//        /// <param name="Logger">mocked Logger</param>
//        public virtual void Init(ILogger Logger)
//        {
//            _logger = Logger;
//        }

//        /// <summary>
//        /// Write message to chose log files.
//        /// </summary>
//        /// <param name="Message">Message to write.</param>
//        /// <param name="Severity">Severity of the message, common values: error, information, warning</param>
//        /// <param name="Target">Write message to trace, to console or to both.</param>
//        /// <exception cref="LoggerException">Can't write message to the log file.</exception>
//        public virtual void Write(string Message, params object[] args)
//        {
//            if (_logger == null)
//                return;

//            if (args != null && args.Length > 0)
//            {
//                Message = string.Format(Message, args);
//            }

//            // Write to log
//            string methodName = new StackTrace().GetFrame(1).GetMethod().Name;
//            _logger.Write(methodName + " - " + Message);
//        }

//        /// <summary>
//        /// Write exception to both trace and console logs.
//        /// </summary>
//        /// <param name="Exception">Exception to write to the logs.</param>
//        /// <exception cref="LoggerException">Can't write message to the log file.</exception>
//        public virtual void Write(Exception Exception)
//        {
//            if (_logger == null)
//                return;

//            _logger.Write(string.Empty, Exception);
//        }

//        /// <summary>
//        ///  Write exception to both trace and console logs.
//        /// </summary>
//        /// <param name="Message">Message to write to the log before the exception.</param>
//        /// <param name="Exception">Exception to write to the logs.</param>
//        /// <exception cref="LoggerException">Can't write message to the log file.</exception>
//        public virtual void Write(string Message, Exception Exception, params object[] args)
//        {
//            if (_logger == null)
//                return;

//            if (args != null && args.Length > 0)
//            {
//                Message = string.Format(Message, args);
//            }

//            _logger.Write(Message, Exception);
//        }

//        /// <summary>
//        /// Close Logger Writer and the resources (files) it use.
//        /// </summary>
//        public virtual void Dispose()
//        {
//            if (_logger == null)
//                return;

//            _logger.Dispose();
//            _logger = null;
//        }

//        #endregion
//    }
//}
