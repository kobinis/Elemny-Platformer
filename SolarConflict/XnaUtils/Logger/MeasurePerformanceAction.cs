//using System;
//using System.Diagnostics;
//using XnaUtils;

//namespace SolarConflict.Framework.Logger
//{
//    /// <summary>
//    /// Use this class to measure performance using a stop watch.
//    /// The class will write a message to the log when it is created and another message when it is disposed.
//    /// The later message will also contain the time that has passed between the creation and disposal of the instance.
//    /// </summary>
//    public class MeasurePerformanceAction : IDisposable
//   {
//      private string _description;
//      private Stopwatch _stopwatch;

//      /// <summary>
//      /// Creates a new instance of MeasurePerformanceAction
//      /// </summary>
//      /// <param name="description">A description of the action we are measuring the performance of</param>
//      public MeasurePerformanceAction(string description)
//      {
//         _description = description;
//         IOC.Get.Service<ILogService>().Write("{0} - Started", _description);
//         _stopwatch = Stopwatch.StartNew();
//      }

//      ~MeasurePerformanceAction()
//      {
//         dispose(false);
//      }

      
//      public void Dispose()
//      {
//         dispose(true);
//      }

//      private void dispose(bool isDisposing)
//      {
//         if (isDisposing)
//         {
//            GC.SuppressFinalize(this);
//         }

//         _stopwatch.Stop();

//         if (IOC.Get.Contains(typeof(ILogService))) // in case we get to this code through finilizer the IOC might already been disposed.
//         {
//            IOC.Get.Service<ILogService>().Write("{0} - Finished. Time taken: {1}", _description, _stopwatch.Elapsed);
//         }
//      }
//   }
//}
