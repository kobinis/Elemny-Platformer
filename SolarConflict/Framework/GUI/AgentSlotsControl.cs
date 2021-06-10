using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject;
using SolarConflict.Framework.Scenes.Components;
using SolarConflict.XnaUtils.SimpleGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;
using XnaUtils.Graphics;
using XnaUtils.Input;
using XnaUtils.SimpleGui;
using XnaUtils.SimpleGui.Controllers;

namespace SolarConflict.Framework.GUI
{
    /// <summary>
    /// 
    /// </summary>
    public class AgentSlotsControl: HorizontalLayout
    {

        int size = 400;
        const int spacing = 10;
        const int itemSize = 25;
        const int minItemSize = 45;
        public Color SlotColor { get; set; }
        public Color SlotEnabledColor { get; set; }
        public Agent agent { get; private set; }
        private float scale;
        private List<ItemSlotControl> slotControls;
        private CursorItemControl cursorItem;
        private GuiManager gui;
        private GuiControl inputSelect;
        

        public AgentSlotsControl(Agent agent, CursorItemControl cursor, GuiManager gui):base(Vector2.Zero)
        {
            size = (int)( 350 * GuiManager.Scale);

            SlotColor = Palette.SlotColor;
            SlotEnabledColor = Palette.SlotEnabledColor;
            ShowFrame = true;
            this.gui = gui;
            this.cursorItem = cursor;
            HalfSize = Vector2.One * size * 0.5f;
            this.agent = agent;
            scale = CalculateScale(agent, size);
            slotControls = new List<ItemSlotControl>();
            Init(gui);
        }

        public void Init(GuiManager gui)
        {
            
            GuiControl agentSlotsControl = new GuiControl(Vector2.Zero, Vector2.One * size);//-Vector2.UnitX * (itemSize + spacing), new Vector2(Width, Height));
            agentSlotsControl.Sprite = null;
            // Make the agent face the right way; the right way differs based on the agent's GameObjectType
            var rotation = (agent.gameObjectType & GameObjectType.NonRotating) != 0 ? 0f : -90f;
            agentSlotsControl.DrawFuction = delegate (GuiControl control, SpriteBatch sb, Color? color)
            {
                Vector2 origin = new Vector2(agent.Sprite.Width / 2, agent.Sprite.Height / 2);
                sb.Draw(agent.Sprite.Texture, control.Position, null, new Color(100, 100, 100), MathHelper.ToRadians(rotation), origin, scale, SpriteEffects.None, 0);
            };
            AddChild(agentSlotsControl);
            var slotContiner = agent.ItemSlotsContainer;
            for (int i = 0; i < slotContiner.AgentSlotsCount; i++) //encapsulate in inventory Controller
            {
                ItemSlot itemSlot = slotContiner[i];
                Vector2 position = new Vector2(itemSlot.DisplayPosition.X, itemSlot.DisplayPosition.Y) * scale;  //new Vector2(itemSlot.Position.Y, itemSlot.Position.X * -1) * _scale;
                position = position.Rotated(MathHelper.ToRadians(rotation));
                float displaySize = Math.Max( itemSize * Item.DisplaySizeMultiplyers[(int)itemSlot.Size] * scale, minItemSize);
                ItemSlotControl itemControl = new ItemSlotControl(itemSlot, position, new Vector2(displaySize));
                itemControl.Rotation = rotation;
                itemControl.ControlColor = Color.LightBlue;
                itemControl.Index = i;
                itemControl.Action += ItemGuiAction;
                itemControl.CursorOn += gui.ToolTipHandler;
                itemControl.CursorOn += ItemControl_CursorOn;
                agentSlotsControl.AddChild(itemControl);
                slotControls.Add(itemControl);
            }

            AddBasicGear(gui);
        }

        ItemSlot slot = new ItemSlot();
        public bool FilterItems(Item item)
        {
            return slot.CanEquip(item);
        }

        private void ItemControl_CursorOn(GuiControl source, CursorInfo cursorLocation)
        {
            slot = (source as ItemSlotControl).ItemSlot;
            cursorItem.Filter = FilterItems;
        }

        private void AddBasicGear(GuiManager gui)
        {
            float scale = GuiManager.Scale;
            var slotContiner = agent.ItemSlotsContainer;
            if (slotContiner.BasicSlotsCount > 0)
            {
                VerticalLayout layout = new VerticalLayout(Vector2.Zero);
                layout.ShowFrame = true;
                ImageControl info = new ImageControl(TextureBank.Inst.GetSprite("helpicon"), Vector2.Zero, new Vector2(itemSize * 2 * scale));
                info.TooltipText = AgentUtils.DescribeStatsAndAbilities(agent);
                info.CursorOn += gui.ToolTipHandler;
                layout.AddChild(info);
                for (int i = slotContiner.AgentSlotsCount; i < slotContiner.Count; i++)
                {
                    ItemSlot itemSlot = slotContiner[i];
                    ItemSlotControl itemControl = new ItemSlotControl(itemSlot, Vector2.Zero, new Vector2(itemSize*2 * scale));
                    itemControl.Index = i;
                    itemControl.Action += ItemGuiAction;
                    itemControl.CursorOn += gui.ToolTipHandler;
                    itemControl.CursorOn += ItemControl_CursorOn;
                    layout.AddChild(itemControl);
                    slotControls.Add(itemControl);
                }
                AddChild(layout);
            }
        }

