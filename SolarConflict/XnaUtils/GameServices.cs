//using Microsoft.Xna.Framework;

//namespace XnaUtils
//{
//    /// <summary>Yet another singleton. A lightweight one, for registering and looking up services.</summary>
//    public static class GameServicesContiner
//    {
//        private static GameServiceContainer _container;

//        public static GameServiceContainer Inst
//        {
//            get
//            {
//                _container = _container ?? new GameServiceContainer();

//                return _container;
//            }
//        }

//        public static T GetService<T>()
//        {
//            return (T)Inst.GetService(typeof(T));
//        }

//        public static void AddService<T>(T service)
//        {
//            Inst.AddService(typeof(T), service);
//        }

//        public static void RemoveService<T>()
//        {
//            Inst.RemoveService(typeof(T));
//        }
//    }
//}
