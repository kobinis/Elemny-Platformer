//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Text;


//namespace SolarConflict.Framework.Utils.GraphTraversal {

//    /// <summary>Possibly TEMP for debugging, tells the traversal utility to ignore the code in question.</summary>
//    public class DontTraverseAttribute : Attribute { }

//    public static class Predicates {
//        public static Predicate<T> Conjunction<T>(params Predicate<T>[] predicates) {
//            return (o) => predicates.All(p => p(o));
//        }

//        public static Predicate<object> Disjunction(params Predicate<object>[] predicates) {
//            return (o) => predicates.Any(p => p(o));
//        }

//        public static Predicate<FieldInfo> IsSerializedField => (f) => !f.GetCustomAttributes(false).Any(a => a.GetType() == typeof(NonSerializedAttribute));
//    }

//    /// <summary>Encapsulates the results of an object graph traversal (also provides a static method for traversal).</summary>
//    /// <remarks>Uses TraverseGraph() internally, constructor parameters have the same meaning as in that method, and most of the remarks in its comment block apply.
//    /// Note especially that this class has some trouble telling nulls apart from the string literal "null". For example, PathTo("null") may well return the
//    /// shortest path to a null in the object's graph (and the FinalObject field in that path will be "null").</remarks>
//    /// <see cref="TraverseGraph"/>
//    public class Traversal {
//        public object Root;

//        /// <summary>{object->(minDepth, lastStepInShortestPath)}</summary>
//        Dictionary<object, Tuple<int, TraversalStep>> _shortestPathParents = new Dictionary<object, Tuple<int, TraversalStep>>();

//        /// <summary>Disregard this, it's an implementation detail for multiply-traversed objects. A bit redundant, too.</summary>
//        Dictionary<TraversalStep, object> _stepsTaken = new Dictionary<TraversalStep, object>();
//        Dictionary<object, TraversalStep[]> _stepsTakenInverse = new Dictionary<object, TraversalStep[]>();

//        /// <summary>Does a depth-first traversal of an object's graph.</summary>
//        /// <param name="maxDepth">Maximum depth to traverse (the root is 0).</param>
//        /// <param name="revisitNodes">If true, iterator will yield an object each time it finds it through a distinct traversal step, so each node in the graph will be yielded as many times as it has
//        /// "referenced by" edges. If false, each unique object will be yielded once. TODO: maybe rename this.
//        /// 
//        /// To clarify, if revisitNodes is true, we will yield an object each time we discover it through a new parent, or through a new field, property, or index of a known parent.</param>
//        /// <param name="traverseProperties">If false, traversal will ignore properties.</param>
//        /// <param name="objectPrerequisite">If non-null, traversal will ignore objects that don't satisfy the given predicate.</param>
//        /// <param name="fieldPrerequisite">If non-null, traversal will ignore fields that don't satisfy the given predicate.</param>
//        /// <param name="debugDepthInterval">For debugging, traversal will output debug data whenever it exceeds the max previously-reported depth by the given amount.</param>
//        /// <remarks>Note that the order of traversal is not always straightforward. If there are multiple paths to a given node, it's possible for this iterator to visit that node and some
//        /// of its descendants, leave that branch due to the depth limit (maxDepth), then rediscover it via a shorter path, and move on to those descendants it had previously overlooked.
//        /// This should not affect which elements are traversed or how often, simply the order in which this occurs (each unique object will still be traversed once if revisitNodes == false,
//        /// else once per distinct final traversal step to the object in question).</remarks>
//        /// <warning>Internally converts nulls to the string literal "null". If such a string exists in the graph, will be unable to distinguish it from a null.</warning>
//        static public IEnumerable<object> TraverseGraph(object obj, int maxDepth = 100, bool revisitNodes = true, bool traverseProperties = false, Predicate<object> objectPrerequisite = null,
//            Predicate<FieldInfo> fieldPrerequisite = null, int debugDepthInterval = -1) {
//            var debugData = debugDepthInterval >= 0 ? new InternalFunctions.TraversalDebugData(debugDepthInterval) : null;
//            return InternalFunctions.TraverseGraph1(obj, 0, maxDepth, revisitNodes, traverseProperties, false, objectPrerequisite, fieldPrerequisite,
//                null, new Dictionary<object, Tuple<int, TraversalStep>>(), new Dictionary<TraversalStep, object>(), null, debugData);
//        }

