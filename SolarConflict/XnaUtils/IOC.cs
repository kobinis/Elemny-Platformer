//using SolarConflict.Framework.Logger;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace XnaUtils
//{
//    /// <summary>
//    /// This class is the container of all Singleton Services in the System. 
//    /// </summary>
//    public class IOC
//    {
//        // REFACTOR: check "http://www.hanselman.com/blog/ListOfNETDependencyInjectionContainersIOC.aspx" site for checking IOC third party libraries and consider replace IOC code with one of those libraries.
//        private static readonly short DEFAULT_NUM_OF_SERVICES = 10;

//        #region static members

//        // Classes Singleton
//        protected static IOC instance;// = new IOC();

//        /// <summary>
//        /// This will return the IOC singleton
//        /// </summary>
//        public static IOC Get
//        {
//            get
//            {
//                if (instance == null)
//                    instance = new IOC();
//                return instance;
//            }
//        }

//        /// <summary>
//        /// Use this class only in unit test when you want to mock the systems services
//        /// </summary>
//        /// <param name="ioc"></param>
//        public static void MockIOC(IOC ioc)
//        {
//            instance = ioc;
//        }

//        #endregion

//        protected Dictionary<Type, object> services = new Dictionary<Type, object>(DEFAULT_NUM_OF_SERVICES);

//        private IOC()
//        {
//            SetService<ILogService>(new LogService());
//            Service<ILogService>().Init();
//        }

//        //private virtual void Init()
//        //{
//        //   SetService<ILogService>(new LogService());      
//        //   IOC.Get.Service<ILogService>().Init();
//        //}

//        /// <summary>
//        /// Add singleton service to the IOC
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="service"></param>
//        public virtual void SetService<T>(T service) where T : class
//        {
//            services[typeof(T)] = service;
//        }

//        /// <summary>
//        /// Get yours service from the IOC
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <returns></returns>
//        public virtual T Service<T>() where T : class
//        {
//            return services[typeof(T)] as T;
//        }

//        public virtual bool Contains(Type service)
//        {
//            return services.ContainsKey(service);
//        }

//        /// <summary>
//        /// Clear all the services that was set in the IOC.
//        /// Initiate dispose of service if implemented.
//        /// </summary>
//        public virtual void ClearServices()
//        {
//            foreach (IDisposable service in services.Values.OfType<IDisposable>())
//            {
//                // Dispose all services that implement IDisposable.
//                service.Dispose();
//            }

//            // Clear all services in the IOC.
//            services.Clear();
//        }
//    }
//}
