//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using XnaUtils;
//using fastJSON;

//namespace SolarConflict.XnaUtils
//{
//    class ActivityStatisticsExtractor
//    {
//        public ActivityStatisticsExtractor()
//        { }

//        public string ExtractStatisticsText(Activity source, Activity destination)
//        {
//            StringBuilder sb = new StringBuilder();
//            if (source == null)
//                sb.Append("null");
//            else
//                sb.Append(source.GetType().ToString());
//            sb.Append(" ");
//            if (destination == null)
//                sb.Append("null");
//            else
//                sb.Append(destination.GetType().ToString());
//            sb.Append(" ");
//            var ts = DateTime.Now;
//            sb.AppendLine(ts.ToString());
//            return sb.ToString();
//            //TODO: add logic that removes idle time from activity times
//        }

//        public string ExtractStatisticsJson(Activity source, Activity destination)
//        {
//            StringBuilder jsonString = new StringBuilder(JSON.ToJSON(source));
//            jsonString.Append("\n");
//            jsonString.Append(JSON.ToJSON(destination));
//            jsonString.Append("\n");
//            var ts = DateTime.Now;
//            jsonString.AppendLine(ts.ToString());
//            jsonString.Append("\n");
//            return jsonString.ToString();
//        }
//    }
//}