//        /// <remarks>>Uses TraverseGraph() internally, parameters have the same meaning as in that method,
//        /// and most of the remarks in its comment block apply.</remarks>
//        /// <see cref="TraverseGraph"/>            
//        public Traversal(object obj, bool traverseProperties = false, int maxDepth = 100, bool revisitNodes = true,
//            Predicate<object> objectPrerequisite = null, Predicate<FieldInfo> fieldPrerequisite = null, int debugDepthInterval = -1) {
//            var debugData = debugDepthInterval >= 0 ? new InternalFunctions.TraversalDebugData(debugDepthInterval) : null;
//            Root = obj;
//            InternalFunctions.TraverseGraph1(obj, 0, maxDepth, revisitNodes, traverseProperties, false, objectPrerequisite, fieldPrerequisite,
//                null, _shortestPathParents, _stepsTaken, null, debugData).ToArray(); // evaluate the iterator, we make use of the dicts it modifies     

//            // KLUDGE: since we don't initially associate the root with a step (it's our starting point), it's initially absent from _stepsTaken.Values.
//            // However, if we rediscover the root, we'll add it to said values, which is fine if revisitNodes == true. If revisitNodes == false, though, we only
//            // want ObjectsTraversed to iterate over the root once, so we'll remove it from _stepsTaken            
//            if (!revisitNodes)
//                _stepsTaken = _stepsTaken.Where(kv => kv.Value != obj).ToDictionary(kv => kv.Key, kv => kv.Value);           
                    
//            var inverted = new Dictionary<object, List<TraversalStep>>();

//            _stepsTaken.Do(kv => {
//                if (!inverted.ContainsKey(kv.Value))
//                    inverted[kv.Value] = new List<TraversalStep>();
//                inverted[kv.Value].Add(kv.Key);
//            });

//            _stepsTakenInverse = inverted.ToDictionary(kv => kv.Key, kv => kv.Value.ToArray());
//        }

//        /// <summary>Traversed objects, sorted by and annotated with shortest-path length
//        /// [(object, minDepth)]</summary>
//        public Tuple<object, int>[] ObjectsByDepth => _shortestPathParents.OrderBy(kv => kv.Value.Item1).Select(kv => Tuple.Create(kv.Key, kv.Value.Item1)).ToArray();

//        public IEnumerable<object> ObjectsTraversed => _shortestPathParents.Keys;

//        /// <summary>Given an object in the graph, returns all paths to it which were memoized by the traversal.</summary>
//        /// <remarks>Results depend on the nature if the traversal. If allowed to revisit nodes (see constructor parameter: revisitNodes),
//        /// then for each object, this method will return one path per unique final traversal step to the object (the shortest path through that ends in
//        /// the given step). If the traversal was non-revisiting, it will return the shortest path to the object (the array will have a single value equal to PathTo(obj)).</remarks>
//        /// <param name="obj"></param>
//        /// <returns></returns>
//        public ChainTraversal[] PathsTo(object obj) {
//            var result = new List<ChainTraversal>();

//            if (obj == Root)
//                result.Add(new ChainTraversal(new TraversalStep[0], obj));

//            foreach (var finalStep in _stepsTakenInverse[obj]) {
//                List<TraversalStep> steps;
//                if (finalStep.Parent == null)
//                    steps = new List<TraversalStep>();
//                else
//                    steps = PathTo(finalStep.Parent).Steps.ToList();
//                steps.Add(finalStep);

//                result.Add(new ChainTraversal(steps, obj));
//            }

//            return result.ToArray();
//        }

//        /// <summary>Given an object in the graph, returns the shortest path to it (from the root, not guaranteed unique).</summary>        
//        public ChainTraversal PathTo(object obj) {
//            obj = obj ?? "null"; // possibly TEMP, TraverseGraph1 presently does the same

//            Utility.Assert(_shortestPathParents.ContainsKey(obj), "Object not found in graph");

//            var steps = new List<TraversalStep>();
//            var step = _shortestPathParents[obj].Item2;

//            while (step != null) {
//                steps.Add(step);
//                step = step.Parent == null ? null : _shortestPathParents[step.Parent].Item2;
//            }

//            if (steps.Count == 0)
//                Utility.Assert(obj == Root, "Object not found in graph");

//            steps.Reverse();

//            return new ChainTraversal(steps, obj);
//        }        
//    }

//    /// <summary>Describes one step in an object graph traversal, from parent to child.</summary>
//    public abstract class TraversalStep : ICloneable {
//        public object Parent;

//        public TraversalStep(object parent) {
//            Parent = parent;
//        }

//        public abstract object Clone();

//        public override int GetHashCode() {
//            return (Parent?.GetHashCode() ?? 1) * 3;
//        }

//        public abstract override bool Equals(object obj);

//        public override string ToString() {
//            return (Parent ?? "null").ToString() + ToStringSuffix();
//        }

//        public virtual string ToStringSuffix() {
//            return "";
//        }        
//    }

//    public class MemberTraversal : TraversalStep {
//        public string MemberName;
//        public bool MemberIsProperty;

