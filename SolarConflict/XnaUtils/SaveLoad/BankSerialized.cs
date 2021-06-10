//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.Serialization;
//using System.Text;

//namespace XnaUtils.SaveLoad {

//    /// <remarks>
//    /// Gotta use BankSerialized in XNAUtils, but the ContentBank isn't available in that project, so we'll just omit all references to the bank
//    /// and replace them with these callbacks
//    /// </remarks>
//    public static class CallbacksForBankSerialized {
//        public static Func<object, string, string> AddToBank;
//        public static Func<string, object> GetFromBank;
//    }

//    /// <summary>Generic class for wrapping nonserialized data and looking it up in the content bank on deserialization.</summary>
//    [Serializable]
//    public class BankSerialized<T> where T : class {
//        public T Value => _value;

//        string _id;
//        [NonSerialized]
//        T _value;

//        /// <param name="value">To save on typing when prototyping, you can pass an object its value here, and said value'll be added to the content bank.</param>        
//        public static BankSerialized<T> Create(T value, string id = null) {
//            //id = ContentBank.Inst.AddGeneric(value, id);
//            id = CallbacksForBankSerialized.AddToBank(value, id);

//            return new BankSerialized<T>(id);
//        }

//        public BankSerialized(string id) {
//            _id = id;
//            //_value = ContentBank.Inst.GetGeneric<T>(_id);
//            _value = CallbacksForBankSerialized.GetFromBank(_id) as T;
//        }

//        [OnDeserialized]
//        void OnDeserializedMethod(StreamingContext context) {
//            //_value = ContentBank.Inst.GetGeneric<T>(_id);
//            _value = CallbacksForBankSerialized.GetFromBank(_id) as T;
//        }
//    }
//}
