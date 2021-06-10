using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PaintPlay;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PaintPlay.XnaUtils.Input;

namespace PaintPlay.XnaUtils.MyGui
{
    class GuiControl
    {
        protected Norma norma;
        protected int radX;
        protected int radY;        

        public Color ControlColor {set; get;}
        public Color PressedControlColor { set; get; }
        public Vector2 Position {set; get;}

        public bool IsInputOn {set; get;}//needs to be replaced into a single struct or class
        public TouchState InputState { set; get; }//needs to be replaced into a single struct or class

        public Icon icon;

        protected bool isPressed;

        protected Texture2D texture; //pressedTexture;   //no need in base class   

        public string helpText = null;

        public GuiControl()
        {
            /*this.radX = radX;
            this.radY = radY;
            this.norma = norma;*/
            isPressed = false;
            Position = new Vector2();
            ControlColor = new Color(1f, 1f, 1f, 0.6f);
            PressedControlColor = new Color(0.95f, 0.95f, 0.95f, 0.6f);
            IsInputOn = false;
            icon = null;
            
            //InputState = new TouchState();
            //texture = null;

        }

        public void AddIcon(Texture2D texture)
        {
            icon = new Icon(texture, radX, radY,0.8f);
        }
        
        public virtual bool IsPositionOnControl(Vector2 inputPos)
        {
            return norma((inputPos.X - Position.X) / (float)radX, (inputPos.Y - Position.Y) / (float)radY) <= 1;
        }
       
        

        public virtual void Update(Gui gui,List<TouchState> inputs) //add gui
        {
            IsInputOn = false;
            foreach (TouchState state in inputs)
            {
                if (IsPositionOnControl(state.Position - gui.Position))
                {
                    InputState = state;
                    inputs.Remove(state);
                    IsInputOn = true;
                    break;
                }
            }

        }

        public virtual bool IsPressed()
        {
            return isPressed ;
        }

        public virtual void Press()
        {
            isPressed = true;
        }

        public virtual void UnPress()
        {
            isPressed = false;
        }

        public virtual int GetValue()
        {
            return 0;
        }

        public virtual int GetGroup()
        {
            return 0;
        }

        public virtual void Draw(Gui gui) //very bad
        {
            Vector2 origin = new Vector2(radX, radY);
            if (IsPressed())
            {
                MyGraphics.sb.Draw(texture, Position + gui.Position, null, PressedControlColor, (float)Math.PI, origin, 1, SpriteEffects.None, 0);
            }
            else
            {
                MyGraphics.sb.Draw(texture, Position - origin + gui.Position, ControlColor);
            }

            if (icon != null)
                icon.Draw(Position + gui.Position);
                
            
        }

        public virtual void Draw(Gui gui, Vector2 position)
        {
            Vector2 origin = new Vector2(radX, radY);
            if (IsPressed())
            {
                MyGraphics.sb.Draw(texture, position, null, PressedControlColor, (float)Math.PI, origin, 1, SpriteEffects.None, 0);
            }
            else
            {
                MyGraphics.sb.Draw(texture, position - origin, ControlColor);
            }

            if(icon != null)          
                icon.Draw(position);
               


        }
    
    }
}
