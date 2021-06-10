//using System;
//using System.Diagnostics;
//using System.IO;
//using System.Threading;

//namespace SolarConflict.Framework.Logger
//{
//    /// <summary>
//    ///  TextWriterLogger implement writing log messages logs files through TextWriter.
//    /// </summary>
//    public class TextWriterLogger : ILogger
//    {
//        #region Consts 

//        private readonly string FILE_NAME = "StarSingularity{0:yyyy-MM-dd_hh-mm-ss-tt}.log";
//        private readonly string MESSAGE_FORMAT = "{0:G} [{1}] - {2}";

//        private readonly bool LOG_ACTIVATE_DEFAULT_VALUE = true;
//        private readonly string LOG_PATH_DEFAULT_VALUE = "Log";

//        #endregion


//        #region Fields

//        private TextWriter _logWriter;
//        private bool? _logActivated;
//        private string _logPath;

//        #endregion


//        #region Proprieties

//        protected string LogPath
//        {
//            get
//            {
//                if (string.IsNullOrEmpty(_logPath))
//                {
//                    // Changed by Yochai Gani 12/05/2016
//                    _logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "StarSingularity\\" + LOG_PATH_DEFAULT_VALUE);                    
//                    Directory.CreateDirectory(_logPath);
//                }

//                return _logPath;
//            }
//        }

//        /// <summary>
//        /// If in ScanMode we create new log file for each new process.
//        /// </summary>
//        protected bool LogActivated
//        {
//            get
//            {
//                _logActivated = LOG_ACTIVATE_DEFAULT_VALUE;
//                return _logActivated.Value;
//            }
//        }

//        #endregion


//        #region Constructors

//        /// <summary>
//        /// Create TextWriterLogger for console and log.
//        /// </summary>
//        /// <remarks>
//        /// Check if console and log are activate, for each activate log it the
//        /// directory from configuration if needed.
//        /// For log it create new file.
//        /// For console it delete old file and create new one.
//        /// Notice - If we use default values because the values in the configuration 
//        /// file are invalid or not exist we don't writing an error message about it 
//        /// to the log/console file.
//        /// </remarks>
//        public TextWriterLogger()
//        {
//            // Create log file
//            if (LogActivated)
//            {
//                createlogFile();
//            }
//        }

//        #endregion


//        #region Public Methods

//        /// <summary>
//        /// Write log message to log file.
//        /// </summary>
//        /// <param name="Message">Message to write.</param>
//        /// <param name="Severity">Severity of the message, common values: error, information, warning</param>
//        /// <exception cref="LoggerException">Can't write message to the log file.</exception>
//        public virtual void Write(string Message)
//        {
//            if (!LogActivated || string.IsNullOrEmpty(Message) ||
//                string.IsNullOrEmpty(Message.Trim()))
//            {
//                return;
//            }

//            try
//            {
//                _logWriter.WriteLine(MESSAGE_FORMAT, DateTime.Now,
//               Thread.CurrentThread.ManagedThreadId, Message);

//                // Update the underlying file.
//                _logWriter.Flush();
//            }
//            catch (Exception)
//            {
//                //writeMessageNotToTextFile(CONSOLE_ACTIVATE_KEY, CONSOLE_ACTIVATE_DEFAULT_VALUE.ToString());
//                _logActivated = false;
//            }
//        }


//        /// <summary>
//        /// Write exception to both log and console logs. 
//        /// (Different message format for each log file).
//        /// </summary>
//        /// <param name="Message">Message to write to the log before the exception.</param>
//        /// <param name="Exception">Exception to write to the logs.</param>
//        /// <exception cref="LoggerException">Can't write message to the log file.</exception>
//        public virtual void Write(string Message, Exception Exception)
//        {
//            if (Exception == null)
//                return;

//            // Check if we need to add splitter between the message to the exception message.
//            string splitter = string.IsNullOrEmpty(Message) ? string.Empty : " ";

//            // Write exception to console. 
//            string message = Message + splitter + "Error message - " + Exception.Message.Replace("\n", "");

//            Write(message);

//            // Write exception to log.
//            string methodName = new StackTrace().GetFrame(2).GetMethod().Name;

//            message = string.Format("{0} - {1}{2}Exception data: {3}", methodName,
//               Message, splitter, Exception.ToString());
//            Write(message);
//        }

//        public void Dispose()
//        {
//            Dispose(true);

//            // No need to finalize, we called Dispose(true)
//            GC.SuppressFinalize(this);
//        }

//        ~TextWriterLogger()
//        {
//            Dispose(false);
//        }

//        protected virtual void Dispose(bool disposing)
//        {
//            // If we were called from Dispose() (managed resources haven't been disposed yet)
//            if (disposing)
//            {
//                // Free managed resources
//                if (_logWriter != null)
//                {
//                    _logActivated = false;
//                    _logWriter.Close();
//                    _logWriter.Dispose();
//                }
//            }

//            // Free native resources... (none here)
//        }

//        #endregion


//        #region Private Methods

//        /// <summary>
//        /// Use this method when log/console file not exist yet.
//        /// This Method will use to write messages to the event viewer or to the log file at later state.
//        /// </summary>
//        /// <param name="keyName"></param>
//        /// <param name="DefaultValue"></param>
//        private void writeMessageNotToTextFile(string keyName, string DefaultValue)
//        {
//            //TODO: develop this method after approve of YaronP.
//            // use errors DNAPR0058E - DNAPR0060E
//        }

//        private void createlogFile()
//        {
//            try
//            {
//                string fileName = string.Format(FILE_NAME, DateTime.Now);
//                _logWriter = TextWriter.Synchronized(File.AppendText(LogPath + "\\" + fileName));

//            }
//            catch (Exception ex)
//            {
//                throwException("Can't create log file.", ex);
//            }
//        }

//        /// <summary>
//        /// assumption - if we can't create one log file we can't create the other as well.
//        /// There fore we will set to false both the logs files.
//        /// </summary>
//        /// <param name="Message"></param>
//        /// <param name="Ex"></param>
//        private void throwException(string Message, Exception Ex)
//        {
//            _logActivated = false;
//            throw new LoggerException(Message, Ex);  
//        }

//        #endregion
//    }
//}
