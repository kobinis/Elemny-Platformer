//using SolarConflict.Framework;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;

//namespace SolarConflict.XnaUtils
//{
//    class TextFileStatisticsLogger : IActivityStatisticsLogger
//    {
//        private string _dir;
//        private string _fullPath;

//        public TextFileStatisticsLogger(string statisticsPath)
//        {
//            _dir = string.Format("{0}/", statisticsPath);
//            Directory.CreateDirectory(_dir);
//            var now = DateTime.Now;
//            string fileSuffix = string.Format("{0}{1}{2}{3}{4}{5}", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
//            string fileName = string.Format("statistics{0}.txt", fileSuffix);
//            _fullPath = Path.Combine(_dir, fileName);
//        }

//        public void LogStatistic(string statisticText)
//        {
//            File.AppendAllText(_fullPath, statisticText);
//        }
//    }
//}
