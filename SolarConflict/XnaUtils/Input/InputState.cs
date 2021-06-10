using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using SolarConflict.XnaUtils;
using SolarConflict.XnaUtils.Input;
using XnaUtils.Input;
using Redux.Utilities.Managers;
using System.Linq;

namespace XnaUtils
{  
    //TODO: add Xbox Control, add Back (???, Select, Up and Down ???)
    //Change to struct, move update to different class
    //In the feature add touch support //??


    /// <summary>
    /// Holds all inputs from user
    /// </summary>    
    [Serializable] // TEMP, should not be serialized
    public class InputState //change to struct?
    {
        static readonly InputState emptyState = new InputState();
        public static InputState EmptyState //Not Good, can be changed
        {
            get { return InputState.emptyState; }
        }        

        const int MaxGamePadNumber = 4;
                 
        public CursorInfo Cursor;
        
        public KeyboardState LastKeyboardState;
        public KeyboardState KeyboardState; //change to private
                
        MouseState mouseState;        
        MouseState lastMouseState;
        
        public readonly GamePadState[] CurrentGamePadStates; //add gamepadLast State;

        public KeyboardManager keyboardManager;
        public string TextBuffer;

        /// <summary>
        /// Constructs a new input state.
        /// </summary>
        public InputState()
        {
            CurrentGamePadStates = new GamePadState[MaxGamePadNumber];
            for (int i = 0; i < CurrentGamePadStates.Length; i++)
            {
                CurrentGamePadStates[i] = new GamePadState(); //maybe last
            }

            KeyboardState = new KeyboardState();
            //CursorCollection = new CursorLocation[maxCapacity];
            Cursor = new CursorInfo();
            keyboardManager = new KeyboardManager();
        }

        public virtual void Update(GameTime gameTime, Game game)
        {
            if (game != null && !game.IsActive)
            {
                return;
            }

            CurrentGamePadStates[0] = GamePad.GetState(0);

            //Keyboard
            LastKeyboardState = KeyboardState;
            KeyboardState = Keyboard.GetState();

            var keysPressedThisFrame = KeyboardState.GetPressedKeys().Where(k => IsKeyPressed(k)).ToArray();

            keyboardManager.Update(gameTime, keysPressedThisFrame, KeyboardState.GetPressedKeys());
            TextBuffer = keyboardManager.Text;
            //Mouse            
            MouseUtils.Inst.Update(); //change            
            mouseState = MouseUtils.Inst.mouseState;
            lastMouseState = MouseUtils.Inst.lastMouseState;            
 
            CursorInfo cl = new CursorInfo();
            if(Cursor.ActiveGuiControl != null)
               cl.ActiveGuiControl = Cursor.ActiveGuiControl;
            cl.IsActive = true;
            cl.WasNotStartedOnGui = Cursor.WasNotStartedOnGui || (mouseState.LeftButton == ButtonState.Released && mouseState.RightButton == ButtonState.Released);
            cl.PreviousPosition = Cursor.Position;
            cl.Position = new Vector2(mouseState.X, mouseState.Y);
           // cl.PreviousPosition = new Vector2(lastMouseState.X, lastMouseState.Y);
            cl.IsPressedLeft = mouseState.LeftButton == ButtonState.Pressed;
            cl.IsLastPressedLeft = lastMouseState.LeftButton == ButtonState.Pressed;
            cl.IsPressedRight = mouseState.RightButton == ButtonState.Pressed;
            cl.IsLastPressedRight = lastMouseState.RightButton == ButtonState.Pressed;
            if (cl.OnPressLeft || cl.OnPressRight)
            {
                cl.FirstPosition = cl.Position;
            }
            else
            {
                cl.FirstPosition = Cursor.FirstPosition;
            }
         /*   cl.ScrollValue = mouseState.ScrollWheelValue;
            cl.LastScrollValue = lastMouseState.ScrollWheelValue;*/
            Cursor = cl;
           // CursorCollection[0] = cl;
        }        

        public bool IsKeyDown(Keys key) {
            return KeyboardState.IsKeyDown(key);
        }

