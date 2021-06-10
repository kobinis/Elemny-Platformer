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
    /// Vertical Slider
    /// </summary>
    public class VerticalSliderControl : GuiControl
    {
        private float _value;
        private int _sliderSize;
        private GuiControl _selectionButton;
        public bool HasValueChanged { get; private set; }
        

        public float Value
        {
            get { return _value; }
            set
            {
                float newValue = MathHelper.Clamp(value, 0, 1);
                if (_value != newValue)
                {
                    HasValueChanged = true;
                    _value = value;
                    UpdateSlectionButtonPosition();
                }
            }
        }

        public int SliderSize
        {
            get { return _sliderSize; }
            set
            {
                _sliderSize = Math.Max(value, 0);
                UpdateSelectionButtonSize();
                UpdateSlectionButtonPosition();
            }
        }

        public VerticalSliderControl(Vector2 size) : base(Vector2.Zero, size)
        {
            _selectionButton = new GuiControl(Vector2.Zero, new Vector2(size.X, size.X));
            int sliderSize = (int)size.X;
            _selectionButton.IsConsumingInput = false;
            _selectionButton.ControlColor = Color.White;
            _selectionButton.CursorOverColor = Color.White;
            AddChild(_selectionButton);
            SliderSize = sliderSize;
        }

        public override void UpdateLogic(InputState inputState)
        {
            HasValueChanged = false;            
            if (IsPositionOn(inputState.Cursor.FirstPosition) && inputState.Cursor.IsPressedLeft)
            {
                Value = PositionToValue(inputState.Cursor.Position.Y - Position.Y);
            }
        }

        public float PositionToValue(float position)
        {            
            return MathHelper.Clamp((position + halfHeight) / (Height - _sliderSize), 0, 1);
        }

        private void UpdateSelectionButtonSize()
        {            
            _selectionButton.HalfSize = new Vector2(_selectionButton.HalfSize.X, _sliderSize * 0.5f);
        }

        private void UpdateSlectionButtonPosition()
        {
            float position = (_value - 0.5f) * (Height - _sliderSize);
            _selectionButton.LocalPosition = new Vector2(_selectionButton.LocalPosition.X, position);
        }


    }
}
