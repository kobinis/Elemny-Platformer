using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using XnaUtils;
using XnaUtils.SimpleGui;

namespace SolarConflict.Framework.Menu
{
    [Serializable]
    public class MenuEntry
    {
        public MenuEntry() : this(null)
        { }

        public MenuEntry(string displayText)
        {
            DisplayText = displayText;
            ActivityName = string.Empty;
            ActivityParams = null;
            TooltipText = null;
            activity = null;
            Data = null;
            Action = null;
            ParamDictionary = null;
            RememberPreviousActivity = true;
            ActivationKey = Keys.None;
        }

        public bool RememberPreviousActivity;
        public string DisplayText;
        public string ActivityName;
        public string ActivityParams;
        public string TooltipText;
        public Keys ActivationKey;
        public List<string> ParamList;

        [XmlIgnore]
        public Dictionary<string, string> ParamDictionary
        {
            get {
                return StringUtils.ListToStringDictionery(ParamList);
            }
            set
            {
                ParamList = StringUtils.DictioneryToStringList(value);
            }
        }
              
        public Activity activity;
        [XmlIgnore]
        public Object Data;
        [XmlIgnore]
        public ActionEventHandler Action;

        public ActivityParameters GetActivityParameters()
        {
            if (ActivityParams == null && ParamList == null)
                return null;
            ActivityParameters activityParameters = new ActivityParameters(ActivityParams);
            activityParameters.ParamDictionary = ParamDictionary;
            return activityParameters;
        }
    }
}