        /// <summary>Returns true when the key is pressed (is down and was up)</summary>
        public bool IsKeyPressed(Keys key)
        {
            return (KeyboardState.IsKeyDown(key) && LastKeyboardState.IsKeyUp(key));
        }

        public bool IsKeyReleased(Keys key)
        {
            return (KeyboardState.IsKeyUp(key) && LastKeyboardState.IsKeyDown(key));
        }

        public bool IsGKeyDown(GKeys key)
        {
            bool res = MouseUtils.IsMouseButtonsDown(mouseState, key.MouseButton) && (Cursor.IsActive && Cursor.WasNotStartedOnGui); //TODO
            res |= KeyboardState.IsKeyDown(key.Key);            
            return res;
        }

        public bool IsPreviousGKeyDown(GKeys key)
        {
            bool res = MouseUtils.IsMouseButtonsDown(lastMouseState, key.MouseButton);
            res |= LastKeyboardState.IsKeyDown(key.Key);
            return res;
        }

        public bool IsGKeyPressed(GKeys key)
        {
            return (IsGKeyDown(key) && !IsPreviousGKeyDown(key));
        }

        


        public GKeys GetPressedKey()
        {

            if (lastMouseState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed)
                return new GKeys(MouseButtons.LeftButton);
            if (lastMouseState.RightButton == ButtonState.Released && mouseState.RightButton == ButtonState.Pressed)
                return new GKeys(MouseButtons.RightButton);
            if (lastMouseState.MiddleButton == ButtonState.Released && mouseState.MiddleButton == ButtonState.Pressed)
                return new GKeys(MouseButtons.MiddleButton);

            //var mouseValues = ReflectionUtils.GetValues<MouseButtons>();
            //foreach (var mouseBut in mouseValues)
            //{
            //    if(IsGKeyPressed(new GKeys(mouseBut)))
            //    {
            //        return new GKeys(mouseBut);
            //    }
            //}

            var values= ReflectionUtils.GetValues<Keys>();
            foreach (var key in values)
            {
                if (IsKeyPressed(key))
                    return key;
            }

            return Keys.None; ;
        }


        public string Convert(Keys[] keys) //TODO: Change this shit

        {

            string output = "";
            bool usesShift = false;
           
            foreach (Keys key in keys)

            {

                if (key >= Keys.A && key <= Keys.Z)

                    output += key.ToString();

                else if (key >= Keys.NumPad0 && key <= Keys.NumPad9)

                    output += ((int)(key - Keys.NumPad0)).ToString();

                else if (key >= Keys.D0 && key <= Keys.D9)

                {

                    string num = ((int)(key - Keys.D0)).ToString();

                    #region special num chars

                    if (usesShift)

                    {

                        switch (num)

                        {

                            case "1":

                                {

                                    num = "!";

                                }

                                break;

                            case "2":

                                {

                                    num = "@";

                                }

                                break;

                            case "3":

                                {

                                    num = "#";

                                }

                                break;

                            case "4":

                                {

                                    num = "$";

                                }

                                break;

                            case "5":

                                {

                                    num = "%";

                                }

                                break;

                            case "6":

                                {

                                    num = "^";

                                }

                                break;

                            case "7":

                                {

                                    num = "&";

                                }

                                break;

                            case "8":

                                {

                                    num = "*";

                                }

                                break;

                            case "9":

                                {

                                    num = "(";

                                }

                                break;

                            case "0":

                                {

                                    num = ")";

                                }

                                break;

                            default:

                                //wtf?

                                break;

                        }

                    }

                    #endregion

                    output += num;

                }

                else if (key == Keys.OemPeriod)

                    output += ".";

                else if (key == Keys.OemTilde)

                    output += "'";

                else if (key == Keys.Space)

                    output += " ";

                else if (key == Keys.OemMinus)

                    output += "-";

                else if (key == Keys.OemPlus)

                    output += "+";

                else if (key == Keys.OemQuestion && usesShift)

                    output += "?";

                else if (key == Keys.Back) //backspace

                    output += "\b";

 

                if (!usesShift) //shouldn't need to upper because it's automagically in upper case

                    output = output.ToLower();

            }

            return output;

	        }

        private bool IsKeyPressed(object key)
        {
            //throw new NotImplementedException();
            return false;
        }
    }


}
