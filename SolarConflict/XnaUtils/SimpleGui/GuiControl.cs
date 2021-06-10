using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils.Input;
using Microsoft.Xna.Framework.Input;
using SolarConflict.XnaUtils.SimpleGui;
using SolarConflict.XnaUtils.SimpleGui.TextureGeneration;
using SolarConflict;
using XnaUtils.Graphics;
using System.Runtime.Serialization;

namespace XnaUtils.SimpleGui
{
    public delegate void ActionEventHandler(GuiControl source, CursorInfo cursorLocation);
    [Serializable]
    public class GuiControl
    {
        public delegate void DrawFunctionality(GuiControl control, SpriteBatch sb, Color? color = null);
        public delegate void LogicFunctionality(GuiControl control, InputState inputState);
        public enum ActivationLogicOperatorType { Or, And }

        public event ActionEventHandler CursorOn;
        public event ActionEventHandler Action;
        public Keys ActivationKey;
        public Keys SecondaryActivationKey;
        public ActivationLogicOperatorType ActivationLogicOperator;

        public Color ControlColor { set; get; }
        public Color? CursorOverColor { set; get; }
        public Color PressedControlColor { set; get; }

        public string TooltipText;
        public object Data;
        public Dictionary<string, object> _dataDictionary;
        public object GetData(string key)
        {
            if (_dataDictionary == null)
                return null;
            object data;
            _dataDictionary.TryGetValue(key, out data);
            return data;
        }

        public void SetData(string key, object value)
        {
            if (_dataDictionary == null)
                _dataDictionary = new Dictionary<string, object>();
            _dataDictionary[key] = value;
        }

        public DrawFunctionality DrawFuction;
        public LogicFunctionality LogicFunction;

        public bool IsConsumingInput { get; set; }
        public bool IsToggleable { set; get; }

        public int Index { get; set; }
        public Sprite Sprite;
        public Sprite PressedSprite;
        public Sprite CursorOverSprite;

        public bool DontInvoke { get; set; }

        /// <summary>
        /// Stops activation of callbacks, darkenes the control
        /// </summary>
        public bool Disable { get; set; }

        public bool DisableAll { //Chane to fuction
            set {
                    Disable = value;
                    foreach (var item in children)
                        {
                            item.DisableAll = value;
                        }
                 }
        }
      
        public string UserData { get; set; }

        public GuiControl Parent
        {
            get { return parent; }
            set { parent = value; }
        }
        private GuiControl parent;

        public Vector2 Position
        {
            get
            {
                if (parent == null)
                    return localPosition;
                else
                    return localPosition + parent.Position;
            } //maybe hold local var called position that updates once per update
            set
            {
                if (parent == null)
                    localPosition = value;
                else
                    localPosition = value - parent.Position;
            }
        }

        /// <summary>Translates (but does NOT resize) to fit to screen, if possible</summary>
        public void FitToScreen()
        {
            var position = Position;
            position.X = MathHelper.Clamp(position.X, HalfSize.X, ActivityManager.ScreenRectangle.Width - HalfSize.X);
            position.Y = MathHelper.Clamp(position.Y, HalfSize.Y, ActivityManager.ScreenRectangle.Height - HalfSize.Y);
            Position = position;
        }

        public void SetAllColors(Color color, bool alsoSetCursorOver)
        {
            ControlColor = color;
            PressedControlColor = color;
            if (alsoSetCursorOver)
                CursorOverColor = color;
        }

        public virtual string GetTooltipText()
        {
            return TooltipText;
        }

        public Vector2 LocalPosition
        {
            get { return localPosition; }
            set { localPosition = value; }
        }
        private Vector2 localPosition;

        /*public Vector2 LeftPosition
        {
            get { return  new Vector2(position.X + width/2f, position.Y + height/2f); }
            set { Position = new Vector2(value.X - width / 2f, value.Y + height / 2f); }
        } */

        public float Width
        {
            get { return halfWidth * 2f; }
            set { halfWidth = value * 0.5f; }
        }
        protected float halfWidth;

