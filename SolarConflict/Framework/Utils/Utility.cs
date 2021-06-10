using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System;
using Microsoft.Xna.Framework;
using System.Text;
using System.IO;
using SolarConflict.Framework.Utils;
using XnaUtils;
using SolarConflict;
using Microsoft.Xna.Framework.Input;
using XnaUtils.Graphics;
using SolarConflict.XnaUtils.Input;

namespace SolarConflict.Framework.Utils {
    public class Utility {

        public const int FramesPerSecond = 60;

        public static int Frames(float seconds) {
            return (int)Math.Round(seconds * FramesPerSecond);
        }

        public static void Log(params System.Object[] messageArgs) {
            System.Diagnostics.Debug.WriteLine(ToString(messageArgs));
        }

        /// <summary>Convenience overload of Enumerable.Range(), returns ints in the interval [start, stop)</summary>        
        public static IEnumerable<int> Range(int start, int stop) {
            Debug.Assert(stop >= start, "Start value exceeds stop");

            return Enumerable.Range(start, stop - start);
        }        

        public static Vector2 Vector(float angle) {
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }

        public static string ToString(params System.Object[] args) {
            return string.Join(", ", args.Select((arg, _) => (arg == null) ? null : arg.ToString()).ToArray());
        }
        
    }

    #region Roll
    // ROLL
    [Serializable]
    public struct Roll {
        public float min;
        public float max;

        public Roll(float min = 0f, float? max = null) {
            this.min = min;
            this.max = max ?? min;
        }        

        public float Value(System.Random random) {
            Debug.Assert(max >= min, "min > max");

            return random.FloatBetween(min, max);
        }

        public static implicit operator Roll(float value) {
            return new Roll(value);
        }
    }  

    [Serializable]
    public struct RollInt {
        public int min;
        public int max;

        public RollInt(int min = 0, int? max = null) {
            this.min = min;
            this.max = max ?? min;
        }

        public int Value(System.Random random) {
            Debug.Assert(max >= min, "min > max");

            return random.IntBetween(min, max);
        }

        public static implicit operator RollInt(int value) {
            return new RollInt(value);
        }
    }
    #endregion
}

// EXTENSIONS
#region Extensions
static class Extensions {
    #region Array    
    public static void Shuffle<T>(this T[] array, Random random = null) {
        random = random ?? new Random();
        // Fisher-Yates shuffle
        int n = array.Count();
        while (n > 1) {
            n--;
            int k = random.Next(n + 1);
            var temp = array[k];
            array[k] = array[n];
            array[n] = temp;
        }
    }
    #endregion

    #region Camera
    public static bool IsOnScreen(this Camera camera, GameObject obj, float sizeMult = 1.5f) {
        return camera.IsOnScreen(obj.Position, obj.Size, sizeMult);
    }
    #endregion

    #region Color
    /// <summary>Componentwise multiplication</summary>
    public static Color Multiply(this Color color, Color otherColor)
    {
        return new Color(color.ToVector4() * otherColor.ToVector4());
    }

    public static Color Multiply(this Color color, float scalar, bool includeAlpha = false)
    {
        if (includeAlpha)
            return new Color(color.ToVector4() * scalar);

        var a = color.A;
        var result = new Color(color.ToVector4() * scalar);
        result.A = a;
        return result;
    }

    /// <summary>Convert a Color to a tag for our text formatting system, apply it to some text</summary>    
    public static string ToTag(this Color color, string message) {
        return $"#color{{{(int)(color.R)},{(int)(color.G)},{(int)(color.B)}}}"+message+"#dcolor{}";
    }
    /// <summary>Convert a Color to a tag for our text formatting system, apply it to some text</summary>    
    public static string ToTag(this Color color) {
        return $"#color{{{(int)(color.R)},{(int)(color.G)},{(int)(color.B)}}}";
    }
    #endregion

    #region IComparable
    public static T Clamp<T>(this T value, T min, T max) where T : IComparable<T> {
        Debug.Assert(min.CompareTo(max) <= 0);

        if (value.CompareTo(min) < 0)
            return min;

        if (value.CompareTo(max) > 0)
            return max;

        return value;
    }
    #endregion

