using SolarConflict.Framework.Scenes.Activitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;
using XnaUtils.SimpleGui;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SolarConflict.XnaUtils.SimpleGui;
using SolarConflict.Framework.GUI;
using XnaUtils.SimpleGui.Controllers;
using SolarConflict.Framework;
using XnaUtils.Input;
using XnaUtils.Graphics;
using SolarConflict.Framework.Scenes.HudEngine;
using SolarConflict.AI.GameAI;
using SolarConflict.Framework.Scenes;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using XnaUtils.Framework.Graphics;
using SolarConflict.Framework.Agents.Systems.Misc;

namespace SolarConflict.GameContent.Activities.SceneActivitys
{
    /// <summary>
    /// Guide on crafting
    /// </summary>
    public class ImbueActivity : SceneActivity
    {
        // private List<TutorialGoal> tutorialGoals;
        //  private Sprite guiMarking = Sprite.Get("invArrow");

        private CursorItemControl _cursorItem;
        private Agent playerShip;
        //  private RichTextControl moneyControl;
        private GuiManager gui;
        private AgentSlotsControl agentSlotsControl;
        private InventoryControl inventoryControl;
        private ItemControl _materialControl;
        private ItemControl _imbueControl;


        protected override void Init(ActivityParameters parameters)
        {
            base.Init(parameters);
            gui = new GuiManager();
            playerShip = _scene.PlayerAgent;
            bool hideCrafting = parameters.ParamDictionary.ContainsKey("hide_crafting");
            _cursorItem = new CursorItemControl(new Vector2(100 * 0.6f));
            _cursorItem.Sprite = null;
            if (playerShip != null && playerShip.Inventory != null && GameplaySettings.AutoSortInventory) //TODO: only when auto sort is on
                playerShip.Inventory.Sort();
            gui.Root = MakeGui(playerShip, _cursorItem, gui, hideCrafting);
            AddHelp(TextBank.Inst.GetString("HelpInventory"));
            _imbueControl.CursorOn += (e, s) => { _cursorItem.Filter = ImbuingFilter; };
            _materialControl.CursorOn += (e, s) => { _cursorItem.Filter = MaterialFilter; };

        }

        public GuiControl MakeGui(Agent player, CursorItemControl cursor, GuiManager gui, bool hideCrafting = false)
        {

            var guiControl = new ControlsGroup();
            HorizontalLayout mainHorizontalLayout = new HorizontalLayout(ActivityManager.ScreenSize / 2);
            mainHorizontalLayout.Spacing = 2;
            guiControl.AddChild(mainHorizontalLayout);
            VerticalLayout verticalLayout = new VerticalLayout(Vector2.Zero);

            verticalLayout.AddChild(MakeImbueControl(_cursorItem, gui, out _materialControl, out _imbueControl));

            mainHorizontalLayout.AddChild(verticalLayout);
            GuiControl playerControl = GuiControlFactory.MakeAgentControl(player, cursor, gui, out agentSlotsControl, out inventoryControl);
            verticalLayout.AddChild(playerControl);
            List<Agent> agents = new List<Agent>();
            agents.Add(player);
            List<Inventory> invList = new List<Inventory>();
            invList.Add(player.Inventory);
            //var allyAgent = GuiControlFactory.GetAllyAgent(_scene, player);

            

            return guiControl;
        }

