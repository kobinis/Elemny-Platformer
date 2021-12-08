using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SolarConflict.XnaUtils;
using XnaUtils.Framework.Graphics;
using XnaUtils.SimpleGui.Controllers;
using System.IO;
using Microsoft.Xna.Framework.Input;
using SolarConflict.Framework;
using System.Reflection;
using System.Linq;
using Microsoft.Xna.Framework.Audio;
using System;
using XnaUtils.SimpleGui;
using SolarConflict;

namespace XnaUtils
{
    public delegate Activity ActivityProvider(string param);
    //TODO: add background activity, on the same or diffrent thred 
    //TODO: remove transition, apply effect in activity
    //TODO: add on activity enter event, onn activity exit event

    public class ActivityManager
    {
        #region Singelton
        private static ActivityManager _inst;
        public static ActivityManager Inst
        {
            get { return _inst; }
        }

        public static void Init(Game game)
        {
            _inst = new ActivityManager(game);
        }
        #endregion

        #region Static
        
        //public static SpriteBatch SpriteBatch { get; private set; }
        public static GraphicsDevice GraphicsDevice { get { return GraphicsSettingsUtils.GraphicsDevice; } }                
        public static Texture2D CursorTexture { get; set; }
                
        public static int ScreenWidth { get; private set; } //TODO: update value evrey update according to viewport
        public static int ScreenHeight { get; private set; }
        public static Vector2 ScreenCenter { get; private set; }
        public static Vector2 ScreenSize { get; private set; }
        public static Rectangle ScreenRectangle { get; private set; }

        private static List<string> _errors;
        #endregion

        public Game Game { get; private set; }
        private Texture2D blank;
        private Stack<Activity> _activityStack;
        public bool TakeScreenshot;        
        private const int transitionTime = 20;
        private Color _transitionColor = Color.Black;
        private bool _isCursorVisible;
        public bool IsCursorVisible
        {
            get { return _isCursorVisible; }
            set { Game.IsMouseVisible = !value; _isCursorVisible = value; }
        }
        public string ScreenshotPath { get; private set; }
        public Activity DefaultActivity { set; get; } //TODO: change to provider      

        public InputState InputState { get { return _inputState; } set { _inputState = value; } }
        private InputState _inputState;
        private ActivityState _activityState;
        private float _transitionTimer;
        private RichTextControl _toast;
        private int _toastTimer;
        private int _frameTime;

        private Dictionary<string, ActivityProvider> _activityProviders;
        private Activity _currentActivity;
        private Activity _targetActivity;
        private Activity _currentPopupActivity;
        private bool _quit = false;
        private bool _transitioningBack;
        
        public GameTime GameTime { get; private set; }
        public SoundEngine SoundEngine { get; private set; }
        public float MasterVolume;

        public Activity BackgroundActivity;

               
        private ActivityManager(Game game)
        {
            this.Game = game;
            IsCursorVisible = true;
            _activityProviders = new Dictionary<string, ActivityProvider>();
            _inputState = new InputState();
            _transitioningBack = false;
            _currentPopupActivity = null;
            SoundEngine = new SoundEngine();
            _activityStack = new Stack<Activity>();
            GenerateBlankTexure();
        }

        private void GenerateBlankTexure()
        {
            Canvas canvas = new Canvas(4, 4, GraphicsSettingsUtils.GraphicsDevice);
            canvas.Clear(Color.White);
            canvas.SetData();
            blank = canvas.GetTexture();
        }

        /// <summary>
        /// Loads ActivityProviders using reflection
        /// </summary>
        public void LoadActivityProviders(Assembly executingAssembly, string namespaceSerachPath)
        {
            Type[] types = ReflectionUtils.GetTypesUnderNamespace(executingAssembly, namespaceSerachPath);
            for (int i = 0; i < types.Length; i++)
            {
                var method = types[i].GetMethod("ActivityProvider");
                if (method != null)
                {
                    ActivityProvider activityProvider = (ActivityProvider)Delegate.CreateDelegate(typeof(ActivityProvider), method);
                    AddActivityProvider(types[i].Name, activityProvider);
                }
            }
        }

        public static void UpdateScreenSizeFields()
        {
            ScreenWidth = GraphicsDevice.Viewport.Width; //GraphicsSettings.ResWidth;
            ScreenHeight = GraphicsDevice.Viewport.Height;
            ScreenSize = new Vector2(ScreenWidth, ScreenHeight);
            ScreenCenter = ScreenSize * 0.5f;
            ScreenRectangle = new Rectangle(0, 0, ScreenWidth, ScreenHeight);
            GuiManager.Scale = MathHelper.Clamp(ScreenHeight / 1080f, 0.6f, 1);
        }