//        public MemberTraversal(object parent, string memberName, bool memberIsProperty) : base(parent) {
//            MemberName = memberName;
//            MemberIsProperty = memberIsProperty;
//        }

//        public override object Clone() {
//            return new MemberTraversal(Parent, MemberName, MemberIsProperty);
//        }

//        public override int GetHashCode() {
//            return base.GetHashCode() + MemberName.GetHashCode();
//        }

//        public override bool Equals(object obj) {
//            var cast = obj as MemberTraversal;

//            return (cast?.MemberIsProperty == MemberIsProperty && cast.MemberName == MemberName && cast.Parent == Parent);
//        }

//        public override string ToStringSuffix() {
//            return (MemberIsProperty ? "->" : ".") + MemberName;
//        }
//    }

//    public class IndexTraversal : TraversalStep {
//        public int Index;

//        public IndexTraversal(object parent, int index) : base(parent) {
//            Index = index;
//        }

//        public override object Clone() {
//            return new IndexTraversal(Parent, Index);
//        }

//        public override int GetHashCode() {
//            return base.GetHashCode() + Index;
//        }

//        public override bool Equals(object obj) {
//            var cast = obj as IndexTraversal;

//            return (cast?.Index == Index && cast.Parent == Parent);
//        }

//        public override string ToStringSuffix() {
//            return $"[{Index}]";
//        }
//    }

//    /// <summary>Describes a descending traversal in some part of an object's graph.</summary>
//    /// <remarks>Traversal isn't guaranteed to end in a leaf, hence "FinalNode" instead of "Leaf" and "ChainTraversal" instead of "BranchTraversal".</remarks>
//    public class ChainTraversal : ICloneable {
//        public object FinalObject;
//        public TraversalStep[] Steps;

//        public object[] Objects => Steps.Select(s => s.Parent).Concat(new object[] { FinalObject }).ToArray();

//        public ChainTraversal(IEnumerable<TraversalStep> steps, object finalObject) {            
//            Steps = steps.ToArray();
//            FinalObject = finalObject;
//        }

//        public void AddStep(TraversalStep step, object newFinal) {
//            Steps = Steps.Concat(new TraversalStep[] { step }).ToArray();
//            FinalObject = newFinal;
//        }

//        public object Clone() {
//            var result = new ChainTraversal(Steps.Select(s => s.Clone() as TraversalStep), FinalObject);
//            result.FinalObject = FinalObject;

//            return result;
//        }

//        public int Depth => Steps.Count();

        
//        public override string ToString() {            
//            var builder = new StringBuilder();
//            Steps.Do(s => builder.Append(s.ToStringSuffix()));
//            return builder.ToString();
//        }

//        public string ToVerboseString() {
//            var builder = new StringBuilder();
//            Steps.Do(s => {
//                if (s.Parent != null)
//                    builder.Append($"<{s.Parent}>");
//                builder.Append(s.ToStringSuffix());
//            });
//            if (FinalObject != null)
//                builder.Append($"<{FinalObject}>");
//            return builder.ToString();
//        }
//    }

//    static class InternalFunctions {
//        public class TraversalDebugData {            
//            public int LastReportedDepth = 0;
//            /// <summary>Traversal will report status every time its depth increases by ReportDepthInterval</summary>
//            public int ReportDepthInterval;

//            public TraversalDebugData(int reportDepthInterval) {
//                ReportDepthInterval = reportDepthInterval;
//            }
//        };

//        /// <param name="iterateOverStrings">If true, upon finding a string, will recur for its child characters, fields, etc, else will treat strings as atomic.</param>        
//        /// <param name="currentChain">Purely for debug, so we can return meaningful info about our position in the graph mid-traversal. Obsoleted by the Traversal class if we can
//        /// actually complete a traversal.</param>
//        /// <remarks>BUG: If you have a property with an anonymous setter, like public List<CraftingCost> CraftingCostList { get; private set; },
//        /// then ToString() wonks out a bit with the backing field        
//        /// </remarks>
//        public static IEnumerable<object> TraverseGraph1(object obj, int currentDepth, int maxDepth, bool revisitNodes, bool traverseProperties,
//            bool iterateOverStrings, Predicate<object> objectPrerequisite, Predicate<FieldInfo> fieldPrerequisite, TraversalStep step,
//            Dictionary<object, Tuple<int, TraversalStep>> shortestPathParents, Dictionary<TraversalStep, object> stepsTaken, ChainTraversal currentChain, TraversalDebugData debugData) {            

//            // Debug stuff
//            if (debugData != null) {
//                // Debug stuff
//                if (currentChain == null)
//                    currentChain = new ChainTraversal(new TraversalStep[0], obj);
//                else
//                    currentChain.AddStep(step, obj);

