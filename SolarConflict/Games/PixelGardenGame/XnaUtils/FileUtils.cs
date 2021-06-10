using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace PaintPlay
{
    class FileUtils
    {
        
            public static void SaveTexture(Texture2D texture, string fileName)
            {
                Stream myStream = File.OpenWrite(fileName);
                texture.SaveAsPng(myStream, texture.Width, texture.Height);
                myStream.Close();
            }

            public static Texture2D LoadTexture(string fileName)
            {                
                Stream myStream = File.OpenRead(fileName);
                Texture2D loadedTexture = Texture2D.FromStream(MyGraphics.gdm.GraphicsDevice, myStream);                                             
                myStream.Close();
                return loadedTexture;
            }

            public static void SaveByte(byte[] data, string fileName)
            {
                Stream myStream = File.OpenWrite(fileName);
                myStream.Write(data, 0, data.Length);
                myStream.Close();
            }

            public static byte[] LoadByte(string fileName)
            {                
                FileStream myStream = File.OpenRead(fileName);
                byte[] data = new byte[myStream.Length];
                myStream.Read(data, 0, data.Length);
                myStream.Close();
                return data;
            }
        
    }
}
