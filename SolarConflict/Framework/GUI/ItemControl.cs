using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SolarConflict.Framework;
using SolarConflict.XnaUtils.SimpleGui.TextureGeneration;
using System;
using System.Collections.Generic;
using XnaUtils;
using XnaUtils.Graphics;
using XnaUtils.SimpleGui;

namespace SolarConflict
{
    [Serializable]
    public class ItemControl : GuiControl
    {
        public static Texture2D highlight = TextureBank.Inst.GetTexture("HighlightGreen");
        protected Item _item;
        public Color HighlightColor;
        public float PriceMult;
        public bool IsDisabled { get; set; }
        private bool isTooltipRefreshed;
        public string EmptyText;
        public List<Inventory> InventoryList;
        

        public int BuyPrice
        {
            get
            {
                if (_item == null)
                    return 0;
                return (int)Math.Round(_item.Profile.BuyPrice * PriceMult);
            }
        }
      
        public int TotalSellPrice
        {
            get
            {
                if (_item == null)
                    return 0;
                return _item.Stack * (int)Math.Round(_item.Profile.SellPrice * PriceMult);
            }
        }


        public void SetItem(Item item, float priceMult = 1)
        {
            PriceMult = priceMult;
            _item = item;
            if(_item == null ||  _item.Stack ==0)
            {
                TooltipText = EmptyText;
            }
        }

        public Item GetItem()
        {
            return Item;            
        }

        public Item Item
        {
            get { return _item; }  
            set
            {
                _item = value;
                RefreshTooltip();
            }        
        }

       public ItemControl(Item item, Vector2 pos, Vector2 size, float priceMult = 1):base(pos, size)
        {
          //  TextureDesign = (GuiDesign)GameGuiDesign.ItemControl;
            ControlColor = new Color(60, 70, 60, 200);
            PriceMult = priceMult;
            Item = item;                        
        //    image = new ImageControl(texture, Vector2.Zero, size * 0.9f);
            //image.DepthOffset = 0.1f;
            //this.AddChild(image);
 

            this.CursorOverColor = new Color(240, 240, 20, 200);
            /*  this.ControlColor = new Color(100, 100, 240, 255);
              this.PressedControlColor = ControlColor;*/
            UserData = string.Empty;
            HighlightColor = Color.Transparent;
        }

        private void RefreshTooltip()
        {
            if (!isTooltipRefreshed)
            {
                if (_item == null || _item.Stack == 0)
                {
                    TooltipText = EmptyText;// string.Empty;
                }
                else
                {
                    TooltipText = Item.GetTooltipText(priceMult: PriceMult);
                    if (DebugUtils.ShowItemID)
                        TooltipText +="\nID: "+ Item.ID;
                }
            }
            isTooltipRefreshed = true;
        }

        public bool AddItem(Item newItem)
        {
            if(Item.CanStack(Item, newItem.ID, newItem.Stack))
            {
                if (Item == null)
                    Item = newItem;
                else
                {
                    Item.Stack += newItem.Stack;
                    RefreshTooltip();
                }
            }
            return false;
        }

        public override void UpdateLogic(InputState inputState)
        {
            isTooltipRefreshed = false;           
        }

        public override string GetTooltipText()
        {
            RefreshTooltip();
            return TooltipText;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="color"></param>
        protected virtual void DrawItem(SpriteBatch sb)
        {
            if (_item != null && _item.Stack> 0)
            {
                Color itemColor = Color.White;

                if (IsDisabled)
                {
                    itemColor = new Color(50, 50, 50);
                }

                //  image.Position = this.Position;
                //   image.Draw(sb);
                Rectangle rectangle = new Rectangle((int)(Position.X ), (int)(Position.Y ), (int)Width, (int)Height);                
                _item.DrawItemIcon(sb, rectangle, itemColor);

                if (_item.IsStackable && _item.Stack > 1)
                {
                    SpriteFont font = Game1.font; //TODO:FromFont Bank, maybe IFont
                    Vector2 pos = Position + new Vector2(halfWidth, halfHeight)*0.8f;

                    string text = _item.Stack.ToString();
                    Vector2 size = font.MeasureString(text);
                    sb.DrawString(font, text, pos - new Vector2(size.X, size.Y * 0.5f) - Vector2.One, Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                    sb.DrawString(font, text, pos - new Vector2(size.X, size.Y*0.5f), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);                                        
                }  
             }          
        }

        protected override void DrawLogic(SpriteBatch sb, Color? color = null)
        {
            base.DrawLogic(sb, color);
            DrawItem( sb);
            Rectangle rect = GetRectangle();
            sb.Draw(highlight, rect, HighlightColor);
        }
    }
}
