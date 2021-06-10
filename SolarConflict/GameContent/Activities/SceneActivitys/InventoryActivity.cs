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

namespace SolarConflict.GameContent.Activities.SceneActivitys
{  
    public class InventoryActivity : SceneActivity
    {
        private List<TutorialGoal> tutorialGoals;
        private Sprite guiMarking = Sprite.Get("invArrow");

        private CursorItemControl cursorItem;
        private Agent playerShip;
        private RichTextControl moneyControl;
        private GuiManager gui;
        private AgentSlotsControl agentSlotsControl;
        private InventoryControl inventoryControl;
        private InventoryControl allyInventoryControl;
        private AgentSlotsControl allySlotsControl;
        private CraftingControl _craftingControl;
        private List<Agent> agents;

        protected override void Init(ActivityParameters parameters)
        {
            base.Init(parameters);
            tutorialGoals = new List<TutorialGoal>();
            tutorialGoals.AddRange(_scene.GetTutorialGoals());            

            gui = new GuiManager();
            playerShip = _scene.PlayerAgent;
            bool hideCrafting = parameters.ParamDictionary.ContainsKey("hide_crafting");
            cursorItem = new CursorItemControl(new Vector2(96));
            cursorItem.SetData("Player", playerShip);
            cursorItem.Sprite = null;
            
            
           // cursorItem.Action += CursorItem_Action;
            if (playerShip != null && playerShip.Inventory != null && GameplaySettings.AutoSortInventory) //TODO: only when auto sort is on
                playerShip.Inventory.Sort();
            gui.Root = MakeGui(playerShip, cursorItem, gui, hideCrafting);
            gui.Root.AddChild(cursorItem);
            AddHelp(TextBank.Inst.GetString("HelpInventory"));           
        }

        //private void CursorItem_Action(GuiControl source, CursorInfo cursorLocation)
        //{
        //    throw new NotImplementedException();
        //}

        public GuiControl MakeGui(Agent player, CursorItemControl cursor, GuiManager gui , bool hideCrafting = false)
        {
            var guiControl = new ControlsGroup();
            HorizontalLayout mainHorizontalLayout = new HorizontalLayout(ActivityManager.ScreenSize / 2);
            mainHorizontalLayout.Spacing = 2;
            guiControl.AddChild(mainHorizontalLayout);
            VerticalLayout verticalLayout = new VerticalLayout(Vector2.Zero);
            mainHorizontalLayout.AddChild(verticalLayout);
            GuiControl playerControl = GuiControlFactory.MakeAgentControl(player, cursor, gui, out agentSlotsControl, out inventoryControl);
            verticalLayout.AddChild(playerControl);
            agents = new List<Agent>();
            agents.Add(player);
            List<Inventory> invList = new List<Inventory>();
            invList.Add(player.Inventory);
            cursorItem.InventoryList = invList;
            var allyAgent = GuiControlFactory.GetAllyAgent(_scene, player);
            if (!hideCrafting)
                verticalLayout.AddChild(MakeCommandsControl(player.Inventory, allyAgent, playerControl.Width));
            if (allyAgent != null)
            {
                agents.Add(allyAgent);                
                invList.Add(allyAgent.Inventory);
                verticalLayout.AddChild(GuiControlFactory.MakeAgentControl(allyAgent, cursor, gui, out allySlotsControl, out allyInventoryControl));
            }  
            
            if(!hideCrafting)
                mainHorizontalLayout.AddChild(GuiControlFactory.MakeCraftingGui(player, cursor, gui, _scene.GameEngine, agents, invList, _scene, out _craftingControl, !_scene.HideRecycleBean && tutorialGoals.Count == 0 , new Point(2, 6)));

            moneyControl = new RichTextControl("");
            moneyControl.Position = Vector2.One * 50;
            moneyControl.Position = new Vector2(moneyControl.HalfSize.X + 10, ActivityManager.ScreenSize.Y - moneyControl.HalfSize.Y - 10);
            moneyControl.IsShowFrame = true;
            guiControl.AddChild(moneyControl);
            
            return guiControl;
        }
      
