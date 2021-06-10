using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace XnaUtils
{
    public class ParserUtils
    {
        /// <summary>
        /// Parse the value to number.
        /// </summary>
        /// <exception cref="ArgumentException">Can't parse value to number</exception>
        /// <returns></returns>
        public static int ParseInt(string value, int defaultValue = 0)
        {
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            return ParseInt(value);
        }

        /// <summary>
        /// Parse the value to number.
        /// </summary>
        /// <exception cref="ArgumentException">Can't parse value to number</exception>
        /// <returns></returns>
        public static int ParseInt(string value)
        {
            int result = 0;
            int number;

            bool isParsed = int.TryParse(value, out number);

            if (isParsed)
            {
                result = number;
            }
            else
            {
                throw new ArgumentException(string.Format("Can't parse {0} to number.", value));
            }

            return result;
        }

        /// <summary>
        /// Parse the value to float number.
        /// </summary>
        /// <exception cref="ArgumentException">Can't parse value to float number</exception>
        /// <returns></returns>
        public static float ParseFloat(string value, float defaultValue = 0)
        {
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            return ParseFloat(value);
        }

        /// <summary>
        /// Parse the value to float number.
        /// </summary>
        /// <exception cref="ArgumentException">Can't parse value to float number</exception>
        /// <returns></returns>
        public static float ParseFloat(string value)
        {
            float result = 0;
            float number;

            bool isParsed = float.TryParse(value, System.Globalization.NumberStyles.Any, new CultureInfo("en-US"), out number);

            if (isParsed)
            {
                result = number;
            }
            else
            {
                throw new ArgumentException(string.Format("Can't parse {0} to float number.", value));
            }

            return result;
        }

        /// <summary>    
        /// Parse the value to boolean.
        /// </summary>
        /// <exception cref="ArgumentException">Can't parse value to boolean</exception>
        /// <returns></returns>
        public static bool ParseBool(string value, bool defaultValue = false)
        {
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            return ParseBool(value).Value;
        }

        /// <summary>
        /// Parse the value to boolean.
        /// </summary>
        /// <exception cref="ArgumentException">Can't parse value to boolean</exception>
        /// <returns></returns>
        public static bool? ParseBool(string value)
        {
            bool? result = null;
            bool flag;

            bool isParsed = bool.TryParse(value, out flag);

            if (isParsed)
            {
                result = flag;
            }
            else
            {
                throw new ArgumentException(string.Format("Can't parse {0} to boolean.", value));
            }

            return result;
        }


        public static T ParseEnum<T>(string value, T defaultValue = default(T)) where T : struct
        {
            if (string.IsNullOrEmpty(value)) return default(T);
            T result;
            return Enum.TryParse<T>(value, true, out result) ? result : defaultValue;
        }    

        /// <summary>
        /// Parse the value string the enum value with the same name.
        /// </summary>
        /// <exception cref="ArgumentException">Can't parse value to enum</exception>
        /// <returns></returns>
        public static T ParseEnum<T>(string value)
            where T : struct, IComparable, IConvertible, IFormattable
        {
            T enumValue = default(T);

            if (!string.IsNullOrEmpty(value))
            {
                enumValue = (T)Enum.Parse(typeof(T), value);
            }

            return enumValue;
        }

        public static Color ParseColor(string value, Color defaultValue)
        {
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            return ParseColor(value).Value;
        }         

        public static Color? ParseColor(string value)
        {
            Color? xColor = null;

            string[] colorValues = value.Split(':');
            switch (colorValues.Length)
            {
                default:
                    break;
            }

            if ((colorValues.Length == 1 && !string.IsNullOrEmpty(colorValues[0])) || colorValues.Length == 2) // Example: "Red" or "Red:100"
            {
                throw new Exception("ColorParser: not enough parameters");
            }
            else if (colorValues.Length > 2)
            {
                if (colorValues.Length == 3) // Example: "100:100:100"
                {
                    xColor = new Color(ParseInt(colorValues[0]), ParseInt(colorValues[1]), ParseInt(colorValues[2]));

                }
                else  // Example: "100:100:100:100"
                {
                    xColor = new Color(ParseInt(colorValues[0]), ParseInt(colorValues[1]), ParseInt(colorValues[2]), ParseInt(colorValues[3]));
                }
            }

            return xColor;
        }
    }
}
