using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace XnaUtils.Graphics {
    /// <summary>Some stuff for masked drawing via the stencil buffer. Blatantly unoptimized.</summary>
    public class Masking {

        public static AlphaTestEffect AlphaEffect {
            get {
                var graphicsDevice = ActivityManager.GraphicsDevice;

                return new AlphaTestEffect(graphicsDevice) {
                    Projection = Matrix.CreateOrthographicOffCenter(0,
                 graphicsDevice.PresentationParameters.BackBufferWidth,
                 graphicsDevice.PresentationParameters.BackBufferHeight,
                 0, 0, 1)
                };
            }
        }

        /// <summary>Stencil state for drawing to the stencil buffer</summary>
        /// <remarks>Will set the stencil buffer to the given value for each pixel to which we draw. Will also draw to the current render target; this is seemingly unavoidable. Can't write to the stencil buffer
        /// without changing the values of the corresponding render target pixels, and can't retain stencil state when changing render targets. I suppose you could minimize this side effect by
        /// using an additive BlendState and drawing everything in a barely-perceptible color (unitary RGB), but that's really ugly.</remarks>
        public static DepthStencilState DrawToStencil(int value = 1) {
            return new DepthStencilState {
                StencilEnable = true,
                StencilFunction = CompareFunction.Always,
                StencilPass = StencilOperation.Replace,
                ReferenceStencil = value,
                DepthBufferEnable = false,
            };
        }

        /// <param name="spriteBatch">Must not be mid-operation (between Begin() and End() calls)</param>
        /// <warning>Swaps render targets, therefore clears the back buffer. Render target will be null on completion.</warning>                
        public static RenderTarget2D Invert(RenderTarget2D mask, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice = null) {
            graphicsDevice = graphicsDevice ?? ActivityManager.GraphicsDevice;

            System.Diagnostics.Debug.Assert(graphicsDevice.GetRenderTargets().Count() == 0); // potentially overzealous safeguard

            var whitePixel = new RenderTarget2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color, DepthFormat.None);
            var result = new RenderTarget2D(graphicsDevice, mask.Width, mask.Height, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
            // TODO: might wanna optimize via color formats, or by making the result an out parameter. Profile first.

            // Create a 1x1 white texture. We'll use this as an alternative to GraphicsDevice.Clear() that doesn't also clear the stencil buffer
            graphicsDevice.SetRenderTarget(whitePixel);
            graphicsDevice.Clear(Color.White);

            // Copy the mask, drawing to the stencil buffer as we do so
            graphicsDevice.SetRenderTarget(result);
            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, DrawToStencil(), null, AlphaEffect);
            spriteBatch.Draw(mask, Vector2.Zero, Color.White);
            spriteBatch.End();

            var wholeRect = new Rectangle(0, 0, mask.Width, mask.Height);

            // Blank out the copy (retain only the stencil data and a white image)
            spriteBatch.Begin();
            spriteBatch.Draw(whitePixel, wholeRect, Color.White);
            spriteBatch.End();
            // Using the mask, blit a big fat nothing onto the copy. Inversion complete.
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, null, MaskWithStencil(), null, null);
            spriteBatch.Draw(whitePixel, wholeRect, Color.Transparent);
            spriteBatch.End();

            graphicsDevice.SetRenderTarget(null);

            return result;
        }

        /// <summary>Stencil state for masked drawing</summary>
        /// <remarks>Will only draw to those pixels that have a stencil value matching the given value, and will not modify the stencil.</remarks>
        public static DepthStencilState MaskWithStencil(int value = 1) {
            return new DepthStencilState {
                StencilEnable = true,
                StencilFunction = CompareFunction.Equal,
                StencilPass = StencilOperation.Keep,
                ReferenceStencil = value,
                DepthBufferEnable = false,
            };
        }
    }
}
