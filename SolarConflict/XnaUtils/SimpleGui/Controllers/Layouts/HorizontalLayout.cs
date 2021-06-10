using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaUtils.SimpleGui
{
    [Serializable]
    public class HorizontalLayout : GuiControl //change it
    {
        private float _spacing;
        public bool ShowFrame = false;
        public bool IsAutoUpadeSize = false;

        public float Spacing
        {
            get
            {
                return _spacing;
            }
            set
            {
                _spacing = value;
            }
        }


        public HorizontalLayout(Vector2 position, float spaceing = 10, bool showFrame = false) : base(position, Vector2.One)
        {
            Spacing = spaceing;
            ShowFrame = showFrame;
        }

        public override void UpdateLogic(InputState inputState)
        {
            base.UpdateLogic(inputState);
            if (IsAutoUpadeSize)
                RefreshSize();

        }

        protected override void DrawLogic(SpriteBatch sb, Color? color = null)
        {
            if (ShowFrame)
            {
                base.DrawLogic(sb, color); //add option to draw background
            }
        }

        public void SetHorizontalPositions() //TODO: move to GUI CONTROL
        {
            float horizontalPos = -HalfSize.X + Spacing;
            foreach (var item in children)
            {
                item.LocalPosition = new Vector2(horizontalPos + item.HalfSize.X, item.LocalPosition.Y);
                horizontalPos += item.Width + Spacing;
            }
        }

        Vector2 CalculateHalfSize()
        {
            Vector2 size = Vector2.One * Spacing;
            foreach (var item in children)
            {
                size.X = size.X + item.Width + Spacing;
                size.Y = Math.Max(size.Y, item.Height + 2 * Spacing);
            }
            return size * 0.5f;
        }

        public override void AddChild(GuiControl guiController)
        {
            base.AddChild(guiController);

            RefreshSize();
            SetHorizontalPositions();
        }

        public void RefreshSize() {
            HalfSize = CalculateHalfSize();
        }


    }
}
