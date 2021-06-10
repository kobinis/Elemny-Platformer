using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace XnaUtils
{
    public class CsvUtils
    {
        private string[] _splitHeader;
        private Dictionary<string, string> _valuesDictionary;
        private string[] _values;

        public CsvUtils(string[] splitHeader)
        {
            _splitHeader = splitHeader;
            _valuesDictionary = new Dictionary<string, string>();           
        }

        public String[] SplitLine(string line)
        {
            line = line + ",";            
            bool quotationIsOn = false;
            List<string> values = new List<string>();
            int prevIndex = -1;
            for (int i = 0; i < line.Length; i++)
            {
                switch (line[i])
                {
                    case '"':
                        quotationIsOn = !quotationIsOn;
                        break;
                    case ',':
                        if(!quotationIsOn)
                        {
                            var value = line.Substring(prevIndex + 1, i - prevIndex - 1);
                            if ((value.Length > 1) && (value[0] == '"') && ((value[value.Length - 1] == '"')))
                                value = value.Substring(1, value.Length - 2);
                            values.Add(value);
                            prevIndex = i;
                        }
                        break;

                    default:
                        break;
                }
            }            
                var value1 = line.Substring(prevIndex + 1, line.Length - prevIndex - 1);
                if ((value1.Length > 1) && (value1[0] == '"') && ((value1[value1.Length - 1] == '"')))
                    value1 = value1.Substring(1, value1.Length - 2);
                values.Add(value1);
            
            //values.Add(line.Substring(prevIndex + 1, line.Length - prevIndex -1));
            return values.ToArray();
        }

        public string[] ReadLine(string line)
        {
            string[] splitLine = SplitLine(line);
            ReadLine(splitLine);
            return splitLine;
        }

        private void ReadLine(string[] values)
        {
            _values = values;
            _valuesDictionary.Clear();

            for (int i = 0; i < _splitHeader.Length; i++)
            {
                string value = string.Empty;
                if (i < _values.Length)
                    value = _values[i];
                if(!string.IsNullOrWhiteSpace(_splitHeader[i]))
                    _valuesDictionary.Add(_splitHeader[i], value);
            }
        }

        public bool HasValue(string key)
        {
            return !string.IsNullOrEmpty(_valuesDictionary[key]);
        }

        #region Get GetEnum

        public T GetEnum<T>(int index)
            where T : struct, IComparable, IConvertible, IFormattable
        {
            return ParserUtils.ParseEnum<T>(_values[index]);
        }

        public T GetEnum<T>(string key)
            where T : struct, IComparable, IConvertible, IFormattable
        {
            return ParserUtils.ParseEnum<T>(_valuesDictionary[key]);
        }

        public T GetEnum<T>(int index, T defaultValue)
        where T : struct, IComparable, IConvertible, IFormattable
        {
            return ParserUtils.ParseEnum<T>(_values[index], defaultValue);
        }

        public T GetEnum<T>(string key, T defaultValue)
            where T : struct, IComparable, IConvertible, IFormattable
        {
            return ParserUtils.ParseEnum<T>(_valuesDictionary[key], defaultValue);
        }

        #endregion


        #region Get Bool

        public bool? GetBool(int index)
        {
            return ParserUtils.ParseBool(_values[index]);
        }

        public bool? GetBool(string key)
        {
            return ParserUtils.ParseBool(_valuesDictionary[key]);
        }

        public bool GetBool(int index, bool defaultValue)
        {
            return ParserUtils.ParseBool(_values[index], defaultValue);
        }

        public bool GetBool(string key, bool defaultValue)
        {
            return ParserUtils.ParseBool(_valuesDictionary[key], defaultValue);
        }

        #endregion


        #region Get Float 

        public float GetFloat(int index)
        {
            return ParserUtils.ParseFloat(_values[index]);
        }

        public float GetFloat(string key)
        {
            return ParserUtils.ParseFloat(_valuesDictionary[key]);
        }

        public float GetFloat(int index, float defaultValue)
        {
            return ParserUtils.ParseFloat(_values[index], defaultValue);
        }

        public float GetFloat(string key, float defaultValue)
        {
            return ParserUtils.ParseFloat(_valuesDictionary[key], defaultValue);
        }

        #endregion


        #region Get Int

        public int GetInt(int index)
        {
            return ParserUtils.ParseInt(_values[index]);
        }

        public int GetInt(string key)
        {
            return ParserUtils.ParseInt(_valuesDictionary[key]);
        }

        public int GetInt(int index, int defaultValue)
        {
            return ParserUtils.ParseInt(_values[index], defaultValue);
        }

        public int GetInt(string key, int defaultValue)
        {
            return ParserUtils.ParseInt(_valuesDictionary[key], defaultValue);
        }


        #endregion


        #region Get Color

        public Color? GetColor(int index)
        {
            return ParserUtils.ParseColor(_values[index]);
        }

        public Color? GetColor(string key)
        {
            return ParserUtils.ParseColor(_valuesDictionary[key]);
        }

        public Color GetColor(int index, Color defaultValue)
        {
            return ParserUtils.ParseColor(_values[index], defaultValue);
        }

        public Color GetColor(string key, Color defaultValue)
        {
            return ParserUtils.ParseColor(_valuesDictionary[key], defaultValue);
        }

        #endregion

        #region Get String

        public String GetString(int index)
        {
            return StringHelper.ParseString(_values[index]);
        }

        public String GetString(string key)
        {
            if (_valuesDictionary.ContainsKey(key))
                return StringHelper.ParseString(_valuesDictionary[key]);
            else
                return null;
        }

        public String GetRawString(string key)
        {
            return _valuesDictionary[key];
        }

        public String GetString(int index, String defaultValue)
        {
            string result = _values[index];

            if (string.IsNullOrEmpty(result))
            {
                result = defaultValue;
            }

            return result;
        }

        public String GetString(string key, String defaultValue)
        {
            string result = _valuesDictionary[key];

            if (string.IsNullOrEmpty(result))
            {
                result = defaultValue;
            }

            return result;
        }

        #endregion
    }
}
