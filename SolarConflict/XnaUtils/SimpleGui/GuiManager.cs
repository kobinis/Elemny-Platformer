using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils.Input;
using XnaUtils.Framework.Graphics;
using XnaUtils.SimpleGui.Controllers;
using SolarConflict.XnaUtils.SimpleGui.TextureGeneration;
using SolarConflict.XnaUtils;

using SolarConflict;
using XnaUtils.Graphics;

namespace XnaUtils.SimpleGui
{
    [Serializable]
    public class GuiManager
    {        
        private const int TOOLTIP_DELAY_FRAMES = 20;
        public const float DisableShade = 0.4f;
        public static float Scale = 1f;

        private struct GuiGroupData
        {
            public int UseTime;
            public bool IsActive;
            public bool IsBlocking;
            public bool MoveForwardOnUse;
        }

        #region Back Texure
        public static Sprite BackTexture { get; set; }
        public static Sprite ScrollTexture { get; set; }

        public static Texture2D GetTexture()
        {
            return null;
        }

        #endregion

        public static Color DefalutGuiColor = new Color(20, 100, 90);  //new Color(0, 224, 216); //new Color(229, 108, 37);


        private SpriteBatch _spriteBatch => Game1.sb;
        private RichTextControl _tooltip;
        private int drawTooltip;
        // Camera _camera; //TODO:

        private List<GuiControl> _controlsList; //TODO: add time used to enable GUI covering 

        public GuiControl Root
        {
            get {
                if (_controlsList.Count < 1)
                    return null;
                return _controlsList[0];
            }
            set
            {
                if (_controlsList.Count < 1)
                {
                    _controlsList.Add(value);
                }
                else
                {
                    _controlsList[0] = value;
                }
            }
        }
                
      //  public bool IsActive = true; //remove

        
        private static Sprite backTexture;
                       
        public GuiManager()
        {
            _controlsList = new List<GuiControl>();
            _tooltip = new RichTextControl("", FontBank.DefaultFont);
            _tooltip.ControlColor = new Color(20, 20, 160, 100);
            _tooltip.IsShowFrame = true;
            _tooltip.MaxLineWidth = ActivityManager.ScreenSize.X * 0.95f;
        }

        public void RemoveControl(GuiControl control)
        {
            _controlsList.Remove(control);
        }

        public void AddControl(GuiControl control)
        {
            _controlsList.Add(control);
        }

        public void Update(InputState inputState)
        {
            drawTooltip = Math.Max(drawTooltip - 1, 0);
            for (int i = _controlsList.Count-1; i >= 0; i--)
            {
                _controlsList[i].Update(inputState);
            }            
            if (drawTooltip >= TOOLTIP_DELAY_FRAMES)
            {
                _tooltip.Update(InputState.EmptyState);
            }
            
        }

        public void Draw(Color? color = null)
        {
            //  _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.CreateScale(scale));
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            foreach (var control in _controlsList)
            {
                control.Draw(_spriteBatch, color);
            }

            if (drawTooltip >= TOOLTIP_DELAY_FRAMES)
            {
                _tooltip.Draw(_spriteBatch);
            }
            _spriteBatch.End();
        }

        

       public void ToolTipHandler(GuiControl source, CursorInfo cursorLocation)
       {
           float offset = 10;
            string tooltipText = source.GetTooltipText();
           if(!string.IsNullOrEmpty(tooltipText))
           {
                drawTooltip = Math.Min(drawTooltip + 2, TOOLTIP_DELAY_FRAMES);
                _tooltip.ControlColor = new Color(40, 60, 150, 220);
                _tooltip.Text = tooltipText;                
                _tooltip.Position  = _tooltip.HalfSize + cursorLocation.Position + Vector2.One * offset;
                
                _tooltip.FitToScreen();
            }
       }

       
        public void ScaleGui(float scale)
        {
            foreach (var item in _controlsList)
            {
                item.HalfSize = item.HalfSize * scale;
                ScaleChildren(item,scale);
            }
        }

        public static void ScaleChildren(GuiControl gui, float scale)
        {
            foreach (var item in gui.GetChildren())
            {
                item.HalfSize = item.HalfSize * scale;
                ScaleChildren(item, scale);
            }
        }
        
    }
}