        public bool DrawShade { get; set; }

        public float Height
        {
            get { return halfHeight * 2f; }
            set { halfHeight = value * 0.5f; }
        }
        protected float halfHeight;

        public Vector2 HalfSize
        {
            get { return new Vector2(halfWidth, halfHeight); }
            set { halfWidth = value.X; halfHeight = value.Y; }
        }

        public bool IsCursorOn
        {
            get
            {
                return isCursorOn;
            }
        }
        bool isCursorOn;


        public virtual bool IsPressed { get; set; }


        protected List<GuiControl> children;





        public GuiControl() : this(Vector2.Zero, 1, 1)
        {
        }

        public GuiControl(Vector2 position, Vector2 size) : this(position, size.X, size.Y)
        {
        }

        public GuiControl(Vector2 position, float width, float height, GuiManager guiHolder = null)
        {
            Sprite = GuiManager.BackTexture;
            IsConsumingInput = true;
            Width = width;
            Height = height;
            localPosition = position;
            ControlColor = GuiManager.DefalutGuiColor;
            PressedControlColor = ControlColor;
            CursorOverColor = null;
            children = new List<GuiControl>();
            DrawShade = true;
            ActivationKey = Keys.None;
            SecondaryActivationKey = Keys.None;
            ActivationLogicOperator = ActivationLogicOperatorType.Or;
        }

        public void AddChilds(params GuiControl[] children)
        {
            foreach (var item in children)
            {
                AddChild(item);
            }          
        }

        public virtual void AddChild(GuiControl guiController)
        {
            guiController.parent = this;
            children.Add(guiController);
        }

        public virtual void RemoveAllChildren()
        {
            children.Clear();
        }

        public virtual void RemoveChild(GuiControl guiController)
        {
            guiController.parent = null;
            children.Remove(guiController);
        }

        public virtual bool IsPositionOn(Vector2 position)
        {
            Vector2 fixedVector = (Position - position) / new Vector2(halfWidth, halfHeight);
            return Math.Max(Math.Abs(fixedVector.X), Math.Abs(fixedVector.Y)) <= 1;
            //return position.X >= Position.X - halfWidth && position.X <= Position.X + halfWidth &&
            //    position.Y >= Position.Y - halfHeight && position.Y <= Position.Y + halfHeight;
        }

        public virtual void UpdateLogic(InputState inputState)
        {

        }       

        public virtual void Update(InputState inputState) //KOBI: get guiholder
        {
            if (LogicFunction != null)
            {
                LogicFunction.Invoke(this, inputState);
            }
            if (Disable)
                return;

            if (!IsToggleable)
            {
                IsPressed = false;
            }

            for (int i = children.Count - 1; i >= 0; i--)
            {
                children[i].Update(inputState);
            }

            isCursorOn = false;
            //  for (int i = 0; i < inputState.Capacity; i++)
            {
                CursorInfo cursorLocation = inputState.Cursor; //inputState.CursorCollection[i];                                       
                //if Corsor.IsActive
                bool cursorOn = cursorLocation.IsActive && IsPositionOn(cursorLocation.Position);
                if (cursorOn)
                {
                    inputState.Cursor.ActiveGuiControl = this;
                    //remove input;
                    // inputState.CursorCollection.Remove(cursorLocation);
                    if (CursorOn != null)
                    {
                        CursorOn.Invoke(this, cursorLocation);
                    }
                   

                    if (cursorLocation.OnPressLeft || cursorLocation.OnPressRight)
                    {                        
                        IsPressed = !IsPressed;// true; //??
                        InvokeAction(cursorLocation);

                    }

                    if (IsConsumingInput)
                    {
                        inputState.Cursor.IsActive = false;
                        inputState.Cursor.WasNotStartedOnGui = false;
                    }

                    if (cursorLocation.OnReleaseLeft || cursorLocation.OnReleaseRight)
                    {

                    }

                }

                if (Action != null) //TODO: change
                {
                    if (ActivationLogicOperator == ActivationLogicOperatorType.Or)
                    {
                        if (inputState.IsKeyPressed(ActivationKey) || inputState.IsKeyPressed(SecondaryActivationKey)) //change?
                        {
                            //   isPressed = !isPressed;
                            InvokeAction(new CursorInfo());
                        }
                    }
                    else  //ActivationLogicOperator == ActivationLogicOperatorType.And
                    {
                        if (inputState.IsKeyPressed(ActivationKey) && inputState.IsKeyDown(SecondaryActivationKey))
                        {
                            //   isPressed = !isPressed;
                            InvokeAction(new CursorInfo());
                        }
                    }
                }

                isCursorOn |= cursorOn;
            }

            

            UpdateLogic(inputState);
        }


