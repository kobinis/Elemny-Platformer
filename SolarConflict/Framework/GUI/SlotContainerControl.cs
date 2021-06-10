//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using SolarConflict.Framework.Scenes.Components;
//using SolarConflict.XnaUtils.SimpleGui;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using XnaUtils;
//using XnaUtils.Input;
//using XnaUtils.SimpleGui;
//using XnaUtils.SimpleGui.Controllers;

//namespace SolarConflict
//{
//    [Serializable]
//    public class SlotContainerControl : HorizontalLayout //Add Title and Icon
//    {        
//        public Color SlotColor { get; set; }
//        public Color SlotEnabledColor { get; set;}
//        private readonly int ITEM_SIZE = 60; //in pixels
//        private readonly int SPACE = 35;        
//        private ItemControl _cursorItem;
//        private ItemSlotsContainer _slotContiner;
//        public ItemSlotsContainer ItemSlotsContainer {  get { return _slotContiner; } }
//        private Agent _agent;
//        private GridControl _inputSelect;
//        private float _scale = 1.65f;
//        //GuiHolder guiHolder = null;
//        private GuiManager _guiHolder;
//        private List<ItemSlotControl> slotControls;

//        public SlotContainerControl(Agent agent, ItemControl cursorItem, Vector2 inPosition, GuiManager guiHolder)
//            : base(Vector2.Zero)
//        {
//            _guiHolder = guiHolder;            
//            Position = inPosition;
//            //this.inventory = inventory;
//            _agent = agent;
//            _slotContiner = agent.ItemSlotsContainer;

//            _cursorItem = cursorItem;          
//            MakeGui();
//            ShowFrame = true;
//            SlotColor = new Color(100,100 ,100,200);
//            SlotEnabledColor = new Color(0,255,0,200);
           
//        }

//        private void MakeGui( ) //can be simpler as it will never be null
//        {
//            slotControls = new List<ItemSlotControl>();
//            CalculateRectangleSize();

//            AddShipSlots();
//            AddBasicGear();                     
//        }

//        private void CalculateRectangleSize()
//        {         
//            Width = _agent.Sprite.Width ;
//            Height = _agent.Sprite.Height;
//            _scale = Math.Min(400 / (float)Math.Max(Width, Height), 2);

//            Width = Width * _scale + SPACE;
//            Height = Height * _scale + SPACE;
//        }

//        private void AddShipSlots()
//        {
//            CalculateRectangleSize();
//            GuiControl agentSlotsControl = new GuiControl(-Vector2.UnitX*(ITEM_SIZE + SPACE), new Vector2(Width, Height) );
//            agentSlotsControl.Sprite = null;
//            // Make the agent face the right way; the right way differs based on the agent's GameObjectType
//            var rotation = (_agent.gameObjectType & GameObjectType.NonRotating) != 0 ? 0f : -90f;
//            agentSlotsControl.DrawFuction = delegate (GuiControl control, SpriteBatch sb, Color? color)
//            {    Vector2 origin = new Vector2(_agent.Sprite.Width / 2, _agent.Sprite.Height / 2);
//                 sb.Draw(_agent.Sprite.Texture, control.Position, null, new Color(100,100,100), MathHelper.ToRadians(rotation), origin, _scale, SpriteEffects.None, 0);            
//            };
//            AddChild(agentSlotsControl);
//            for (int i = 0; i < _slotContiner.AgentSlotsCount; i++) //encapsulate in inventory Controller
//            {
//                ItemSlot itemSlot = _slotContiner[i];
//                Vector2 position = new Vector2(itemSlot.DisplayPosition.X, itemSlot.DisplayPosition.Y) * _scale;  //new Vector2(itemSlot.Position.Y, itemSlot.Position.X * -1) * _scale;
//                position = position.Rotated(MathHelper.ToRadians(rotation));
//                float displaySize = ITEM_SIZE * Item.DisplaySizeMultiplyers[(int)itemSlot.Size];
//                ItemSlotControl itemControl = new ItemSlotControl(itemSlot, position, new Vector2(displaySize));
//                itemControl.Rotation = rotation;
//                itemControl.ControlColor = Color.White;
//                itemControl.Index = i;
//                itemControl.Action += ItemGuiAction;
//                itemControl.CursorOn += _guiHolder.ToolTipHandler;                
//                agentSlotsControl.AddChild(itemControl);
//                slotControls.Add(itemControl);
//            }
//        }

//        private void AddBasicGear()
//        {
//            if (_slotContiner.BasicSlotsCount > 0)
//            { 
//                VerticalLayout layout = new VerticalLayout(Vector2.Zero);
//                layout.ShowFrame = true;
//                for (int i = _slotContiner.AgentSlotsCount; i < _slotContiner.Count; i++) //encapsulate in inventory Controller
//                {
//                    ItemSlot itemSlot = _slotContiner[i];
//                    float positionY = -halfHeight + SPACE + (i - _slotContiner.Count + 1) * ITEM_SIZE;
//                    Vector2 position = new Vector2(halfWidth, positionY) * _scale;
//                    ItemSlotControl itemControl = new ItemSlotControl(itemSlot, Vector2.Zero, new Vector2(ITEM_SIZE));
//                    itemControl.Index = i;
//                    itemControl.Action += ItemGuiAction;
//                    itemControl.CursorOn += _guiHolder.ToolTipHandler;
//                    layout.AddChild(itemControl);
//                    slotControls.Add(itemControl);
//                }
//                AddChild(layout);
//            }
//        }        