    #region Dictionary
    /// <summary>Kinda like Dictionary.Update(), but with valuewise addition</summary>
    /// <remarks>Might wanna rename, Add() usually connotes adding something to a collection, not adding two collections together.</remarks>
    public static void Add<K, V>(this Dictionary<K, V> dict, Dictionary<K, V> other, Func<V, V, V> summator) where V : new() {
        other.Do(p => {
            if (!dict.ContainsKey(p.Key))
                dict[p.Key] = new V();
            dict[p.Key] = summator(dict[p.Key], p.Value);
        });
    }
    public static void Add<K>(this Dictionary<K, float> dict, Dictionary<K, float> other) {
        Add(dict, other, (l, r) => l + r);
        // Shame there's no type constraint for addition being defined, can't make this truly generic
    }
    public static void Add<K>(this Dictionary<K, int> dict, Dictionary<K, int> other) {
        Add(dict, other, (l, r) => l + r);
    }

    public static Dictionary<K, V> Added<K, V>(this Dictionary<K, V> dict, Dictionary<K, V> other, Func<V, V, V> summator) where V : new() {
        var result = dict.Clone();
        result.Add(other, summator);
        return result;
    }
    public static Dictionary<K, float> Added<K>(this Dictionary<K, float> dict, Dictionary<K, float> other) {
        var result = dict.Clone();
        result.Add(other);
        return result;
    }
    public static Dictionary<K, int> Added<K>(this Dictionary<K, int> dict, Dictionary<K, int> other) {
        var result = dict.Clone();
        result.Add(other);
        return result;
    }

    public static Dictionary<K, V> Clone<K, V>(this Dictionary<K, V> dict) {
        return dict.ToDictionary(p => p.Key, p => p.Value);
    }    

    public static V Get<K, V>(this Dictionary<K, V> dict, K key, V defaultValue) {
        if (dict.ContainsKey(key))
            return dict[key];

        return defaultValue;
    }

    public static V Get<K, V>(this Dictionary<K, V> dict, K key) {
        return dict.Get(key, default(V));
    }

    /// Like Update(), but not in-place. Or like LINQ Union(), but less generic and returns a dict instead of an enumerable
    public static Dictionary<K, V> Unified<K, V>(this Dictionary<K, V> dict, Dictionary<K, V> other) {
        return dict.Union(other).ToDictionary(keyAndValue => keyAndValue.Key, keyAndValue => keyAndValue.Value);
    }

    public static void Update<K, V>(this Dictionary<K, V> dict, Dictionary<K, V> other) {
        foreach (var keyAndValue in other)
            dict[keyAndValue.Key] = keyAndValue.Value;
    }
    #endregion

    #region IEnumerable

    public static T Choice<T>(this IEnumerable<T> enumerable, float roll) {
        var count = enumerable.Count();
        var index = (int)(roll * count);
        if (index == count) // this is possible, just not terribly likely
            --index;
        return enumerable.ElementAt(index);
    }
    public static T Choice<T>(this IEnumerable<T> enumerable, Random random) {
        return enumerable.ElementAt(random.Next() % enumerable.Count());
    }
    
    /// <summary>Picks several elements from the given enumerable, without repetition</summary>    
    public static T[] Choices<T>(this IEnumerable<T> enumerable, int numChoices, Random random) {   

        var count = enumerable.Count();

        if (numChoices == count)
            return enumerable.Shuffled(random).ToArray();

        Debug.Assert(numChoices < count);

        // Just reroll until we have n distinct numbers
        var indices = new HashSet<int>();
        while (indices.Count < numChoices)
            indices.Add(random.Next() % count);

        return indices.Select(i => enumerable.ElementAt(i)).ToArray();
    }

