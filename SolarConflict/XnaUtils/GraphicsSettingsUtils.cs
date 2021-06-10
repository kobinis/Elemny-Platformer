using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XnaUtils;
using XnaUtils.Framework.Graphics;
using XnaUtils.SimpleGui;

namespace SolarConflict.XnaUtils
{
    /// <summary>
    /// Helps with borderless window and other graphics settings
    /// </summary>
    static class GraphicsSettingsUtils
    {
        public static GraphicsDeviceManager GraphicsDeviceManager { get; private set; }
        public static GraphicsDevice GraphicsDevice { get { return GraphicsDeviceManager.GraphicsDevice; } private set { } }
        public static RenderTarget2D renderTargetFullA;        

        /// <summary>
        /// InitGame sets GraphicsDeviceManager and Game in ActivityManager, to be called in game constructor
        /// </summary>
        public static void InitStatic(GraphicsDeviceManager gdm)
        {
            if (gdm == null)
                throw new Exception("InitGame parameters can't be null");
            if (GraphicsDeviceManager != null)
                throw new Exception("InitGame cannot be called twice");
            GraphicsDeviceManager = gdm;            
          //  UpdateScreenSizeFields();
        }

        public static void SetResolution()
        {
            GraphicsDeviceManager.PreferredBackBufferWidth = GraphicsSettings.ResWidth;
            GraphicsDeviceManager.PreferredBackBufferHeight = GraphicsSettings.ResHeight;
            GraphicsDeviceManager.ApplyChanges();
            UpdateScreenSizeFields();
            //SetWindowPosition();
        }


        public static void Borderless(Game game)
        {
            
            IntPtr hWnd = game.Window.Handle;
            var control = System.Windows.Forms.Control.FromHandle(hWnd);
            var form = control.FindForm();            
            if (GraphicsSettings.IsBorderless)
                form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            else
                form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            game.Window.Position = new Point(0, 0);
            //form.StartPosition = System.Windows.Forms.FormStartPosition.
        }

        public static void SetWindowPosition()
        {
            //var windowHandle = Game.Window.Handle;
            //System.Windows.Forms.Control control = System.Windows.Forms.Control.FromHandle(windowHandle);
            //System.Windows.Forms.Form form = control.FindForm();
            //var screen = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
            // form.DesktopLocation = new System.Drawing.Point(screen.Width / 2 - GraphicsSettings.ResWidth / 2, screen.Height / 2 - GraphicsSettings.ResHeight / 2);
        }


        public static void UpdateScreenSizeFields()
        {
            int ScreenWidth = GraphicsDevice.Viewport.Width; //GraphicsSettings.ResWidth;
            int ScreenHeight = GraphicsDevice.Viewport.Height;            
            if (renderTargetFullA != null)
            {
                renderTargetFullA.Dispose();
            }
            renderTargetFullA = new RenderTarget2D(GraphicsDevice, ScreenWidth, ScreenHeight, false, SurfaceFormat.HalfVector4, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);
            ActivityManager.UpdateScreenSizeFields();
        }       


    }
}
