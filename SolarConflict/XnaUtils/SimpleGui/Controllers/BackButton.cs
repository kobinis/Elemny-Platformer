using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils;
using XnaUtils.Graphics;
using XnaUtils.Input;
using XnaUtils.SimpleGui;
using XnaUtils.SimpleGui.Controllers;

namespace SolarConflict.XnaUtils.SimpleGui.Controllers
{
    public class BackButton : ImageControl
    {
        public BackButton(Vector2 pos, Vector2 size) :
            base(Sprite.Get("back"), pos, size)
        {
            ControlColor = GuiManager.DefalutGuiColor;        
            Action += Exit_Action;
        }

        public static void Exit_Action(GuiControl source, CursorInfo cursorLocation)
        {
            ActivityManager.Inst.Back();
        }
    }
}
