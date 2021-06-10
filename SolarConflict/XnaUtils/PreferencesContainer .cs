using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace XnaUtils
{
    public class PreferencesContainer
    {        
        private Dictionary<string, string> _values;

        public PreferencesContainer()
        {
            _values = new Dictionary<string, string>();
        }

        public void SetBool(string key, bool value)
        {
            _values[key] = value ? "t" : "f";
        }

        public bool GetBool(string key, bool defaultValue)
        {
            try
            {
                if (_values.ContainsKey(key))
                    return _values[key] == "t" || _values[key].ToLower() == "true";
                else
                {
                    _values[key] = defaultValue ? "t" : "f";
                    return defaultValue;
                }

            }
            catch (Exception)
            {
                return defaultValue;
            }
        }



        public void SetFloat(string key, float value)
        {
            _values[key] = value.ToString(new CultureInfo("en-US"));
        }

        public float GetFloat(string key, float defaultValue)
        {
            try
            {
                if (_values.ContainsKey(key))
                    return float.Parse(_values[key], new CultureInfo("en-US"));
                else
                {
                    _values[key] = defaultValue.ToString(new CultureInfo("en-US"));
                    return defaultValue;
                }

            }
            catch (Exception)
            {

                return defaultValue;
            }

        }

        public void SetEnum<T>(string key, T value)
            where T : struct, IComparable, IConvertible, IFormattable {
            _values[key] = value.ToString();
        }

        public void SetString(string key, string value)
        {
            _values[key] = value.ToString();
        }

        public string GetString(string key)
        {
            return _values[key];
        }

        public string TryGetString(string key)
        {
            string s;
            _values.TryGetValue(key, out s);
            return s;
        }

        public T GetEnum<T>(string key, T defaultValue)
            where T : struct, IComparable, IConvertible, IFormattable
        {
            try
            {
                if (!_values.ContainsKey(key))
                    return defaultValue;
                return ParserUtils.ParseEnum<T>(_values[key]);

            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public void Save(string path)
        {
            string[] lines = new string[_values.Count];
            int counter = 0;
            foreach (var pair in _values)
            {
                lines[counter] = pair.Key + "," + pair.Value;
                counter++;
            }
            File.WriteAllLines(path, lines);
        }

        public static PreferencesContainer Load(string path)
        {
            PreferencesContainer preferences = new PreferencesContainer();
            string[] lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                try
                {
                    string[] keyValue = line.Split(new char[] { ',' }, 2);
                    preferences._values.Add(keyValue[0], keyValue[1]);
                }
                catch (Exception)
                {
                    //Log                    
                }                
            }            
            return preferences;
        }


    }
}
