using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict.XnaUtils.SimpleGui.TextureGeneration
{
    /// <summary>
    /// This class hold the design of a texture frame
    /// </summary>
    public class TextureDesign
    {
        public int Id { get; private set; }
        public FrameDesign FrameDesign { get; set; }
        public int FrameSize { get; set; }
        public Color[,] BackgroundTexture { get; private set; }
        public int OffsetX { get; set; }
        public int OffsetY { get; set; }
        public TextureLayout TextureLayout { get; set; }
        public float BaseBrightness { get; set; }
        public int FadeFrames { get; set; }
        public Color FrameColor { get; set; }
        public Color BodyColor { set; get; }
        public int? CornerSize { get; set; }

        public TextureDesign(int id) :this()
        {
            Id = id;
        }

        public TextureDesign()
        {
            BaseBrightness = 0.8f;
        }
            
        public void SetBackgroundTexture(Texture2D texture) //TODO: maybe get a string
        {
            Color[] colorBuffer = new Color[texture.Width * texture.Height]; //TODO: check if can work with null
            texture.GetData<Color>(colorBuffer);
            BackgroundTexture = new Color[texture.Width, texture.Height];
            for (int i = 0; i < colorBuffer.Length; i++)
            {
                BackgroundTexture[i % texture.Width, i / texture.Width] = colorBuffer[i];
            }
            OffsetX = texture.Width / 2;
            OffsetY = texture.Height / 2;
            FrameColor = Color.White;
            BodyColor = Color.White;
        }

        public Color GetPixel(int x, int y) //TODO: maybe move
        {
            int indexX = x + OffsetX;
            int indexY = y + OffsetY;
            switch (TextureLayout)
            {
                case TextureLayout.Warp:
                    indexX = FMath.Mod(indexX, BackgroundTexture.GetLength(0));
                    indexY = FMath.Mod(indexY, BackgroundTexture.GetLength(1));
                    break;
                case TextureLayout.Reflect:
                    indexX = FMath.Zigzag(indexX, BackgroundTexture.GetLength(0) -1);
                    indexY = FMath.Zigzag(indexY, BackgroundTexture.GetLength(1) -1);
                    break;
                case TextureLayout.Clamp:
                    indexX = Math.Max(Math.Min(indexX, BackgroundTexture.GetLength(0) - 1), 0);
                    indexY = Math.Max(Math.Min(indexY, BackgroundTexture.GetLength(1) - 1), 0);
                    break;
            }
            return BackgroundTexture[indexX, indexY];
        }
    }
}
