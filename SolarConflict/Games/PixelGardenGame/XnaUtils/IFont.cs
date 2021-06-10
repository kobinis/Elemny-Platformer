using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PaintPlay.XnaUtils
{
    enum FontAlligment {Left, Right, Middle}
    interface IFont
    {
        void DrawFont(String text, Vector2 position, Color color);
        void DrawFont(String text, Vector2 position, float scale,  Color color);
        void DrawFont(String text, Vector2 position, float scale, FontAlligment alligment, Color color);
        void FrawFont(String text, Vector2 position, float scale, FontAlligment alligment,float spaceing, Color color);

    }
}
