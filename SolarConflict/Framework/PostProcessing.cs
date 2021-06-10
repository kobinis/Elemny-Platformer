using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils;
using XnaUtils.SimpleGui;
using System;

namespace SolarConflict
{
    static class PostProcessing
    {
        //Quad drawing
        private static VertexBuffer quadVertexBuffer;
        private static IndexBuffer quadIndexBuffer;

        private static short[] quadIndexData = new short[] { 0, 1, 2, 2, 3, 0 };
        private static VertexPositionTexture[] quadFacets;

        public static RenderTarget2D[] arrayRT;

        public static float[][][] kernelArray;

        public static Effect processingEffect;

        private static EffectParameter procBloomThreshold;
        private static EffectParameter procBloomBias;
        private static EffectParameter procViewport;
        private static EffectParameter procBloomSaturation;
        private static EffectParameter procBloomIntensity;
        private static EffectParameter procSampleCount;
        private static EffectParameter procWeights;
        private static EffectParameter procOffsets;

        public static void Init()
        {
            quadFacets = new VertexPositionTexture[]
            {
                          new VertexPositionTexture(new Vector3(1, -1, 0),
                                                    new Vector2(1,1)),
                          new VertexPositionTexture(new Vector3(-1, -1, 0),
                                                    new Vector2(0,1)),
                          new VertexPositionTexture(new Vector3(-1, 1, 0),
                                                    new Vector2(0,0)),
                          new VertexPositionTexture(new Vector3(1, 1, 0),
                                                    new Vector2(1,0))
            };
            quadVertexBuffer = new VertexBuffer(ActivityManager.GraphicsDevice, typeof(VertexPositionTexture), 4, BufferUsage.WriteOnly);
            quadVertexBuffer.SetData<VertexPositionTexture>(quadFacets);

            quadIndexBuffer = new IndexBuffer(ActivityManager.GraphicsDevice, IndexElementSize.SixteenBits, 6, BufferUsage.WriteOnly);
            quadIndexBuffer.SetData<short>(quadIndexData);

            arrayRT = new RenderTarget2D[6];

            int downSample;
            for (int i = 0; i < 3; i++)
            {
                downSample = (int)Math.Pow(2, i);
                arrayRT[i * 2] = new RenderTarget2D(ActivityManager.GraphicsDevice, ActivityManager.GraphicsDevice.Viewport.Width / downSample, ActivityManager.GraphicsDevice.Viewport.Height / downSample, false, SurfaceFormat.HalfVector4, DepthFormat.None);
                arrayRT[i * 2 + 1] = new RenderTarget2D(ActivityManager.GraphicsDevice, ActivityManager.GraphicsDevice.Viewport.Width / downSample, ActivityManager.GraphicsDevice.Viewport.Height / downSample, false, SurfaceFormat.HalfVector4, DepthFormat.None);
            }

            kernelArray = new float[3][][];

            int kernelRadius = 17;
            kernelArray[0] = createGauss1DKernel(kernelRadius, 0.3f * (kernelRadius / 2 - 1) + 0.8);
            kernelRadius = 25;
            kernelArray[1] = createGauss1DKernel(kernelRadius, 0.3f * (kernelRadius / 2 - 1) + 0.8);
            kernelRadius = 33;
            kernelArray[2] = createGauss1DKernel(kernelRadius, 0.3f * (kernelRadius / 2 - 1) + 0.8);

            procBloomThreshold = processingEffect.Parameters["BloomTreshold"];
            procBloomBias = processingEffect.Parameters["BloomBias"];
            procViewport = processingEffect.Parameters["viewport"];
            procBloomSaturation = processingEffect.Parameters["BloomSaturation"];
            procBloomIntensity = processingEffect.Parameters["BloomInstensity"];

            procSampleCount = processingEffect.Parameters["sampleCount"];
            procWeights = processingEffect.Parameters["weight"];
            procOffsets = processingEffect.Parameters["offset"];

            processingEffect.Parameters["bloomTex1"].SetValue(arrayRT[0]);
            processingEffect.Parameters["bloomTex2"].SetValue(arrayRT[2]);
            processingEffect.Parameters["bloomTex3"].SetValue(arrayRT[4]);
        }


        //resize all RTs
        public static void Resize()
        {
            int downSample;
            for (int i = 0; i < 3; i++)
            {
                if (arrayRT[i * 2] != null)
                    arrayRT[i * 2].Dispose();

                if (arrayRT[i * 2 + 1] != null)
                    arrayRT[i * 2 + 1].Dispose();

                downSample = (int)Math.Pow(2, i);
                arrayRT[i * 2] = new RenderTarget2D(ActivityManager.GraphicsDevice, ActivityManager.GraphicsDevice.Viewport.Width / downSample, ActivityManager.GraphicsDevice.Viewport.Height / downSample, false, SurfaceFormat.HalfVector4, DepthFormat.None);
                arrayRT[i * 2 + 1] = new RenderTarget2D(ActivityManager.GraphicsDevice, ActivityManager.GraphicsDevice.Viewport.Width / downSample, ActivityManager.GraphicsDevice.Viewport.Height / downSample, false, SurfaceFormat.HalfVector4, DepthFormat.None);
            }
        }

        // Draw full screen quad into specified target
        public static void DrawFullscreenQuad(RenderTarget2D inputRT, RenderTarget2D outputRT, Effect effect)
        {

            ActivityManager.GraphicsDevice.SetRenderTarget(outputRT);
            ActivityManager.GraphicsDevice.BlendState = BlendState.Opaque;
            FlushFullQuad(effect, inputRT);
        }