        public static GuiControl MakeImbueControl(CursorItemControl cursorItem, GuiManager gui, out ItemControl itemToBeImbued, out ItemControl imbuingItemControl)
        {
            VerticalLayout layout = new VerticalLayout(Vector2.Zero);
            layout.ShowFrame = true;

            HorizontalLayout materialCraftingLayout = new HorizontalLayout(Vector2.Zero); //new RelativeLayout();
            itemToBeImbued = new ItemControl(null, Vector2.Zero, Vector2.One * 60);
            itemToBeImbued.TooltipText = "Place Shield or Generator.";
            itemToBeImbued.CursorOn += gui.ToolTipHandler;
            itemToBeImbued.Data = cursorItem;
            itemToBeImbued.Action += (GuiControl source, CursorInfo cursorLocation) =>
            {
                ItemControl cursorControl = (ItemControl)source.Data;
                ItemControl matControl = source as ItemControl;
                if (cursorControl.Item == null || (cursorControl.Item.ItemFlags & ItemFlags.Imbuable) > 0)
                {
                    Item item = matControl.Item;
                    matControl.Item = cursorControl.Item;
                    cursorControl.Item = item;
                }
                else
                    ActivityManager.Inst.AddToast("Place Shield or Generator", 30, Color.Red);

            };
            itemToBeImbued.LogicFunction = (GuiControl control, InputState input) =>
            {

                CursorItemControl cursorControl = (CursorItemControl)control.Data;
                control.SetAllColors(Palette.SlotColor, true);
                if (cursorItem.Item != null || cursorItem.ItemUnderCursor != null)
                {

                    var itemToCheck = cursorItem.Item;
                    if (itemToCheck == null)
                    {
                        itemToCheck = cursorItem.ItemUnderCursor;
                    }
                    //ItemSlotControl slotControl = item as ItemSlotControl;
                    if ((itemToCheck.ItemFlags & ItemFlags.Imbuable) > 0)
                    {
                        control.SetAllColors(Palette.SlotEnabledColor, true);
                    }
                }
            };
            materialCraftingLayout.AddChild(itemToBeImbued);//AddChild(_materialControl, HorizontalAlignment.Left, VerticalAlignment.Down, playerControl, 0); //Chack 

            imbuingItemControl = new ItemControl(null, Vector2.Zero, Vector2.One * 60);
            imbuingItemControl.CursorOn += gui.ToolTipHandler;
            imbuingItemControl.Data = cursorItem;
            imbuingItemControl.TooltipText = "Place imbuing component.";
            imbuingItemControl.Action += (GuiControl source, CursorInfo cursorLocation) =>
            {
                ItemControl cursorControl = (ItemControl)source.Data;
                ItemControl control = source as ItemControl;
                if (cursorControl.Item == null || (cursorControl.Item.Category & ItemCategory.Imbuing) > 0)
                {
                    Item item = control.Item;
                    control.Item = cursorControl.Item;
                    cursorControl.Item = item;
                }
                else
                    ActivityManager.Inst.AddToast("Place imbuing item", 30, Color.Red);

            };
            imbuingItemControl.LogicFunction = (GuiControl control, InputState input) =>
            {

                ItemControl cursorControl = (ItemControl)control.Data;
                control.SetAllColors(Palette.SlotColor, true);
                if (cursorItem.Item != null || cursorItem.Data != null)
                {

                    var itemToCheck = cursorItem.Item;
                    if (itemToCheck == null)
                    {
                        itemToCheck = cursorItem.Data as Item;
                    }
                    //ItemSlotControl slotControl = item as ItemSlotControl;
                    if ((itemToCheck.Category & ItemCategory.Imbuing) > 0)
                    {
                        control.SetAllColors(Palette.SlotEnabledColor, true);
                    }
                }
            };


            materialCraftingLayout.AddChild(imbuingItemControl);

            RichTextControl textControl = new RichTextControl("Imbue!");
            textControl.IsShowFrame = true;            
            textControl.Data = new Tuple<ItemControl, ItemControl>(itemToBeImbued, imbuingItemControl); ;
            textControl.Action += (GuiControl source, CursorInfo cursorLocation) =>
            {
                Tuple<ItemControl, ItemControl> imbuePair = textControl.Data as Tuple<ItemControl, ItemControl>;
                ItemControl toBeImbue = imbuePair.Item1;
                ItemControl imbuing = imbuePair.Item2;
                if(toBeImbue.Item != null && imbuing.Item != null)
                {
                    var imbueSystem = (imbuing.Item.System as ImbueSystem);
                    if(imbueSystem != null)
                    {
                        if(imbueSystem.ImbueItem(toBeImbue.Item, imbuing.Item.ID))
                        {
                            imbuing.Item = null;
                            ActivityManager.Inst.AddToast("Item Imbued.", 30, Color.Green);
                        }
                    }
                }
                else
                    ActivityManager.Inst.AddToast("Place both imbuable and imbeing items.", 30, Color.Red);
            };
            materialCraftingLayout.AddChild(textControl);//AddChild(text, HorizontalAlignment.Right, VerticalAlignment.Center, _materialControl);
            layout.AddChild(materialCraftingLayout);
            
                       
            return layout;
        }

