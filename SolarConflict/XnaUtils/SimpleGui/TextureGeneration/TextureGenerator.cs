using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SolarConflict.XnaUtils.SimpleGui.TextureGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;
using XnaUtils.Framework.Graphics;

namespace SolarConflict.XnaUtils.SimpleGui
{
    //TODO: read designs from files
    //Add design bank
   
    public enum FrameDesign { None, Normal, Smooth, Center}
    public enum TextureLayout { Warp, Reflect, Clamp }
   
    public class TextureGenerator
    {                
        public static Texture2D GenerateTexture(int width, int height, TextureDesign design)
        {
            //TODO: change all to vector 4
            Vector3 bodyColorVec = design.BodyColor.ToVector3();
            Vector3 frameColor = design.FrameColor.ToVector3();
            Canvas canvas = new Canvas(width, height, GraphicsSettingsUtils.GraphicsDevice);
            int offsetX = design.OffsetX;
            int offsetY = design.OffsetY;  
            int numberOfFrames = design.FrameSize;
            Vector3 baseColor = new Vector3( design.BaseBrightness);
            Vector3 textureVec = new Vector3(1);
            int cornerSize = Math.Min((int)(Math.Min(width, height) * 0.2f), 10);
            if (design.CornerSize != null)
                cornerSize = design.CornerSize.Value;
            for (int y = 0; y < canvas.Height; y++)
            {
                for (int x = 0; x < canvas.Width; x++)
                {
                    if (design.BackgroundTexture != null)
                    {
                        Color color = design.GetPixel(x - width / 2, y - height / 2);
                        textureVec = color.ToVector3();
                    }
                    
                    int frameIndex = Math.Min(Math.Min(x, y), Math.Min(canvas.Width - x - 1, canvas.Height - y - 1));
                    frameIndex = Math.Min(frameIndex, Math.Min(Math.Min(x + y - cornerSize, canvas.Height - y - 1 + x - cornerSize),
                            Math.Min(canvas.Width - x + canvas.Height - y - 2 - cornerSize, canvas.Width - x - 1 + y - cornerSize)));
                    if (frameIndex >= 0)
                    {
                        if (frameIndex < numberOfFrames) //TODO: maybe if frameIndex < 0 then transparent
                        {
                            float frameColorFactor = 1f - (frameIndex / (float)numberOfFrames);
                            float frameShade = 1;
                            switch (design.FrameDesign)
                            {
                                case FrameDesign.None:
                                    frameShade = 0;
                                    break;
                                case FrameDesign.Normal:
                                    frameShade = Math.Min(x, y) < Math.Min(canvas.Width - x, canvas.Height - y) ? -1 : 1;
                                    break;
                                case FrameDesign.Smooth:
                                    //throw new NotImplementedException();
                                    //KOBI: implement smooth
                                    break;
                                case FrameDesign.Center:
                                    frameShade = 1;
                                    break;
                                default:
                                    break;
                            }
                            Vector3 colorVec = (baseColor - Vector3.One * frameColorFactor * 0.4f * frameShade) * textureVec * frameColor;
                            Color color = new Color(colorVec);
                            if (design.FadeFrames > 0)
                            {
                                float alpha = Math.Min(frameIndex / (float)design.FadeFrames, 1);
                                color.A = (byte)(alpha * 255);
                            }
                            canvas.SetPixel(x, y, color);
                        }
                        else
                        {
                            Vector3 color = baseColor * textureVec * bodyColorVec;
                            canvas.SetPixel(x, y, new Color(color));
                        }
                    }
                    else
                    {

                    }

                }
            }
            canvas.SetData();
            return canvas.GetTexture();
        }

        //public static float FrameShade(float x, float y, FrameDesgin desgin)
        //{
        //    double deg;
        //    switch (desgin)
        //    {

        //        case FrameDesgin.Fade:
        //            deg = Math.Atan2(y, x) - Math.PI / 4.0;
        //            return (float)Math.Cos(deg);
        //        case FrameDesgin.Normal:
        //            return (float)FMath.DegSign((float)(Math.Atan2(y, x) + Math.PI / 4));
        //        case FrameDesgin.Centerd:
        //            return 1;
        //        case FrameDesgin.None:
        //            return 0;
        //        default:
        //            deg = Math.Atan2(y, x) - Math.PI / 4.0;
        //            return (float)Math.Cos(deg);
        //    }

        //}
    }
}