    public static void Do<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        foreach (var t in enumerable)
            action(t);
    }

    public static IEnumerable<Tuple<int, T>> Enumerate<T>(this IEnumerable<T> enumerable) {
        int i = 0;
        foreach (var e in enumerable) {
            yield return new Tuple<int, T>(i, e);
            ++i;
        }
    }

    /// <summary>Returns the index of the given element, or -1 if not found</summary>    
    public static int IndexOf<T>(this IEnumerable<T> enumerable, T element) where T : class {
        return enumerable.IndexOfFirst((e) => e == element);
    }
    // There's gotta be a simpler way to require that T be amenable to operator== without having one case for classes and others for primitives
    public static int IndexOf(this IEnumerable<int> enumerable, int element) {
        return enumerable.IndexOfFirst((e) => e == element);
    }
    public static int IndexOf(this IEnumerable<float> enumerable, float element) {
        return enumerable.IndexOfFirst((e) => e == element);
    }

    /// <summary>Returns the index of the first element satisfying the given predicate, or -1 if not found</summary>    
    public static int IndexOfFirst<T>(this IEnumerable<T> enumerable, Predicate<T> predicate) {
        foreach (var t in enumerable.Enumerate())
            if (predicate(t.Item2))
                return t.Item1;
        return -1;
    }

    /// <summary>Given an enumerable and a way of evaluating elements, returns the highest-value element (or first maximal element, if tied)</summary>
    /// <remarks>TODO: optimize, no need to run the evaluator more than once per element</remarks>
    public static T Maximal<T>(this IEnumerable<T> enumerable, Func<T, float> evaluator) {
        return enumerable.Aggregate((l, r) => evaluator(l) >= evaluator(r) ? l : r);
    }

    /// <summary>Given an enumerable and a way of evaluating elements, returns the highest-value element (or first maximal element, if tied)</summary>
    public static T Maximal<T>(this IEnumerable<T> enumerable, Func<T, int> evaluator) {
        return enumerable.Aggregate((l, r) => evaluator(l) >= evaluator(r) ? l : r);
    }

    /// <summary>Given an enumerable and a way of evaluating elements, returns the lowest-value element (or first minimal element, if tied)</summary>
    public static T Minimal<T>(this IEnumerable<T> enumerable, Func<T, float> evaluator) {
        return enumerable.Aggregate((l, r) => evaluator(l) <= evaluator(r) ? l : r);
    }

    /// <summary>Given an enumerable and a way of evaluating elements, returns the lowest-value element (or first minimal element, if tied)</summary>
    public static T Minimal<T>(this IEnumerable<T> enumerable, Func<T, int> evaluator) {
        return enumerable.Aggregate((l, r) => evaluator(l) <= evaluator(r) ? l : r);
    }

    public static IEnumerable<T> Shuffled<T>(this IEnumerable<T> enumerable, Random random = null) {
        random = random ?? new Random();
        return enumerable.OrderBy(e => random.Next());
    }

    public static HashSet<T> ToSet<T>(this IEnumerable<T> enumerable) {
        return new HashSet<T>(enumerable);
    }

    /// <summary>Given an enumerable, a way of weighing elements, and a source of randomness, makes a weighted choice from among those elements.</summary>    
    /// <param name="roll">Random number in [0, 1]</param>
    public static T WeightedChoice<T>(this IEnumerable<T> enumerable, Func<T, float> evaluator, float roll) {
        Debug.Assert(enumerable.Count() > 0);
        Debug.Assert(roll >= 0f && roll <= 1f);
        var weightedElements = enumerable.Select(e => Tuple.Create(evaluator(e), e)).ToList();
        // TODO: optimize

        var sum = weightedElements.Select(t => t.Item1).Sum();

        if (sum == 0f)
            // Unweighted choice
            return Choice(enumerable, roll);

        roll *= sum;
        foreach (var e in weightedElements) {
            roll -= e.Item1;
            if (roll <= 0f)
                return e.Item2;
        }
        return weightedElements.Last().Item2;
    }
    public static T WeightedChoice<T>(this IEnumerable<T> enumerable, Func<T, float> evaluator, Random random) {
        return enumerable.WeightedChoice(evaluator, random.NextFloat());
    }

    #endregion

    #region GKey

    public static Sprite GetIcon(this GKeys key) {
        var id = KeysSettings.KeyIconIDs.Get(key);
        if (id != null)
            return Sprite.Get(id);
        return null;
    }

    /// <summary>If icon available, returns parser tag for it, else text description</summary>
    public static string GetTag(this GKeys key) {
        var sprite = GetIcon(key);
        if (sprite == null)
            return key.ToString();
        return sprite.ToTag();
    }

    #endregion

    #region HashSet
    public static void Discard<T>(this HashSet<T> set, T element) {
        if (set.Contains(element))
            set.Remove(element);
    }
    #endregion

    #region InputState
    public static bool IsCommandDown(this InputState inputState, PlayerCommand command) {
        //return PlayerMouseAndKeys.commandsBinding[command].Any(k => inputState.IsKeyDown(k));
        // Oh, wait, we only support a single KeyBinding for each command. Why?
        return inputState.IsGKeyDown(KeysSettings.Data.CommandBindings.Get(command, Keys.None));
    }

    /// <summary>Returns true when the command is pressed (one or more matching keys is down and all were up last frame)</summary>
    public static bool IsCommandPressed(this InputState inputState, PlayerCommand command) {
        return inputState.IsGKeyPressed(KeysSettings.Data.CommandBindings.Get(command, Keys.None));
    }

    public static bool IsControlSignalDown(this InputState inputState, ControlSignals signal) {        
        return inputState.IsGKeyDown(KeysSettings.Data.KeyBindings.Get(signal, Keys.None));        
    }

    /// <summary>Returns true when the signal is pressed (one or more matching keys/buttons is down and all were up last frame)</summary>
    public static bool IsControlSignalPressed(this InputState inputState, ControlSignals signal) {
        return inputState.IsGKeyPressed(KeysSettings.Data.KeyBindings.Get(signal, Keys.None));
    }
    #endregion

    #region List
    /// <warning>Copypasted from the array extension method, for performance</warning>
    public static void Shuffle<T>(this List<T> list, Random random) {        
        // Fisher-Yates shuffle
        int n = list.Count();
        while (n > 1) {
            n--;
            int k = random.Next(n + 1);
            var temp = list[k];
            list[k] = list[n];
            list[n] = temp;
        }
    }
    #endregion

    #region Random

    public static float FloatBetween(this System.Random random, float min, float max) {
        Debug.Assert(min <= max);
        return (float)(min + random.NextDouble() * (max - min));
    }

    /// <remarks>Inclusive</remarks>
    public static int IntBetween(this System.Random random, float min, float max) {
        Debug.Assert(min <= max);
        return (int)Math.Round(random.FloatBetween(min, max));
    }
   
    public static float NextAngle(this Random random)
    {
        return (float)random.NextDouble()*MathHelper.TwoPi;
    }

    public static float NextFloat(this Random random, float min = 0f, float max = 1f) {
        return (float)random.NextDouble(min, max);
    }

    public static double NextDouble(this Random random, double min, double max) {
        return min + (random.NextDouble() * (max - min));
    }

    

    public static Vector2 PointInRing(this Random random, float innerRadius, float outerRadius) {
        Debug.Assert(innerRadius <= outerRadius, "Inner radius greater than outer");

        var angle = random.NextFloat() * MathHelper.TwoPi;
        var ratio = random.NextFloat();
        var innerRadiusSquared = innerRadius * innerRadius;

        var distance = (float)Math.Sqrt(innerRadiusSquared + ratio * (outerRadius * outerRadius - innerRadiusSquared));

        return new Vector2((float)Math.Cos(angle) * distance, (float)Math.Sin(angle) * distance);
    }

    #endregion

    #region string
    public static string Truncate(this string value, int maxLength, bool fromLeft = false) {
        if (string.IsNullOrEmpty(value))
            return value;
        if (value.Length <= maxLength)
            return value;

        if (fromLeft) 
            return value.Substring(value.Length - maxLength);
        
        return value.Substring(0, maxLength);
    }

    public static string Resize(this string value, int length, char paddingCharacter = ' ') {
        return value.Truncate(length).PadRight(length, paddingCharacter);
    }
    #endregion

    #region Type

    public static bool IsSameOrSubclass(this Type type, Type other) {
        return type == other || type.IsSubclassOf(other);
    }

    #endregion

    #region Vectors
    /// <summary>Hadamard product, aka this.Transpose().Multiply(other).</summary>    
    public static Vector2 Hadamard(this Vector2 value, Vector2 other) {
        return new Vector2(value.X * other.X, value.Y * other.Y);
    }

    public static float Dot(this Vector3 value, Vector3 other) {
        return value.X * other.X + value.Y * other.Y + value.Z * other.Z;
    }    

    public static Vector2 Normalized(this Vector2 value) {
        var result = Vector2.Zero + value;
        result.Normalize();
        return result;
    }
    public static Vector3 Normalized(this Vector3 value) {
        var result = Vector3.Zero + value;
        result.Normalize();
        return result;
    }
    
    /// <param name="angle">In radians</param>
    public static Vector2 Rotated(this Vector2 value, float angle) {
        var sin = (float)Math.Sin(angle);
        var cos = (float)Math.Cos(angle);
        return new Vector2(value.X * cos - value.Y * sin, value.X * sin + value.Y * cos);        
    }

    public static string ToString(this Vector2 value, string format) {
        return $"({value.X.ToString(format)}, {value.Y.ToString(format)})";
    }

    public static string ToString(this Vector3 value, string format) {
        return $"({value.X.ToString(format)}, {value.Y.ToString(format)}, {value.Z.ToString(format)})";
    }

    /// <summary>
    /// Get round percent of part/total.
    /// </summary>
    public static int GetPercent(int part, int total)
    {
        int notZeroTotal;

        if ((double)total != 0)
        {
            notZeroTotal = total;
        }
        else
        {
            notZeroTotal = 1;
        }

        return (int)Math.Round((((double)part / notZeroTotal) * 100));
    }
    #endregion
}
#endregion