        public bool ImbuingFilter(Item item)
        {
            if(item == null || (item.Category & ItemCategory.Imbuing) > 0)
            {
                return true;
            }
            return false;
        }

        public bool MaterialFilter(Item item)
        {
            if (item == null || (item.Category & (ItemCategory.Shield | ItemCategory.Generator)) > 0)
            {
                return true;
            }
            return false;
        }

        public override void Update(InputState inputState)
        {
            base.Update(inputState);

            _cursorItem.Update(inputState);
            //_cursorItem.Position = _scene.InputState.Cursor.Position;
            //_cursorItem.ItemUnderCursorCooldown++;

            gui.Update(_scene.InputState);
            //if (inputState.Cursor.IsActive)
            //    _cursorItem.Data = null;
            //ActivityManager.Inst.AddToast(inputState.Cursor.IsActive.ToString(), 50);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawBackground(spriteBatch);
            if (gui.Root != null)
            {
                gui.Draw();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                _cursorItem.Draw(spriteBatch);
                spriteBatch.End();
            }

            base.Draw(spriteBatch);
            //spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);        
            //spriteBatch.End();
        }

        private void CallOnExit()
        {
           
            playerShip.control.controlAi = ParameterControl.MakeAIFromAgent(playerShip); //***

            if (_cursorItem.Item != null) //TODO: add if shift is held drop the item or maybe move the item do scene cursour item
            {
                ////_scene.CursorItem = cursorItem.Item;
                if (playerShip.Inventory.HasRoom(_cursorItem.Item.ID, _cursorItem.Item.Stack))// and not drop to space
                {
                    playerShip.AddItemToInventory(_cursorItem.Item);
                }
                else
                {
                    _cursorItem.Item.Position = this.playerShip.Position;//_scene.GameEngine.Camera.GetWorldPos(_scene.InputState.Cursor.Position);
                    _scene.GameEngine.AddGameObject(_cursorItem.Item);
                }
                //_cursorItem.Item.Position = _scene.GameEngine.Camera.GetWorldPos(_scene.InputState.Cursor.Position);
                //_scene.GameEngine.AddGameObject(_cursorItem.Item);
            }

            if (_materialControl.Item != null) //TODO: add if shift is held drop the item or maybe move the item do scene cursour item
            {
                ////_scene.CursorItem = cursorItem.Item;
                if (playerShip.Inventory.HasRoom(_materialControl.Item.ID, _materialControl.Item.Stack))// and not drop to space
                {
                    playerShip.AddItemToInventory(_materialControl.Item);
                }
                else
                {
                    _materialControl.Item.Position = this.playerShip.Position;  // _scene.GameEngine.Camera.GetWorldPos(_scene.InputState.Cursor.Position);
                    _scene.GameEngine.AddGameObject(_materialControl.Item);
                }
            }

            if(_imbueControl.Item != null)
            {
                ItemControl control = _imbueControl;
                if (playerShip.Inventory.HasRoom(control.Item.ID, control.Item.Stack))// and not drop to space
                {
                    playerShip.AddItemToInventory(control.Item);
                }
                else
                {
                    _materialControl.Item.Position = this.playerShip.Position;  // _scene.GameEngine.Camera.GetWorldPos(_scene.InputState.Cursor.Position);
                    _scene.GameEngine.AddGameObject(control.Item);
                }
            }



        }

        public override ActivityParameters OnLeave()
        {
            CallOnExit();
            return base.OnLeave();
        }

        public override ActivityParameters OnBack()
        {
            CallOnExit();
            return base.OnBack();
        }

        private static void Control_Action(GuiControl source, CursorInfo cursorLocation)
        {
            var tuple = source.Data as Tuple<CraftingControl, SlotType>;
            tuple.Item1.FilterBySlotType(tuple.Item2);
        }

        public static Activity ActivityProvider(string parameters)
        {
            return new ImbueActivity();
        }
    }
}