        public bool Update(GameTime gameTime, Game game)
        {
            UpdateScreenSizeFields(); //Just in case

            GameTime = gameTime;           
            _inputState.Update(gameTime, game);                                                 
            SoundEngine.Update(VolumeSettings.EffectsVolume);
            UpdateToast();
            if (BackgroundActivity != null)
                BackgroundActivity.Update(InputState.EmptyState);
            switch (_activityState)
            {
                case ActivityState.Active:
                    if (_currentActivity != null && (_currentPopupActivity == null || !_currentPopupActivity.IsBlocking))
                    {
                        if (_frameTime > 1)
                            _currentActivity.Update(_inputState);
                        else
                            _currentActivity.Update(InputState.EmptyState);
                    }
                    if (_currentPopupActivity != null)
                        if (_frameTime > 1)
                            _currentPopupActivity.Update(_inputState);
                        else
                            _currentPopupActivity.Update(InputState.EmptyState);
                    break;
                case ActivityState.TransitionOn:
                    _transitionTimer--;
                    if (_transitionTimer <= 0)
                    {
                        _activityState = ActivityState.Active;
                    }
                    break;
                case ActivityState.TransitionOff:
                    _transitionTimer--;
                    if (_transitionTimer == 0)
                    {
                        _activityState = ActivityState.TransitionOn;
                        _transitionTimer = transitionTime;
                        if (!_transitioningBack)
                        {
                            if (_targetActivity != null)
                            {
                                var parameters = _currentActivity.OnLeave();    //gather output from current activity
                                _targetActivity.TryInit(parameters);
                                _targetActivity.OnEnter(parameters);             //supply it to new activity
                                _targetActivity.Update(InputState.EmptyState); //???????
                            }
                        }
                        else
                        {
                            _targetActivity.OnResume(_currentActivity.OnBack());
                            _transitioningBack = false;
                        }
                        _currentActivity = _targetActivity;
                        if (_currentPopupActivity != null)
                        {
                            _currentPopupActivity.OnLeave();
                            _currentPopupActivity = null;
                        }
                        _targetActivity = null;
                    }
                    break;

                case ActivityState.Hidden:
                    break;

                default:
                    break;
            }


#if DEBUG
            if (_inputState.IsKeyPressed(Keys.PrintScreen))
            {
                TakeScreenshot = true;
            }
            if (_inputState.IsKeyPressed(Keys.F10))
            {
                IsCursorVisible = !IsCursorVisible;             
            }
#endif
            _frameTime++;
            return _quit;
        }

        public void FadeDraw(SpriteBatch sb, float alpha, Color color)
        {
            byte balpha = (byte)(alpha * 255);
            color.A = balpha;
            sb.Draw(blank, ScreenRectangle, color);
        }

        public void Draw(SpriteBatch sb)
        {
            
            //if (_currentPopupActivity == null || _currentPopupActivity.IsDrawBackgroundActivity)
            _currentActivity?.Draw(sb);

            if (_activityState == ActivityState.TransitionOn || _activityState == ActivityState.TransitionOff)
            {
                sb.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
                byte alpha = (byte)(GetAlpha() * 255);
                Color color = _transitionColor;
                color.A = alpha;
                sb.Draw(blank, ScreenRectangle, color);
                sb.End();
            }

            if (_currentPopupActivity != null)
            {
                _currentPopupActivity.Draw(sb);
            }

            DrawToast(sb);

            if (TakeScreenshot)
            {
                SaveScreenshot();
            }

            if (CursorTexture != null && IsCursorVisible)
            {
                sb.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
                Vector2 center = Vector2.Zero; //new Vector2(CursorTexture.Width, CursorTexture.Height) * 0.5f;
                sb.Draw(CursorTexture, _inputState.Cursor.Position - center, Color.White);
                sb.End();
            }
        }

