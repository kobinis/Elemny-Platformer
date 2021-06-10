using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils;
using XnaUtils.SimpleGui;

namespace SolarConflict.XnaUtils.SimpleGui
{
    /// <summary>
    /// Updates and draws the selected one of the kids
    /// </summary>
    [Serializable]
    public class GuiControlSelector : GuiControl
    {        
        private int _selectedControl;
        public int SelectedControl {
            get { return _selectedControl; }
            set { _selectedControl = Math.Min(Math.Max(value, 0), children.Count-1); }
        }


        public GuiControlSelector():base()
        {
            IsConsumingInput = false;
        }

        public override void Update(InputState inputState)
        {
            if (LogicFunction != null)
            {
                LogicFunction.Invoke(this, inputState);
            }

            if (!IsToggleable)
            {
                IsPressed = false;
            }

            HalfSize = children[_selectedControl].HalfSize;
            children[_selectedControl].Position = this.Position; //if Update position
            children[_selectedControl].Update(inputState);
          
            UpdateLogic(inputState);
        }

        public override void Draw(SpriteBatch sb, Color? color = default(Color?))
        {
            DrawLogic(sb, color);
            children[_selectedControl].Draw(sb, color);            
        }

        protected override void DrawLogic(SpriteBatch sb, Color? color = default(Color?))
        {
            //base.DrawLogic(sb, color);
        }

        public override void AddChild(GuiControl guiController)
        {
            HalfSize = guiController.HalfSize;
            LocalPosition = guiController.LocalPosition;
            
            guiController.Parent = this.Parent;
            children.Add(guiController);
        }
        
    }
}