        public static void Apply(RenderTarget2D inputRT)
        {
            //HDR three phase bloom
            procBloomThreshold.SetValue(0.7f);//0.8f
            procBloomSaturation.SetValue(1.0f);
            procBloomIntensity.SetValue(0.7f);

            //Extract input into downscaled RTs
            processingEffect.CurrentTechnique = processingEffect.Techniques["ExtractBloom"];
            for (int i = 0; i < 3; i++)
            {
                procViewport.SetValue(new Vector2(arrayRT[i * 2].Width, arrayRT[i * 2].Height));
                DrawFullscreenQuad(inputRT, arrayRT[i * 2], processingEffect);
            }
            
            //Blur all extraction RTs horizontally
            processingEffect.CurrentTechnique = processingEffect.Techniques["HorizontalBlur"];
            for (int i = 0; i < 3; i++)
            {
                procSampleCount.SetValue(kernelArray[i][0].Length);
                procOffsets.SetValue(kernelArray[i][1]);
                procWeights.SetValue(kernelArray[i][0]);

                procViewport.SetValue(new Vector2(arrayRT[i * 2].Width, arrayRT[i * 2].Height));
                DrawFullscreenQuad(arrayRT[i * 2], arrayRT[i * 2 + 1], processingEffect);
            }
            
            //Blur all extractions RTs vertically
            processingEffect.CurrentTechnique = processingEffect.Techniques["VerticalBlur"];
            for (int i = 0; i < 3; i++)
            {
                procSampleCount.SetValue(kernelArray[i][0].Length);
                procOffsets.SetValue(kernelArray[i][1]);
                procWeights.SetValue(kernelArray[i][0]);

                procViewport.SetValue(new Vector2(arrayRT[i * 2].Width, arrayRT[i * 2].Height));
                DrawFullscreenQuad(arrayRT[i * 2 + 1], arrayRT[i * 2], processingEffect);
            }


            //Combine HDR with input and render into backbuffer
            processingEffect.CurrentTechnique = processingEffect.Techniques["CombineBloom"];
            DrawFullscreenQuad(inputRT, null, processingEffect);
        }

        // Helper function to render high performance full screen quad using custom vertex shader
        static public void FlushFullQuad(Effect effect, Texture2D texture)
        {
            /*
            if (effect == null) //default shader, generally pass through
            {
                effect = combineMat.shader;
                effect.CurrentTechnique = combineMat.passThrough;
            }*/

            var passes = effect.CurrentTechnique.Passes;
            foreach (var pass in passes)
            {
                pass.Apply();
                ActivityManager.GraphicsDevice.Textures[0] = texture;


                ActivityManager.GraphicsDevice.SetVertexBuffer(quadVertexBuffer);
                ActivityManager.GraphicsDevice.Indices = quadIndexBuffer;
                ActivityManager.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 2);
            }

        }


        // Function to create gauss kernel that uses linear sampling rules for optimization, this is run only once during inicialization
        static private float[][] createGauss1DKernel(int radius, double weight)
        { 
            double[] Kernel = new double[radius];
            int center = radius / 2 + 1;
            double[] halfKernel = new double[center];
            double sumTotal = 0;

            int kernelRadius = radius / 2;
            double distance = 0;
            double calculatedEuler = 1.0 /
            (2.0 * Math.PI * Math.Pow(weight, 2));


            for (int filterX = -kernelRadius;
                filterX <= kernelRadius; filterX++)
            {
                distance = ((filterX * filterX) / (2 * (weight * weight)));
                Kernel[filterX + kernelRadius] = calculatedEuler * Math.Exp(-distance);
                sumTotal += Kernel[filterX + kernelRadius];
            }

            for (int x = 0; x <= radius / 2; x++)
            {
                halfKernel[x] = Kernel[x + center - 1] * (1.0 / sumTotal);
            }

            return CalculateLinearGaussV2(halfKernel);
        }

        private static float[][] CalculateLinearGaussV2(double[] weightsN)
        {
            float[] offsetsN = new float[weightsN.Length];
            float[] offsetsL = new float[(weightsN.Length + 1) / 2];
            float[][] output = CreateJaggedArray<float[][]>(2, (weightsN.Length + 1) / 2);
            for (int i = 0; i < weightsN.Length; i++)
            {
                offsetsN[i] = i;
            }

            float[] weightsL = new float[(weightsN.Length + 1) / 2];
            weightsL[0] = (float)weightsN[0];
            offsetsL[0] = 0;
            for (int i = 1; i < weightsL.Length; i++)
            {
                weightsL[i] = (float)(weightsN[(i * 2) - 1] + weightsN[i * 2]);
            }

            for (int i = 1; i < offsetsL.Length; i++)
            {
                offsetsL[i] = (float)((offsetsN[(i * 2) - 1] * weightsN[(i * 2) - 1] + offsetsN[i * 2] * weightsN[i * 2]) / weightsL[i]);
            }
            output[0] = weightsL;
            output[1] = offsetsL;
            return output;
        }

        private static T CreateJaggedArray<T>(params int[] lengths)
        {
            return (T)InitializeJaggedArray(typeof(T).GetElementType(), 0, lengths);
        }

        private static object InitializeJaggedArray(Type type, int index, int[] lengths)
        {
            Array array = Array.CreateInstance(type, lengths[index]);
            Type elementType = type.GetElementType();

            if (elementType != null)
            {
                for (int i = 0; i < lengths[index]; i++)
                {
                    array.SetValue(
                        InitializeJaggedArray(elementType, index + 1, lengths), i);
                }
            }

            return array;
        }
    }


}
