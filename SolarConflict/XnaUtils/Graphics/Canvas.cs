using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace XnaUtils.Framework.Graphics
{
    /// <summary>
    /// This class enables painting on textures
    /// </summary>
    public class Canvas
    {
        [NonSerialized]
        private Texture2D texture;
        public Color[] buffer;

        private int width;
        public int Width
        {
            get { return width; }
        }

        private int height;
        public int Height
        {
            get { return height; }
        }

        private GraphicsDevice graphicsDevice;

        public Canvas(int width, int height, GraphicsDevice graphicsDevice)
        {
            this.width = width;
            this.height = height;
            this.graphicsDevice = graphicsDevice;
            buffer = new Color[width * height];
            texture = new Texture2D(graphicsDevice, width, height);
        }

        public Canvas(Texture2D texture):this(texture.Width, texture.Height, texture.GraphicsDevice)
        {
            this.texture = texture;            
            this.texture.GetData<Color>(buffer);
        }

        public void SetPixel(int x, int y, Color color)
        {
            buffer[x + y * width] = color;
        }

        public void SetPixelLimt(int x, int y, Color color)
        {
            if (x >= 0 && x < width && y >= 0 && y < height)
                buffer[x + y * width] = color;
        }

        public Color GetPixel(int x, int y)
        {
            return buffer[x + y * width];
        }

        public Color GetPixelLimit(int x, int y)
        {
            if (x >= 0 && x < width && y >= 0 && y < height)
                return buffer[x + y * width];
            else
                return Color.Transparent;
        }

        public void Clear(Color color)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = color;
            }
        }


        public void SetData()
        {
            graphicsDevice.Textures[0] = null;
            texture.SetData<Color>(buffer);
        }

        public Texture2D GetTexture()
        {
            return texture;
        }

        public void Save(string fileName)
        {
            Stream stream = File.OpenWrite(fileName);
            texture.SaveAsPng(stream, texture.Width, texture.Height);
            stream.Close();
        }



    }
}