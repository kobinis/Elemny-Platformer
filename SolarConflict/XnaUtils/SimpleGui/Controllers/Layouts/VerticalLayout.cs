using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaUtils.SimpleGui
{
    [Serializable]
    public class VerticalLayout: GuiControl
    {
        private float _spacing;
        public bool ShowFrame = false;
        public bool IsUpdatingPosition = false;
        public bool IsResizeChildrenHorizontally = false;

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

        public VerticalLayout(Vector2 position, float spaceing = 10, bool showFrame = false, bool isResizeChildrenHorizontally = false) : base(position, Vector2.One)
        {
            Spacing = spaceing;
            ShowFrame = showFrame;
            IsResizeChildrenHorizontally = isResizeChildrenHorizontally;
        }        

        public override void UpdateLogic(InputState inputState)
        {
            base.UpdateLogic(inputState);
            if(IsResizeChildrenHorizontally)
            {
                
                float maxWidth = children.Max(child => child.Width);
              //  float maxHeight = children.Max(child => child.Height);
                foreach (var item in children)
                {
                    item.Width = maxWidth;
                   // item.Height = maxHeight;
                }
            }
            if(IsUpdatingPosition)
            {
                SetHorizontalPositions();
            }
        }

        protected override void DrawLogic(SpriteBatch sb, Color? color = null)
        {
            if(ShowFrame)
                base.DrawLogic(sb, color); //add option to draw background
        }

        private void SetHorizontalPositions() //TODO: move to GUI CONTROL
        {
            float verticalPos = -HalfSize.Y + Spacing;
            foreach (var item in children)
            {
                item.LocalPosition = new Vector2(item.LocalPosition.X, verticalPos + item.HalfSize.Y);
                verticalPos += item.Height + Spacing;
            }
        }

        private Vector2 CalculateControlSize()
        {
            Vector2 size = Vector2.One * Spacing;
            foreach (var item in children)
            {
                size.Y = size.Y + item.Height + Spacing;
                size.X = Math.Max(size.X, item.Width + 2 * Spacing);
            }
            return size;
        }

        public override void AddChild(GuiControl guiController)
        {
            base.AddChild(guiController);
            this.HalfSize = CalculateControlSize() * 0.5f;
            SetHorizontalPositions();
        }
    }
}
