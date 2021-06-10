using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace XnaUtils
{
    public class ReflectionUtils
    {
        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        /// <summary>
        /// Gets all types in or under a namespace
        /// </summary>        
        public static Type[] GetTypesUnderNamespace(Assembly assembly, string nameSpace, bool includeNested = false)
        {
            return assembly.GetTypes().Where(t => t.Namespace != null && t.Namespace.StartsWith(nameSpace) && t.IsNested == includeNested).ToArray();
        }

        public static List<FieldInfo> GetField(Type type)
        {
            return type.GetFields(BindingFlags.Public | BindingFlags.Static).ToList();
        }

    }
}
