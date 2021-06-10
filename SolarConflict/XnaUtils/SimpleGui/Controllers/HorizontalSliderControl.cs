using Microsoft.Xna.Framework;
using System;
using XnaUtils;
using XnaUtils.Graphics;
using XnaUtils.SimpleGui;

namespace SolarConflict.XnaUtils.SimpleGui.Controllers
{
    /// <summary>
    /// Horizontal slider
    /// </summary>
    public class HorizontalSliderControl : GuiControl
    {
        private float _value;
        private int _sliderSize;
        private GuiControl _selectionButton;
        private bool _isCursorStartedOnControl;
        public bool HasValueChanged { get; private set; }
        public event ActionEventHandler OnValueChange;
        


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

        public HorizontalSliderControl(Vector2 size, string spriteID) : base(Vector2.Zero, size)
        {
            
            _selectionButton = new GuiControl(Vector2.Zero, new Vector2(size.Y, size.Y));
            _selectionButton.IsConsumingInput = false;
            _selectionButton.ControlColor = Color.White;
            _selectionButton.CursorOverColor = Color.White;
            _selectionButton.Sprite = TextureBank.Inst.GetSprite(spriteID);
            SliderSize = (int)size.Y;
            AddChild(_selectionButton);            
        }

        public override void UpdateLogic(InputState inputState)
        {
            HasValueChanged = false;
            if(inputState.Cursor.OnPressLeft)
            {
                _isCursorStartedOnControl = true;
            }

            if (_isCursorStartedOnControl && IsPositionOn(inputState.Cursor.FirstPosition) && inputState.Cursor.IsPressedLeft)
            {
                Value = PositionToValue(inputState.Cursor.Position.X - Position.X);
            }
            if (!inputState.Cursor.IsPressedLeft)
                _isCursorStartedOnControl = false;
            if (HasValueChanged)
                OnValueChange(this, inputState.Cursor);
        }

        public float PositionToValue(float position)
        {
            return MathHelper.Clamp((position +halfWidth) / (Width - _sliderSize), 0, 1);
        }

        private void UpdateSelectionButtonSize()
        {
            _selectionButton.HalfSize = new Vector2(_sliderSize * 0.5f, _selectionButton.HalfSize.Y);
        }

        private void UpdateSlectionButtonPosition()
        {
            float position = (_value - 0.5f) * (Width - _sliderSize);
            _selectionButton.LocalPosition = new Vector2(position, _selectionButton.LocalPosition.Y);
        }

        public static void BindToValueLogic(GuiControl control, InputState inputState)
        {
            PreferencesContainer container = control.Data as PreferencesContainer;
            container.SetFloat(control.UserData, (control as HorizontalSliderControl).Value);                
        }


    }
}
