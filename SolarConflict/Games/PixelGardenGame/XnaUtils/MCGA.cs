using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;

namespace PaintPlay
{
    /// <summary>
    /// Simulates an raster screen with a platte
    /// </summary>
    public class MCGA
    {
        GraphicsDevice gd;
        SpriteBatch sb;
        int sWidth, sHeight;
        public Texture2D mcgaTexture;
        Color[] screen; //color        
        Rectangle screenRect;

        double xRatio, yRatio;


        public MCGA(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, int width, int height)
        {
            gd = graphicsDevice;
            sb = spriteBatch;
            sWidth = width;
            sHeight = height;
            mcgaTexture = new Texture2D(gd, sWidth, sHeight);
            screen = new Color[sWidth * sHeight];

            screenRect = new Rectangle(0, 0, gd.Viewport.Width, gd.Viewport.Height);

            xRatio = (double)sWidth / screenRect.Width; //not good
            yRatio = (double)sHeight / screenRect.Height;     
        }

        public MCGA(int width, int height)
            : this(MyGraphics.gdm.GraphicsDevice, MyGraphics.sb, width, height)
        {
        }

        public void SetData(Texture2D data)
        {
            data.GetData<Color>(screen);
        }

        public void Load(String filename)
        {
            this.mcgaTexture = FileUtils.LoadTexture(filename);

        //        int sWidth, sHeight;
                //public Texture2D mcgaTexture;
            mcgaTexture.GetData<Color>(screen);       
            
        }

        public void Save(String filename)
        {
            FileUtils.SaveTexture(mcgaTexture, filename);
        }

        


        /// <summary>
        /// clears the mcga "screen"
        /// </summary>
        /// <param name="color"></param>
        public void Cls(Color color)
        {          
            //Array.Clear
            for (int i = 0; i < screen.Length; i++)
            {
                screen[i] = color;
            }
        }


        public void Putpixel(int x, int y, Color color)
        {
            screen[x + y * sWidth] = color;//palette[color];
        }

        public void PutpixelOn(int x, int y, Color color)
        {
            if (x < 0 || x >= sWidth || y < 0 || y >= sHeight)
                return; //check
            int index = x + y * sWidth;
            Color temp = screen[index];                       
            float alpha = (float)color.A / 255f;
            temp.R = (byte)(temp.R * (1 - alpha) + color.R * alpha);
            temp.G = (byte)(temp.G * (1 - alpha) + color.G * alpha);
            temp.B = (byte)(temp.B * (1 - alpha) + color.B * alpha);           
            screen[index] = temp;                                  
        }



        public void Limpixel(int x, int y, Color color)
        {
            if (x < 0 || x >= sWidth || y < 0 || y >= sHeight)
                return; 
            screen[x + y * sWidth] = color;
        }

        public Color Getpixel(int x, int y)
        {
            if (x < 0 || x >= sWidth || y < 0 || y >= sHeight)
                return Color.Black;
            return screen[x + y * sWidth];
        }

        public void Circle(int x, int y, double rad, Color color) 
        {
            double dotNum = 7 * rad;
            double deg;
            for (int i = 0; i < dotNum; i++)
            {
                deg = i / (double)dotNum * Math.PI * 2;
                Limpixel((int)Math.Round(x + Math.Cos(deg) * rad), (int)Math.Round(y + Math.Sin(deg) * rad), color);
            }
        }

        public void Line(int x1, int y1, int x2, int y2, Color color)
        {            
            int x, y; //implement bresenham algo, and replace function with my graphics function
            int deltaX = Math.Abs(x1 - x2);
            int deltaY = Math.Abs(y1 - y2);

            int N = Math.Max(deltaX, deltaY);

            float dx = (float)(x2 - x1) / N;
            float dy = (float)(y2 - y1) / N;

            for (int i = 0; i <= N; i++)
            {
                x = (int)Math.Round(x1 + i * dx); //can be replaced by addtions
                y = (int)Math.Round(y1 + i * dy);
                Limpixel(x, y, color);
            }

        }

        public void LineOn(int x1, int y1, int x2, int y2, Color color) //change to
        {
            int x, y;
            int deltaX = Math.Abs(x1 - x2);
            int deltaY = Math.Abs(y1 - y2);

            int N = Math.Max(deltaX, deltaY);

            float dx = (float)(x2 - x1) / N;
            float dy = (float)(y2 - y1) / N;

            for (int i = 0; i <= N; i++)
            {
                x = (int)Math.Round(x1 + i * dx); //can be replaced by addtions
                y = (int)Math.Round(y1 + i * dy);
                PutpixelOn(x, y, color);
            }

        }


        public void SetData()
        {
            gd.Textures[0] = null;
            mcgaTexture.SetData<Color>(screen);
        }

        public void SetDataFromFile(Color[] loadedScreen) // set texture data that was retrieved from a file
        {
            gd.Textures[0] = null;
            screen = loadedScreen;
            mcgaTexture.SetData<Color>(screen);
        }

        public void Draw()
        {        
            sb.Draw(mcgaTexture, screenRect, Color.White);            
        }

        public void Draw(Color color)
        {
            sb.Draw(mcgaTexture, screenRect, color);
        }


        public void LoadPal(string fullFilePath)
        {
            // this method is limited to 2^32 byte files (4.2 GB)

            FileStream fs = File.OpenRead(fullFilePath);
            try
            {
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
                //fs.Close();

                for (int i = 0; i < fs.Length; i++)
                {
                    // palette[i] = Color.f
                }
                //return bytes;
            }
            finally
            {
                fs.Close();
            }

        }



        /****************************************************************************************************/

        public Vector2 McgaToScreen(int x, int y)
        {
            return new Vector2(x * screenRect.Width / sWidth, y * screenRect.Height / sHeight);
        }

        public int GetMcgaX(int screenX)
        {

            return (screenX * sWidth / screenRect.Width);
        }

        public int GetMcgaY(int screenY)
        {
            return (screenY * sHeight / screenRect.Height);
        }

        public int Width
        {
            get
            {
                return sWidth;
            }           
        }

        public int Height
        {
            get
            {
                return sHeight;
            }
        }


    }
}
