using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Redux.Utilities.Managers;
using XnaUtils;
using XnaUtils.SimpleGui;
using XnaUtils.SimpleGui.Controllers;

namespace SolarConflict.XnaUtils.SimpleGui.Controllers
{
    public class TextBox : TextControl
    {
        public ActionEventHandler EditDoneEvent;
        private bool _isFocusOn;

        public TextBox(string text, SpriteFont font)
            : base(text, font)
        {
            
            
            
        }

        public override void UpdateLogic(InputState inputState)
        {
            if (_isFocusOn)
            {
                if (inputState.keyboardManager.IsBackspaceActive && base.Text.Length > 0)
                {
                    base.Text = base.Text.Remove(base.Text.Length - 1);
                }
                else
                {
                    string deltaText = inputState.TextBuffer;
                    base.Text += deltaText;
                }
            }
        }
    }
}