        public override void Update(InputState inputState)
        {            
            base.Update(inputState);            
            cursorItem.Data = null;
            if (playerShip != null)
            {
                int money = (int)playerShip.GetMeterValue(MeterType.Money);
                moneyControl.Text = money.ToString() + "#image{coin}";
                moneyControl.Position = new Vector2(moneyControl.HalfSize.X + 10, ActivityManager.ScreenSize.Y - moneyControl.HalfSize.Y - 10);
            }
            //cursorItem.Position = _scene.InputState.Cursor.Position;
            //cursorItem.ItemUnderCursorCooldown++;

            gui.Update(_scene.InputState);

            if (inputState.IsKeyDown(Keys.LeftControl))
            {
                
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawBackground(spriteBatch);
            if (gui.Root != null)
            {
                gui.Draw();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                cursorItem.Draw(spriteBatch);
                spriteBatch.End();
            }
          
            base.Draw(spriteBatch);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            DrawAllGoals();
            spriteBatch.End();
        }
        

        private void DrawAllGoals()
        {
            bool isDone = true;
            foreach (var goal in tutorialGoals)
            {
                isDone &= goal.DrawTutorialGoal(_craftingControl, cursorItem, agentSlotsControl, inventoryControl, allySlotsControl, allyInventoryControl);
                if (!isDone)
                    break;
            }

            if (isDone && tutorialGoals.Count > 0)
            {

                ActivityManager.Inst.AddToast("Press ESC to go back", 10, Color.Yellow);  //Change    
                HudUtils.DrawArrow(Game1.sb, guiMarking, _back.Position);
            }
        }

        
        private void CallOnExit()
        {
            SettingsManager.Inst.Save();
            foreach (var agent in agents)
            {
                if(agent.control != null)
                {
                    agent.control.controlAi = ParameterControl.MakeAIFromAgent(agent);
                }
            }
            
            
            if (cursorItem.Item != null) //TODO: add if shift is held drop the item or maybe move the item do scene cursour item
            {
                ////_scene.CursorItem = cursorItem.Item;
                if (playerShip.Inventory.HasRoom(cursorItem.Item.ID, cursorItem.Item.Stack))// and not drop to space
                {
                    playerShip.AddItemToInventory(cursorItem.Item);
                }
                else
                {
                    cursorItem.Item.Position = _scene.GameEngine.Camera.GetWorldPos(_scene.InputState.Cursor.Position);
                    _scene.GameEngine.AddGameObject(cursorItem.Item);
                }
                //_cursorItem.Item.Position = _scene.GameEngine.Camera.GetWorldPos(_scene.InputState.Cursor.Position);
                //_scene.GameEngine.AddGameObject(_cursorItem.Item);
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

        private GuiControl MakeCommandsControl(Inventory mainInventory, Agent allyAgent, float width) //move 
        {
            RelativeLayout layout = new RelativeLayout();
            
            //layout.Spacing = 4;
            //layout.ShowFrame = true;
            HorizontalLayout sortLayout = new HorizontalLayout(Vector2.Zero);
            sortLayout.Spacing = 1;
            sortLayout.ShowFrame = false;

            GuiControl highlightToogle = new GuiControl(Vector2.Zero, Vector2.One * 45);
            highlightToogle.IsToggleable = true;
            highlightToogle.IsPressed = GameplaySettings.IsFilter;
            highlightToogle.TooltipText = "Toggle item help highlight";
            highlightToogle.CursorOn += gui.ToolTipHandler;
            highlightToogle.Sprite = Sprite.Get("unselected");
            highlightToogle.PressedSprite = Sprite.Get("selected");
            highlightToogle.LogicFunction += (GuiControl tc, InputState input) =>
            {
                GameplaySettings.IsFilter = tc.IsPressed;
            };
            sortLayout.AddChild(highlightToogle);

            RichTextControl control = new RichTextControl("Sort", isShowFrame:true);            
            control.Data = new Tuple<Inventory, Inventory>(mainInventory, allyAgent?.Inventory);
            control.Action += (GuiControl source, CursorInfo cursorLocation) =>
            {
                Tuple<Inventory, Inventory> invs = source.Data as Tuple<Inventory, Inventory>;
                invs.Item1?.Sort();
                invs.Item2?.Sort();
            };
            control.TooltipText = "Sort will stack and sort inventory items, it will also remove basic items since they can be crafted with no cost.";
            control.CursorOn += gui.ToolTipHandler;
            sortLayout.AddChild(control);

            GuiControl toogleSort = new GuiControl(Vector2.Zero, Vector2.One * 40);
            toogleSort.IsToggleable = true;
            toogleSort.IsPressed = GameplaySettings.AutoSortInventory;
            toogleSort.TooltipText = "Toggle Auto Sort";
            toogleSort.CursorOn += gui.ToolTipHandler;
            toogleSort.Sprite = Sprite.Get("Ongoing");
            toogleSort.PressedSprite = Sprite.Get("Completed");
            toogleSort.LogicFunction += (GuiControl toggleControl, InputState input) =>
            {
                GameplaySettings.AutoSortInventory = toggleControl.IsPressed;
            };
            sortLayout.AddChild(toogleSort);

            layout.AddChild(sortLayout, HorizontalAlignment.Left, VerticalAlignment.Center);
            sortLayout.Height = control.Height;

            //RichTextControl autoEquip
           

            //Drop all
            //Loot all
            //Auto equip
            if (allyAgent != null && allyAgent.Inventory != null)
            {
                control = new RichTextControl(" " + Sprite.Get("arrowDown").ToTag(Palette.GuiFrame)+ " ");
                control.IsShowFrame = true;
                control.TooltipText = "Stash";
                control.CursorOn += gui.ToolTipHandler;
                control.Data = new Tuple<Inventory, Inventory>(mainInventory, allyAgent.Inventory);
                control.Action += (GuiControl source, CursorInfo cursorLocation) =>
                {
                    Tuple<Inventory, Inventory> invTuple = source.Data as Tuple<Inventory, Inventory>;
                    if(cursorLocation.OnPressLeft)
                        invTuple.Item1.TryTransferExcept(invTuple.Item2, ItemCategory.None);

                    if (cursorLocation.OnPressRight)
                        invTuple.Item1.TryTransferExcept(invTuple.Item2, ItemCategory.None, Inventory.QUICK_USE_COUNT);
                    
                };
                layout.AddChild(control, HorizontalAlignment.Left, VerticalAlignment.Center, layout.LastChildAdded);

                control = new RichTextControl(" " + Sprite.Get("arrowUp").ToTag(Palette.GuiFrame)+ " ");
                control.TooltipText = "Loot";
                control.CursorOn += gui.ToolTipHandler;
                control.IsShowFrame = true;
                control.Data = new Tuple<Inventory, Inventory>(mainInventory, allyAgent.Inventory);
                control.Action += (GuiControl source, CursorInfo cursorLocation) =>
                {
                    Tuple<Inventory, Inventory> invTuple = source.Data as Tuple<Inventory, Inventory>;
                    if (cursorLocation.OnPressLeft)
                        invTuple.Item2.TryTransferExcept(invTuple.Item1, ItemCategory.None);
                    if (cursorLocation.OnPressRight)
                        invTuple.Item2.TryTransferExcept(invTuple.Item1, ItemCategory.None, Inventory.QUICK_USE_COUNT);
                    
                };
                layout.AddChild(control, HorizontalAlignment.Left, VerticalAlignment.Center, layout.LastChildAdded);
            }

            layout.Width = width;
            return layout;
        }

        public void QuickEquipp()
        {

        }

        public static Activity ActivityProvider(string parameters)
        {
            return new InventoryActivity();
        }
    } 
}
