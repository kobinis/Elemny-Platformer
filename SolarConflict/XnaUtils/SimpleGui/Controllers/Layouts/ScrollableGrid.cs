using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.SimpleGui;
using XnaUtils;
using Microsoft.Xna.Framework.Input;
using SolarConflict.XnaUtils.SimpleGui.Controllers;
using Microsoft.Xna.Framework.Graphics;
using SolarConflict.XnaUtils.Input;

namespace SolarConflict.XnaUtils.SimpleGui
{
    /// <summary>
    /// Grid Control is a ControlGroup that displays items in a two-dimensional, scrollable grid.
    /// </summary>
    [Serializable]
    public class ScrollableGrid : GuiControl
    {
        public int Padding { get; set; } 
        public int Capacity { get { return _xBinNum * _yBinNum; } }
        public int Count { get { return _allChildren.Count; } }
        private int _spaceing = 5; 
        private int _xBinNum;
        private int _yBinNum;
        private Vector2 _binSize;
        private int _rowOffset = 0;
        private List<GuiControl> _allChildren;
        private float _scrollBarWidth;
        private VerticalScrollbarControl  _scrollBar;   
        public Vector2 ControlSize { get { return _binSize; } }


        public ScrollableGrid(int xBinNum, int yBinNum, Vector2 binSize, int spacing = 5, int padding = 15)
        {
            _spaceing = spacing;
            Padding = padding;
            _scrollBarWidth = 15;
            _xBinNum = xBinNum;
            _yBinNum = yBinNum;
            _binSize = binSize;
            base.HalfSize = CalculateSize() * 0.5f;
            _scrollBar = new VerticalScrollbarControl (new Vector2(_scrollBarWidth, this.Height - 2 * Padding),1);
            _scrollBar.ControlColor = Color.Black;
            _scrollBar.CursorOverColor = Color.Black;
            _scrollBar.LocalPosition = new Vector2(this.halfWidth - _scrollBar.HalfSize.X - Padding, 0);
            _allChildren = new List<GuiControl>();
            _scrollBar.Parent = this;
        }

        public override void RemoveAllChildren()
        {
            base.RemoveAllChildren();
            _scrollBar.Value = 0;
            _allChildren.Clear();
            _rowOffset = 0;
        }

        public override void AddChild(GuiControl guiController)
        {
            //add limit on the number of items you can add
            guiController.HalfSize = _binSize * 0.5f; //change  
            _allChildren.Add(guiController);
            if (_allChildren.Count <= _xBinNum * _yBinNum )
            {
                base.AddChild(guiController);
            }
            int index = _allChildren.Count-1;
            guiController.Index = index;
            int x = index % _xBinNum;
            int y = index / _xBinNum;
            guiController.LocalPosition = new Vector2(Padding*2 + (x + 0.5f) * (_binSize.X + _spaceing) - halfWidth - _scrollBarWidth
                , Padding + (y + 0.5f) * (_binSize.Y + _spaceing) - halfHeight);
            _scrollBar.MaxValue =  Math.Max((int)Math.Ceiling((_allChildren.Count) / (float)(_xBinNum) -_yBinNum),0);            
        }

        public override void RemoveChild(GuiControl guiController)
        {
            base.RemoveChild(guiController);
            _allChildren.Remove(guiController);
            _scrollBar.HasValueChanged = true;
        }

        private Vector2 CalculateSize()
        {
            return new Vector2(Padding * 3 + _xBinNum * (_spaceing + _binSize.X) + _scrollBarWidth
                , Padding * 2 + _yBinNum * (_spaceing + _binSize.Y));
        }

        public override void Update(InputState inputState)
        {
            _scrollBar.Update(inputState);
            base.Update(inputState);
        }

        public override void UpdateLogic(InputState inputState)
        {
            _scrollBar.Value -= Math.Sign(MouseUtils.Inst.GetDScroolWheel()); //TODO: use inputState

            if (_scrollBar.HasValueChanged)
            {
                _rowOffset = _scrollBar.Value;
                RowOffsetChanged();
            }
        }

        private void RowOffsetChanged()
        {
            children.Clear();
            for (int i = 0; i < _xBinNum * _yBinNum; i++)
            {
                int index = i;
                if (i + _rowOffset * _xBinNum < _allChildren.Count)
                {
                    GuiControl guiController = _allChildren[i + _rowOffset * _xBinNum];
                    base.AddChild(guiController);
                    int x = index % _xBinNum;
                    int y = index / _xBinNum;
                    guiController.LocalPosition = new Vector2(Padding + (x + 0.5f) * (_binSize.X + _spaceing) - halfWidth, Padding + (y + 0.5f) * (_binSize.Y + _spaceing) - halfHeight);
                }
            }                        
        }
        //setPadding(int, int, int, int)

        public override void Draw(SpriteBatch sb, Color? color = default(Color?))
        {
            base.Draw(sb, color);
            //if(_scrollBar.MaxValue > 0)
            _scrollBar.Draw(sb, color);
        }

        
    }
    
}