        private void SaveScreenshot()
        {
            return;
            TakeScreenshot = false;
            int width = GraphicsSettingsUtils.GraphicsDevice.Viewport.Width;
            int height = GraphicsSettingsUtils.GraphicsDevice.Viewport.Height;
            Color[] buff = new Color[width * height];  //If Hidef
            //GraphicsSettingsUtils.GraphicsDevice.GetBackBufferData<Color>(buff);
            Texture2D screenShot = new Texture2D(GraphicsSettingsUtils.GraphicsDevice, width, height);
            screenShot.SetData<Color>(buff);
            var now = DateTime.Now;
            string datetimeString = string.Format("{0}{1}{2}{3}{4}{5}", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            Directory.CreateDirectory(ScreenshotPath);
            string fileName = string.Format("{0}{1}{2}{3}", ScreenshotPath, "Screencap", datetimeString, ".png");
            Stream file = File.Create(fileName);
            screenShot.SaveAsPng(file, width, height);
            file.Close();
        }

        private void DrawToast(SpriteBatch sb)
        {
            if (_toast != null)
            {
                byte alpha = (byte)Math.Min(_toastTimer * 15, 255);

                sb.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
                _toast.Draw(sb, new Color((byte)255, (byte)255, (byte)255, alpha));
                sb.End();
            }
        }

        public void AddActivityProvider(string name, ActivityProvider provider)
        {            
            _activityProviders.Add(name, provider);
        }

        public Activity SwitchActivity(string name, ActivityParameters parameters, bool saveBack = true)
        {
            if (name == "default")
            {
                SwitchActivity(DefaultActivity);
            }

            if (name == "back") //change
            {
                return Back();
            }

            if (name == "popback") //change
            {
                _currentPopupActivity?.OnBack();
                _currentPopupActivity = null;
                return Back();
            }

            if (name == "quit") //change
            {
                _quit = true;
                return null;
            }

            if (_activityProviders.ContainsKey(name))
            {
                Activity activity = _activityProviders[name].Invoke(parameters);
                if (activity != null)
                {
                    activity.TryInit(parameters);
                    activity.Update(InputState.EmptyState);
                    SwitchActivity(activity, saveBack);
                }
                return activity;
            }
            else
            {
                throw new Exception("Did not find ActivityProviders named: " + name);
            }            
        }

        public void SwitchActivity(Activity targetActivity, bool saveBack = true)
        {
            _frameTime = 0;
            //System.Diagnostics.Debug.WriteLine($"Switching to {targetActivity} ({targetActivity.GetHashCode()})");

            //_statLogger.LogStatistic(_statExtractor.ExtractStatisticsJson(currentActivity, targetActivity));
            if (targetActivity != null && !targetActivity.IsPopup)
            {
                if (saveBack)
                    _activityStack.Push(_currentActivity);
                //targetActivity.CallingActivity = _currentActivity;//change

                if (_currentActivity != null)
                {
                    this._targetActivity = targetActivity;
                    _transitionTimer = transitionTime;
                    _activityState = ActivityState.TransitionOff;
                }
                else
                {
                    _currentActivity = targetActivity;
                    _activityState = ActivityState.Active;
                    _currentActivity.OnEnter(null);
                }
            }
            else
            {
                _currentPopupActivity?.OnLeave();
                _currentPopupActivity = targetActivity;
            }
        }

        public Activity Back()
        {
            _frameTime = 0;
            if (_currentPopupActivity != null)
            {
                //_statLogger.LogStatistic(_statExtractor.ExtractStatisticsJson(currentPopupActivity, currentPopupActivity.CallingActivity));
                _currentPopupActivity.OnBack();
                //TODO: On Leave call
                _currentPopupActivity = null;
                return _currentActivity;
            }

            Activity activity = null;
            if (_activityStack.Count > 0)
            {
                activity = _activityStack.Pop();


            }
            else
            {
                if (DefaultActivity == null || _currentActivity == DefaultActivity)
                {
                    //_statLogger.LogStatistic(_statExtractor.ExtractStatisticsJson(currentActivity, null));  //exiting the game...never happens
                    _quit = true;
                    return null;
                }
                activity = DefaultActivity;
            }
            _transitioningBack = true;
            SwitchActivity(activity, false);
            return activity;
        }


        private float GetAlpha()
        {
            switch (_activityState)
            {
                case ActivityState.TransitionOn:
                    return _transitionTimer / (float)transitionTime;
                case ActivityState.TransitionOff:
                    return 1f - _transitionTimer / (float)transitionTime;
            }

            return 1;
        }

        private void UpdateToast()
        {
            if (_toast != null)
            {
                _toastTimer--;

                if (_toastTimer <= 0)
                {
                    _toast = null;
                }
            }
        }

        /// <summary>
        /// Add a message that displayed for a few frames and vanishes.
        /// <param name="time">time in number of frames</param>
        /// </summary>
        public void AddToast(string message, int time, Color? color = null, bool isOverride = true)
        {
            if (isOverride || _toast == null)
            {
                Vector2 tostMinSize = new Vector2(20, 20);
                _toastTimer = time;
                _toast = new RichTextControl(message);
                _toast.Width = Math.Max(_toast.Width, tostMinSize.X);
                _toast.Height = Math.Max(_toast.Height, tostMinSize.Y);
                _toast.Position = new Vector2(ScreenRectangle.Center.X, ScreenRectangle.Bottom - _toast.HalfSize.Y - 40);
                _toast.IsShowFrame = true;
                _toast.ControlColor = Color.Green; //TODO: tost color
                if (color != null)
                {
                    _toast.TextColor = color.Value;
                }
            }
        }

        public void Error(string text)
        {
            //TODO: write to log
            AddToast(text, 60 * 10, Color.Red);
            string filename = "BuggyMcBugBug" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".txt";
            File.WriteAllText(filename, text);
        }



        // Returns true if the current activity's type name is 'activityName'
        public bool IsCurrentPopupActivity(string activityName)
        {
            if (activityName == null) return false;
            if (_currentPopupActivity == null) return false;
            return _currentPopupActivity.GetType().Name.Equals(activityName);
        }



        public static void ErrorLog(string text)
        {
            if (_errors == null)
                _errors = new List<string>();
            _errors.Add(text);
        }

        public static void SaveErrors()
        {

        }

    }
}
