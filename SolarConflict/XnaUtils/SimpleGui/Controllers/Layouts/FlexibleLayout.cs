using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace XnaUtils.SimpleGui {

    public enum Alignment {
        LeftToRight,
        RightToLeft,
        BottomToTop,
        TopToBottom
    }

    /// <remarks>Positioning of children relative to each other is wholly a function of alignment. An offset is added to ensure our two anchors (AnchorInSelf and AnchorInParent) have
    /// the same world position. Bear in mind the anchors are in normalized coordinates. An AnchorInSelf of (1f, 1f) is our bottom-right corner, and an AnchorInParent of (0.5f, 0.5f) is the center
    /// of our parent. So, for example, if AnchorInSelf == (1f, 0f) and AnchorInParent == (0f, 0f), our top right corner would be at our parent's top left corner.
    /// 
    /// TODO: add padding/margins</remarks>
    [Serializable]
    public class ControlChain {
        public Alignment Alignment;

        /// <remarks>In normalized coordinates; components should be in the interval [0, 1]</remarks>
        public Vector2? AnchorInSelf;
        /// <remarks>In normalized coordinates; components should be in the interval [0, 1]</remarks>
        public Vector2? AnchorInParent;

        public List<GuiControl> Elements;

        public float Padding;
        /// <summary>Spacing between elements, independent of padding</summary>
        public float Spacing;

        public ControlChain(params GuiControl[] controls) {
            Elements = controls.ToList();
        }

        public Vector2 Size() {            
            if (Alignment == Alignment.LeftToRight || Alignment == Alignment.RightToLeft)
                // Horizontal
                return new Vector2(Padding + Elements.Sum(c => c.Width), Elements.Max(c => c.Height)) + new Vector2(Spacing * Elements.Count, 0f);

            // Vertical
            return new Vector2(Elements.Max(c => c.Width), Padding + Elements.Sum(c => c.Height)) + new Vector2(0f, Spacing * Elements.Count);
        }

        public Rectangle BoundingRectangle(GuiControl parent) {
            var size = Size();
            var minimum = GlobalPositionOfMinimum(parent, size);

            return new Rectangle((int)minimum.X, (int)minimum.Y, (int)size.X, (int)size.Y);
        }

        Vector2 GlobalPositionOfMinimum(GuiControl parent, Vector2 size) {
            Vector2 localOffset;

            var parentRect = parent.GetRectangle();

            Vector2 parentAnchor;
            Vector2 selfAnchor;

            // Set default anchors, and define an offset and an arrangeElement function to help us arrange out children
            switch (Alignment) {
                case Alignment.LeftToRight:
                    parentAnchor = AnchorInParent ?? new Vector2(0f, 0.5f);
                    selfAnchor = AnchorInSelf ?? new Vector2(0f, 0.5f);
                    localOffset = new Vector2(0f, size.Y / 2f);                    

                    break;

                case Alignment.RightToLeft:
                    parentAnchor = AnchorInParent ?? new Vector2(1f, 0.5f);
                    selfAnchor = AnchorInSelf ?? new Vector2(1f, 0.5f);
                    localOffset = new Vector2(size.X, size.Y / 2f);                    

                    break;

                case Alignment.TopToBottom:
                    parentAnchor = AnchorInParent ?? new Vector2(0.5f, 1f);
                    selfAnchor = AnchorInSelf ?? new Vector2(0.5f, 1);
                    localOffset = new Vector2(size.X / 2f, 0f);                    

                    break;

                case Alignment.BottomToTop:
                    parentAnchor = AnchorInParent ?? new Vector2(0.5f, 0f);
                    selfAnchor = AnchorInSelf ?? new Vector2(0.5f, 0);
                    localOffset = new Vector2(size.X / 2f, size.Y);                    

                    break;

                default:
                    throw new NotImplementedException();
            }

            // Get our rect, find the offset that will align our anchors
            Debug.Assert(new float[] { AnchorInParent.Value.X, AnchorInParent.Value.Y, AnchorInSelf.Value.X, AnchorInSelf.Value.Y }.All(c => c <= 1f && c >= 0f), "Anchors aren't normalized");

            var globalSelfAnchor = new Vector2(selfAnchor.X * size.X, selfAnchor.Y * size.Y);
            var globalParentAnchor = new Vector2(parentRect.Left + parentAnchor.X * parent.Width, parentRect.Top + parentAnchor.Y * parent.Height);

            return localOffset + globalParentAnchor - globalSelfAnchor;
        }

        public void RefreshChildPositions(GuiControl parent) {
            RefreshChildPositions(GlobalPositionOfMinimum(parent, Size()) - parent.Position);            
        }

        void RefreshChildPositions(Vector2 localPositionOfMinimum) {
            Func<GuiControl, Vector2, Vector2> arrangeElement;            

            // Apply padding and define a function for arranging our elements locally
            switch (Alignment) {
                case Alignment.LeftToRight:
                    localPositionOfMinimum += new Vector2(Padding, 0f);
                    arrangeElement = (control, position) => {
                        var increment = new Vector2(control.HalfSize.X, 0f);
                        control.LocalPosition = position + increment;
                        return control.LocalPosition + increment + new Vector2(Spacing, 0f);
                    };

                    break;

                case Alignment.RightToLeft:
                    localPositionOfMinimum += new Vector2(-Padding, 0f);
                    arrangeElement = (control, position) => {
                        var increment = new Vector2(-control.HalfSize.X, 0f);
                        control.LocalPosition = position + increment;
                        return control.LocalPosition + increment + new Vector2(-Spacing, 0f);
                    };

                    break;

                case Alignment.TopToBottom:
                    localPositionOfMinimum += new Vector2(0f, Padding);
                    arrangeElement = (control, position) => {
                        var increment = new Vector2(0f, control.HalfSize.Y);
                        control.LocalPosition = position + increment;
                        return control.LocalPosition + increment + new Vector2(0f, Spacing);
                    };

                    break;

                case Alignment.BottomToTop:
                    localPositionOfMinimum += new Vector2(0f, -Padding);
                    arrangeElement = (control, position) => {
                        var increment = new Vector2(0f, -control.HalfSize.Y);
                        control.LocalPosition = position + increment;
                        return control.LocalPosition + increment + new Vector2(0f, -Spacing);
                    };

                    break;

                default:
                    throw new NotImplementedException();
            }            

            // Combine said function with our given global offset
            foreach (var e in Elements) 
                localPositionOfMinimum = arrangeElement(e, localPositionOfMinimum);
        }
    }

    [Serializable]
    public class FlexibleLayout : GuiControl {
        public List<ControlChain> Chains;
        public bool ShowFrame = false;
        public bool AutoRefresh = false;

        Vector2 _halfSizeLastRefresh;

        public FlexibleLayout() {
            Chains = new List<ControlChain>();
        }

        public void AddChain(Alignment alignment, float spacing, float padding, params GuiControl[] controls) {
            var anchor = Vector2.Zero;

            switch (alignment) {
                case Alignment.BottomToTop:
                    anchor = new Vector2(0.5f, 1f);
                    break;

                case Alignment.LeftToRight:
                    anchor = new Vector2(0f, 0.5f);
                    break;

                case Alignment.RightToLeft:
                    anchor = new Vector2(1f, 0.5f);
                    break;

                case Alignment.TopToBottom:
                    anchor = new Vector2(0.5f, 0f);
                    break;
            }

            AddChain(alignment, anchor, spacing, padding, controls);
        }

        public void AddChain(Alignment alignment, params GuiControl[] controls) {            
            AddChain(alignment, 0f, 0f, controls);
        }

        public void AddChain(Alignment alignment, Vector2 anchor, params GuiControl[] controls) {
            AddChain(alignment, anchor, 0f, 0f, controls);
        }        

        public void AddChain(Alignment alignment, Vector2 anchor, float spacing, float padding, params GuiControl[] controls) {
            AddChain(alignment, anchor, anchor, spacing, padding, controls);
        }

        public void AddChain(Alignment alignment, Vector2 anchorInChain, Vector2 anchorInControl, float spacing, float padding, params GuiControl[] controls) {
            Chains.Add(new ControlChain(controls) { Alignment = alignment, AnchorInSelf = anchorInChain, AnchorInParent = anchorInControl, Spacing = spacing, Padding = padding });

            Refresh();
        }

        /// <remarks>Will adopt any chain elements which aren't direct children, but won't orphan any children not in chains</remarks>
        public void Refresh() {
            var childSet = new HashSet<GuiControl>(children);

            foreach (var chain in Chains) {
                foreach (var control in chain.Elements)
                    if (!childSet.Contains(control))
                        AddChild(control);

                chain.RefreshChildPositions(this);
            }

            _halfSizeLastRefresh = HalfSize;
        }

        public override void UpdateLogic(InputState inputState) {
            base.UpdateLogic(inputState);
            if (AutoRefresh || (_halfSizeLastRefresh != HalfSize))
                Refresh();
        }

        /// <summary>The size of the layout's bounding rect</summary>
        /// <remarks>It's perfectly legal to define a chain that's innately outside its parent's bounds (e.g. LeftToRight alignment with both anchors' x-component being 1),
        /// so we can't auto-resize the layout to fit its children. Might wanna change that, though that'd cost us some flexibility.</remarks>
        public Vector2 CalculateActualSize() {
            Debug.Assert(Parent != null, "FlexibleLayout: null parent unsupported"); // could just have parent default to the screen rect

            var boundingRectangles = Chains.Select(c => c.BoundingRectangle(Parent));

            var min = new Vector2(boundingRectangles.Min(r => r.Left), boundingRectangles.Min(r => r.Top));
            var max = new Vector2(boundingRectangles.Max(r => r.Right), boundingRectangles.Max(r => r.Bottom));

            return max - min;
        }

        protected override void DrawLogic(SpriteBatch sb, Color? color = null) {
            if (ShowFrame) 
                base.DrawLogic(sb, color); 
        }
    }
}
