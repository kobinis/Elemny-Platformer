using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.SimpleGui;
using Microsoft.Xna.Framework;
using XnaUtils.Input;
using XnaUtils.SimpleGui.Controllers;
using SolarConflict.XnaUtils.SimpleGui;
using XnaUtils.Graphics;
using SolarConflict.Framework;
using XnaUtils;
using Microsoft.Xna.Framework.Graphics;
using SolarConflict.GameContent.Activities.SceneActivitys;
using SolarConflict.Framework.GUI;

namespace SolarConflict
{
    public class InventoryControl : ScrollableGrid,IInventoryControl //Add Title and Icon
    {
        private float itemSize;
        private CursorItemControl _cursorItem;
        public Inventory Inventory { get; private set; }
        public ItemControl[] ItemControls { get; private set; }
        public bool TakeClone;
        private Dictionary<string, float> _itemPriceMultiplier;

        public InventoryControl(Inventory inventory, CursorItemControl cursorItem, Vector2 inPosition, GuiManager guiHolder, Dictionary<string, float> itemPriceMultiplier = null, int lineNum = 2, float itemSize = 70)
            : base(9, lineNum, Vector2.One* itemSize, 2)
        {
            this.itemSize = itemSize;
            _itemPriceMultiplier = itemPriceMultiplier;
            Position = inPosition;
            this.Inventory = inventory;
            this._cursorItem = cursorItem;          
            MakeGui(guiHolder);            
            TakeClone = false;
        }


        private void MakeGui(GuiManager guiHolder)
        {
            ItemControls = new ItemControl[Inventory.Size];
            for (int i = 0; i < Inventory.Size; i++)
            {
                Item item = Inventory.GetItem(i);                
                ItemControl itemControl = new ItemControl(item, Vector2.Zero, Vector2.One* itemSize);                                                            
                itemControl.CursorOn += guiHolder.ToolTipHandler;
                itemControl.Index = i;
                itemControl.CursorOn += ItemGuiAction;
                itemControl.CursorOn += CursorOverHandler;
                itemControl.IsConsumingInput = false;
                if (i < Inventory.QUICK_USE_COUNT && !TakeClone)  //InputUtils.NumberOfQuickStart)
                {
                   // itemControl.Sprite = TextureBank.Inst.GetSprite("one");
                    itemControl.ControlColor = Color.Gray; //new Color(0.8f, 0.8f, 0.5f);
                    itemControl.EmptyText = "Activation Key: #color{255,255,0}(" + (i+1).ToString()+")#dcolor{}";
                    //itemControl.DrawFuction = delegate (GuiControl control, SpriteBatch sb, Color? color)
                    //{
                    //    ItemIndicator.DrawKeyBinding(sb, control.Position - control.HalfSize, Inventory.quickUseBind[control.Index], false, 20);
                    //    itemControl.Sprite = TextureBank.Inst.GetSprite("one");
                    //};                    
                    itemControl.CursorOn += guiHolder.ToolTipHandler;                    
                    ImageControl image = new ImageControl(Sprite.Get("c" + (i + 1).ToString("0")), Vector2.Zero, itemControl.HalfSize);
                    image.Position = new Vector2(-itemControl.HalfSize.X /2f, itemControl.HalfSize.Y /2f);
                    image.IsConsumingInput = false;
                    itemControl.CursorOn += (s, c) => { _cursorItem.Filter = QuickUseFilter; }; 
                   itemControl.AddChild(image);
                }
                ItemControls[i] = itemControl;
                AddChild(itemControl);
            }            
        }

        public bool QuickUseFilter(Item item)
        {
            return item.Profile.IsActivatable && item.Profile.IsWorkingInInventory;
        }
        


        private void CursorOverHandler(GuiControl source, CursorInfo cursorLocation)
        {
            _cursorItem.ItemUnderCursor = (source as ItemControl).Item;
        }

        public override void UpdateLogic(global::XnaUtils.InputState inputState)
        {
            ItemFilter filter = _cursorItem.Filter;
            base.UpdateLogic(inputState);
            for (int i = 0; i < Inventory.Size; i++)
            {
                Item item = Inventory.GetItem(i);
                ItemControls[i].SetItem(item, Item.GetItemPriceMult(item, _itemPriceMultiplier));
                if (GameplaySettings.IsFilter && filter != null && ItemControls[i].Item != null && filter(ItemControls[i].Item))
                    ItemControls[i].HighlightColor = Color.White; //Color.Yellow;// Palette.Highlight;
                else
                    ItemControls[i].HighlightColor = Color.Transparent;
            }    
        }



        private void ItemGuiAction(GuiControl source, CursorInfo cursorLocation) //copy terraria bhiviour
        {            
            ItemControl itemControl = (ItemControl)source;            
            if (cursorLocation.OnPressLeft)// take on press/ putt on relese  || cursorLocation.OnReleaseLeft) //on leftClick 
            {
                LeftClick(itemControl);                
            }

            if (cursorLocation.OnPressRight)
            {

                RightClick(itemControl);
            }
        }

        //move it as a static to Item
        public bool CanStack(Item target, Item source)
        {
            if (target == null)
                return true;
            if (source == null)
                return false;
            return target.Stack < target.MaxStack && target.ID == source.ID;                        
        }

        private void LeftClick(ItemControl itemControl)
        {
            if (_cursorItem.InventoryList != null && (ActivityManager.Inst.InputState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift)
                || ActivityManager.Inst.InputState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightShift)) )
            {
                Inventory inv = Inventory;
                foreach (var item in _cursorItem.InventoryList)
                {
                    if(item != Inventory)
                    {
                        inv = item;
                    }
                }
                Inventory.TryTransfer(inv, itemControl.Index);
                
            }
            else
            {
                if (itemControl.Item != null && CanStack(itemControl.Item, _cursorItem.Item))
                {
                    if (!TakeClone)
                    {
                        int amount = Math.Min(itemControl.Item.MaxStack - itemControl.Item.Stack, _cursorItem.Item.Stack);
                        itemControl.Item.Stack += amount;
                        _cursorItem.Item.Stack -= amount;
                        if (_cursorItem.Item.Stack == 0)
                        {
                            _cursorItem.Item = null;
                        }
                    }
                }
                else
                {
                    if (!TakeClone)
                    {
                        itemControl.Item = _cursorItem.Item;
                        Item invItem = Inventory.SetItem(itemControl.Index, _cursorItem.Item);
                        _cursorItem.Item = invItem;
                    }
                    else
                    {
                        _cursorItem.Item = itemControl.Item.GetWorkingCopy();
                    }

                }
            }
        }

        private void RightClick(ItemControl source)
        {
            
            Item targetItem = _cursorItem.Item;
            Item sourceItem = source.Item;
            if (sourceItem != null && sourceItem.MaxStack > 1)
            {
                Item.StackOne(ref targetItem, ref sourceItem);
                _cursorItem.Item = targetItem;
                source.Item = sourceItem;
            }
            else
            {
                Agent player = _cursorItem.GetData("Player") as Agent;
                if (player != null)
                {
                    int? index = player.ItemSlotsContainer.FindItemSlot(sourceItem, false);
                    if (index.HasValue)
                    {
                        Item tmpItem = player.ItemSlotsContainer[index.Value].Item;
                        player.ItemSlotsContainer[index.Value].Item = sourceItem;
                        //      source.Item = tmpItem;
                        Inventory.SetItem(source.Index, tmpItem);
                        //Inventory()
                    }
                }
            }
        }

    }
}
