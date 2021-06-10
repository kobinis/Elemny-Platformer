using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;
using XnaUtils.SimpleGui;

namespace SolarConflict.XnaUtils.SimpleGui.Controllers
{
    /// <summary>
    /// Vertical Scrollbar
    /// </summary>
    [Serializable]
    public class VerticalScrollbarControl :GuiControl
    {
        private int _value;
        private int _maxValue;
        private GuiControl _selectionButton;
        public bool HasValueChanged { get; set; }

        public int Value
        {
            get { return _value; }
            set
            {
                int newValue = Math.Min(Math.Max(value, 0), MaxValue);
                if(_value != newValue)
                {
                    HasValueChanged = true;
                    _value = value;
                    UpdateSlectionButtonPosition();
                }
                
            }
        }

        public int MaxValue
        {
            get { return _maxValue; }
            set
            {                
                _maxValue = Math.Max(value,0);
                UpdateSelectionButtonSize();
                UpdateSlectionButtonPosition();
            }
        }

        public VerticalScrollbarControl (Vector2 size, int maxValue):base(Vector2.Zero, size)
        {
            Sprite = GuiManager.ScrollTexture;
            _selectionButton = new GuiControl(Vector2.Zero, new Vector2(size.X, size.X));
            _selectionButton.Sprite = GuiManager.ScrollTexture;
            //_selectionButton.ControlColor = 
            _selectionButton.IsConsumingInput = false;
            _selectionButton.ControlColor = Color.White;
            _selectionButton.CursorOverColor = Color.White;
            AddChild(_selectionButton);
            MaxValue = maxValue;            
        }
        

        public override void UpdateLogic(InputState inputState)
        {
            HasValueChanged = false;            
            //if (Math.Abs(inputState.Cursor.GetDScroolWheel()) > 0)
            //{
            //    Value += inputState.Cursor.GetDScroolWheel();
            //}

            if (IsPositionOn(inputState.Cursor.FirstPosition) && inputState.Cursor.IsPressedLeft)
            {
                Value = PositionToValue(inputState.Cursor.Position.Y - Position.Y);
            }


        }  
        
        public int PositionToValue(float position)
        {
            int value = (int)Math.Round((position  +halfHeight) / Height * MaxValue);
            value = Math.Min(Math.Max(value, 0), MaxValue);
            return value;
        }       

        private void UpdateSelectionButtonSize()
        {
            float halfHeight = this.halfHeight / (_maxValue+1);
            _selectionButton.HalfSize = new Vector2(_selectionButton.HalfSize.X, halfHeight);
        }

        private void UpdateSlectionButtonPosition()
        {            
            float position = _value /(float) _maxValue * (this.HalfSize.Y*2 - _selectionButton.HalfSize.Y*2) - this.HalfSize.Y + _selectionButton.HalfSize.Y;
            _selectionButton.LocalPosition = new Vector2(_selectionButton.LocalPosition.X, position);
        }

        
    }
}
