using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.SimpleGui;

namespace SolarConflict.XnaUtils.SimpleGui
{
    /// <summary>
    /// Grid Control is a ControlGroup that displays items in a two-dimensional, scrollable grid.
    /// </summary>
    [Serializable]
    public class GridControl : GuiControl //TODO: add scrollable, (add a difrent control that is editable grid?)
    {
        private int _padding = 10; //cjange to up,down,left,right padding
        private int _spaceing = 5; //maybe vector2
        private int _xBinNum;
        private int _yBinNum;
        private Vector2 _binSize;

        public int Count { get { return _xBinNum* _yBinNum; } }

        public int Padding
        {
            get
            {
                return _padding;
            }

            set
            {
                _padding = value;
            }
        }

        public GridControl(int xBinNum, int yBinNum, Vector2 binSize)
        {
            _xBinNum = xBinNum;
            _yBinNum = yBinNum;
            _binSize = binSize;
            base.HalfSize = CalculateSize()*0.5f;
        }

        public override void AddChild(GuiControl guiController)
        {
            //add limit on the number of items you can add
            guiController.HalfSize = _binSize * 0.5f; //change             
            base.AddChild(guiController);
            int index = children.Count - 1;
            guiController.Index = index;
            int x = index % _xBinNum;
            int y = index / _xBinNum;
            guiController.LocalPosition = new Vector2(Padding + (x+0.5f) * (_binSize.X + _spaceing) - halfWidth, Padding + (y + 0.5f)* (_binSize.Y +_spaceing) - halfHeight);
        }

        private Vector2 CalculateSize()
        {
            return new Vector2(Padding * 2 + _xBinNum * (_spaceing + _binSize.X), Padding * 2 + _yBinNum * (_spaceing + _binSize.Y));
        }

        //setPadding(int, int, int, int)

    }
}