        protected virtual void DrawLogic(SpriteBatch sb, Color? color = null)
        {

            if (Sprite != null) //&& show Frame
            {
                Color controlColor = ControlColor;                

                if (IsPressed)
                {
                    controlColor = PressedControlColor;
                }

                if (IsCursorOn && CursorOverColor.HasValue)
                {
                    if (IsPressed)
                        controlColor = Color.Lerp(CursorOverColor.Value, PressedControlColor, 0.5f);
                    else
                        controlColor = CursorOverColor.Value;
                }

                if (color.HasValue)
                {
                    Vector4 colorVec = color.Value.ToVector4();
                    controlColor = new Color(controlColor.ToVector4() * colorVec);
                }

                if (Disable)
                    controlColor = controlColor * GuiManager.DisableShade;

                Rectangle rectangle = GetRectangle();
                //if (!(isPressed ^ FlipTexture))
                //    sb.Draw(Texture, rectangle, null, controlColor, 0, Vector2.Zero, SpriteEffects.None, 0);
                //else
                //    sb.Draw(Texture, rectangle, null, controlColor, 0, Vector2.Zero, SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically, 0);
                if (IsPressed == false || PressedSprite == null)
                    Sprite.Draw(sb, rectangle, controlColor);
                else
                    PressedSprite.Draw(sb, rectangle, controlColor);
            }
            if (DrawFuction != null)
            {
                DrawFuction.Invoke(this, sb, color);
            }

        }

        public Rectangle GetRectangle()
        {
            return new Rectangle((int)(Position.X - halfWidth), (int)(Position.Y - halfHeight), (int)Width, (int)Height);
        }

        public virtual void Draw(SpriteBatch sb, Color? color = null) //remove virtual
        {
            DrawLogic(sb, color);

            foreach (var guiControl in children)
            {
                guiControl.Draw(sb, color);
            }
        }

        public virtual void InvokeAction(CursorInfo cursorLocation)
        {
            if (!DontInvoke && !Disable)
            {
                if (Action != null)
                {
                    Action.Invoke(this, cursorLocation);
                }
            }
        }

        public void IvokeCursorOn(CursorInfo cursorLocation) //??
        {
            if (CursorOn != null)
            {
                CursorOn.Invoke(this, cursorLocation);
            }
        }

        public GuiControl[] GetChildren()
        {
            return children.ToArray();
        }

        public void AlignnUp(int spaceing = 10, GuiControl control = null)
        {
            float anchor = 0;
            if (control != null)
            {
                anchor = control.Position.Y + control.HalfSize.Y;
            }
            Position = new Vector2(Position.X, anchor + HalfSize.Y + spaceing);
        }

        public void AlignnDown(int spaceing = 10, GuiControl control = null)
        {
            float anchor = ActivityManager.ScreenSize.Y;
            if (control != null)
            {
                anchor = control.Position.Y - control.HalfSize.Y;
            }
            Position = new Vector2(Position.X, anchor - HalfSize.Y - spaceing);
        }


        //public bool IsInputOn { set; get; }//needs to be replaced into a single struct or class
        //public TouchState InputState { set; get; }//needs to be replaced into a single struct or class

        //[OnDeserialized]
        //public void OnDeserializedMethod(StreamingContext context)
        //{

        //    Sprite = GenerateSprite((int)this.Width, (int)this.Height);
        //}
    }
}
