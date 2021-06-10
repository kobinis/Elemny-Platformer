using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaUtils.SimpleGui
{
    //vertical layout  
    [Serializable]
    public class GuiLayout:GuiControl //change it
    {
        public float baseVertical = 20; //insted use position
        float horizontalPos = ActivityManager.ScreenRectangle.Width / 2; //change
        float spacing = 15;
        public bool AutoUpdatePos = false;
        //vertical layout                
        public GuiLayout(Vector2 position):base(position, Vector2.One)
        {            
        }

        public override void UpdateLogic(InputState inputState)
        {
            base.UpdateLogic(inputState);
            if (AutoUpdatePos)
            {
                float verticalPos = baseVertical;
                foreach (var item in children)
                {
                    item.LocalPosition = new Vector2(item.LocalPosition.X, verticalPos + item.HalfSize.Y);
                    verticalPos += item.Height + spacing;
                }
            }           
        }

        protected override void DrawLogic(SpriteBatch sb, Color? color = null)
        {
            //base.DrawLogic(sb, color); //add option to draw background
        }

        public override void AddChild(GuiControl guiController)
        {
            float verticalPos = baseVertical;
            foreach (var item in children)
            {
                verticalPos += item.Height + spacing;
            }            
            guiController.Position = new Vector2(guiController.Position.X, verticalPos + guiController.HalfSize.Y);
            base.AddChild(guiController);

        }
        

    }
}