//                if (currentDepth >= debugData.LastReportedDepth + debugData.ReportDepthInterval) {
//                    Utility.Log($"Discovered new depths, now at {currentDepth}, at:\n{currentChain.ToString()}\n{currentChain.ToVerboseString()}");
//                    debugData.LastReportedDepth = currentDepth;
//                }
//            }
            

//            obj = obj ?? "null";

//            if (objectPrerequisite != null && !(objectPrerequisite(obj)))
//                // We wanna ignore this type of object
//                yield break;

//            if (shortestPathParents.ContainsKey(obj)) {
//                // Already been here (or someplace similar, if this is a value type)
//                if (revisitNodes) {
//                    // But are we visiting this node in a new way? From a different parent, or a different field/index of the same parent?
//                    if (!stepsTaken.ContainsKey(step)) {
//                        // Yup                
//                        if (step != null)
//                            stepsTaken[step] = obj;
//                        yield return obj;
//                    }
//                }

//                // Have we just found a shorter path to this node, though?
//                if (shortestPathParents[obj].Item1 <= currentDepth)
//                    // Nope. Nothing to do here, then. If we did, we'd wanna continue, revisiting this object's descendants, updating their depths/shortest paths and potentially
//                    // going further than we did before (if we previously hit the depth limit)
//                    yield break;
//            } else
//                // Never been here before
//                yield return obj;

//            shortestPathParents[obj] = Tuple.Create(currentDepth, step);
//            if (step != null)
//                stepsTaken[step] = obj; // only actually relevant if revisitNodes == true

//            if (obj == null || currentDepth >= maxDepth || ((obj is string) && !iterateOverStrings))
//                // Dead end or max depth reached, or a string which we don't wanna inspect further
//                yield break;

//            ++currentDepth;

//            var enumerable = obj as IEnumerable;
//            if (enumerable != null) {
//                // Object is enumerable
//                int i = 0;
//                foreach (var element in enumerable.Cast<object>()) {
//                    foreach (var subResult in TraverseGraph1(element, currentDepth, maxDepth, revisitNodes, traverseProperties, iterateOverStrings, objectPrerequisite, fieldPrerequisite,
//                        new IndexTraversal(obj, i), shortestPathParents, stepsTaken, currentChain?.Clone() as ChainTraversal, debugData))
//                        yield return subResult;
//                    ++i;
//                }
//            }

//            // Recur for fields (kludgily ignore non serialized, if appropriate)
//            var type = obj.GetType();
//            var bindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic;
//            // TODO: maybe make this ^ a parameter       

//            /*foreach (var subResult in type.GetFields(bindingFlags)
//                .Where(f => ((fieldPrerequisite == null || fieldPrerequisite(f)))
//                && (!f.GetCustomAttributes(false).Any(a => a.GetType() == typeof(DontTraverseAttribute))))
//                .SelectMany(f => TraverseGraph1(f.GetValue(obj), currentDepth, maxDepth, revisitNodes, traverseProperties, iterateOverStrings, objectPrerequisite, fieldPrerequisite,
//                    new MemberTraversal(obj, f.Name, false), shortestPathParents, stepsTaken, currentChain?.Clone() as ChainTraversal, debugData)))
//                yield return subResult;*/
//            // ^ Not very amenable to debugging, hence this:
//            var fields = type.GetFields(bindingFlags)
//                .Where(f => ((fieldPrerequisite == null || fieldPrerequisite(f)))
//                && (!f.GetCustomAttributes(false).Any(a => a.GetType() == typeof(DontTraverseAttribute))));
//            var subResults = new List<object>();
//            foreach (var f in fields) {
//                var traversalResults = TraverseGraph1(f.GetValue(obj), currentDepth, maxDepth, revisitNodes, traverseProperties, iterateOverStrings, objectPrerequisite, fieldPrerequisite,
//                    new MemberTraversal(obj, f.Name, false), shortestPathParents, stepsTaken, currentChain?.Clone() as ChainTraversal, debugData);
//                foreach (var r in traversalResults)
//                    subResults.Add(r);
//            }
//            foreach (var s in subResults)
//                yield return s;                

//            // Recur for properties, if appropriate
//            if (traverseProperties)
//                foreach (var subResult in type.GetProperties(bindingFlags).Where(p => !p.GetCustomAttributes(false).Any(a => a.GetType() == typeof(DontTraverseAttribute)))
//                .SelectMany(p => TraverseGraph1(p.GetValue(obj, null), currentDepth, maxDepth,
//                revisitNodes, traverseProperties, iterateOverStrings, objectPrerequisite, fieldPrerequisite, new MemberTraversal(obj, p.Name, true), shortestPathParents, stepsTaken,
//                currentChain?.Clone() as ChainTraversal, debugData)))
//                    yield return subResult;
//        }
//    }
//}