//        private void InputSelectHandler(GuiControl source, CursorInfo cursorLocation)
//        {
//            if (source.Index >= 1)
//            {
//                _slotContiner[source.Parent.Index].ActivationSignal = (ControlSignals)(1 << (source.Index-1));
//            }
//            else
//                _slotContiner[source.Parent.Index].ActivationSignal = ControlSignals.None;
//            _guiHolder.RemoveControl(_inputSelect);
//            _inputSelect = null;
//        }

//        private void MakeInputSelect(GuiManager guiHolder, ItemControl callingControl)
//        {
//            if (_inputSelect == null)
//            {
//                _inputSelect = new GridControl(1, 10, new Vector2(200, 50));
//                _inputSelect.Index = callingControl.Index;

//                ControlSignals signal = ControlSignals.None;
//                RichTextControl textControl = new RichTextControl(signal.ToString(), Game1.font);
//                textControl.Index = -1;
//                textControl.IsShowFrame = true;
//                textControl.Action += InputSelectHandler;
//                _inputSelect.AddChild(textControl);
//                textControl.TooltipText = "Activation command will not be send.";
//                textControl.CursorOn += guiHolder.ToolTipHandler;

//                for (int i = 0; i < 9; i++)
//                {
//                    signal = (ControlSignals)(1 << i);
//                    textControl = new RichTextControl(signal.ToString() +"#color{255,255,0}("+PlayerMouseAndKeys._GetTag(signal)+")", Game1.font);
//                    //textControl.Index = i;
//                    textControl.IsShowFrame = true;
//                    textControl.Action += InputSelectHandler;
//                    _inputSelect.AddChild(textControl);
//                    textControl.TooltipText = (PlayerMouseAndKeys.GetControlSignalString(signal)).ToString(); //TODO: player manager
//                    textControl.CursorOn += guiHolder.ToolTipHandler;
//                }

//                guiHolder.AddControl(_inputSelect);
//                _inputSelect.Position = callingControl.Position + new Vector2(0, _inputSelect.HalfSize.Y);
//            }
//        }

//        public override void UpdateLogic(InputState inputState)
//        {
//            base.UpdateLogic(inputState);
//            foreach (var child in this.children)
//            {
//                foreach (var item in child.GetChildren())
//                {
//                    item.ControlColor = SlotColor;
//                    item.CursorOverColor = SlotColor;
//                    if (_cursorItem.Item != null || _cursorItem.Data != null)
//                    {
                      
//                        var itemToCheck = _cursorItem.Item;
//                        if(itemToCheck == null)
//                        {
//                            itemToCheck = _cursorItem.Data as Item;                            
//                        }
//                        ItemSlotControl slotControl = (ItemSlotControl)item;
//                        if (slotControl.ItemSlot.CanEquip(itemToCheck))
//                        {
//                            slotControl.ControlColor = SlotEnabledColor;
//                            slotControl.CursorOverColor = SlotEnabledColor;
//                        }
//                    }
//                }
//            }
//        }

//        protected override void DrawLogic(SpriteBatch sb, Color? color = null)
//        {
//            base.DrawLogic(sb, color);
//          /*  Vector2 origin = new Vector2(_agent.TextureProxy.Width / 2, _agent.TextureProxy.Height / 2);
//            sb.Draw(_agent.TextureProxy.Texture, Position, null, Color.White, MathHelper.ToRadians(-90), origin, _scale, SpriteEffects.None, 0);            */
//        }


//        private void ItemGuiAction(GuiControl source, CursorInfo cursorLocation) //copy terraria bhiviour
//        {
//            ItemControl itemControl = (ItemControl)source;
//            if (cursorLocation.OnPressLeft) //on leftClick 
//            {
//                LeftClick(itemControl);                
//            }

//            if (cursorLocation.OnPressRight)
//            {

//                RightClick(itemControl);
//            }
//        }

//        private void LeftClick(ItemControl itemControl)
//        {
//            if (itemControl.Item != null && _cursorItem.Item !=null && Item.CanStack(itemControl.Item, _cursorItem.Item.ID))
//            {
//                int amount = Math.Min(itemControl.Item.MaxStack - itemControl.Item.Stack, _cursorItem.Item.Stack);
//                itemControl.Item.Stack += amount;
//                _cursorItem.Item.Stack -= amount;
//                if (_cursorItem.Item.Stack == 0)
//                {
//                    _cursorItem.Item = null;
//                }
//            }
//            else
//            {
//                Item item = _slotContiner[itemControl.Index].EquipItem(_cursorItem.Item);
//                if (item != _cursorItem.Item)
//                {
//                    itemControl.Item = _cursorItem.Item; //fix stacking issue
//                    _cursorItem.Item = item;
//                }    
//            }
//        }

//        private void RightClick(ItemControl source)
//        {
//            MakeInputSelect(_guiHolder, source);
        
//        }

//        public ItemSlotControl GetItemSlot(int index)
//        {
//            return slotControls[index];
//        }

//    }
//}