        public ItemSlotControl GetItemSlot(int index)
        {
            return slotControls[index];
        }

        public override void UpdateLogic(InputState inputState)
        {
            base.UpdateLogic(inputState);
            foreach (var child in this.children)
            {
                foreach (var item in child.GetChildren())
                {
                    item.ControlColor = SlotColor;
                    item.CursorOverColor = SlotColor;
                    if (cursorItem.GetItemToCheck() != null)
                    {

                        var itemToCheck = cursorItem.GetItemToCheck();
                        ItemSlotControl slotControl = item as ItemSlotControl;
                        if (slotControl != null && slotControl.ItemSlot.CanEquip(itemToCheck))
                        {
                            slotControl.ControlColor = SlotEnabledColor;
                            slotControl.CursorOverColor = SlotEnabledColor;
                        }
                    }
                }
            }
        }

        private static float CalculateScale(Agent agent, float targetSize)
        {
            float scale = Math.Min(4, targetSize / (Math.Max(agent.Sprite.Width, agent.Sprite.Height)+spacing) );
            return scale;
        }

        private void ItemGuiAction(GuiControl source, CursorInfo cursorLocation) 
        {
            ItemControl itemControl = (ItemControl)source;
            if (cursorLocation.OnPressLeft)
            {
                LeftClick(itemControl);                
            }

            if (cursorLocation.OnPressRight)
            {

                MakeInputSelect(gui, itemControl);
            }
        }

        private void LeftClick(ItemControl itemControl)
        {
            if (itemControl.Item != null && cursorItem.Item !=null && Item.CanStack(itemControl.Item, cursorItem.Item.ID))
            {
                int amount = Math.Min(itemControl.Item.MaxStack - itemControl.Item.Stack, cursorItem.Item.Stack);
                itemControl.Item.Stack += amount;
                cursorItem.Item.Stack -= amount;
                if (cursorItem.Item.Stack == 0)
                {
                    cursorItem.Item = null;
                }
            }
            else
            {
                Item item = agent.ItemSlotsContainer[itemControl.Index].EquipItem(cursorItem.Item);
                if (item != cursorItem.Item)
                {
                    itemControl.Item = cursorItem.Item; //fix stacking issue
                    cursorItem.Item = item;
                }    
            }
        }

        private void MakeInputSelect(GuiManager guiHolder, ItemControl callingControl)
        {
            if (inputSelect == null)
            {
                inputSelect = new GridControl(1, 11, new Vector2(200, 50));
                inputSelect.LogicFunction = RemoveControl;
                inputSelect.Index = callingControl.Index;
                

                RichTextControl textControl = new RichTextControl("#image{back1}", Game1.font);                
                textControl.IsShowFrame = true;
                textControl.Action += InputSelectHandler;
                inputSelect.AddChild(textControl);
                textControl.TooltipText = "Close";
                textControl.CursorOn += guiHolder.ToolTipHandler;

                ControlSignals signal = ControlSignals.None;
                textControl = new RichTextControl(signal.ToString(), Game1.font);                
                textControl.IsShowFrame = true;
                textControl.Action += InputSelectHandler;                
                inputSelect.AddChild(textControl);
                textControl.TooltipText = "Activation command will not be send.";
                textControl.UserData = ControlSignals.None.ToString();
                textControl.CursorOn += guiHolder.ToolTipHandler;

                for (int i = 0; i < 9; i++)
                {
                    signal = (ControlSignals)(1 << i);
                    textControl = new RichTextControl(signal.ToString() + "#color{255,255,0}(" + KeysSettings.GetTag(signal) + ")", Game1.font);
                    textControl.Index = i;
                    textControl.IsShowFrame = true;
                    textControl.Action += InputSelectHandler;
                    inputSelect.AddChild(textControl);
                    textControl.TooltipText = (KeysSettings.GetControlSignalString(signal)).ToString(); //TODO: player manager
                    textControl.CursorOn += guiHolder.ToolTipHandler;
                    textControl.UserData = signal.ToString();
                }

                guiHolder.AddControl(inputSelect);
                inputSelect.Position = callingControl.Position + new Vector2(0, inputSelect.HalfSize.Y);
                inputSelect.Position = new Vector2(inputSelect.Position.X,  Math.Min(inputSelect.Position.Y, ActivityManager.ScreenSize.Y - inputSelect.Height / 2));
            }
        }

        public void RemoveControl(GuiControl control, InputState inputState)
        {
            if(inputState.Cursor.OnPressLeft && !control.IsCursorOn)
            {
                gui.RemoveControl(inputSelect);
                inputSelect = null;
            }
        }

        private void InputSelectHandler(GuiControl source, CursorInfo cursorLocation)
        {
            if (source.Index >=1)
            {
                agent.ItemSlotsContainer[source.Parent.Index].ActivationSignal = (ControlSignals)Enum.Parse(typeof(ControlSignals), source.UserData);                                    
            }
            gui.RemoveControl(inputSelect);
            inputSelect = null;
        }

    }
}
