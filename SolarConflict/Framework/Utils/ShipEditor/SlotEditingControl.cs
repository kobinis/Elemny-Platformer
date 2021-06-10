using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.SimpleGui;
using Microsoft.Xna.Framework;
using XnaUtils.Input;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils.SimpleGui.Controllers;
using SolarConflict.Framework.Scenes.Components;
using System.IO;

namespace SolarConflict
{
    [Serializable]
    class SlotEditingControl : GuiControl
    {
        private Agent _ship;
        private float _scale;
        private float _itemSize; //in pixels
        private string _currentCursorPosition;
        private GuiManager _guiHolder;        
        public SlotType SlotType { get; set; }
        public float Rotation { get; set; }
        public ControlSignals Activation { get; set; }
        
         
        public SlotEditingControl(Agent ship, Vector2 position, GuiManager guiHolder) : base()
        {
            Position = position;
            Init(ship, guiHolder);
        }

        public void Init(Agent ship, GuiManager guiHolder)
        {
            this.children.Clear();
            _currentCursorPosition = "";
            _guiHolder = guiHolder;
            _ship = ship;
            _scale = 700f / Math.Max(ship.GetSprite().Width, ship.GetSprite().Height);
            _itemSize = Math.Max(30 * _scale, 2);
            
            Activation = ControlSignals.None;

            if (ship.ItemSlotsContainer == null)
            {
                ship.ItemSlotsContainer = new ItemSlotsContainer();
            }

            this.Action += AddItemSlot;
            MakeGui(guiHolder);
        }

        private void MakeGui(GuiManager guiHolder)
        {       
            const int space = 15;

            float maxX = _ship.GetSprite().Width * _scale/2f;
            float maxY = _ship.GetSprite().Height * _scale/2f;            
            if (_ship.ItemSlotsContainer != null)
            {
                for (int i = 0; i < _ship.ItemSlotsContainer.AgentSlotsCount; i++) 
                {
                    ItemSlot itemSlot = _ship.ItemSlotsContainer[i];
                    Vector2 position = itemSlot.Position * _scale;
                    float size = _itemSize * Item.DisplaySizeMultiplyers[(int)itemSlot.Size];
                    ItemSlotControl itemControl = new ItemSlotControl(itemSlot, position, new Vector2(size));
                    itemControl.Index = i;
                    itemControl.Action += ItemGuiAction;
                    itemControl.CursorOn += guiHolder.ToolTipHandler;
                    //itemControl.DepthOffset = 0.4f;
                    AddChild(itemControl);
                }
            }

            Width = maxX * 2 +_itemSize + space;
            Height = maxY * 2 +_itemSize + space;
        }

        protected override void DrawLogic(SpriteBatch sb, Color? color = null)
        {
           // base.DrawLogic(sb, color);
            Vector2 origin = new Vector2(_ship.Sprite.Width / 2, _ship.Sprite.Height / 2);
            sb.Draw(_ship.GetSprite(), Position, null, Color.White, 0, origin, _scale, SpriteEffects.None, 0);
        }

        private void ItemGuiAction(GuiControl source, CursorInfo cursorLocation)
        {
            ItemControl itemControl = (ItemControl)source;
            if (cursorLocation.OnPressLeft) //on leftClick 
            {
                //SelectSlot() to edit size, rotation, items...
            }

            if (cursorLocation.OnPressRight)
            {
                RemoveChild(itemControl);
            }
        }

        private Vector2 GetCursorPosition(Vector2 position)
        {
            Vector2 res;
            res = (position - this.Position) / _scale;
            res.X = (float)Math.Round(res.X);
            res.Y = (float)Math.Round(res.Y);
            return res;
        }

        public override void UpdateLogic(global::XnaUtils.InputState inputState)
        {
            base.UpdateLogic(inputState);
            Vector2 pos = inputState.Cursor.Position - this.Position;
            _currentCursorPosition = GetCursorPosition(inputState.Cursor.Position).ToString();
            //textControl.Text = (pos / scale).ToString() + "\n " +
            //    Math.Round(pos.X).ToString() + "," + Math.Round(pos.Y).ToString();

        }

        public override void Draw(SpriteBatch sb, Color? color = null)
        {
            base.Draw(sb, color);

            sb.DrawString(Game1.font, _currentCursorPosition, Vector2.One * 5, Color.White);
        }

        public void AddItemSlot(Vector2 itemSlotPosition, bool OnPressRight)
        {
            float rotation = Rotation; //TODO: read size
            ItemSlot slot = new ItemSlot(SlotType, (SizeType)1, itemSlotPosition, MathHelper.ToRadians(rotation), Activation);
            //Vector2 position = slot.Position - this.Position;
            Vector2 position = slot.Position * _scale;
            ItemSlotControl itemControl = new ItemSlotControl(slot, position, new Vector2(_itemSize));
            itemControl.Action += ItemGuiAction;
            itemControl.CursorOn += _guiHolder.ToolTipHandler;
            AddChild(itemControl);

            if (OnPressRight)
            {                
                if (Rotation == 90)
                {
                    rotation = 270;
                }
                if (Rotation == 270)
                {
                    rotation = 90;
                }

                itemSlotPosition.Y = -itemSlotPosition.Y;
                slot = new ItemSlot(SlotType, (SizeType)1, itemSlotPosition, MathHelper.ToRadians(rotation), Activation);                
                position.Y = -position.Y;
                itemControl = new ItemSlotControl(slot, position, new Vector2(_itemSize));
                itemControl.Action += ItemGuiAction;
                itemControl.CursorOn += _guiHolder.ToolTipHandler;                
                AddChild(itemControl);
            }
        }

        private void AddItemSlot(GuiControl source, CursorInfo cursorLocation)
        {
            Vector2 itemSlotPosition = GetCursorPosition(cursorLocation.Position);
            AddItemSlot(itemSlotPosition, cursorLocation.OnPressRight);
        }

        public List<ItemSlot> GetSlotList()
        {
            var slotList = new List<ItemSlot>();
            foreach (var item in children)
            {
                slotList.Add(((ItemSlotControl)item).ItemSlot);                
            }
            return slotList;
        }

        public void Save()
        {
            LinkedList<string> text = new LinkedList<string>();
            string fileName = _ship.ID + ".txt";

            foreach (var item in children)
            {
                text.AddLast(((ItemSlotControl)item).ItemSlot.ToCode());
            }

            //ItemSlotsContainer itemSlotContiner = new ItemSlotsContainer();
            //foreach (var item in children)
            //{                
            //    itemSlotContiner.AddAgentSlot()
            //}
            

            //_ship.ItemSlotsContainer._itemSlots.Clear();
            //foreach (var item in children)
            //{
            //    text.AddLast(((ItemSlotControl)item).ItemSlot.ToCode());
            //    _ship.ItemSlotsContainer._itemSlots.Add(((ItemSlotControl)item).ItemSlot);
            //}
            
            //children


            File.WriteAllLines(fileName, text.ToArray());
        }
    }
}
