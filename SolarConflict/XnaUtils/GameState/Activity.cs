using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace XnaUtils
{

    /// <summary>
    /// Enum describes the screen transition state.
    /// </summary>
    public enum ActivityState
    {
        TransitionOn,
        Active,
        TransitionOff,
        Hidden,
    }

    [Serializable]
    public class ActivityParameters
    {
        public string Parameter;
        public Dictionary<string, string> ParamDictionary { get; set; }
        public Dictionary<string, object> DataParams { get; private set; }

        public ActivityParameters(Dictionary<string, string> paramDic, string parameter = null)
        {
            Parameter = parameter;
            ParamDictionary = paramDic.Clone();
        }

        public ActivityParameters(string parameter = null)
        {
            Parameter = parameter;
            ParamDictionary = new Dictionary<string, string>();
            DataParams = new Dictionary<string, object>();
        }

        public string GetParam(string id, string defaultValue = null)
        {
            string res;
            ParamDictionary.TryGetValue(id, out res);
            if (res == null)
                res = defaultValue;
            return res;
        }

        public void SetParam (string key, string value)
        {
            ParamDictionary[key] = value;
        }

        public object GetObjectParam(string id, object defaultValue = null)
        {
            object res;
            this.DataParams.TryGetValue(id, out res);
            if (res == null)
                res = defaultValue;
            return res;
        }

        public List<string> GetList(string id, char seperator = ',')
        {
            string param = GetParam(id);
            if (param == null)
                return new List<string>();
            return param.Split(seperator).ToList();
        }

        // User-defined conversion from Digit to double
        public static implicit operator string(ActivityParameters value)
        {
            return value.Parameter; ;
        }
        //  User-defined conversion from double to Digit
        public static implicit operator ActivityParameters(string value)
        {
            return new ActivityParameters(value);
        }
    }

    [Serializable]
    public abstract class Activity
    {
        #region Fields                     
        private ActivityState _activityState; //TODO: make property
        protected bool _isInitialized = false;
        // private float transitionTime;
        //private bool otherScreenHasFocus;
        #endregion

        #region Constructors     
        public Activity()
        {
            IsPopup = false;
            IsBlocking = true;
            _activityState = ActivityState.Active;
        }
        #endregion

        #region Properties
        //public ActivityManager ActivityManager //Remove
        //{
        //    get
        //    {
        //        return ActivityManager.Inst;
        //    }
        //}

        public bool IsActive
        {
            get
            {
                return !(_activityState == ActivityState.TransitionOn ||
                        _activityState == ActivityState.Active);
            }
        }

        //protected Activity _callingActivity;
        //public Activity CallingActivity{ get { return _callingActivity; } set { _callingActivity = value; } }
        public bool IsDrawBackgroundActivity { get; set; }
        public bool IsPopup { 
            get; 
            protected set; 
        }
        public bool IsBlocking { get; set; }
        #endregion

        #region Public Methods
        /// <summary>Called once after construction</summary>
        protected virtual void Init(ActivityParameters parameters) {
            
        }

        /// <summary>Initializes is uninitialized</summary>
        public void TryInit(ActivityParameters parameters) {
            if (!_isInitialized)
                Init(parameters);
            _isInitialized = true;
        }


        /// <summary>
        /// Called when initially entering activity???
        /// </summary>
        public virtual void OnEnter(ActivityParameters parameters)
        {            
        }

        /// <summary>
        /// Called when returning to activity from a called activity (paired with OnBack)
        /// </summary>
        public virtual void OnResume(ActivityParameters parameters = null)  
        {
        }

        /// <summary>
        /// Called when leaving this activity to activate another
        /// </summary>
        public virtual ActivityParameters OnLeave()
        {
            return null;
        }

        /// <summary>
        /// Called from the current activity when going back to the calling activity
        /// </summary>
        public virtual ActivityParameters OnBack()
        {
            return null;
        }        

        #endregion

        #region Update/Draw
        public abstract void Update(InputState inputState);//List<TouchState> touches);
        public abstract void Draw(SpriteBatch sb);
        #endregion

        #region Private Methods
        #endregion
    }
}
