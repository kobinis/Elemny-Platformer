using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using SolarConflict.Framework;
using XnaUtils;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using XnaUtils.Graphics;
using System.Linq;
using SolarConflict.GameContent;
using SolarConflict.Framework.Agents.Systems;
using SolarConflict.Framework.Agents;
using SolarConflict.AI.GameAI;
using System.Reflection;

namespace SolarConflict
{
    /// <summary>
    /// Used to Serialization of static classes 
    /// </summary>
    public class PreferencesClassSerialization
    {
        private string _filename;
        public List<Tuple<string, Type>> _dataIdAndType; //No need 
        private Type _settingsClass;


        public PreferencesClassSerialization(string filename, Type settingsClass)
        {
            _filename = filename;
            _dataIdAndType = new List<Tuple<string, Type>>();
            _settingsClass = settingsClass;
            var fInfo = ReflectionUtils.GetField(_settingsClass);
            foreach (var info in fInfo)
            {
                _dataIdAndType.Add(new Tuple<string, Type>(info.Name, info.FieldType));
            }
        }

        public void SetDataFromContinerToClass(PreferencesContainer container)
        {
            foreach (var item in _dataIdAndType)
            {
                if (item.Item2 == typeof(int))
                {
                    int value = (int)container.GetFloat(item.Item1, 0);
                    var fi = _settingsClass.GetField(item.Item1, BindingFlags.Public | BindingFlags.Static);                    
                    fi.SetValue(_settingsClass, value);
                }
                if (item.Item2 == typeof(float))
                {
                    float value = container.GetFloat(item.Item1, 0);
                    var fi = _settingsClass.GetField(item.Item1, BindingFlags.Public | BindingFlags.Static);
                    fi.SetValue(_settingsClass, value);
                }
                if (item.Item2 == typeof(bool))
                {
                    bool value = container.GetBool(item.Item1, false);
                    var fi = _settingsClass.GetField(item.Item1, BindingFlags.Public | BindingFlags.Static);
                    fi.SetValue(_settingsClass, value);
                }
                if (item.Item2 == typeof(string))
                {
                    string value = container.GetString(item.Item1);
                    var fi = _settingsClass.GetField(item.Item1, BindingFlags.Public | BindingFlags.Static);
                    fi.SetValue(_settingsClass, value);
                }


            }
        }

        public void SetDataFromClassToContiner(PreferencesContainer container)
        {
            foreach (var item in _dataIdAndType)
            {
                if (item.Item2 == typeof(int))
                {
                    var fi = _settingsClass.GetField(item.Item1, BindingFlags.Public | BindingFlags.Static);
                    int value = (int)fi.GetValue(null);
                    container.SetFloat(item.Item1, value);
                }
                if (item.Item2 == typeof(float))
                {
                    var fi = _settingsClass.GetField(item.Item1, BindingFlags.Public | BindingFlags.Static);
                    float value = (float)fi.GetValue(null);
                    container.SetFloat(item.Item1, value);
                }
                if (item.Item2 == typeof(bool))
                {
                    var fi = _settingsClass.GetField(item.Item1, BindingFlags.Public | BindingFlags.Static);
                    var value = (bool)fi.GetValue(null);
                    container.SetBool(item.Item1, value);
                }
                if (item.Item2 == typeof(string))
                {
                    var fi = _settingsClass.GetField(item.Item1, BindingFlags.Public | BindingFlags.Static);
                    string value = (string)fi.GetValue(null);
                    container.SetString(item.Item1, value);
                }
            }
        }

        public void Load(string path)
        {
            var fullPath = Path.Combine(path, _filename);
            try
            {
                var container = PreferencesContainer.Load(fullPath);
                SetDataFromContinerToClass(container);
                //TODO: read values and set in settinfs
            }
            catch (Exception)
            {
                //Container = new PreferencesContainer();
                //Container.Save(fullPath);
            }
        }

        public void Save(string path)
        {
            var fullPath = Path.Combine(path, _filename);

            var container = new PreferencesContainer();
            SetDataFromClassToContiner(container);
            container.Save(fullPath);

            try
            {
                //read from settings class and set to continer
              
            }
            catch (Exception)
            {
                //TODO: wrtie to log
            }
        }
    }
}