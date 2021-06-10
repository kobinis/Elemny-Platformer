//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Input;
//using SolarConflict.GameContent.ContentGeneration;
//using SolarConflict.GameContent.Projectiles.Shots;
//using SolarConflict.NewContent.Projectiles;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Runtime.Serialization;
//using System.Text;
//using XnaUtils.Graphics;

//namespace SolarConflict.Framework.Utils {
//    public static class DebugStuff {

//        public static Dictionary<string, object> State = new Dictionary<string, object>();

//        /// <summary>Projectiles which didn't provide custom tweaking methods, and were therefore modified using highly fallible generic methods.</summary>
//        static public HashSet<string> ProjectilesTweakedGenerically = new HashSet<string>();
//        static public HashSet<string> RelevantItems;
//        static public HashSet<string> StuffToDefinitelyIncludeInLoadoutEditor;
//        static public HashSet<string> TweakableWeapons;

//        static public HashSet<string> LoadoutsCreatedThisSession = new HashSet<string>();

//        static public HashSet<string> TweakedShips = new string[] {
//            "Fighter1", "Fighter2",
//            "HeavyEscort1", "HeavyEscort2",
//            "Interceptor1", "Interceptor2",
//            "Lancer1",
//            "LPB1", "LPB2",
//            "StandoffBomber1", "StandoffBomber2", "StandoffBomber3",
//            }.ToSet();

//        [Serializable]
//        public class KeyboardHelper {
//            public Keys[] KeysJustPressed = new Keys[0];
//            public Keys[] KeysHeld = new Keys[0];
//            public Keys[] KeysJustReleased = new Keys[0];

//            public void Update() {
//                var held = Keyboard.GetState().GetPressedKeys();
//                KeysJustPressed = held.Except(KeysHeld).ToArray();
//                KeysJustReleased = KeysHeld.Except(held).ToArray();
//                KeysHeld = held;
//            }

//            [OnDeserialized]
//            void OnDeserializedMethod(StreamingContext context) {
//                KeysHeld = new Keys[0];
//                KeysJustPressed = new Keys[0];
//                KeysJustReleased = new Keys[0];
//            }
//        }

//        /// <param name="testEverything">If false, we'll return a singleton list as soon as an element fails to serialize, else we'll test everything.</param>
//        /// <remarks>TODO: add testInvasively param which, if true, nulls graph elements' references, so we can actually test them in isolation
//        /// 
//        /// Two shortcomings:
//        /// 1. Cyclical references can cheapen the meaning of "bottom up" (if a leaf element has a ref to the root, failing to serialize that
//        /// element isn't particularly informative)
//        /// 2. There may be objects over which we don't know how to iterate
//        /// 
//        /// BUG: occasionally just fails in an inexplicable, nonlocal way, which can't be easily explained by the aforementioned shortcomings.
//        /// To demonstrate, check out branch e90431010e1fa640101c0526c77e3d7008379aed. Remove the NonSerialized tag from Activity._callingActivity and try to serialize the StartingNode.
//        /// Note how you consistently get the same exception, apparently when serializing a projectile that has nothing to do with said field.</remarks>
//        /// <returns>[(failedObject, pathToObject, exceptionRaised)]</returns>
//        static public List<Tuple<object, ChainTraversal, Exception>> BottomUpSerializationTest(object obj,
//            bool rethrow, int maxDepth, out Exception exception, bool testEverything = false) {

//            var traversal = new Traversal(obj, false, maxDepth, false, fieldPrerequisite: Predicates.IsSerializedField);
//            var bottomUpElements = traversal.ObjectsByDepth.Reverse().ToArray();


//            var deepestElement = bottomUpElements[0];
//            Utility.Log($"Deepest element (depth {deepestElement.Item2}) is at\n{traversal.PathTo(deepestElement.Item1).ToVerboseString()}");

//            exception = null;

//            var result = new List<Tuple<object, ChainTraversal, Exception>>();
            
//            foreach (var e in bottomUpElements.Where(e => e.Item1 != null)) {
//                if (BottomUpSerializationTest1(e.Item1, out exception)) {                    
//                    // Error serializing element
//                    result.Add(Tuple.Create(e.Item1, traversal.PathTo(e.Item1), exception));

//                    if (!testEverything)
//                        // Fail immediately, all we need is the deepest element                                                
//                        break;                    
//                }
//            }

//            if (result.Count > 0) {
//                // Describe deepest error found
//                Utility.Log($"SERIALIZATION FAILED: {result[0].Item1} at depth {result[0].Item2.Depth}, path: \n{result[0].Item2}\nVerbosely: {result[0].Item2.ToVerboseString()}");

//                if (rethrow)
//                    throw result[0].Item3;
//            }
                    

//            return result;
//        }

//        static public List<Tuple<object, ChainTraversal, Exception>> BottomUpSerializationTest(object obj, bool rethrow = false, int maxDepth = 100,
//            bool testEverything = false) {
//            var dummy = new Exception();
//            return BottomUpSerializationTest(obj, rethrow, maxDepth, out dummy, testEverything);
//        }


//        static bool BottomUpSerializationTest1(object obj, out Exception exception) {
//            // SEMI-KLUDGE: dictionary entries can't (and needn't) be serialized; recognize them by type name, ignore them
//            // (note that the keys and values will still get inspected, just not the entry that combines them)
//            if (obj.GetType().FullName.StartsWith(@"System.Collections.Generic.Dictionary`2+Entry")) {
//                exception = null;
//                return false;
//            }

//            var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

//            try {
//                formatter.Serialize(Stream.Null, obj);
//            }
//            catch (Exception e) {
//                exception = e;

//                return true;
//            }

//            exception = null;
//            return false;
//        }

//        static bool BottomUpSerializationTest1(object obj) {
//            var dummy = new Exception();
//            return BottomUpSerializationTest1(obj, out dummy);
//        }
//    }

//    /// <summary>For saving like two lines of typing when you wanna cycle through a list of values.</summary>
//    public class Cycler<T> {
//        int _index;

//        public List<T> Values;

//        public Cycler() {
//            Values = new List<T>();
//        }

//        public T Current() {
//            _index = _index.Clamp(0, Values.Count - 1);

//            return Values[_index];
//        }

//        public T Next() {
//            ++_index;

//            return Current();
//        }

//        public T Previous() {
//            --_index;

//            return Current();
//        }
//    }
//}
