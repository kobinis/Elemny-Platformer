using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaUtils.SimpleGui
{
    public enum VerticalAlignment { None, Up, Center, Down }
    public enum HorizontalAlignment { None, Left, Center, Right }
    [Serializable]
    public class RelativeLayout : GuiControl
    {
        [Serializable]
        public class AlignmentData
        {
            public AlignmentData(HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment, GuiControl anchor, int spacing = 0, bool isInside = false)
            {
                HorizontalAlignment = horizontalAlignment;
                VerticalAlignment = verticalAlignment;
                Anchor = anchor;
                IsInside = isInside;
                Spacing = spacing;
            }
            public HorizontalAlignment HorizontalAlignment;
            public VerticalAlignment VerticalAlignment;
            public GuiControl Anchor;
            public int Spacing;
            public bool IsInside;
        }

        public float Spacing { get; set; }
        public bool ShowFrame = false;
        public bool IsAutoUpadeSize;
        private GuiControl GlobalAnchor { get { return this.Parent; } }
        Dictionary<GuiControl, AlignmentData> alignmentDataDic;
        public AlignmentData DefaultAligmentData { get; set; }        
        public GuiControl LastChildAdded
        {
            get {
                if (children.Count > 0) return 
                        children.Last();
                return null;
            }
        }

        //vertical layout                
        public RelativeLayout() : base(Vector2.Zero, Vector2.One)
        {
            DefaultAligmentData = new AlignmentData(HorizontalAlignment.Center, VerticalAlignment.Center, this, 0, true);
            alignmentDataDic = new Dictionary<GuiControl, AlignmentData>();
            Spacing = 8;
            IsAutoUpadeSize = true;
        }

        public override void UpdateLogic(InputState inputState)
        {
            base.UpdateLogic(inputState);
            if (IsAutoUpadeSize)
                SetPositions();
        }

        protected override void DrawLogic(SpriteBatch sb, Color? color = null)
        {
            if (ShowFrame)
            {
                base.DrawLogic(sb, color); //add option to draw background
            }
        }

        private void SetPositions() //TODO: move to GUI CONTROL
        {           
            foreach (var item in children)
            {
                SetControlPosition(item);
            }
        }

        private Vector2 CalculateControlSize()
        {
            Vector2 size = Vector2.One * Spacing;
            foreach (var item in children)
            {
                size.X = size.X + item.Width + Spacing;
                size.Y = Math.Max(size.Y, item.Height + 2 * Spacing);
            }
            return size;
        }

        public override void AddChild(GuiControl guiController)
        {
            base.AddChild(guiController);
            this.HalfSize = CalculateControlSize() * 0.5f;
            SetPositions();
        }

        public void AddChild(GuiControl guiControl, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment, GuiControl anchor = null, int spacing = 0)
        {
            alignmentDataDic.Add(guiControl, new AlignmentData(horizontalAlignment, verticalAlignment, anchor, spacing, anchor == null));
            AddChild(guiControl);
        }

        public override void RemoveChild(GuiControl guiController)
        {
            alignmentDataDic.Remove(guiController);
            base.RemoveChild(guiController);
        }

        private void SetControlPosition(GuiControl control)
        {
            AlignmentData aligment = null;
            GuiControl anchor = null;//aligment.Anchor;
            if (!alignmentDataDic.TryGetValue(control, out aligment))
            {
                aligment = DefaultAligmentData;                
            }
            //else
            //{
            //    anchor = aligment.Anchor;
            //}
            anchor = aligment.Anchor;
            if (anchor == null)
                anchor = DefaultAligmentData.Anchor;
            
            Vector2 position = control.Position;
            switch (aligment.HorizontalAlignment)
            {
                case HorizontalAlignment.None:
                    break;
                case HorizontalAlignment.Left:
                    if (aligment.IsInside)
                        position.X = anchor.Position.X - anchor.HalfSize.X + control.HalfSize.X + Spacing;
                    else
                        position.X = anchor.Position.X + anchor.HalfSize.X + control.HalfSize.X + Spacing;
                    break;
                case HorizontalAlignment.Center:
                    position.X = anchor.Position.X;
                    break;
                case HorizontalAlignment.Right:
                    if (aligment.IsInside)
                        position.X = anchor.Position.X + anchor.HalfSize.X - control.HalfSize.X - Spacing;
                    else
                        position.X = anchor.Position.X - anchor.HalfSize.X - control.HalfSize.X - Spacing;
                    break;
                default:
                    break;
            }

            switch (aligment.VerticalAlignment)
            {
                case VerticalAlignment.None:
                    break;
                case VerticalAlignment.Up:
                    if (aligment.IsInside)
                        position.Y = anchor.Position.Y - anchor.HalfSize.Y + control.HalfSize.Y + Spacing;
                    else
                        position.X = anchor.Position.Y + anchor.HalfSize.Y + control.HalfSize.Y + Spacing;
                    break;
                case VerticalAlignment.Center:
                    position.Y = anchor.Position.Y;
                    break;
                case VerticalAlignment.Down:
                    if (aligment.IsInside)
                        position.Y = anchor.Position.Y + anchor.HalfSize.Y - control.HalfSize.Y - Spacing;
                    else
                        position.X = anchor.Position.Y - anchor.HalfSize.Y - control.HalfSize.Y - Spacing;
                    break;
                default:
                    break;
            }
            control.Position = position;

        }

    }
}

